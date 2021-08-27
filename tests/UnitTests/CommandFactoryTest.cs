using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SerialProtocolAbstraction;
using System.Reflection;
using UnitTests.TestParameters;

namespace UnitTests
{

    [TestFixture]
    public class CommandFactoryTest
    {
        CommandsHandledMock _handler;
        CommandsFactoryMock _factory;
        [SetUp]
        public void Init()
        {
            _factory = new CommandsFactoryMock();
            _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "UnitTests.TestParameters");

            _handler = CommandsHandledMock.GetInstance();
        }

        [Test, Description("Asserts that throws a specific exception for the specified namespace.")]
        [TestCase("UnitTests.RepeatedParameterExceptionMock1", typeof(ParameterRepeatedException))]
        [TestCase("UnitTests.RepeatedParameterExceptionMock2", typeof(ParameterRepeatedException))]
        [TestCase("UnitTests.RepeatedParameterExceptionMock3", typeof(ParameterRepeatedException))]
        [TestCase("UnitTests.RepeatedParameterExceptionMock4", typeof(ParameterRepeatedException))]
        [TestCase("UnitTests.RepeatedParameterExceptionMock5", typeof(CommandGroupTogetherException))]
        [TestCase("UnitTests.RepeatedParameterExceptionMock6", typeof(ICommandWithoutCommandAttributeException))]
        [TestCase("UnitTests.RepeatedParameterExceptionMock7", typeof(CommandRepeatedException))]
        public void T0001_CreateCommandTree(string namespaceToSearch, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => { _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), namespaceToSearch); });
        }


        [Test, Description("Asserts that given some commands, successfully calls them")]
        [TestCase("CONF:UNIT:OTHER 1 2 3 4", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.UnitGroup), typeof(ConfigureGroup.UnitGroup.OtherCommand) })]
        [TestCase("CONF:PRES Pt-100", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.PressGroup) })]
        [TestCase("CONF:UNIT 1", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.UnitGroup) })]
        [TestCase("CONF:HELLO", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.Hello) })]
        [TestCase("CONF", new Type[] { typeof(ConfigureGroup) })]
        [TestCase("CONF:PRES:ONE_ARGUMENT 55", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.PressGroup), typeof(ConfigureGroup.PressGroup.OneArgumentCommand) })]
        [TestCase("CONF:PRES:NO_ARGUMENT", new Type[] { typeof(ConfigureGroup), typeof(ConfigureGroup.PressGroup), typeof(ConfigureGroup.PressGroup.NoArgumentCommand) })]
        public void T0002_ExecuteFactoryCommand(string fullCommand, Type[] types)
        {
            _factory.ExecuteCommand(fullCommand);

            foreach (var i in types)
                Assert.That(_handler.RunnedCommands.Contains(i));

            Assert.AreEqual(types.Length, _handler.RunnedCommands.Count);
        }

        [Test, Description("Asserts that delivers the correct Attributes and Options needed.")]
        [TestCase("CONF:UNIT:OTHER 1 2 3 4", new string[] { "table", "other_param", "other_param2", "other_param3" }, new string[] { "option", "option2" })]
        [TestCase("CONF:PRES Pt-100", new string[] { "type" }, new string[] { "option4", "option5" })]
        [TestCase("CONF:UNIT 1", new string[] { "table" }, new string[] { "option" })]
        [TestCase("CONF:HELLO", new string[] {  }, new string[] {  })]
        [TestCase("CONF", new string[] {  }, new string[] {  })]
        [TestCase("CONF:PRES:ONE_ARGUMENT 55", new string[] { "one_argument" }, new string[] { "option4", "option5" })]
        [TestCase("CONF:PRES:NO_ARGUMENT", new string[] { }, new string[] { "option3" })]
        [TestCase("CONF:UNIT:special_command table_argument", new string[] { "table" }, new string[] { "special_option" })]
        [TestCase("CONF:UNIT:special_command2 special_attribute_argument", new string[] { "special_attribute" }, new string[] { "special_option2" })]
        public void T0003_ExecuteFactoryCommand(string fullCommand, string[] argumentsExpected, string[] optionsExpected)
        {
            _factory.ExecuteCommand(fullCommand);

            string[] argsReceived = new string[_factory.ArgumentsNeeded.Count];
            for (int i = 0; i < _factory.ArgumentsNeeded.Count; i++)
                argsReceived[i] = _factory.ArgumentsNeeded[i].Parameter;

            string[] optsReceived = new string[_factory.OptionsNeeded.Count];
            for (int i = 0; i < _factory.OptionsNeeded.Count; i++)
                optsReceived[i] = _factory.OptionsNeeded[i].Parameter;

            foreach (var arg in argumentsExpected) 
            {
                Assert.That(argsReceived.Contains(arg), "Arguments Expected = " + string.Join("|", argumentsExpected) +
                    "\n  Received = " + string.Join("|", argsReceived));
            }
            foreach (var opt in optionsExpected)
            {
                Assert.That(optsReceived.Contains(opt), "Options Expected = " + string.Join("|", optionsExpected) +
                    "\n  OptionsReceived = " + string.Join("|", optsReceived));
            }
        }

        [Test, Description("Asserts that given some commands, throws a specifc exception")]
        [TestCase("CONF:PRES UNABLE@ARGUMENTS", typeof(MissingArgumentException))]
        [TestCase("CONF:UNIT UNABLE@ARGUMENTS", typeof(MissingArgumentException))]
        [TestCase("CONF:PRES:ONE_ARGUMENT UNABLE@ARGUMENTS", typeof(MissingArgumentException))]
        [TestCase("SOME:COMMAND:NOT:TO:BE:FOUND", typeof(CommandNotFoundException))]
        [TestCase("AAAA", typeof(CommandNotFoundException))]
        [TestCase("CONF:PRES:ONE_ARGUMENT:AAA", typeof(CommandNotFoundException))]
        [TestCase("CONF:NOTFOUND", typeof(CommandNotFoundException))]
        public void T0004_ExecuteFactoryCommand(string fullCommand, Type exceptionType)
        {
            var ex = Assert.Throws(exceptionType, () => { _factory.ExecuteCommand(fullCommand); });
        }


        [Test, Description("Asserts that given some commands, throws a `MissingArgumentException` exception with specific missing argument")]
        [TestCase("CONF:PRES UNABLE@ARGUMENTS", "type")]
        [TestCase("CONF:UNIT UNABLE@ARGUMENTS", "table")]
        [TestCase("CONF:PRES:ONE_ARGUMENT UNABLE@ARGUMENTS", "one_argument")]
        public void T0005_ExecuteFactoryCommand(string fullCommand, string missingArgumentExpected)
        {
            var ex = Assert.Throws<MissingArgumentException>(() => { _factory.ExecuteCommand(fullCommand); });

            string expected = new MissingArgumentException(missingArgumentExpected).Message;
            Assert.AreEqual(expected, ex.Message);
        }

        [Test, Description("Asserts that given some commands, throws a `FactoryCommandTreeNotCreatedException` when the factory is not started.")]
        public void T0006_ExecuteFactoryCommand()
        {
            CommandsFactoryMock _fac = new CommandsFactoryMock();
            var ex = Assert.Throws<FactoryCommandTreeNotCreatedException>(() => { _fac.ExecuteCommand("SOME:COMMAND"); });
        }

        [TearDown]
        public void Dispose()
        {
            CommandsHandledMock.RestartCommand();
        }
    }
}
