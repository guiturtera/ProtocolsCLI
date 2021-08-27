using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class CommonCommandAttribute : Attribute
    {
        public string Help = "";
        public List<string> Names;
        public bool DisableAttributeChain = false, DisableOptionChain = false;
        public CommonCommandAttribute(params string[] names)
        {
            Names = names.ToList();
        }

        public override string ToString()
        {
            return this.GetType().Name + " -> " + String.Format(string.Join("|", Names.ToArray()));
        }
    }
}
