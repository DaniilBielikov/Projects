using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryRemotePC
{
    internal class OperationFlags
    {
        internal Int16 NumberPC { set; get; }
        internal List<UInt16> ForbiddenTypes { set; get; }

        internal OperationFlags(Int16 numberPC)
        {
            NumberPC = numberPC;
        }
    }
}
