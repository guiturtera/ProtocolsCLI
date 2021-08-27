using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialProtocolAbstraction;
using System.Reflection;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandsFactory _factory;
            
            _factory = CommandsFactory.GetCommand(EnumProtocol.SCPI);
            _factory.CreateCommandTree(Assembly.GetExecutingAssembly(), "ConsoleApplication1");

            Console.WriteLine(_factory.CreateDocumentation(new SCPIDocumentation()));
            Console.ReadKey();
        }
    }

    [Group("CONFigure", "CONF", Help ="some help for CONF")]
    public class ConfigureGroup : ICommand
    {
        public void Run(Dictionary<string, string> kwargs)
        {
        }

        [Group("PRES", "PRESsure", Help="help for pressure")]
        [Argument("type", "help for config <type>", 1, new string[] { "op1", "op2" })]
        public class PressGroup : ICommand
        {
            public void Run(Dictionary<string, string> kwargs)
            {
            }

            [Command("ONEARGUMENT", DisableAttributeChain = true, Help = "some help for CONF")]
            [Argument("one_argument", "help for config <one_argument>", 1)]
            public class OneArgumentCommand : ICommand
            {
                public void Run(Dictionary<string, string> kwargs)
                {
                }
            }

            [Command("NOARGUMENT", DisableAttributeChain = true, Help = "some help for CONF")]
            public class NoArgumentCommand : ICommand
            {
                public void Run(Dictionary<string, string> kwargs)
                {
                }
            }

        }

        [Group("UNIT", Help = "some help for CONF")]
        [Argument("table", "help for config <table>", 1)]
        public class UnitGroup : ICommand
        {
            public void Run(Dictionary<string, string> kwargs)
            {
            }

            [Command("OTHER", Help = "some help for CONF")]
            [Argument("other_param", "help for config <other_param>", 1)]
            [Argument("other_param2", "help for config <other_param2>", 2)]
            [Argument("other_param3", "help for config <other_param3>", 3)]
            public class OtherCommand : ICommand
            {
                public void Run(Dictionary<string, string> kwargs)
                {
                }
            }

        }

        [Command("HELLO")]
        public class Hello : ICommand
        {
            public void Run(Dictionary<string, string> kwargs)
            {
            }
        }
    }
}
