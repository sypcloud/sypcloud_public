using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    /// <summary>
    /// 马斯京根模型
    /// </summary>
    public class MaSiJingGen
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        [DisplayName("序号")]
        [XmlIgnore]
        [DataMember]
        public int ID { get; set; }
        /// <summary>
        /// 流域名称
        /// </summary>
        [DisplayName("流域名称")]
        [XmlIgnore]
        [DataMember]
        public string LiuyuName { get; set; }
        /// <summary>
        /// 流域编码
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [DataMember]
        public string LiuyuCode { get; set; }
        /// <summary>
        /// 河段名称
        /// </summary>
        [XmlIgnore]
        [DisplayName("河段名称")]
        [DataMember]
        public string HedaoName { get; set; }
        /// <summary>
        /// 河道编码
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [DataMember]
        public string HeDaoCode { get; set; }

        bool _isHaveKX = true;
        [DataMember]
        /// <summary>
        /// 是否有KX值
        /// </summary>
        [Browsable(false)]
        public bool IsHaveKX
        {
            get
            {
                return _isHaveKX;
            }
            set
            {
                _isHaveKX = value;
            }
        }

        double _k;
        [DataMember]
        /// <summary>
        /// 计算参数K
        /// </summary>
        public double K
        {
            get
            {
                return _k;
            }
            set
            {
                _k = value;
            }
        }

        double _x;
        [DataMember]
        /// <summary>
        /// 计算参数X
        /// </summary>
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        string _duanMianShape;
        [DataMember]
        /// <summary>
        /// 断面形状
        /// </summary>
        [Browsable(false)]
        public string DuanMianShape
        {
            get
            {
                return _duanMianShape;
            }
            set
            {
                _duanMianShape = value;
            }
        }

        bool _isHaveDuanMianInfo = true;
        [DataMember]
        /// <summary>
        /// 是否有断面信息
        /// </summary>
        [Browsable(false)]
        public bool IsHaveDuanMianInfo
        {
            get
            {
                return _isHaveDuanMianInfo;
            }
            set
            {
                _isHaveDuanMianInfo = value;
            }
        }

        double _maxWidth;
        [DataMember]
        /// <summary>
        /// 最大水面宽度
        /// </summary>
        [Browsable(false)]
        public double MaxWidth
        {
            get
            {
                return _maxWidth;
            }
            set
            {
                _maxWidth = value;
            }
        }

        double _maxDeep;
        [DataMember]
        /// <summary>
        /// 最大水深
        /// </summary>
        [Browsable(false)]
        public double MaxDeep
        {
            get
            {
                return _maxDeep;
            }
            set
            {
                _maxDeep = value;
            }
        }

        float _manNingFactor;
        [DataMember]
        /// <summary>
        /// 曼宁系数
        /// </summary>
        [Browsable(false)]
        public float ManNingFactor
        {
            get
            {
                return _manNingFactor;
            }
            set
            {
                _manNingFactor = value;
            }
        }

    }
}
