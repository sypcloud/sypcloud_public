using SY.Common;
using SY.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{
    [DataContract]
    [KnownType(typeof(ScenarionTime))]
    public class ScenarioBase:IScenarioBase
    {
        [DataMember]
        public Guid OwnerID { get; set; }

        [DataMember]
        public Guid ParentID { get; set; }

        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Creater { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public DateTime ModifyTime { get; set; }

        [DataMember]
        public ScenarionTime ScenarioTime { get; set; }

        [DataMember]
        public eumScenarioType Type { get; set; }

        [DataMember]
        public bool OneStepRun { get; set; }

        [DataMember]
        public bool IsAutoForecasting { get; set; }

        [DataMember]
        public bool IsCustomizing { get; set; }

        [DataMember]
        public string ControlfileName { get; set; }

        [DataMember]
        public string EngineFileName { get; set; }

        [DataMember]
        public Dictionary<enumModelName, string> ModelEngine { get; set; }

        [DataMember]
        public bool UploadedAsZip { get; set; }

        [DataMember]
        public string OutputFile { get; set; }

        [DataMember]
        public enumParallelMode ParallelMode { get; set; }

        [DataMember]
        public eumApplications AppName { get; set; }

        [DataMember]
        public enumProcessType ModelProcess { get; set; }

        /// <summary>
        /// 方案模型空间拓扑坐标投影系
        /// </summary>
        [DataMember]
        public eumCoordsProjSys Projection { get; set; }
        /// <summary>
        /// 方案模型空间拓扑中心经度
        /// </summary>
        [DataMember]
        public int CentralLgtd { get; set; }

        [DataMember]
        public string Catalog { get; set; }
        [DataMember]
        public string Basin { get; set; }
        [DataMember]
        public string River { get; set; }
        /// <summary>
        /// 方案计算状态：未计算=0,正在计算=1,计算成功=2,计算失败=3,
        /// </summary>
        [DataMember]
        public int RunningStatus { get; set; }
    }
}
