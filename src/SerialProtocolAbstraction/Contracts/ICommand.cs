using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialProtocolAbstraction
{
    /// <summary>
    /// EVERY IMPLEMENTATION MUST HAVE [Group()] or [Command()] attribute.
    /// Only one of them, one time.
    /// </summary>
    public interface ICommand
    {
        void Run(Dictionary<string, string> kwargs);
    }
}
