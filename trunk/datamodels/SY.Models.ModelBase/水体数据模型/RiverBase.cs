using Sy.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    [KnownType(typeof(PointD))]
    public class RiverBase
    {
        [DataMember]
        public string RvrMdCode { get; set; }
        [DataMember]
        public string TopoID { get; set; }
        /// <summary>
        /// 河流中文名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RvrName { get; set; }
        [DataMember]
        public string RchName { get; set; }
        [DataMember]
        public float StChainage { get; set; }
        [DataMember]
        public float EdChainage { get; set; }
        //public string UpRvrCode { get; set; }
        //public string UpChainage { get; set; }
        //public string DwRvrCode { get; set; }
        //public string DwChainage { get; set; }
        [DataMember]
        public List<PointD> Points { get; set; }
        /// <summary>
        /// 存储河段上的断面桩号
        /// </summary>
        [DataMember]
        public List<float> Stations { get; set; }
        [DataMember]
        public List<string> Stations2 { get; set; }
        /// <summary>
        /// 存储河段计算网格（线段）坐标wkt字符串
        /// </summary>
        [DataMember]
        public List<List<PointD>> SegmentPoints { get; set; }
    }
}
