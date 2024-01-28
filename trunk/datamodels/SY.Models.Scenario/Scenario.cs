using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SY.Common;
using SY.Models.ModelBase;

namespace SY.Models.Scenario
{
    [DataContract]
    [KnownType(typeof(ScenarioInput))]
    [KnownType(typeof(ScenarionTime))]
    public class Scenario : ScenarioBase,IScenario
    {
        [DataMember]
        public enumScenarioFeild Feild { get; set; }

        [DataMember]
        public enumMethodType Method { get; set; }

        [DataMember]
        public enumModelDomainType ModelDomain { get; set; }

        [DataMember]
        public enumGridType GridType { get; set; }

        [DataMember]
        public List<enumModelName> WaterFlowModelName { get; set; }  
      
        [DataMember]
        public List<enumModelName> BasinModelName { get; set; }

        [DataMember]
        public ScenarioInput ScenarioInput { get; set; }


        //CIRP*********************************************************

        /// <summary>
        /// 方案事故名称
        /// </summary>
        [DataMember]
        public string ScenarioName { get; set; }
        /// <summary>
        /// 方案创建者
        /// </summary>
        [DataMember]
        public string ScenarioCreateUser { get; set; }
        /// <summary>
        /// 方案描述
        /// </summary>
        [DataMember]
        public string ScenarioDescribeInfo { get; set; }

        [DataMember]
        public bool IsExsitedFlow { get; set; }

        [DataMember]
        public bool IsUserDefinedFlow { get; set; }

        [DataMember]
        public double ModelCalScope { get; set; }

        [DataMember]
        public double ModelCalGridSize { get; set; }

        [DataMember]
        public int ModelCalGridLayers { get; set; }

        [DataMember]
        public int ModelCalGridSourceLayer { get; set; }

        [DataMember]
        public enumModelScale ModelScale { get; set; }

        [DataMember]
        public string ModelTemplateName { get; set; }

        [DataMember]
        public bool SendResult2Server { get; set; }

        [DataMember]
        public string SourceBoundaryFilePath { get; set; }

        [DataMember]
        public DateTime ReceiveInfo_Time { get; set; }

    }
}
