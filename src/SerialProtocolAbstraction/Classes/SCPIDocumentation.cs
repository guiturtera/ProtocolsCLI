using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class SCPIDocumentation : IDocumentationGenerator
    {
        public string CreateDocumentation(Command[] rootCommands)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var command in rootCommands)
            {
                builder.AppendLine(GetDocumentedCommand(command, 0, new List<Command>()));
            }

            return builder.ToString();
        }

        private string GetDocumentedCommand(Command command, int depth, List<Command> parentCommands)
        {
            StringBuilder builder = new StringBuilder();
            if (command.CommandData.Help != "")
            {
                builder.AppendFormat("{0}{1}", AppendListOfCommands(parentCommands, ":"), GetCommandLine(command));
                builder.AppendLine();
                foreach (var parameter in command.Parameters)
                  builder.Append(GetParameterLine(parameter, depth));
            }

            if (command is Group)
            {
                parentCommands.Add(command);
                var nestedCommands = (command as Group).NestedCommands;
                foreach (var cmd in nestedCommands)
                {
                    builder.AppendLine(GetDocumentedCommand(cmd, depth + 1, parentCommands));
                }
                parentCommands.Add(command);
            }

            return builder.ToString();
        }

        private string AppendListOfCommands(List<Command> parentCommands, string separator)
        {
            if (parentCommands.Count == 0) return "";

            StringBuilder builder = new StringBuilder();

            foreach (var parentCommand in parentCommands)
            {
                builder.Append(GetCommandSCPIFormat(parentCommand));
                builder.Append(separator);
            }

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

        private string GetCommandLine(Command command)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(" ");
            foreach (var argument in command.Arguments)
            {
                builder.Append("{<0>} ").Replace("0", argument.Parameter);
            }

            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        private string GetParameterLine(ParameterAttribute parameter, int depth)
        {
            StringBuilder builder = new StringBuilder();
            string format = parameter is ArgumentAttribute ? "<{0}>" : "({0})";


            builder.Append(GetTabStrings(depth + 1));
            builder.AppendFormat(format, parameter.Parameter);
            builder.AppendLine();
            builder.Append(GetTabStrings(depth + 2));

            builder.Append("Help -> ");
            builder.AppendLine(parameter.Help);
            if (parameter.OptionsHelp.Length > 1)
            {
                builder.Append(GetTabStrings(depth + 2));
                builder.Append("Parameters -> {");
                builder.Append(string.Join("|", parameter.OptionsHelp));
                builder.AppendLine("}");
            }


            return builder.ToString();
        }


        private string GetTabStrings(int tabAmount)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tabAmount; i++)
                builder.Append("\t");
            return builder.ToString();
        }
    }
}
