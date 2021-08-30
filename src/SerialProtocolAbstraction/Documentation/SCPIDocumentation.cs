using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class SCPIDocumentation : DefaultDocumentationGenerator
    {
        protected override string GetCommandLine(List<Command> commandChain, List<ArgumentAttribute> arguments, List<OptionAttribute> options)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(GetCommands(commandChain));
            builder.Append(" ");
            builder.Append(GetArguments(arguments));
            // SCPI DOES NOT HAVE OPTIONS HERE.

            return builder.ToString();
        }

        private string GetCommands(List<Command> commandChain)
        {
            StringBuilder builder = new StringBuilder();
            commandChain.ForEach(command => builder.AppendFormat("{0}:", GetCommandSCPIFormat(command)));

            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        private string GetArguments(List<ArgumentAttribute> arguments)
        {
            if (arguments.Count < 1) return "";

            StringBuilder builder = new StringBuilder();
            arguments.ForEach(arg => builder.Append("{<0>} ".Replace("0", arg.Parameter)));

            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        private string GetCommandSCPIFormat(Command command)
        {
            List<string> names = command.CommandData.Names;
            if (names.Count == 1)
            {
                return CheckUpperCase(names[0], command);
            }
            else if (names.Count == 2)
            {
                if (names[0].Length == names[1].Length)
                    throw new OutsideSCPIFormatException(command);
                else if (names[0].Length > names[1].Length)
                    return CheckStringContains(names[1], names[0], command);
                else
                    return CheckStringContains(names[0], names[1], command);
            }
            else throw new OutsideSCPIFormatException(command);
        }

        private string CheckStringContains(string shorterName, string longerName, Command errorCommand)
        {
            if (!longerName.Contains(shorterName))
                throw new OutsideSCPIFormatException(errorCommand);

            return longerName;
        }

        private string CheckUpperCase(string name, Command errorCommand)
        {
            foreach (char letter in name)
            {
                if (!char.IsUpper(letter))
                    throw new OutsideSCPIFormatException(errorCommand);
            }
            return name;
        }
    }
}
