using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;
using UnitTests.CommandsMock;

namespace UnitTests.WithoutICommandInterfaceCommandsMockError
{
    [Group("CONFigure", "CONF")]
    public class WithoutICommandInterfaceCommandsMockError : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }

        [Group("PRES", "PRESsure")]
        [Argument("type", "help for config <type>", 1)]
        public class PressGroup : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }
        }

        [Group("UNIT")]
        [Argument("table", "help for config <table>", 1)]
        public class UnitGroup : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }

            [Argument("other_param", "help for config <other_param>", 1)]
            public class OtherCommand : ICommand
            {
                public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
                public void Run(Dictionary<string, string> kwargs)
                {
                    _mockComm.RunnedCommands.Add(this.GetType());
                    _mockComm.ArgumentsPassed = kwargs;
                }
            }

        }

        [Command("HELLO")]
        public class Hello : ICommand
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
