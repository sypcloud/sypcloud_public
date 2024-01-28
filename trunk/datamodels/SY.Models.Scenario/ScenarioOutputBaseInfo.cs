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
    [KnownType(typeof(Source))]
    public class ScenarioOutputBaseInfo : IScenarioOutputBaseInfo
    {
        public ScenarioOutputBaseInfo()
        {
            //实例化
            CaseLeakagesInfo = new List<Source>();
        }

        [DataMember]
        public int ScenarioID { get; set; }

        [DataMember]
        public string File_Guid { get; set; }

        [DataMember]
        public string ScenarioName { get; set; }

        [DataMember]
        public string ModelTemplateName { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public DateTime ShowStartTime { get; set; }

        [DataMember]
        public DateTime ShowEndTime { get; set; }

        [DataMember]
        public DateTime ModelStartTime { get; set; }

        [DataMember]
        public DateTime ModelEndTime { get; set; }

        [DataMember]
        public List<Source> CaseLeakagesInfo { get; set; }

        [DataMember]
        public bool IsSelectCase { get; set; }

        [DataMember]
        public string TaskID { get; set; }

        [DataMember]
        public string TaskType { get; set; }

        [DataMember]
        public DateTime ReceiveInfo_Time { get; set; }
    }

}
