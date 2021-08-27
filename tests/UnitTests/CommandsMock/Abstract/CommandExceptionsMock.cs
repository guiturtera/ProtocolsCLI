using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;
using UnitTests.CommandsMock;

namespace UnitTests.RepeatedParameterExceptionMock1
{
    [Group("CONF", "CONFigure")]
    [Option("opt", "help for opt")]
    public class ConfigureGroup : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }

        [Command("ANOTHER COMMAND", "COMMAND")]
        [Option("opt", "help for opt")]
        public class AnotherCommand : ICommand
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

namespace UnitTests.RepeatedParameterExceptionMock2
{
    [Group("CONF", "CONFigure")]
    [Argument("arg", "help for arg", 1)]
    public class ConfigureGroup : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }

        [Command("ANOTHER COMMAND", "COMMAND")]
        [Argument("arg", "help for arg", 2)]
        public class AnotherCommand : ICommand
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

namespace UnitTests.RepeatedParameterExceptionMock3
{
    [Command("CONF", "CONFigure")]
    [Argument("arg", "help for arg", 1)]
    [Argument("arg", "help for arg", 2)]
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

namespace UnitTests.RepeatedParameterExceptionMock4
{
    [Command("CONF", "CONFigure")]
    [Option("arg", "help for arg")]
    [Option("arg", "help for arg")]
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

namespace UnitTests.RepeatedParameterExceptionMock5
{
    [Command("CONF")]
    [Group("CONFigure")]
    [Option("arg", "help for arg")]
    [Option("arg", "help for arg")]
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

namespace UnitTests.RepeatedParameterExceptionMock6
{
    [Option("arg", "help for arg")]
    [Option("arg", "help for arg")]
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

namespace UnitTests.RepeatedParameterExceptionMock7
{
    [Group("CONF")]
    [Argument("arg", "help for arg", 1)]
    public class ConfigureGroup : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }

        [Command("SOME", "COMMAND")]
        public class AnotherCommand : ICommand
        {
            public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
            public void Run(Dictionary<string, string> kwargs)
            {
                _mockComm.RunnedCommands.Add(this.GetType());
                _mockComm.ArgumentsPassed = kwargs;
            }
        }
    }

    [Command("CONF")]
    public class AnotherConfigureGroup : ICommand
    {
        public CommandsHandledMock _mockComm = CommandsHandledMock.GetInstance();
        public void Run(Dictionary<string, string> kwargs)
        {
            _mockComm.RunnedCommands.Add(this.GetType());
            _mockComm.ArgumentsPassed = kwargs;
        }
    }
}