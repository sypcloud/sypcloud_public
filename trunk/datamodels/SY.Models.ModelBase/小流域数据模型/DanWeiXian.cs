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
    [KnownType(typeof(DWX))]
    /// <summary>
    /// 单位线
    /// </summary>
    public class DanWeiXian
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
        /// 子流域名称
        /// </summary>
        [XmlIgnore]
        [DisplayName("子流域名称")]
        [DataMember]
        public string ZiliuyuName { get; set; }
        /// <summary>
        /// 子流域编码
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [DataMember]
        public string ZiliuyuCode { get; set; }

        [DataMember]
        [XmlArray(ElementName = "DWXData")]
        [XmlArrayItem(typeof(DWX), ElementName = "DWX")]
        [Browsable(false)]
        public List<DWX> DWXData { get; set; }

        float _effluentNum = 1;
        [DataMember]
        /// <summary>
        /// 时间长度
        /// </summary>
        [DisplayName("时间长度")]
        public float EffluentNum
        {
            get
            {
                return _effluentNum;
            }
            set
            {
                _effluentNum = value;
            }
        }

        float _kSubSurFlow = 10;
        [DataMember]
        /// <summary>
        /// 壤中流出流系数
        /// </summary>
        [DisplayName("壤中流出流系数")]
        public float KSubSurFlow
        {
            get
            {
                return _kSubSurFlow;
            }
            set
            {
                _kSubSurFlow = value;
            }
        }

        float _kUnderGround = 1;
        [DataMember]
        /// <summary>
        /// 地下水出流系数
        /// </summary>
        [DisplayName("地下水出流系数")]
        public float KUnderGround
        {
            get
            {
                return _kUnderGround;
            }
            set
            {
                _kUnderGround = value;
            }
        }

        string _dwxpath;
        [DataMember]
        /// <summary>
        /// 单位线文件
        /// </summary>
        [Browsable(false)]
        public string DWXPath
        {
            get
            {
                return _dwxpath;
            }
            set
            {
                _dwxpath = value;
            }
        }

    }

    [DataContract]
    [Serializable]
    public class DWX
    {
        [DataMember]
        [XmlAttribute(AttributeName = "value")]
        public float Data { get; set; }
    }
}
