using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SerialProtocolAbstraction;
using System.Reflection;
using UnitTests.CommandsMock;


namespace UnitTests
{
    [TestFixture]
    public class SCPIFactoryTest
    {
        CommandsHandledMock _handler;
        CommandsFactory _factory;
        [SetUp]
        public void Init() 
        {
            _factory = CommandsFactory.GetCommand(EnumProtocol.SCPI);
            _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.CommandsMock");

            _handler = CommandsHandledMock.GetInstance();
        }

        [Test, Description("Asserts that given some commands, successfully calls them")]
        [TestCase("CONF:UNIT:OTHER 1 2 3 4", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.UnitGroup), typeof(ConfigureGroup.UnitGroup.OtherCommand) })]
        [TestCase("CONF:PRES Pt-100", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.PressGroup) })]
        [TestCase("CONF:UNIT 1", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.UnitGroup) })]
        [TestCase("CONF:HELLO", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.Hello) })]
        [TestCase("CONF", new Type[] { typeof(ConfigureGroup) })]
        [TestCase("CONF:PRES:ONE_ARGUMENT 55",  new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.PressGroup), typeof(ConfigureGroup.PressGroup.OneArgumentCommand) })]
        [TestCase("CONF:PRES:NO_ARGUMENT", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.PressGroup), typeof(ConfigureGroup.PressGroup.NoArgumentCommand) })]
        public void T0001_ExecuteFactoryCommand(string fullCommand, Type[] types) 
        {
            _factory.ExecuteCommand(fullCommand);

            foreach (var i in types)
                Assert.That(_handler.RunnedCommands.Contains(i));

            Assert.AreEqual(types.Length, _handler.RunnedCommands.Count);
        }

        [Test, Description("Asserts that successfully pass the kwargs to commands.")]
        [TestCase("CONF", new string[] { }, new string[] { })]
        [TestCase("CONF:HELLO", new string[] { }, new string[] { })]
        [TestCase("CONF:PRES Pt-100", new string[] { "type" }, new string[] { "Pt-100" })]
        [TestCase("CONF:UNIT ITS-90", new string[] { "table" }, new string[] { "ITS-90" })]
        [TestCase("CONF:UNIT:OTHER 1 2 3 4", new string[] { "table", "other_param", "other_param2", "other_param3" }, new string[] { "1", "2", "3", "4" })]
        [TestCase("CONF:PRES:ONE_ARGUMENT 55", new string[] { "one_argument" }, new string[] { "55" })]
        [TestCase("CONF:PRES:NO_ARGUMENT", new string[] { }, new string[] { })]
        public void T0002_ExecuteFactoryCommand(string fullCommand, string[] expectedKeys, string[] expectedValues)
        {
            _factory.ExecuteCommand(fullCommand);
            _handler.ArgumentsPassed.Remove("_full_command");
            
            int expectedLength = expectedKeys.Length;
            Assert.AreEqual(expectedLength, _handler.ArgumentsPassed.Count);

            for (int i = 0; i < expectedLength; i++) 
            {
                Assert.IsTrue(_handler.ArgumentsPassed.ContainsKey(expectedKeys[i]));
                Assert.AreEqual(expectedValues[i], _handler.ArgumentsPassed[expectedKeys[i]]);
            }
        }

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

        [TearDown]
        public void Dispose() 
        {
            CommandsHandledMock.RestartCommand();
        }
    }
}
