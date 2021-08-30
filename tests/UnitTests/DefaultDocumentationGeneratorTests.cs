using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SerialProtocolAbstraction;
using System.Reflection;

namespace UnitTests
{
    [TestFixture]
    public class DefaultDocumentationGeneratorTests
    {
        private DocumentGeneratorMock docGenerator;
        private CommandsFactory _factory;
        [SetUp]
        public void Init() 
        {
            _factory = new CommandsFactoryMock();
            docGenerator = new DocumentGeneratorMock();
        }

        [Test, Description("Asserts that given some commands, successfully pass the parameters to `GetCommandLine`")]
        [TestCase("UnitTests.DocumentationTest1", new string[] { "GP1", "Hello World" }, new string[] { "first_argument", "second_argument" }, new string[] { "first_option", "second_option" })]
        [TestCase("UnitTests.DocumentationTest2", new string[] { "GP1", "Hello World" }, new string[] { "second_argument" }, new string[] { "first_option", "second_option" })]
        [TestCase("UnitTests.DocumentationTest3", new string[] { "GP1", "Hello World" }, new string[] { "first_argument", "second_argument" }, new string[] { "second_option" })]
        [TestCase("UnitTests.DocumentationTest4", new string[] { "GP1", "Hello World" }, new string[] { "second_argument" }, new string[] { "second_option" })]
        [TestCase("UnitTests.DocumentationTest5", new string[] { "GP1" }, new string[] { "first_argument" }, new string[] { "first_option" })]
        public void T0001_CreateDocumentation(string namespaceToSearch, string[] commandsExpected, string[] argumentsExpected, string[] optionsExpected)
        {
            _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), namespaceToSearch);
            var asserter = new EventHandler<EventArgs>((object sender, EventArgs e) =>
            {
                AssertCommands(commandsExpected);
                AssertArguments(argumentsExpected);
                AssertOptions(optionsExpected);
            });

            docGenerator.CommandPassed += new EventHandler<EventArgs>(asserter);

            docGenerator.CreateDocumentation(_factory.AvailableCommands);

            docGenerator.CommandPassed -= new EventHandler<EventArgs>(asserter);
        }

        private void AssertOptions(string[] optionsExpected)
        {
            List<string> options = new List<string>();
            foreach (var opt in docGenerator.Options)
            {
                options.Add(opt.Parameter);
            }

            Assert.AreEqual(options.ToArray(), optionsExpected);
        }

        private void AssertArguments(string[] argumentsExpected)
        {
            List<string> arguments = new List<string>();
            foreach (var arg in docGenerator.Arguments)
            {
                arguments.Add(arg.Parameter);
            }

            Assert.AreEqual(arguments.ToArray(), argumentsExpected);
        }

        private void AssertCommands(string[] commandsExpected)
        {
            List<string> commands = new List<string>();
            foreach (var command in docGenerator.CommandChain) 
            {
                commands.Add(command.CommandData.Names[0]);
            }

            Assert.AreEqual(commands.ToArray(), commandsExpected);
        }

    }
}
