using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SY.Models.ModelBase
{
    [KnownType(typeof(XuManChanLiu))]
    [KnownType(typeof(DanWeiXian))]
    [KnownType(typeof(RunoffModelDatabase))]
    [KnownType(typeof(RoutingModelDatabase))]
    [KnownType(typeof(SY.Common.ViewModelBase))]
    [DataContract]
    [Serializable]
    /// <summary>
    /// 子流域模型
    /// </summary>
    public class ZiLiuYu : ModelObject
    {
        string _ziLiuYuName;
        
        [DataMember]
        /// <summary>
        /// 子流域名称
        /// </summary>
        public string ZiLiuYuName
        {
            get
            {
                return _ziLiuYuName;
            }
            set
            {
                _ziLiuYuName = value;
            }
        }

        long _fid;
        
        [DataMember]
        /// <summary>
        /// FID号
        /// </summary>
        public long Fid
        {
            get
            {
                return _fid;
            }
            set
            {
                _fid = value;
            }
        }

        string _ziLiuYuCode;
        
        [DataMember]
        /// <summary>
        /// 子流域编码
        /// </summary>
        public string ZiLiuYuCode
        {
            get
            {
                return _ziLiuYuCode;
            }
            set
            {
                _ziLiuYuCode = value;
            }
        }

        double _area;
        
        [DataMember]
        /// <summary>
        /// 流域面积
        /// </summary>
        public double Area
        {
            get
            {
                return _area;
            }
            set
            {
                _area = value;
            }
        }

        string _DownStreamType;
        
        [DataMember]
        /// <summary>
        /// 子流域下游单元类型
        /// </summary>
        public string DownStreamType
        {
            get
            {
                return _DownStreamType;
            }
            set
            {
                _DownStreamType = value;
            }
        }

        string _downstreamid;
        
        [DataMember]
        /// <summary>
        /// 下游单元
        /// </summary>
        public string DownstreamID
        {
            get
            {
                return _downstreamid;
            }
            set
            {
                _downstreamid = value;
            }
        }

        string _runoffModel = "蓄满产流";
        
        [DataMember]
        /// <summary>
        /// 产流模型
        /// </summary>
        public string RunoffModel
        {
            get
            {
                return _runoffModel;
            }
            set
            {
                _runoffModel = value;
            }
        }

        XuManChanLiu _xuManChanLiu;
        
        [DataMember]
        /// <summary>
        /// 蓄满产流
        /// </summary>
        public XuManChanLiu XuManChanLiu
        {
            get
            {
                return _xuManChanLiu;
            }
            set
            {
                _xuManChanLiu = value;
                this.OnPropertyChanged("XuManChanLiu");
            }
        }

        ChaoShenChanLiu _chaoShenChanLiu;
        
        [DataMember]
        /// <summary>
        /// 超渗产流
        /// </summary>
        public ChaoShenChanLiu ChaoShenChanLiu
        {
            get
            {
                return _chaoShenChanLiu;
            }
            set
            {
                _chaoShenChanLiu = value;
            }
        }

        [DataMember]
        [XmlArray(ElementName = "PrecipitationStationCode")]
        [XmlArrayItem(typeof(StationCode), ElementName = "Code")]
        public List<StationCode> PrecipitationSTCodes { get; set; }

        [DataMember]
        [XmlArray(ElementName = "PrecipitationData")]
        [XmlArrayItem(typeof(Precipitation), ElementName = "Precipitation")]
        public List<Precipitation> PrecipitationData { get; set; }

        [DataMember]
        [XmlArray(ElementName = "EvaporationData")]
        [XmlArrayItem(typeof(Evaporation), ElementName = "Evaporation")]
        public List<Evaporation> EvaporationData { get; set; }

        string _convergeModel = "单位线";
        
        [DataMember]
        /// <summary>
        /// 汇流模型
        /// </summary>
        public string ConvergeModel
        {
            get
            {
                return _convergeModel;
            }
            set
            {
                _convergeModel = value;
            }
        }

        DanWeiXian _danWeiXian;
        
        [DataMember]
        /// <summary>
        /// 单位线
        /// </summary>
        public DanWeiXian DanWeiXian
        {
            get
            {
                return _danWeiXian;
            }
            set
            {
                _danWeiXian = value;
                this.OnPropertyChanged("DanWeiXian");
            }
        }

        DiXiaJingLiuXianXingShuiKu _diXiaJingLiuXianXingShuiKu;
        
        [DataMember]
        /// <summary>
        /// 地下径流线性水库
        /// </summary>
        public DiXiaJingLiuXianXingShuiKu DiXiaJingLiuXianXingShuiKu
        {
            get
            {
                return _diXiaJingLiuXianXingShuiKu;
            }
            set
            {
                _diXiaJingLiuXianXingShuiKu = value;
            }
        }

        RangZhongLiuXianXingShuiKu _rangZhongLiuXianXingShuiKu;
        
        [DataMember]
        /// <summary>
        /// 壤中流线性水库
        /// </summary>
        public RangZhongLiuXianXingShuiKu RangZhongLiuXianXingShuiKu
        {
            get
            {
                return _rangZhongLiuXianXingShuiKu;
            }
            set
            {
                _rangZhongLiuXianXingShuiKu = value;
            }
        }

        bool _isChanCompuate = false;

        private ChanLiuResult _ChanResult;
        
        /// <summary>
        /// 产流结果
        /// </summary>
        [DataMember]
        public ChanLiuResult ChanResult
        {
            get { return this._ChanResult; }
            set { this._ChanResult = value; }
        }

        private HuiLiuResult _HuiResult;
        private HuiLiuResultEx _HuiResultEx;

        /// <summary>
        /// 汇流结果
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public HuiLiuResult HuiResult
        {
            get { return this._HuiResult; }
            set { this._HuiResult = value; }
        }

        [DataMember]
        public HuiLiuResultEx HuiResultEx
        {
            get { return this._HuiResultEx; }
            set { this._HuiResultEx = value; }
        }
        
        [DataMember]
        /// <summary>
        /// 判断产流计算
        /// </summary>
        public bool IsChanCompute
        {
            get
            {
                return _isChanCompuate;
            }
            set
            {
                _isChanCompuate = value;
            }
        }


        bool _isHuiCompuate = false;
        [DataMember]
        /// <summary>
        /// 判断汇流是否计算
        /// </summary>
        public bool IsHuiCompute
        {
            get
            {
                return _isHuiCompuate;
            }
            set
            {
                _isHuiCompuate = value;
            }
        }
        
        /// <summary>
        /// 前期降雨
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public List<SY.Common.Model.StationInfo> lstPreStationInfo { get; set; }

        /// <summary>
        /// 预报期降雨
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public List<SY.Common.Model.StationInfo> lstAftStationInfo { get; set; }
        /// <summary>
        /// 应用于该子流域的雨量站信息
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public List<SY.Common.Model.StationInfo> AppliedStationInfo { get; set; }
        /// <summary>
        /// 子流域产流模型库
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public RunoffModelDatabase RunoffModelDB { get; set; }
        /// <summary>
        /// 子流域汇流模型库
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public RoutingModelDatabase RoutingModelDB { get; set; }
        [XmlIgnore]
        [DataMember]
        public float ForecastedRainfall { get; set; }
    }

    [DataContract]
    [Serializable]
    public class GptoolsConfig
    {
        [DataMember]
        [XmlAttribute(AttributeName = "name")]
        public string ToolName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "url")]
        public string ToolUrl { get; set; }
    }
    /// <summary>
    /// 产流模型库
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(RunoffModelSet))]
    public class RunoffModelDatabase
    {
        /// <summary>
        /// 模型集合
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public List<RunoffModelSet> ModelCollection { get; set; }
    }
    /// <summary>
    /// 模型配置
    /// </summary>
    [DataContract]
    [Serializable]
    public class RunoffModelSet
    {
        /// <summary>
        /// 模型名称enum值
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public int ModelName { get; set; }
        /// <summary>
        /// 相应模型参数索引
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public int ModelParameterIndex { get; set; }
    }
    /// <summary>
    /// 汇流模型库
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(RoutingfModelSet))]
    public class RoutingModelDatabase
    {
        /// <summary>
        /// 模型集合
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public List<RoutingfModelSet> ModelCollection { get; set; }
    }
    /// <summary>
    /// 汇流模型配置
    /// </summary>
    [DataContract]
    [Serializable]
    public class RoutingfModelSet
    {
        /// <summary>
        /// 汇流模型名称enum值
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public int ModelName { get; set; }
        /// <summary>
        /// 相应模型参数索引
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public int ModelParameterIndex { get; set; }
    }    
}
