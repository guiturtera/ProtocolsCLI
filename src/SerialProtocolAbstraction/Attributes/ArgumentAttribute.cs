using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public sealed class ArgumentAttribute : ParameterAttribute, IComparable<ArgumentAttribute>
    {
        internal int _internalOrder;
        public ArgumentAttribute(string parameter, string help, int order) 
            : base(parameter, help)
        {
           _internalOrder = order;
        }

        public ArgumentAttribute(string parameter, string help, int order, string[] optionsHelp)
            : this(parameter, help, order)
        {
            OptionsHelp = optionsHelp;
        }

        #region IComparable<ArgumentAttribute> Members

        public int CompareTo(ArgumentAttribute other)
        {
            if (other._internalOrder > this._internalOrder)
                return -1;
            else if (other._internalOrder < this._internalOrder)
                return 1;
            else
                return 0;
        }

        #endregion
    }

}
