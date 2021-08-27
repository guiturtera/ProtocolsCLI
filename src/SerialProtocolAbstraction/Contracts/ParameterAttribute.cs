using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ParameterAttribute : Attribute
    {
        public string[] OptionsHelp { get; protected set; }
        public string Parameter = "", Help = "";
        public ParameterAttribute(string parameter, string help)
        {
            Parameter = parameter;
            Help = help;
            OptionsHelp = new string[0];
        }

        public ParameterAttribute(string parameter, string help, string[] optionsHelp)
            : this(parameter, help)
        {
            OptionsHelp = optionsHelp;
        }
    }
}
