using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    /// <summary>
    /// 壤中流线性水库
    /// </summary>
    public class RangZhongLiuXianXingShuiKu
    {
        int _numDetriver;
        [DataMember]
        /// <summary>
        /// 总时段长
        /// </summary>
        public int NumDetriver
        {
            get
            {
                return _numDetriver;
            }
            set
            {
                _numDetriver = value;
            }
        }

        int _numRain;
        [DataMember]
        /// <summary>
        /// 净雨时段长，产流模块输出“地下径流”数组的长度
        /// </summary>
        public int NumRain
        {
            get
            {
                return _numRain;
            }
            set
            {
                _numRain = value;
            }
        }

        float[] _rSubSurFlow;
        [DataMember]
        /// <summary>
        /// 壤中流，产流模块输出
        /// </summary>
        public float[] RSubSurFlow
        {
            get
            {
                return _rSubSurFlow;
            }
            set
            {
                _rSubSurFlow = value;
            }
        }

        double _basinArea;
        [DataMember]
        /// <summary>
        /// 流域面积
        /// </summary>
        public double BasinArea
        {
            get
            {
                return _basinArea;
            }
            set
            {
                _basinArea = value;
            }
        }

        float _factor;
        [DataMember]
        /// <summary>
        /// 壤中流出流系数
        /// </summary>
        public float Factor
        {
            get
            {
                return _factor;
            }
            set
            {
                _factor = value;
            }
        }

        float _det;
        [DataMember]
        /// <summary>
        /// 时间步长，以小时为单位，由之前降雨的时间间隔除以60得到
        /// </summary>
        public float Det
        {
            get
            {
                return _det;
            }
            set
            {
                _det = value;
            }
        }
    }
}
