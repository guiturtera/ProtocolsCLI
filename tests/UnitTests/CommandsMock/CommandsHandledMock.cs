using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public class CommandsHandledMock
    {
        #region Singleton
        private CommandsHandledMock() { }
        private static CommandsHandledMock _instance;
        public static CommandsHandledMock GetInstance()
        {
            if (_instance == null)
                _instance = new CommandsHandledMock();

            return _instance;
        }
        #endregion

        public static void RestartCommand()
        {
            _instance = new CommandsHandledMock();
        }

        public List<Type> RunnedCommands = new List<Type>();

        public Dictionary<string, string> ArgumentsPassed = new Dictionary<string, string>();
    }
}
