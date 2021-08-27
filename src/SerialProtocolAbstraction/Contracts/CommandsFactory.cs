using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SerialProtocolAbstraction
{
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

        public Command[] CreateCommandTree(Assembly assembly, string nameSpace)
        {
            Type[] types = GetTypesInNamespace(assembly, nameSpace).Where(t => t.IsNestedPublic == false).ToArray();
            AvailableCommands = CreateMultipleCommands(types);
            return AvailableCommands;
        }

        protected abstract string[] GetCommands(string fullCommand);
        protected abstract Dictionary<string, string> GetParameters(string fullCommand, List<ArgumentAttribute> arguments, List<OptionAttribute> options);

        public string CreateDocumentation(IDocumentationGenerator generator)
        {
            return generator.CreateDocumentation(AvailableCommands);
        }

        public void ExecuteCommand(string fullCommand)
        {
            fullCommand = fullCommand.Trim();
            string[] rawCommands = GetCommands(fullCommand); // must be override

            List<Command> commands = ConvertRawCommands(rawCommands); // don't need override
            List<ArgumentAttribute> argumentsNeeded = GetArgumentsNeeded(commands); // don't need override
            List<OptionAttribute> optionsNeeded = GetOptionsNeeded(commands);

            Dictionary<string, string> kwargs = GetParameters(fullCommand, argumentsNeeded, optionsNeeded); // need override
            CheckAllArguments(argumentsNeeded, kwargs);

            kwargs.Add("_full_command", fullCommand);

            RunCommands(commands, kwargs);
            
            //GetCommands(out commands, out parameters, string fullCommand, string delimCommand, string delimParam, bool paramsInFinal);
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

        private List<OptionAttribute> GetOptionsNeeded(List<Command> commands)
        {
            List<OptionAttribute> options = new List<OptionAttribute>();
            foreach (var cmd in commands)
            {
                if (cmd.CommandData.DisableOptionChain)
                    options.Clear();

                options.AddRange(cmd.Options);
            }
            return options;
        }

        private List<ArgumentAttribute> GetArgumentsNeeded(List<Command> commands)
        {
            List<ArgumentAttribute> arguments = new List<ArgumentAttribute>();
            foreach (var cmd in commands)
            {
                if (cmd.CommandData.DisableAttributeChain)
                    arguments.Clear();

                arguments.AddRange(cmd.Arguments);
            }
            return arguments;
        }

        private void RunCommands(List<Command> commands, Dictionary<string, string> kwargs)
        {
            foreach (var cmd in commands)
            {
                ICommand instance = (ICommand)Activator.CreateInstance(cmd.CommandType);
                instance.Run(kwargs);
            }
        }

        private bool CommandExists(Command[] commands, string currentCommand, out Command commandFound)
        {
            foreach (var command in commands)
            {
                foreach (string name in command.CommandData.Names)
                {
                    if (name == currentCommand)
                    {
                        commandFound = command;
                        return true;
                    };
                }
            }

            commandFound = null;
            return false;
        }

        private List<Command> ConvertRawCommands(string[] commands)
        {
            if (AvailableCommands == null)
                throw new FactoryCommandTreeNotCreatedException();

            List<Command> commandQueue = new List<Command>();
            Command currentCommand;
            Command[] nestedCommands = AvailableCommands;

            for (int i = 0; i < commands.Length; i++)
            {
                string currentCommandName = commands[i];
                CommandExists(nestedCommands, currentCommandName, out currentCommand);

                bool exists = CommandExists(nestedCommands, currentCommandName, out currentCommand);
                if (exists && !(currentCommand.CommandData is CommandAttribute && i != commands.Length - 1))
                    commandQueue.Add(currentCommand);
                else
                    throw new CommandNotFoundException(string.Join(" - ", commands), currentCommandName);

                if (currentCommand is Group)
                    nestedCommands = (currentCommand as Group).NestedCommands;

            }

            return commandQueue;
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
