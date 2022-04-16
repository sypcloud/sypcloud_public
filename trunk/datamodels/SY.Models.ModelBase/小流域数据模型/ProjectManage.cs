using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    public class Project : ModelObject
    {

        private string projectNameField;

        private string projectTimeField;

        private string projectDataField;

        private int projectYear;

        private string projectDescribeField;

        private int fIDField;

        private List<ChanLiuResult> projectchanliuresult = new List<ChanLiuResult>();

        private List<HuiLiuResult> projectHuiLiuresult = new List<HuiLiuResult>();

        private List<ShuiKuResult> projectShuiKuResult = new List<ShuiKuResult>();

        private List<YanJinResult> projectYanJinresult = new List<YanJinResult>();

        private List<DemData> projectdemdata = new List<DemData>();

        private string projectStartTime;

        private double projectTimeinterval;

        private double projectAllTime;

        [DataMember]
        /// <summary>
        /// 方案名称
        /// </summary>
        public string ProjectName
        {
            get
            {
                return this.projectNameField;
            }
            set
            {
                this.projectNameField = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 方案数据
        /// </summary>
        public string ProjectData
        {
            get
            {
                return this.projectDataField;
            }
            set
            {
                this.projectDataField = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 创建时间
        /// </summary>
        public string ProjectTime
        {
            get
            {
                return this.projectTimeField;
            }
            set
            {
                this.projectTimeField = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 创建时间
        /// </summary>
        public int ProjectYear
        {
            get
            {
                return this.projectYear;
            }
            set
            {
                this.projectYear = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 方案描述
        /// </summary>
        public string ProjectDescribe
        {
            get
            {
                return this.projectDescribeField;
            }
            set
            {
                this.projectDescribeField = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 方案的产流结果
        /// </summary>
        public List<ChanLiuResult> ProjectChanLiuResult
        {
            get
            {
                return this.projectchanliuresult;
            }
            set
            {
                this.projectchanliuresult = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 方案的汇流结果
        /// </summary>
        public List<HuiLiuResult> ProjectHuiLiuResult
        {
            get
            {
                return this.projectHuiLiuresult;
            }
            set
            {
                this.projectHuiLiuresult = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 水库结果
        /// </summary>
        public List<ShuiKuResult> ProjectShuiKuResult
        {
            get
            {
                return this.projectShuiKuResult;
            }
            set
            {
                this.projectShuiKuResult = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 方案的演进结果
        /// </summary>
        public List<YanJinResult> ProjectYanJinResult
        {
            get { return this.projectYanJinresult; }
            set { this.projectYanJinresult = value; }
        }

        [DataMember]
        /// <summary>
        /// 所有河道的断面高程
        /// </summary>
        public List<DemData> ProjecctDemData
        {
            get { return this.projectdemdata; }
            set { this.projectdemdata = value; }
        }

        [DataMember]
        /// <summary>
        /// 开始时间
        /// </summary>
        public string ProjectStratTime
        {
            get
            {
                return this.projectStartTime;
            }
            set
            {
                this.projectStartTime = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 时间间隔
        /// </summary>
        public double ProjectTimeInterval
        {
            get
            {
                return this.projectTimeinterval;
            }
            set
            {
                this.projectTimeinterval = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 总时长
        /// </summary>
        public double ProjectAllTime
        {
            get
            {
                return this.projectAllTime;
            }
            set
            {
                this.projectAllTime = value;
            }
        }

        [DataMember]
        /// <summary>
        /// 方案编号
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int FID
        {
            get
            {
                return this.fIDField;
            }
            set
            {
                this.fIDField = value;
            }
        }
    }

    [DataContract]
    [Serializable]
    public class ChanLiuResult:ModelObject
    {
        private long _ZiLiuID;

        private List<double> rs;

        private List<double> rss;

        private List<double> rg;

        [DataMember]
        public long ZiLiuID
        {
            get { return _ZiLiuID; }
            set { _ZiLiuID = value; }
        }

        [DataMember]
        public List<double> ProjectRS
        {
            get
            {
                return this.rs;
            }
            set
            {
                this.rs = value;
            }
        }

        [DataMember]
        public List<double> ProjectRSS
        {
            get
            {
                return this.rss;
            }
            set
            {
                this.rss = value;
            }
        }

        [DataMember]
        public List<double> ProjectRG
        {
            get
            {
                return this.rg;
            }
            set
            {
                this.rg = value;
            }
        }

        public ChanLiuResult()
        {
            ProjectRG = new List<double>();
            ProjectRS = new List<double>();
            ProjectRSS = new List<double>();
        }
    }

    [DataContract]
    [Serializable]
    public class HuiLiuResult:ModelObject
    {
        private long _ZiLiuID;

        private List<double> _HuiLiu;

        [XmlIgnore]
        [DataMember]
        public long ZiLiuID
        {
            get { return _ZiLiuID; }
            set { _ZiLiuID = value; }
        }

        [XmlIgnore]
        [DataMember]
        public List<double> HuiLiu
        {
            get { return _HuiLiu; }
            set { _HuiLiu = value; }
        }

        public HuiLiuResult()
        {
            HuiLiu = new List<double>();
        }
    }

    [DataContract]
    [Serializable]
    public class HuiLiuResultEx : ModelObject
    {
        private long _ZiLiuID;

        private List<double> _HuiLiu;

        [DataMember]
        public long ZiLiuID
        {
            get { return _ZiLiuID; }
            set { _ZiLiuID = value; }
        }
        
        [DataMember]
        public List<double> HuiLiu
        {
            get { return _HuiLiu; }
            set { _HuiLiu = value; }
        }

        public HuiLiuResultEx()
        {
            HuiLiu = new List<double>();
        }
    }

    [DataContract]
    [Serializable]
    public class ShuiKuResult:ModelObject
    {
        private long _ShuiKuID;

        private List<double> _ShuiKuRes;

        [DataMember]
        public long ShuiKuID
        {
            get { return _ShuiKuID; }
            set { _ShuiKuID = value; }
        }

        [DataMember]
        public List<double> ShuiKuRes
        {
            get { return _ShuiKuRes; }
            set { _ShuiKuRes = value; }
        }

        public ShuiKuResult()
        {
            ShuiKuRes = new List<double>();
        }
    }

    [DataContract]
    [Serializable]
    public class DuanMianResult:ModelObject
    {
        private long _DuanMianID;

        private List<double> _YanJin;

        [DataMember]
        public long DuanMianID
        {
            get { return _DuanMianID; }
            set { _DuanMianID = value; }
        }

        [DataMember]
        public List<double> YanJin
        {
            get { return _YanJin; }
            set { _YanJin = value; }
        }
        
        public DuanMianResult()
        {
            YanJin = new List<double>();
        }
    }

    [DataContract]
    [Serializable]
    public class YanJinResult:ModelObject
    {
        private string _HeDaoID;

        [DataMember]
        public string HeDaoID
        {
            get { return this._HeDaoID; }
            set { this._HeDaoID = value; }
        }

        [DataMember]
        public long HeDaoFId
        {
            get;
            set;
        }

        private List<double> _HeDaoYanJin = new List<double>();

        [DataMember]
        /// <summary>
        /// 马斯京根河道的结果
        /// </summary>
        public List<double> HeDaoYanJin
        {
            get { return _HeDaoYanJin; }
            set { _HeDaoYanJin = value; }
        }

        private List<DuanMianResult> _DuanMianRes = new List<DuanMianResult>();

        [DataMember]
        public List<DuanMianResult> DuanMianRes
        {
            get { return _DuanMianRes; }
            set { _DuanMianRes = value; }
        }

        private List<WaterFlow> projectflow = new List<WaterFlow>();

        [DataMember]
        /// <summary>
        /// 洪峰流速
        /// </summary>
        public List<WaterFlow> ProjectFlow
        {
            get { return this.projectflow; }
            set { this.projectflow = value; }
        }
    }

    [DataContract]
    [Serializable]
    public class DemData:ModelObject
    {
        private long _HeDaoFID;

        [DataMember]
        /// <summary>
        /// 河道的FID
        /// </summary>
        public long HeDaoFID
        {
            get { return this._HeDaoFID; }
            set { this._HeDaoFID = value; }
        }

        private List<DuanMianDemData> _DuanMiandem = new List<DuanMianDemData>();

        [DataMember]
        /// <summary>
        /// 该河道的所有断面的Dem数据
        /// </summary>
        public List<DuanMianDemData> DuanMianDem
        {
            get { return _DuanMiandem; }
            set { _DuanMiandem = value; }
        }

    }

    [DataContract]
    [Serializable]
    public class WaterFlow:ModelObject
    {
        private long _HeDaoFID;

        [DataMember]
        /// <summary>
        /// 河道的FID
        /// </summary>
        public long HeDaoFID
        {
            get { return this._HeDaoFID; }
            set { this._HeDaoFID = value; }
        }

        private List<DuanMianLiuS> _DuanMiandem = new List<DuanMianLiuS>();

        [DataMember]
        /// <summary>
        /// 该河道的所有断面的Dem数据
        /// </summary>
        public List<DuanMianLiuS> DuanMianL
        {
            get { return _DuanMiandem; }
            set { _DuanMiandem = value; }
        }

    }

    [DataContract]
    [Serializable]
    public class DuanMianDemData:ModelObject
    {
        private long _DuanMianID;

        [DataMember]
        /// <summary>
        /// 断面ID
        /// </summary>
        public long DuanMianID
        {
            get { return this._DuanMianID; }
            set { this._DuanMianID = value; }
        }

        [DataMember]
        /// <summary>
        /// 计算的Dem高程数据
        /// </summary>
        public float DuanMianComputeDem
        {
            get;
            set;
        }
    }

    [DataContract]
    [Serializable]
    public class DuanMianLiuS:ModelObject
    {
        private long _DuanMianID;

        [DataMember]
        /// <summary>
        /// 断面ID
        /// </summary>
        public long DuanMianID
        {
            get { return this._DuanMianID; }
            set { this._DuanMianID = value; }
        }

        [DataMember]
        /// <summary>
        /// 计算的Dem高程数据
        /// </summary>
        public float DuanMianFlow
        {
            get;
            set;
        }
    }

    [DataContract]
    [Serializable]
    public class ProManage:ModelObject
    {

        private Project[] projectField;

        [DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Project")]
        public Project[] Project
        {
            get
            {
                return this.projectField;
            }
            set
            {
                this.projectField = value;
            }
        }
    }

    [DataContract]
    [Serializable]
    /// <summary>
    /// 断面的所有淹没范围过程和流速过程
    /// </summary>
    public class DuanMianAllFlow:ModelObject
    {
        [DataMember]
        public long DuanMianID
        {
            get;
            set;
        }
        private List<float> dmele = new List<float>();

        [DataMember]
        public List<float> DuanMianEle
        {
            get
            {
                return dmele;
            }
            set
            {
                dmele = value;
            }
        }
        private List<float> dmf = new List<float>();

        [DataMember]
        public List<float> DuanMianFlow
        {
            get
            {
                return dmf;
            }
            set
            {
                dmf = value;
            }
        }
    }
}

