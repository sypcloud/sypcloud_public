using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public enum BreakMode
    {
        HourAfterStart,
        Time,
        Level
    }
    public enum StructureDirectionType
    {
        regular,
        side
    }
    [Serializable]
    [DataContract]
    public class DykeBreakingGeometryStruct
    {
        [DataMember]
        public float time { get; set; }//秒

        [DataMember]
        public float level { get; set; }

        [DataMember]
        public float width { get; set; }

        [DataMember]
        public float slope { get; set; }
    }
    [Serializable]
    [DataContract]
    public class DykeGeneralInfo
    {
        [DataMember]
        public string riverName { get; set; }

        [DataMember]
        public float chainage { get; set; }

        [DataMember]
        public string name { get; set; }//溃口名称

        [DataMember]
        public double x { get; set; }//溃口中心位置x坐标

        [DataMember]
        public double y { get; set; }//溃口中心位置y坐标

        [DataMember]
        public float elevation { get; set; }//溃口中心初溃时高程

        [DataMember]
        public float width { get; set; }//溃口宽度

        [DataMember]
        public List<int> Nodes { get; set; }//溃口对接的二维网格节点，顺序排列

        [DataMember]
        public IList<double> XLink { get; set; }

        [DataMember]
        public IList<double> YLink { get; set; }

        [DataMember]
        public BreakMode mode { get; set; }

        [DataMember]
        public DateTime breakTime { get; set; }

        [DataMember]
        public float breakLevel { get; set; }

        [DataMember]
        public StructureDirectionType directionType { get; set; }
    }

    public interface IDykeBreakStructure
    {
        BreakMode Mode { get; set; }
        DykeGeneralInfo GeneralInfo { get; set; }
        IList<DykeBreakingGeometryStruct> BreakPara { get; set; }
        int M21CellId { get; set; }
    }
}
