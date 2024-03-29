﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    public class ModelTime:IModelTime
    {
        /// <summary>
        /// 模型计算开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 模型计算结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 预报时刻
        /// </summary>
        [DataMember]
        public DateTime ForecastingTime { get; set; }
        /// <summary>
        /// 预见期
        /// </summary>
        [DataMember]
        public double ForecastedTime { get; set; }

        [DataMember]
        /// <summary>
        /// 时间步长，以秒为单位
        /// </summary>
        public float TimeStep { get; set; }

        [DataMember]
        public float MaxTimeStep { get; set; }

        [DataMember]
        public float MinTimeStep { get; set; }
        
        [DataMember]
        /// <summary>
        /// 时间序列的步长（以小时为单位）
        /// </summary>
        public float TimeValueStep { get; set; }

        [DataMember]
        /// <summary>
        /// 输出步长（以秒为单位）
        /// </summary>
        public int OutputStep { get; set; }
    }
}
