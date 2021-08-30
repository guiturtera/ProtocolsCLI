using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class WebApiDocumentation : DefaultDocumentationGenerator
    {
        protected override string GetCommandLine(List<Command> commandChain, List<ArgumentAttribute> arguments, List<OptionAttribute> options)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<url>/<root_directory>/");
            builder.Append(GetCommands(commandChain));
            builder.Append("?");
            builder.Append(GetArguments(arguments));

            if (arguments.Count > 0 && options.Count > 0)
                builder.Append("&");

            builder.Append(GetOptions(options));

            return builder.ToString();
        }

        private string GetCommands(List<Command> commandChain)
        {
            StringBuilder builder = new StringBuilder();

            commandChain.ForEach(i => builder.AppendFormat("{0}/", i.CommandData.Names[0]));
            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        private string GetArguments(List<ArgumentAttribute> arguments)
        {
            if (arguments.Count == 0) return "";

            StringBuilder builder = new StringBuilder();

            arguments.ForEach(i => builder.AppendFormat("{0}=<{1}>&", i.Parameter, i.Parameter));

            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        private string GetOptions(List<OptionAttribute> options)
        {
            if (options.Count == 0) return "";

            StringBuilder builder = new StringBuilder();

            options.ForEach(i => builder.AppendFormat("{0}=({1})&", i.Parameter, i.Parameter));
            
            return builder.Remove(builder.Length - 1, 1).ToString();
        }
    }
}
