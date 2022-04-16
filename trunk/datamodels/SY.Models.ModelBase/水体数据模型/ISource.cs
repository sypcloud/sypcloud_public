using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    interface ISource
    {
        string CaseLeakage_Name { get; set; }

        float CaseLeakage_Longitude { get; set; }

        float CaseLeakage_Latitude { get; set; }

        List<string> CaseLeakage_ListNuclideNames { get; set; }

    }
}
