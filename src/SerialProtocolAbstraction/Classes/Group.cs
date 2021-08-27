using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SerialProtocolAbstraction
{
    public class Group : Command
    {
        internal Command[] NestedCommands { get; private set; }
        internal Group(Type type)
            : base(type)
        {
            var factory = SCPICommandsFactory.GetInstance();
            Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public);

            NestedCommands = factory.CreateMultipleCommands(nestedTypes);
        }

    }
}
