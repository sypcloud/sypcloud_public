using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sy.Global;
using SY.Models.ModelBase;

namespace SY.Models.Scenario
{
    public interface IScenarioInput
    {
        IModelDomain ModelDomain { get; set; }

        IList<Boundary> ModelBoundary { get; set; }

        IParameter ModelParameter { get; set; }

        //CIRP**************tt-2018-10-24********************
        IList<CIRPNuclideParaInfo> ModelCIRP_Parameterlist { get; set; }

        ICIRPNuclideDM ModelCIRP_Parameter { get; set; }

        IWeather Weather { get; set; }



    }
}
