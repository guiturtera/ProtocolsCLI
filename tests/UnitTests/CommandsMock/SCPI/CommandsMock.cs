using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;

namespace UnitTests.CommandsMock
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
        [Argument("type", "help for config <type>", 1, new string[] { "op1", "op2" })]
        public class PressGroup : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }

            [Command("ONE_ARGUMENT", DisableAttributeChain=true)]
            [Argument("one_argument", "help for config <one_argument>", 1)]
            public class OneArgumentCommand : ICommand
            {
                public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
                public void Run(Dictionary<string, string> kwargs)
                {
                    _mockComm.RunnedCommands.Add(this.GetType());
                    _mockComm.ArgumentsPassed = kwargs;
                }
            }

            [Command("NO_ARGUMENT", DisableAttributeChain = true)]
            public class NoArgumentCommand : ICommand
            {
                public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
                public void Run(Dictionary<string, string> kwargs)
                {
                    _mockComm.RunnedCommands.Add(this.GetType());
                    _mockComm.ArgumentsPassed = kwargs;
                }
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

            [Command("OTHER")]
            [Argument("other_param", "help for config <other_param>", 1)]
            [Argument("other_param2", "help for config <other_param2>", 2)]
            [Argument("other_param3", "help for config <other_param3>", 3)]
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
