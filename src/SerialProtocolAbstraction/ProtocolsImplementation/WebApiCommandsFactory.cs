using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialProtocolAbstraction
{
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

        protected override string[] GetCommands(string fullCommand)
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, string> GetParameters(string fullCommand, List<ArgumentAttribute> arguments, List<OptionAttribute> options)
        {
            throw new NotImplementedException();
        }
    }
}
