using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class OptionAttribute : ParameterAttribute 
    {
        public OptionAttribute(string parameter, string help)
        : base(parameter, help) { }

        public OptionAttribute(string parameter, string help, string[] optionsHelp)
            : base(parameter, help, optionsHelp) { }
    }
}
