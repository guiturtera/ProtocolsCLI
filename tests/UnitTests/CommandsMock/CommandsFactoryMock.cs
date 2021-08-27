using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;

namespace UnitTests
{
    internal class CommandsFactoryMock : CommandsFactory
    {
        // COMMAND1:COMMAND2:COMMAND3 ARG1:ARG2:ARG3 OPT1=OPTION1|OPT2=OPTION2|OPT3=OPTION3
        protected override string[] GetCommands(string fullCommand)
        {
            string command = fullCommand.Split(' ')[0];
            return command.Split(':');
        }

        public List<ArgumentAttribute> ArgumentsNeeded;
        public List<OptionAttribute> OptionsNeeded;
        protected override Dictionary<string, string> GetParameters(string fullCommand, List<ArgumentAttribute> argumentsNeeded, List<OptionAttribute> optionsNeeded)
        {
            if (fullCommand.Contains("UNABLE@ARGUMENTS"))
                return new Dictionary<string, string>();

            List<string> orderedArguments = GetOrderedArguments(fullCommand);

            Dictionary<string, string> arguments = new Dictionary<string, string>();
            for (int i = 0; i < orderedArguments.Count; i++)
            {
                arguments.Add(argumentsNeeded[i].Parameter, orderedArguments[i]);
            }

            ArgumentsNeeded = argumentsNeeded;
            OptionsNeeded = optionsNeeded;

            return arguments;

        }

        private List<string> GetOrderedArguments(string fullCommand)
        {
            List<string> args = fullCommand.Split(' ').ToList();
            args.RemoveAt(0);
            return args;
        }
    }

}
