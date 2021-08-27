using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GroupAttribute : CommonCommandAttribute
    {
        public GroupAttribute(params string[] names) : base(names) { }
    }
}
