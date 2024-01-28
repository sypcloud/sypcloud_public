using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SY.Models.ModelBase;

namespace SY.Models.Scenario
{
    public interface IScenario:IScenarioBase
    {
        enumScenarioFeild Feild { get; set; }

        enumMethodType Method { get; set; }

        enumModelDomainType ModelDomain { get; set; }

        enumGridType GridType { get; set; }

        List<enumModelName> WaterFlowModelName { get; set; }

        List<enumModelName> BasinModelName { get; set; }

        ScenarioInput ScenarioInput { get; set; }
    }
}
