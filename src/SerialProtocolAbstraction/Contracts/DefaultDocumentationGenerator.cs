using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public abstract class DefaultDocumentationGenerator
    {
        private CommandsExplorer _explorer;
        /// <summary>
        /// Documents the webapi, changing the format of the command.
        /// Will iterate, and for every command that has a HELP, will send all of it's features, 
        /// and document it when CreateDocumentation is called.
        /// </summary>
        protected abstract string GetCommandLine(List<Command> commandChain, List<ArgumentAttribute> arguments, List<OptionAttribute> options);

        public string CreateDocumentation(Command[] rootCommands)
        {
            _explorer = new CommandsExplorer(rootCommands);
            StringBuilder builder = new StringBuilder();
            builder.Append("<argument> -> A parameter between <> is an ARGUMENT.\n" +
            "Arguments must be sent. Otherwise, the command will not work.\n\n");

            builder.Append("(option) -> A parameter between () is an OPTION\n"
                + "You can choose either to send or not an option.\n\n");
            builder.Append("---------------------------------------");
            builder.AppendLine();
            builder.AppendLine("START OF DOCUMENTATION:\n");

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
                builder.Append(GetCommandLine(command, parentCommands));
                builder.AppendLine();
                builder.AppendLine(String.Format("Description -> {0}", command.CommandData.Help));

                foreach (var parameter in command.Parameters)
                    builder.Append(GetParameterLine(parameter, depth));

                builder.AppendLine();
            }

            if (command is Group)
            {
                parentCommands.Add(command);
                var nestedCommands = (command as Group).NestedCommands;
                foreach (var cmd in nestedCommands)
                {
                    builder.Append(GetDocumentedCommand(cmd, depth + 1, parentCommands));
                }
                parentCommands.Remove(command);
            }

            return builder.ToString();
        }

        private string GetCommandLine(Command command, List<Command> parentCommands)
        {
            StringBuilder builder = new StringBuilder();

            List<Command> commands = GetCommandsCopy(command, parentCommands);

            List<ArgumentAttribute> arguments = _explorer.GetArgumentsForACommandChain(commands);
            List<OptionAttribute> options = _explorer.GetOptionsForACommandChain(commands);

            return GetCommandLine(commands, arguments, options);
        }

        private List<Command> GetCommandsCopy(Command command, List<Command> parentCommands)
        {
            var commands = new List<Command>();
            parentCommands.ForEach(i => commands.Add(i));
            commands.Add(command);
            return commands;
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


        /*
        public string CreateDocumentation(Command[] rootCommands)
        {
            StringBuilder builder = new StringBuilder();
            
            builder.AppendLine("[] -> Command");
            builder.AppendLine("<> -> Parameter");
            builder.AppendLine("() -> Option");
            builder.AppendLine("{} -> Available Parameters/Options to send");
            builder.AppendLine("| -> Multiple Options");
            builder.AppendLine();

            builder.AppendLine("QueryString format: -> `url/[command].cgi?<parameter>={available-parameter}&<option>={available-option}`");
            builder.AppendLine("Example:");
            builder.AppendLine("https://presys.com.br/main.cgi?param1=some_value&param2=some_value&option1=some_value");
            builder.AppendLine("MAIN ");
            builder.AppendLine("Where `param1 and param2` keys are values that MUST be passes in order of the command work.");
            builder.AppendLine("Where `main.cgi` is the command");
            builder.AppendLine("Where `option1`, as the name says, you can choose between sending or no. It's your choice (optional)");
            builder.AppendLine("## IMPORTANT: HTTP does not allow you to send chain of commands! ##");
            builder.AppendLine();
            builder.Append("SCPI format: -> `[command]:[command] <parameter>`");
            builder.AppendLine("Example:");
            builder.AppendLine("CONF:CHAN 2");
            builder.AppendLine("Where `CONF` is the first command, and CHAN is the second.");
            builder.AppendLine("Where 2 is the parameter that is needed for the command to work.");
            builder.AppendLine("## IMPORTANT: SCPI does not allow you to send OPTIONS! ##");
            builder.AppendLine("-------------------------");
            builder.AppendLine();
            foreach (var command in rootCommands)
            {
                builder.AppendLine(GetDocumentedCommand(command, 0));
            }

            return builder.ToString();
        }

        private string GetDocumentedCommand(Command command, int depth)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("{0}{1}", GetTabStrings(depth), GetCommandLine(command));
            foreach (var parameter in command.Parameters)
                builder.Append(GetParameterLine(parameter, depth));

            if (command is Group)
            {
                var nestedCommands = (command as Group).NestedCommands;
                foreach (var cmd in nestedCommands)
                {
                    builder.AppendLine(GetDocumentedCommand(cmd, depth + 1));
                }
            }

            return builder.ToString();
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

            builder.AppendLine();

            return builder.ToString();
        }

        private string GetTabStrings(int tabAmount)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tabAmount; i++)
                builder.Append("\t");
            return builder.ToString();
        }

        private string GetCommandLine(Command command)
        {
            StringBuilder builder = new StringBuilder();
            var arguments = command.Arguments;
            var options = command.Options;

            builder.Append("[");
            builder.Append(string.Join("|", command.CommandData.Names.ToArray()));
            builder.Append("]");

            foreach (var opt in options)
                builder.AppendFormat(" ({0})", opt.Parameter);

            foreach (var arg in arguments)
                builder.AppendFormat(" <{0}>", arg.Parameter);

            if (command.CommandData.Help != "")
                builder.AppendFormat(" -> {0}", command.CommandData.Help);

            builder.AppendLine();
            return builder.ToString();
        }
    }*/
}
