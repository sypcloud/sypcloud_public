using SY.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{
    public interface IFxtScenarioBaseInfo
    { 
        string ModelName { get; set; }

        List<MornitorPoint> InterestPoints { get; set; }

        string UnitName { get; set; }

        string UnitType { get; set; }

        string BasinName { get; set; }

        string ReginName { get; set; }

        string OrgnizeName { get; set; }

        string CreateTime { get; set; }

        string Extent { get; set; }

        string UnitMapUrl { get; set; }

        string UnitModelMapUrl { get; set; }
    }
}
