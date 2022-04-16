using SY.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SY.Models.ModelBase
{
    public enum eumRunoffModelType
    {
        蓄满产流,
        超渗产流
    }

    public enum eumAccumulationModelType
    {
        单位线,
        地下径流线性水库,
        壤中流线性水库
    }

    public enum eumRoutingModelType
    {
        马斯京根
    }

    public enum eumElementType
    {
        河道,
        水库
    }

    [KnownType(typeof(Outlet))]
    [KnownType(typeof(ZiLiuYu))]
    [KnownType(typeof(HeDao))]
    [KnownType(typeof(ViewModelBase))]
    [Serializable]
    [DataContract]
    /// <summary>
    /// 小流域模型
    /// </summary>
    public class XiaoLiuYu
    {
        /// <summary>
        /// 所属流域
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ParentBasinName")]
        public string ParentBasinName { get; set; }
        /// <summary>
        /// 流域出口
        /// </summary>
        [DataMember]
        public Outlet BasinOutlet { get; set; }

        private ModelTime _scenarioModelTime;

        [DataMember]
        public ModelTime ScenarioModelTime
        {
            get { return _scenarioModelTime; }
            set { _scenarioModelTime = value; }
        }

        [DataMember]
        [XmlElement(ElementName = "ServerUrl")]
        public MapServiceConfig MapServices { get; set; }

        [DataMember]
        [XmlElement(ElementName = "PrecipitationStationFeatureUrl")]
        public MapServiceConfig PrecipitationStationServices { get; set; }

        [DataMember]
        [XmlElement(ElementName = "CatchmentFeatureUrl")]
        public MapServiceConfig CatchmentFeatureUrl { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ServerGpTools")]
        [XmlArrayItem(typeof(GptoolsConfig), ElementName = "Tool")]
        public List<GptoolsConfig> GpTools { get; set; }

        [DataMember]
        [XmlArray(ElementName = "BasinBaseInfo")]
        [XmlArrayItem(typeof(BasinBaseSection), ElementName = "BasinBaseSection")]
        public List<BasinBaseSection> BasinBaseSectionList { get; set; }

        string _xiaoLiuYuName;
        [DataMember]
        /// <summary>
        /// 小流域名称
        /// </summary>
        public string XiaoLiuYuName
        {
            get
            {
                return _xiaoLiuYuName;
            }
            set
            {
                _xiaoLiuYuName = value;
            }
        }
        string _xiaoLiuYuCode;
        [DataMember]
        /// <summary>
        /// 小流域编码
        /// </summary>
        public string XiaoLiuYuCode
        {
            get
            {
                return _xiaoLiuYuCode;
            }
            set
            {
                _xiaoLiuYuCode = value;
            }
        }

        List<ZiLiuYu> _lstZiLiuYu = new List<ZiLiuYu>();

        [DataMember]
        /// <summary>
        /// 子流域集合
        /// </summary>
        public List<ZiLiuYu> LstZiLiuYu
        {
            get
            {
                return _lstZiLiuYu;
            }
            set
            {
                _lstZiLiuYu = value;
            }
        }

        List<HeDao> _lstHeDao = new List<HeDao>();
        
        [DataMember]
        /// <summary>
        /// 河道集合
        /// </summary>
        public List<HeDao> LstHeDao
        {
            get
            {
                return _lstHeDao;
            }
            set
            {
                _lstHeDao = value;
            }
        }


        List<ShuiKu> _lstShuiKu = new List<ShuiKu>();

        [XmlIgnore]
        [DataMember]
        /// <summary>
        /// 工程设施集合，主要指水库
        /// </summary>
        public List<ShuiKu> LstShuiKu
        {
            get
            {
                return _lstShuiKu;
            }
            set
            {
                _lstShuiKu = value;
            }
        }

        List<JiaoHuiDian> _lstJiaoHuiDian = new List<JiaoHuiDian>();
        
        [XmlIgnore]
        [DataMember]
        /// <summary>
        /// 交汇点集合
        /// </summary>
        public List<JiaoHuiDian> LstJiaoHuiDian
        {
            get
            {
                return _lstJiaoHuiDian;
            }
            set
            {
                _lstJiaoHuiDian = value;
            }
        }

        List<FenChaDian> _lstFenChaDian = new List<FenChaDian>();

        [XmlIgnore]
        [DataMember]
        /// <summary>
        /// 分汊点集合
        /// </summary>
        public List<FenChaDian> LstFenChaDian
        {
            get
            {
                return _lstFenChaDian;
            }
            set
            {
                _lstFenChaDian = value;
            }
        }

        List<Yuan> _lstYuan = new List<Yuan>();

        [XmlIgnore]
        [DataMember]
        /// <summary>
        /// 源点集合
        /// </summary>
        public List<Yuan> LstYuan
        {
            get
            {
                return _lstYuan;
            }
            set
            {
                _lstYuan = value;
            }
        }

        List<Hui> _lstHui = new List<Hui>();

        [XmlIgnore]
        [DataMember]
        /// <summary>
        /// 汇点集合
        /// </summary>
        public List<Hui> LstHui
        {
            get
            {
                return _lstHui;
            }
            set
            {
                _lstHui = value;
            }
        }

        private List<YanJinResult> yanJinresult=new List<YanJinResult>();

        [DataMember]
        public List<YanJinResult> YanJinResult
        {
            get { return this.yanJinresult; }
            set { this.yanJinresult = value; }
        }
        
        private List<DemData> projectdemdata = new List<DemData>();
        
        [XmlIgnore]
        [DataMember]
        /// <summary>
        /// 所有河道的断面高程
        /// </summary>
        public List<DemData> ProjecctDemData
        {
            get { return this.projectdemdata; }
            set { this.projectdemdata = value; }
        }
        
        private List<DuanMianAllFlow> maxDmList = new List<DuanMianAllFlow>();

        [XmlIgnore]
        [DataMember]
        public List<DuanMianAllFlow> MaxDmList
        {
            get { return maxDmList; }
            set { maxDmList = value; }
        }

        public override string ToString()
        {
            return this._xiaoLiuYuName;
        }
        
        /// <summary>
        /// 得到所有起始河道的序号（有可能会包括最后一个河道）
        /// </summary>
        /// <returns></returns>
        public List<int> GetFirstHeDaoIndexs()
        {
            List<long> lstFids = new List<long>();
            List<int> lstIndex = new List<int>();
            if (_lstHeDao == null || _lstHeDao.Count == 0)
                return lstIndex;

            if (_lstHeDao.Count == 1)
            {
                lstIndex.Add(0);
                return lstIndex;
            }

            // 首先把所有的序号都加加进去
            for (int i = 0; i < _lstHeDao.Count; i++)
            {
                lstFids.Add(_lstHeDao[i].Fid);
            }

            long ntmp = -1;
            // 去除所有的下游单元
            for (int i = 0; i < _lstHeDao.Count; i++)
            {
                if (long.TryParse(_lstHeDao[i].DownstreamID, out ntmp))
                {
                    if (_lstHeDao[i].DownStreamType == "河道")
                    {
                        lstFids.Remove(ntmp);
                    }
                }
            }

            // 查找FID所对应的序号
            for (int f = 0; f < lstFids.Count; f++)
            {
                for (int i = 0; i < _lstHeDao.Count; i++)
                {
                    if (lstFids[f] == _lstHeDao[i].Fid)
                        lstIndex.Add(i);
                }
            }
            return lstIndex;
        }
        
        /// <summary>
        /// 根据河道的FID查找对应的河道
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public HeDao GetHeDaoByFID(long fid)
        {
            if (_lstHeDao == null || _lstHeDao.Count == 0)
                return null;

            for (int i = 0; i < _lstHeDao.Count; i++)
            {
                if (_lstHeDao[i].Fid == fid)
                    return _lstHeDao[i];
            }

            return null;
        }
    }

    [DataContract]
    public partial class MapServiceConfig
    {
        [DataMember]
        [XmlAttribute(AttributeName = "displayname")]
        public string DisplayName { get; set; }
        [DataMember]
        [XmlAttribute(AttributeName = "url")]
        public string ServerUrl { get; set; }
    }

    [DataContract]
    [Serializable]
    public class StationCode
    {
        [DataMember]
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "weight")]
        public float Weight { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Precipitation
    {
        [DataMember]
        /// <summary>
        /// 累积时间，单位以秒计
        /// </summary>
        [XmlAttribute(AttributeName = "time")]
        public float TimeStep { get; set; }
        [DataMember]
        [XmlAttribute(AttributeName = "value")]
        public float Value { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Evaporation
    {
        [DataMember]
        /// <summary>
        /// 累积时间，单位以秒计
        /// </summary>
        [XmlAttribute(AttributeName = "time")]
        public string Time { get; set; }
        [DataMember]
        [XmlAttribute(AttributeName = "value")]
        public float EvaporationValue { get; set; }
    }

    [DataContract]
    [Serializable]
    public class BasinBaseSection
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }
    }
}
