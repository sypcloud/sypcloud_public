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
    /// 1D水动力学模型
    /// </summary>
    public class OneDShuiDongLiXue
    {
        int _sectionNum = 2;
        [DataMember]
        /// <summary>
        /// 断面个数
        /// </summary>
        public int SectionNum
        {
            get
            {
                return _sectionNum;
            }
            set
            {
                _sectionNum = value;
            }
        }

        double _stepTime = 5;
        [DataMember]
        /// <summary>
        /// 时间步长
        /// </summary>
        public double StepTime
        {
            get
            {
                return _stepTime;
            }
            set
            {
                _stepTime = value;
            }
        }

        double _totalTime = 10;
        [DataMember]
        /// <summary>
        /// 时间总长
        /// </summary>
        public double TotalTime
        {
            get
            {
                return _totalTime;
            }
            set
            {
                _totalTime = value;
            }
        }

        int _upstreamIndex = 1;
        [DataMember]
        /// <summary>
        /// 上游边界索引
        /// </summary>
        public int UpstreamIndex
        {
            get
            {
                return _upstreamIndex;
            }
            set
            {
                _upstreamIndex = value;
            }
        }
    }
}
