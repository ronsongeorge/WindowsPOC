using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportLibrary
{
    public interface IExport
    {
        void ExportDataToLocation(string location);
    }
}
