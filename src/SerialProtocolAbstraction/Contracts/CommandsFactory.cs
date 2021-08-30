using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SerialProtocolAbstraction
{
    /// <summary>
    /// It creates and handle of Commands Attributes.
    /// It's implementations will parse depending in which protocol it is using.
    /// </summary>
    public abstract class CommandsFactory
    {
        public static CommandsFactory GetCommand(EnumProtocol protocol) 
        {
            switch (protocol) 
            {
                case EnumProtocol.SCPI:
                    return SCPICommandsFactory.GetInstance();
                case EnumProtocol.WebApi:
                    return WebApiCommandsFactory.GetInstance();
            }

            throw new Exception("No protocol found!");
        }

        public Command[] AvailableCommands;

        /// <summary>
        /// Starts the tree based on the Assembly and namespace.
        ///
        /// Exceptions:
        /// ParameterRepeatedException
        /// CommandRepeatedException
        /// CommandWithSubcommandsException
        /// CommandGroupTogetherException
        /// ICommandWithoutCommandAttributeException
        /// FactoryCommandTreeNotCreatedException
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public Command[] CreateCommandTree(Assembly assembly, string nameSpace)
        {
            Type[] types = GetTypesInNamespace(assembly, nameSpace).Where(t => t.IsNestedPublic == false).ToArray();
            AvailableCommands = CreateMultipleCommands(types);
            return AvailableCommands;
        }

        protected abstract string[] GetCommands(string fullCommand);
        protected abstract Dictionary<string, string> GetParameters(string fullCommand, List<ArgumentAttribute> arguments, List<OptionAttribute> options);
        public abstract string CreateDocumentation();

        /// <summary>
        /// Exceptions:
        /// CommandNotFoundException
        /// MissingArgumentException
        /// </summary>
        /// <param name="fullCommand">The command that the implementation will parse</param>
        public void ExecuteCommand(string fullCommand)
        {
            fullCommand = fullCommand.Trim();
            string[] rawCommands = GetCommands(fullCommand); // ABSTRACT

            var explorer = new CommandsExplorer(AvailableCommands);

            List<Command> commands = explorer.GetCommandListByString(rawCommands);
            List<ArgumentAttribute> argumentsNeeded = explorer.GetArgumentsForACommandChain(commands);
            List<OptionAttribute> optionsNeeded = explorer.GetOptionsForACommandChain(commands);

            Dictionary<string, string> kwargs = GetParameters(fullCommand, argumentsNeeded, optionsNeeded); // ABSTRACT
            CheckAllArguments(argumentsNeeded, kwargs);

            kwargs.Add("_full_command", fullCommand);

            RunCommands(commands, kwargs);
        }

        private void ValidateNeededParameters(List<ArgumentAttribute> argumentsNeeded, List<OptionAttribute> optionsNeeded, string fullCommand)
        {
            Dictionary<string, ParameterAttribute> parameters = new Dictionary<string,ParameterAttribute>();
            foreach (var arg in argumentsNeeded) 
            {
                if (parameters.ContainsKey(arg.Parameter))
                    throw new ParameterRepeatedException(arg.Parameter);
                parameters.Add(arg.Parameter, arg);
            }

            foreach (var opt in optionsNeeded)
            {
                if (parameters.ContainsKey(opt.Parameter))
                    throw new ParameterRepeatedException(opt.Parameter);
                parameters.Add(opt.Parameter, opt);
            }
        }

        internal Command[] CreateMultipleCommands(Type[] types)
        {
            Dictionary<string, byte> checkerForRepeatedCommands = new Dictionary<string, byte>(); // Should be HASH SET!
            List<Command> commands = new List<Command>();
            foreach (Type i in types)
            {
                if (i.GetInterfaces().Contains(typeof(ICommand)))
                {
                    Command command = CreateSingleCommand(i);

                    commands.Add(command);
                    CheckRepeatedCommand(checkerForRepeatedCommands, i);

                    CheckForRepeatedParameters(command, new Dictionary<string, ParameterAttribute>());
                }
            }
            return commands.ToArray();
        }

        internal Command CreateSingleCommand(Type type)
        {
            var attribute = Attribute.GetCustomAttributes(type, typeof(CommonCommandAttribute)) as CommonCommandAttribute[];
            
            ValidateCommand(type, attribute);
            return InstantiateSingleCommand(type, attribute[0]);
        }

        #region Helpers

        private void CheckAllArguments(List<ArgumentAttribute> argumentsNeeded, Dictionary<string, string> kwargs)
        {
            for (int i = 0; i < argumentsNeeded.Count; i++)
            {
                if (!kwargs.ContainsKey(argumentsNeeded[i].Parameter))
                    throw new MissingArgumentException(argumentsNeeded[i].Parameter);
            }
        }

        private void CheckForRepeatedParameters(Command command, Dictionary<string, ParameterAttribute> parentParameters)
        {
            Dictionary<string, ParameterAttribute> newDic = ValidateParameters(parentParameters, command.Parameters);
            if (command is Group)
            {
                var nestedCommands = (command as Group).NestedCommands;
                foreach (var nestedCommand in nestedCommands)
                {
                    CheckForRepeatedParameters(nestedCommand, newDic);
                }
            }
        }

        private Dictionary<string, ParameterAttribute> ValidateParameters(Dictionary<string, ParameterAttribute> parentParameters, ParameterAttribute[] commandParameters)
        {
            Dictionary<string, ParameterAttribute> cloneParameters = new Dictionary<string, ParameterAttribute>();
            foreach (var key in parentParameters.Keys)
                cloneParameters.Add(key, parentParameters[key]);

            foreach (var arg in commandParameters)
            {
                if (cloneParameters.ContainsKey(arg.Parameter))
                    throw new ParameterRepeatedException(arg.Parameter);

                cloneParameters.Add(arg.Parameter, arg);
            }

            return cloneParameters;
        }

        private Command InstantiateSingleCommand(Type type, CommonCommandAttribute commonCommandAttribute)
        {
            Command commandToCreate;

            if (commonCommandAttribute is GroupAttribute)
            {
                commandToCreate = new Group(type);
            }
            else // if (attr is Command Attribute)
            {
                commandToCreate = new Command(type);
                if (HasSubcommands(commandToCreate))
                    throw new CommandWithSubcommandsException(commandToCreate.CommandType);
            }

            return commandToCreate;
        }

        private void ValidateCommand(Type type, CommonCommandAttribute[] attr)
        {
            if (attr.Length > 1)
                throw new CommandGroupTogetherException(type);
            if (attr.Length == 0)
                throw new ICommandWithoutCommandAttributeException(type);
        }

        private void RunCommands(List<Command> commands, Dictionary<string, string> kwargs)
        {
            foreach (var cmd in commands)
            {
                ICommand instance = (ICommand)Activator.CreateInstance(cmd.CommandType);
                instance.Run(kwargs);
            }
        }

        private bool HasSubcommands(Command currentCommand)
        {
            var nestedTypes = currentCommand.CommandType.GetNestedTypes(BindingFlags.Public);
            foreach (var nested in nestedTypes) 
            {
                if (IsCommand(nested))
                    return true;
            }

            return false;
        }

        private bool IsCommand(Type type)
        {
            var attrs = Attribute.GetCustomAttributes(type, typeof(CommonCommandAttribute)) as CommonCommandAttribute[];
            return attrs.Length > 0;
        }

        private void CheckRepeatedCommand(Dictionary<string, byte> checkerForRepeatedCommands, Type type)
        {
            var names = (Attribute.GetCustomAttribute(type, typeof(CommonCommandAttribute)) as CommonCommandAttribute).Names;
            foreach (string name in names)
            {
                if (checkerForRepeatedCommands.ContainsKey(name))
                    throw new CommandRepeatedException(name);

                checkerForRepeatedCommands.Add(name, 0);
            }
        }

        private IEnumerable<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
                  assembly.GetTypes()
                          .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
        }

        #endregion
    }
}
