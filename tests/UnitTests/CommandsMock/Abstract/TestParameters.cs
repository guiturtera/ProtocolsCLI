using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;
using UnitTests.CommandsMock;

namespace UnitTests.TestParameters
{
    [Group("CONF", "CONFigure")]
    public class ConfigureGroup : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }

        [Group("PRES", "PRESsure")]
        [Argument("type", "help", 1)]
        [Option("option4", "help")]
        [Option("option5", "help")]
        public class PressGroup : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }

            [Command("ONE_ARGUMENT", DisableAttributeChain = true)]
            [Argument("one_argument", "help", 1)]
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
            [Option("option3", "help")]
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
        [Option("option", "help")]
        [Argument("table", "help", 1)]
        public class UnitGroup : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }

            [Command("OTHER")]
            [Option("option2", "help")]
            [Argument("other_param", "help", 1)]
            [Argument("other_param2", "help", 2)]
            [Argument("other_param3", "help", 3)]
            public class OtherCommand : ICommand
            {
                public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
                public void Run(Dictionary<string, string> kwargs)
                {
                    _mockComm.RunnedCommands.Add(this.GetType());
                    _mockComm.ArgumentsPassed = kwargs;
                }
            }

            [Command("special_command", DisableOptionChain=true)]
            [Option("special_option", "help")]
            public class OtherCommand2 : ICommand
            {
                public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
                public void Run(Dictionary<string, string> kwargs)
                {
                    _mockComm.RunnedCommands.Add(this.GetType());
                    _mockComm.ArgumentsPassed = kwargs;
                }
            }

            [Command("special_command2", DisableOptionChain = true, DisableAttributeChain = true)]
            [Option("special_option2", "help")]
            [Argument("special_attribute", "help", 1)]
            public class OtherCommand3 : ICommand
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
