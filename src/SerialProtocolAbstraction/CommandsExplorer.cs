using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class CommandsExplorer
    {
        public Command[] Commands { get; private set; }
        public CommandsExplorer(Command[] commands) 
        {
            Commands = commands;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands">A list of parsed commands</param>
        /// <returns></returns>
        public List<Command> GetCommandListByString(string[] commands) 
        {
            if (Commands == null)
                throw new FactoryCommandTreeNotCreatedException();

            List<Command> commandQueue = new List<Command>();
            Command currentCommand;
            Command[] nestedCommands = Commands;

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

        public List<ArgumentAttribute> GetArgumentsForACommandChain(List<Command> commands)
        {
            List<ArgumentAttribute> arguments = new List<ArgumentAttribute>();
            foreach (var cmd in commands)
            {
                if (cmd.CommandData.DisableAttributeChain)
                    arguments.Clear();

                arguments.AddRange(GetArguments(cmd.CommandType, new List<ArgumentAttribute>()));
            }
            return arguments;
        }

        public List<OptionAttribute> GetOptionsForACommandChain(List<Command> commands)
        {
            List<OptionAttribute> arguments = new List<OptionAttribute>();
            foreach (var cmd in commands)
            {
                if (cmd.CommandData.DisableOptionChain)
                    arguments.Clear();

                arguments.AddRange(GetOptions(cmd.CommandType, new List<OptionAttribute>()));
            }
            return arguments;
        }

        private bool IsInheritedClass(Type typeToCheck)
        {
            return typeToCheck.BaseType != typeof(object);
        }

        private List<OptionAttribute> GetOptions(Type typeToGet, List<OptionAttribute> options)
        {
            OptionAttribute[] attrs = typeToGet.GetCustomAttributes(typeof(OptionAttribute), false) as OptionAttribute[];
            options.AddRange(attrs);
            if (IsInheritedClass(typeToGet))
                return GetOptions(typeToGet.BaseType, options);

            return options;
        }

        private List<ArgumentAttribute> GetArguments(Type typeToGet, List<ArgumentAttribute> arguments)
        {
            ArgumentAttribute[] attrs = typeToGet.GetCustomAttributes(typeof(ArgumentAttribute), false) as ArgumentAttribute[];
            arguments.AddRange(attrs);
            if (IsInheritedClass(typeToGet))
                return GetArguments(typeToGet.BaseType, arguments);

            return arguments;
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
    }
}
