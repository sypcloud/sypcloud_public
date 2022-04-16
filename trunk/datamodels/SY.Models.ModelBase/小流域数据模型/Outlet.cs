using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    /// <summary>
    /// 流域出口
    /// </summary>
    [DataContract]   
    public class Outlet
    {
        /// <summary>
        /// 流域出口峰值
        /// </summary>
        [DataMember]
        public double Peak { get; set; }
        /// <summary>
        /// 流域出口峰现时间
        /// </summary>
        [DataMember]
        public int PeakTimeStep { get; set; }
        /// <summary>
        /// 流域出口峰现时间
        /// </summary>
        [DataMember]
        public DateTime PeakTime { get; set; }
    }
}
