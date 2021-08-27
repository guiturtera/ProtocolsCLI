using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;
using UnitTests.CommandsMock;

namespace UnitTests.SubcommandsCommandsMockError
{
    [Group("CONFigure", "CONF")]
    public class ConfigureGroup : ICommand
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
        public class UnitGroup : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }

            [Command("OTHER")]
            public class OtherCommand : ICommand
            {
                public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
                public void Run(Dictionary<string, string> kwargs)
                {
                    _mockComm.RunnedCommands.Add(this.GetType());
                    _mockComm.ArgumentsPassed = kwargs;
                }

                [Command("OTHER2")]
                public class OtherCommand2 : ICommand
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
