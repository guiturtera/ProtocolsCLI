using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;
using UnitTests.CommandsMock;

namespace UnitTests.WebApiCommands
{
    [Command("input.cgi")]
    [Argument("constructor_string", "help for <constructor string> option", 1)]
    public class Input : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }
    }

    [Group("input")]
    public class Input2 : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }

        [Command("rtd.cgi")]
        [Argument("table", "help for <table> argument", 1)]
        [Argument("type", "help for <type> argument", 2)]
        [Option("wires", "help for <wires> option")]
        public class RTD : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }
        }

        [Command("tc.cgi")]
        [Argument("table", "help for <table> argument", 1)]
        [Argument("type", "help for <type> argument", 2)]
        public class TC : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }
        }
    }
}
