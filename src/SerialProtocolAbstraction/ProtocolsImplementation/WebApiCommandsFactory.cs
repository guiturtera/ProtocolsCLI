using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialProtocolAbstraction
{
    /// <summary>
    /// It Receives the filename, suppose it is a command, and the key/values as arguments and options.
    /// Example: input.cgi?some_key=some_value&... 
    /// NOTE: The query string must already be parsed to UTF-8.
    /// 
    /// Exceptions:
    /// InvalidParameterException
    /// </summary>
    public class WebApiCommandsFactory : CommandsFactory
    {
        #region Singleton
        private static CommandsFactory _instance;
        private WebApiCommandsFactory() { }
        internal static CommandsFactory GetInstance()
        {
            if (_instance == null)
                _instance = new WebApiCommandsFactory();

            return _instance;
        }
        #endregion

        public override string CreateDocumentation()
        {
            var generator = new WebApiDocumentation();
            return generator.CreateDocumentation(AvailableCommands);
        }

        protected override string[] GetCommands(string fullCommand)
        {
            string rawCommands = fullCommand.Split('?')[0];
            return rawCommands.Split('/');
        }

        protected override Dictionary<string, string> GetParameters(string fullCommand, List<ArgumentAttribute> arguments, List<OptionAttribute> options)
        {
            var parameters = new Dictionary<string, string>();
            string raw_key_values = fullCommand.Split('?')[1];
            if (raw_key_values != "") 
            {
                string[] key_values = raw_key_values.Split('&');
                foreach (string key_value in key_values)
                {
                    string[] pair = key_value.Split('=');
                    string key = pair[0];
                    string value = pair[1];

                    parameters.Add(key, value);

                    if (!arguments.Exists(i => i.Parameter == key) &&
                        !options.Exists(i => i.Parameter == key))
                        throw new InvalidParameterException(key);
                }
            }


            return parameters;
        }

    }
}
