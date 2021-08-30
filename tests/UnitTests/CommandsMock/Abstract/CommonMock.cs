using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialProtocolAbstraction;
using UnitTests;

namespace UnitTest.CommonCommandsMock
{
    [Argument("type", "help", 1)]
    [Option("option4", "help")]
    [Option("option5", "help")]
    public class Press : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }
    }

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
    }

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

    public class Hello : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }
    }

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

    [Group("CONF", "CONFigure")]
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

namespace UnitTests.TestParameters2
{
    [Group("CONF", "CONFigure")]
    public class ConfigureGroup : UnitTest.CommonCommandsMock.ConfigureGroup
        {

        [Group("PRES", "PRESsure")]
        public class PressGroup : UnitTest.CommonCommandsMock.Press
        {
            [Command("ONE_ARGUMENT", DisableAttributeChain = true)]
            public class OneArgumentCommand : UnitTest.CommonCommandsMock.OneArgumentCommand { }

            [Command("NO_ARGUMENT", DisableAttributeChain = true)]
            public class NoArgumentCommand : UnitTest.CommonCommandsMock.NoArgumentCommand { }

        }

        [Group("UNIT")]
        public class UnitGroup : UnitTest.CommonCommandsMock.UnitGroup
        {
            [Command("OTHER")]
            public class OtherCommand : UnitTest.CommonCommandsMock.OtherCommand { }

            [Command("special_command", DisableOptionChain = true)]
            public class OtherCommand2 : UnitTest.CommonCommandsMock.OtherCommand2 { }

            [Command("special_command2", DisableOptionChain = true, DisableAttributeChain = true)]
            public class OtherCommand3 : UnitTest.CommonCommandsMock.OtherCommand3 { }
        }

        [Command("HELLO")]
        public class Hello : UnitTest.CommonCommandsMock.Hello { }
    }
}
