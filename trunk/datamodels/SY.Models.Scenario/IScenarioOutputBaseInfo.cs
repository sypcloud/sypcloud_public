using SY.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{
    interface IScenarioOutputBaseInfo
    {
        int ScenarioID { get; set; }

        string File_Guid { get; set; }

        string ScenarioName { get; set; }

        string ModelTemplateName { get; set; }

        DateTime CreateTime { get; set; }

        DateTime ShowStartTime { get; set; }

        DateTime ShowEndTime { get; set; }

        DateTime ModelStartTime { get; set; }

        DateTime ModelEndTime { get; set; }

        List<Source> CaseLeakagesInfo { get; set; }
    }
}
