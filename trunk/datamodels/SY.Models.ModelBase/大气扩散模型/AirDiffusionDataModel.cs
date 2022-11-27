using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    public class AirDiffusionDataModel
    {
        /// <summary>
        /// 1-点源化学反应：2-点源无反应模型：3-面源化学反应模型；4-面源无反应模型
        /// </summary>
        [DataMember]
        public int MethodCod { get; set; }
        /// <summary>
        /// 排放口经度
        /// </summary>
        [DataMember]
        public float Logtd { get; set; }
        /// <summary>
        /// 排放口纬度
        /// </summary>
        [DataMember]
        public float Latd { get; set; }
        /// <summary>
        /// 面源单元边长
        /// </summary>
        [DataMember]
        public float A { get; set; }
        /// <summary>
        /// 单位时间污染源的排放源强
        /// </summary>
        [DataMember]
        public float Q { get; set; }
        /// <summary>
        /// 实际点源排放效率
        /// </summary>
        [DataMember]
        public float Qv { get; set; }
        /// <summary>
        /// 排气筒烟气排出速度，m/s
        /// </summary>
        [DataMember]
        public float Vs { get; set; }
        /// <summary>
        /// 污染源有效高度
        /// </summary>
        [DataMember]
        public float H { get; set; }
        /// <summary>
        /// 烟气出口流速 m/s
        /// </summary>
        [DataMember]
        public float Us { get; set; }
        /// <summary>
        /// 烟气出口平均流速 m/s
        /// </summary>
        [DataMember]
        public float Usa { get; set; }
        /// <summary>
        /// 烟气出口温度 k
        /// </summary>
        [DataMember]
        public float Ts { get; set; }
        /// <summary>
        /// 烟囱出口直径 m
        /// </summary>
        [DataMember]
        public float D { get; set; }
        /// <summary>
        /// 地表类型：0-农村或城市远郊区 1-城区及近郊区
        /// </summary>
        [DataMember]
        public int SurfaceType { get; set; }
        /// <summary>
        /// 平均风速
        /// </summary>
        [DataMember]
        public float U { get; set; }
        /// <summary>
        /// 风向
        /// </summary>
        [DataMember]
        public float WindAngle { get; set; }
        /// <summary>
        /// 环境大气平均温度 k
        /// </summary>
        [DataMember]
        public float Ta { get; set; }
        /// <summary>
        /// 大气稳定度
        /// </summary>
        [DataMember]
        public string W { get; set; }
        /// <summary>
        /// 化学转化速率常数
        /// </summary>
        [DataMember]
        [DefaultValue(0.0052194f)]
        public float K { get; set; }
        /// <summary>
        /// 烟气出口出的大气压
        /// </summary>
        [DataMember]
        [DefaultValue(10.057)]
        public float Pa { get; set; }
        /// <summary>
        /// 高度矫正方法
        /// </summary>
        [DataMember]
        [DefaultValue("Holland")]
        public string H_fix_method { get; set; }
        /// <summary>
        /// 计算网格x方向步长
        /// </summary>
        [DataMember]
        [DefaultValue(50)]
        public int Deltx { get; set; }
        /// <summary>
        /// 计算网格y方向步长
        /// </summary>
        [DataMember]
        [DefaultValue(50)]
        public int Delty { get; set; }
        /// <summary>
        /// 计算区域扇形半径
        /// </summary>
        [DataMember]
        [DefaultValue(3500)]
        public float Radius { get; set; }
        /// <summary>
        /// 风向两侧多少度纳入计算范围
        /// </summary>
        [DataMember]
        [DefaultValue(30)]
        public int DeltAngle { get; set; }
    }
}
