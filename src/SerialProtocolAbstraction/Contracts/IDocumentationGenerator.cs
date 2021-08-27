using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public interface IDocumentationGenerator
    {
        string CreateDocumentation(Command[] commands);
    }
}
