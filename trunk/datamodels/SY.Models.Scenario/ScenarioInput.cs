using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SY.Models.ModelBase;

namespace SY.Models.Scenario
{
    [DataContract]
    [KnownType(typeof(ModelDomain))]
    [KnownType(typeof(Boundary))]
    [KnownType(typeof(Parameter))]
    [KnownType(typeof(CIRPNuclideDM))]
    [KnownType(typeof(Weather))]
    public class ScenarioInput : IScenarioInput
    {
        public ScenarioInput()
        {
            ModelBoundary = new List<Boundary>();
            ModelDomain = new ModelDomain();
            ModelParameter = new Parameter();
            ModelCIRP_Parameterlist = new List<CIRPNuclideParaInfo>();
            ModelCIRP_Parameter = new CIRPNuclideDM();
            Weather = new Weather();
        }

        [DataMember]
        public IModelDomain ModelDomain { get; set; }

        [DataMember]
        public IList<Boundary> ModelBoundary { get; set; }

        /// <summary>
        /// 一般水动力的计算参数及初始条件
        /// </summary>
        [DataMember]
        public IParameter ModelParameter { get; set; }

        //CIRP**************tt-2018-10-24********************

        /// <summary>
        /// CIRP核素相关计算参数列表（求取事故源项并集的核素计算参数）
        /// </summary>
        [DataMember]
        public IList<CIRPNuclideParaInfo> ModelCIRP_Parameterlist { get; set; }

        [DataMember]
        public ICIRPNuclideDM ModelCIRP_Parameter { get; set; }

        [DataMember]
        public IWeather Weather { get; set; }

        [DataMember]
        public float OutputFrequency { get; set; }


    }
}
