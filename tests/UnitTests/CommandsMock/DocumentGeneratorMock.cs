using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;

namespace UnitTests
{
    internal class DocumentGeneratorMock : DefaultDocumentationGenerator
    {
        internal EventHandler<EventArgs> CommandPassed;
        internal List<Command> CommandChain;
        internal List<ArgumentAttribute> Arguments;
        internal List<OptionAttribute> Options;

        protected override string GetCommandLine(List<Command> commandChain, List<ArgumentAttribute> arguments, List<OptionAttribute> options)
        {
            CommandChain = commandChain;
            Arguments = arguments;
            Options = options;

            CommandPassed(this, EventArgs.Empty);
            return "";
        }
    }
}
