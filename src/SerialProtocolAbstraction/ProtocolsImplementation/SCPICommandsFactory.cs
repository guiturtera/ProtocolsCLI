using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class SCPICommandsFactory : CommandsFactory
    {
        #region Singleton
        private static CommandsFactory _instance;
        private SCPICommandsFactory() { }
        internal static CommandsFactory GetInstance()
        {
            if (_instance == null)
                _instance = new SCPICommandsFactory();

            return _instance;
        }
        #endregion

        protected override string[] GetCommands(string fullCommand)
        {
            string command = fullCommand.Split(' ')[0];
            return command.Split(':');
        }

        // FOR NOW, SCPI WILL NOT IMPLEMENT OPTIONS.
        protected override Dictionary<string, string> GetParameters(string fullCommand, List<ArgumentAttribute> argumentsNeeded, List<OptionAttribute> optionsNeeded)
        {
            List<string> orderedArguments = GetOrderedArguments(fullCommand);
            ValidateReceivedArgs(argumentsNeeded.Count, orderedArguments.Count, fullCommand);

            Dictionary<string, string> arguments = new Dictionary<string, string>();
            for (int i = 0; i < orderedArguments.Count; i++)
            {
                arguments.Add(argumentsNeeded[i].Parameter, orderedArguments[i]);
            }

            return arguments;
        }

        private void ValidateReceivedArgs(int expectedLength, int resultLength, string fullCommand)
        {
            if (expectedLength > resultLength)
                throw new NotEnoughArgumentsException(fullCommand);
            if (expectedLength < resultLength)
                throw new TooManyArgumentsException(fullCommand);
        }

        private List<string> GetOrderedArguments(string fullCommand)
        {
            List<string> args = fullCommand.Split(' ').ToList();
            args.RemoveAt(0);
            return args;
        }
    }
}
