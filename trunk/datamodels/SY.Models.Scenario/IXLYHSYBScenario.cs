using SY.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{
    public interface IXLYHSYBScenario:IScenarioBase
    {
        XiaoLiuYu ScenarioInput { get; set; }
    }
}
