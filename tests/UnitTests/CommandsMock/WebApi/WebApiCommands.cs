using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;
using UnitTests.CommandsMock;

namespace UnitTests.WebApiCommands
{
    [Command("input")]
    [Argument("table", "help for <table> argument", 1)]
    [Argument("type", "help for <type> argument", 2)]
    [Option("wires", "help for <wires> option")]
    public class ConfigureGroup : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }
    }
}
