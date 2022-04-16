using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [KnownType(typeof(DykeGeneralInfo))]
    [KnownType(typeof(DykeBreakingGeometryStruct))]
    [KnownType(typeof(BreakMode))]
    [Serializable]
    [DataContract]
    public class DykeBreakStructure : IDykeBreakStructure
    {
        public DykeBreakStructure()
        {
            GeneralInfo = new DykeGeneralInfo();
            GeneralInfo.XLink = new List<double>();
            GeneralInfo.YLink = new List<double>();

            BreakPara = new List<DykeBreakingGeometryStruct>();
        }
        [DataMember]
        public BreakMode Mode { get; set; }

        [DataMember]
        public DykeGeneralInfo GeneralInfo { get; set; }

        [DataMember]
        public IList<DykeBreakingGeometryStruct> BreakPara { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime SimulationStartTime { get; set; }

        [DataMember]
        public List<float> WaterLevel { get; set; }

        [DataMember]
        public List<float> Discharge { get; set; }

        [DataMember]
        public int M21CellId { get; set; }
    }
}
