using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SerialProtocolAbstraction;

namespace UnitTests.DocumentationTest1
{
    [Group("GP1")]
    [Argument("first_argument", "help for first_argument", 1)]
    [Option("first_option", "help for first_option")]
    public class GP1 : ICommand
    {
        #region ICommand Members

        public void Run(Dictionary<string, string> kwargs)
        {
            return;
        }

        #endregion

        [Command("Hello World", Help = "help for command Hello World")]
        [Argument("second_argument", "help for first_argument", 1)]
        [Option("second_option", "help for second_option")]
        public class HelloWorld : ICommand 
        {
            #region ICommand Members

            public void Run(Dictionary<string, string> kwargs)
            {
                return;
            }

            #endregion
        } 
    }
}

namespace UnitTests.DocumentationTest2
{
    [Group("GP1")]
    [Argument("first_argument", "help for first_argument", 1)]
    [Option("first_option", "help for first_option")]
    public class GP1 : ICommand
    {
        #region ICommand Members

        public void Run(Dictionary<string, string> kwargs)
        {
            return;
        }

        #endregion

        [Command("Hello World", Help = "help for command Hello World", DisableAttributeChain=true)]
        [Argument("second_argument", "help for first_argument", 1)]
        [Option("second_option", "help for second_option")]
        public class HelloWorld : ICommand
        {
            #region ICommand Members

            public void Run(Dictionary<string, string> kwargs)
            {
                return;
            }

            #endregion
        }
    }
}

namespace UnitTests.DocumentationTest3
{
    [Group("GP1")]
    [Argument("first_argument", "help for first_argument", 1)]
    [Option("first_option", "help for first_option")]
    public class GP1 : ICommand
    {
        #region ICommand Members

        public void Run(Dictionary<string, string> kwargs)
        {
            return;
        }

        #endregion

        [Command("Hello World", Help = "help for command Hello World", DisableOptionChain=true)]
        [Argument("second_argument", "help for first_argument", 1)]
        [Option("second_option", "help for second_option")]
        public class HelloWorld : ICommand
        {
            #region ICommand Members

            public void Run(Dictionary<string, string> kwargs)
            {
                return;
            }

            #endregion
        }
    }
}

namespace UnitTests.DocumentationTest4
{
    [Group("GP1")]
    [Argument("first_argument", "help for first_argument", 1)]
    [Option("first_option", "help for first_option")]
    public class GP1 : ICommand
    {
        #region ICommand Members

        public void Run(Dictionary<string, string> kwargs)
        {
            return;
        }

        #endregion

        [Command("Hello World", Help = "help for command Hello World", DisableOptionChain = true, DisableAttributeChain = true)]
        [Argument("second_argument", "help for first_argument", 1)]
        [Option("second_option", "help for second_option")]
        public class HelloWorld : ICommand
        {
            #region ICommand Members

            public void Run(Dictionary<string, string> kwargs)
            {
                return;
            }

            #endregion
        }
    }
}

namespace UnitTests.DocumentationTest5
{
    [Group("GP1", Help="help for GP1")]
    [Argument("first_argument", "help for first_argument", 1)]
    [Option("first_option", "help for first_option")]
    public class GP1 : ICommand
    {
        #region ICommand Members

        public void Run(Dictionary<string, string> kwargs)
        {
            return;
        }

        #endregion

        [Command("Hello World")]
        [Argument("second_argument", "help for first_argument", 1)]
        [Option("second_option", "help for second_option")]
        public class HelloWorld : ICommand
        {
            #region ICommand Members

            public void Run(Dictionary<string, string> kwargs)
            {
                return;
            }

            #endregion
        }
    }
}