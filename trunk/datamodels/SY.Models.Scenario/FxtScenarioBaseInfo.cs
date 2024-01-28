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
    [KnownType(typeof(MornitorPoint))]
    public class FxtScenarioBaseInfo: IFxtScenarioBaseInfo
    {
        public FxtScenarioBaseInfo()
        {
            InterestPoints = new List<MornitorPoint>();
        }
            
        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public List<MornitorPoint> InterestPoints { get; set; }

        [DataMember]
        public string UnitName { get; set; }

        [DataMember]
        public string UnitType { get; set; }

        [DataMember]
        public string BasinName { get; set; }

        [DataMember]
        public string ReginName { get; set; }

        [DataMember]
        public string OrgnizeName { get; set; }

        [DataMember]
        public string CreateTime { get; set; }

        public string Extent { get; set; }

        [DataMember]
        public string UnitMapUrl { get; set; }

        [DataMember]
        public string UnitModelMapUrl { get; set; }
    }
}
