using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    public class BoundaryBase
    {
        [DataMember]
        public enumBoundaryDescription BndDescription { get; set; }
        [DataMember]
        public string Boundarykey { get; set; }
        [DataMember]
        public bool IsConstant { get; set; }
		///
		///对流扩散边界是否为常数
		///
        [DataMember]
        public bool IsAdConstant { get; set; }
        [DataMember]
        public bool IsTimeseriers { get; set; }
        [DataMember]
        public float ConstantValue { get; set; }
        [DataMember]
        public List<float> AdConstantValue { get; set; }
        /// <summary>
        /// 相对路径
        /// </summary>
        [DataMember]
        public string Tsfile { get; set; }
        [DataMember]
        /// <summary>
        /// 源汇边界在水体中的深度
        /// </summary>
        public float Depth { get; set; }

    }
}
