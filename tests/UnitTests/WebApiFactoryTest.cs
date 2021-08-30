using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnitTests.CommandsMock;
using SerialProtocolAbstraction;
using System.Reflection;
using UnitTests.WebApiCommands;

namespace UnitTests
{
    [TestFixture]
    public class WebApiFactoryTest
    {
        CommandsHandledMock _handler;
        CommandsFactory _factory;
        [SetUp]
        public void Init()
        {
            _factory = CommandsFactory.GetCommand(EnumProtocol.WebApi);
            _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.WebApiCommands");

            _handler = CommandsHandledMock.GetInstance();
        }

        [Test, Description("Asserts that given some commands, successfully calls them")]
        [TestCase("input.cgi?constructor_string=Pt-100", new Type[] { typeof(Input) })]
        [TestCase("input/rtd.cgi?type=Pt-100&table=ITS-90", new Type[] { typeof(Input2), typeof(Input2.RTD) })]
        [TestCase("input/rtd.cgi?type=Pt-100&table=ITS-90&wires=three", new Type[] { typeof(Input2), typeof(Input2.RTD) })]
        [TestCase("input/tc.cgi?type=TC-K&table=ITS-90", new Type[] { typeof(Input2), typeof(Input2.TC) })]
        public void T0001_ExecuteFactoryCommand(string fullCommand, Type[] types)
        {
            _factory.ExecuteCommand(fullCommand);

            foreach (var i in types)
                Assert.That(_handler.RunnedCommands.Contains(i));

            Assert.AreEqual(types.Length, _handler.RunnedCommands.Count);
        }

        [Test, Description("Asserts that throws a `MissingArgumentException` when less parameters are sent.")]
        [TestCase("input.cgi?")]
        [TestCase("input/rtd.cgi?table=ITS-90")]
        [TestCase("input/tc.cgi?type=TC-K")]
        public void T0002_ExecuteFactoryCommand(string fullCommand)
        {
            Assert.Throws<MissingArgumentException>(() => { _factory.ExecuteCommand(fullCommand); });
        }

        [Test, Description("Asserts that throws a `MissingArgumentException` when less parameters are sent.")]
        [TestCase("input.cgi?aux=some")]
        [TestCase("input/rtd.cgi?aux=some")]
        [TestCase("input/tc.cgi?aux=some")]
        public void T0003_ExecuteFactoryCommand(string fullCommand)
        {
            Assert.Throws<InvalidParameterException>(() => { _factory.ExecuteCommand(fullCommand); });
        }
        /*

        [Test, Description("Asserts that throws a NotEnoughArgumentsException when less parameters are sent.")]
        [TestCase("CONF:PRES")]
        [TestCase("CONF:UNIT")]
        [TestCase("CONF:UNIT:OTHER ITS-90")]
        [TestCase("CONF:PRES:ONE_ARGUMENT")]
        public void T0003_ExecuteFactoryCommand(string fullCommand)
        {
            Assert.Throws<NotEnoughArgumentsException>(() => { _factory.ExecuteCommand(fullCommand); });
        }

        [Test, Description("Asserts that throws a TooManyArgumentsException when more parameters than needed are sent.")]
        [TestCase("CONF:PRES 2 2")]
        [TestCase("CONF:UNIT Table 2")]
        [TestCase("CONF:UNIT:OTHER 2 2 2 2 2")]
        [TestCase("CONF 2")]
        [TestCase("CONF:PRES:ONE_ARGUMENT 55 55")]
        [TestCase("CONF:PRES:NO_ARGUMENT aa")]
        public void T0004_ExecuteFactoryCommand(string fullCommand)
        {
            Assert.Throws<TooManyArgumentsException>(() => { _factory.ExecuteCommand(fullCommand); });
        }

        [Test, Description("Asserts that throws a CommandNotFoundException when a command is invalid.")]
        [TestCase("AA")]
        [TestCase("CONF:OTHER")]
        [TestCase("CONF:INVALID COMMAND")]
        public void T0005_ExecuteFactoryCommand(string fullCommand)
        {
            Assert.Throws<CommandNotFoundException>(() => { _factory.ExecuteCommand(fullCommand); });
        }

        [Test, Description("Asserts that throws a AttributeWithoutDefinedOrderException when loading one.")]
        public void T0006_CreateCommandTree()
        {
            Assert.Throws<ArgumentWithoutDefinedOrderException>(() => { _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.AttributeWithoutDefinedOrderExceptionCommandsMockError"); });
        }

        [Test, Description("Asserts that throws a CommandGroupTogetherException when there's an ICommand implementation without CommonCommandAttribute.")]
        public void T0007_CreateCommandTree()
        {
            Assert.Throws<CommandGroupTogetherException>(() => { _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.CommandsMockGroupCommandTogether"); });
        }

        [Test, Description("Asserts that throws a ICommandWithoutCommandAttributeException when there's an ICommand implementation without CommonCommandAttribute.")]
        public void T0008_CreateCommandTree()
        {
            Assert.Throws<ICommandWithoutCommandAttributeException>(() => { _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.WithoutICommandInterfaceCommandsMockError"); });
        }

        [Test, Description("Asserts that throws a CommandRepeatedException when loading one.")]
        public void T0009_CreateCommandTree()
        {
            Assert.Throws<CommandRepeatedException>(() => { _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.RepeatedCommandsMockError"); });
        }

        [Test, Description("Asserts that throws a CommandWithSubcommandsException when loading one.")]
        public void T0010_CreateCommandTree()
        {
            Assert.Throws<CommandWithSubcommandsException>(() => { _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.SubcommandsCommandsMockError"); });
        }

        [Test, Description("Asserts that throws a CommandNotFoundException when a command is invalid.")]
        [TestCase("AA")]
        [TestCase("CONF:OTHER")]
        [TestCase("CONF:INVALID COMMAND")]
        public void T0011_ExecuteFactoryCommand(string fullCommand)
        {
            Assert.Throws<CommandNotFoundException>(() => { _factory.ExecuteCommand(fullCommand); });
        }

        [Test, Description("Asserts that throws a successfully returns the documented string.")]
        public void T0012_CreateDocumentation()
        {
            string result = _factory.CreateDocumentation(new DefaultDocumentationGenerator());
            Assert.AreNotEqual("", result);
        }
        */
        [TearDown]
        public void Dispose()
        {
            CommandsHandledMock.RestartCommand();
        }
    }
}
