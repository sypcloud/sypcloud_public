using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    [KnownType(typeof(MaSiJingGen))]
    [KnownType(typeof(FloodingModelDatabase))]
    [KnownType(typeof(SY.Common.ViewModelBase))]
    /// <summary>
    /// 河道
    /// </summary>
    public class HeDao : ModelObject
    {
        string _heDaoName;
        [DataMember]
        /// <summary>
        /// 河道名称
        /// </summary>
        public string HeDaoName
        {
            get
            {
                return _heDaoName;
            }
            set
            {
                _heDaoName = value;
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

        string _DownStreamType;
        [DataMember]
        /// <summary>
        /// 河道下游单元类型
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

        string _heDaoCode;
        [DataMember]
        /// <summary>
        /// 河道编码
        /// </summary>
        public string HeDaoCode
        {
            get
            {
                return _heDaoCode;
            }
            set
            {
                _heDaoCode = value;
            }
        }

        double _poJiang;
        [DataMember]
        /// <summary>
        /// 河道坡降
        /// </summary>
        public double PoJiang
        {
            get
            {
                return _poJiang;
            }
            set
            {
                _poJiang = value;
            }
        }

        double _caoLv;
        [DataMember]
        /// <summary>
        /// 河道糙率
        /// </summary>
        public double CaoLv
        {
            get
            {
                return _caoLv;
            }
            set
            {
                _caoLv = value;
            }
        }

        double _length;
        [DataMember]
        /// <summary>
        /// 河道长度
        /// </summary>
        public double Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
            }
        }

        string _downstreamid;
        [XmlElement("DownstreamID")]
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

        List<DuanMian> _lstDuanMian = new List<DuanMian>();
        
        [DataMember]
        /// <summary>
        /// 断面集合
        /// </summary>
        public List<DuanMian> LstDuanMian
        {
            get
            {
                return _lstDuanMian;
            }
            set
            {
                _lstDuanMian = value;
            }
        }

        string _calculateModel = "1D水动力学";
        
        [DataMember]
        /// <summary>
        /// 运算模型
        /// </summary>
        public string CalculateModel
        {
            get
            {
                return _calculateModel;
            }
            set
            {
                _calculateModel = value;
            }
        }

        OneDShuiDongLiXue _oneDShuiDongLiXue;
        [DataMember]
        /// <summary>
        /// 1D水动力学模型
        /// </summary>
        public OneDShuiDongLiXue OneDShuiDongLiXue
        {
            get
            {
                return _oneDShuiDongLiXue;
            }
            set
            {
                _oneDShuiDongLiXue = value;
            }
        }

        MaSiJingGen _maSiJingGen;
        
        [DataMember]
        /// <summary>
        /// 马斯京根模型
        /// </summary>
        public MaSiJingGen MaSiJingGen
        {
            get
            {
                return _maSiJingGen;
            }
            set
            {
                _maSiJingGen = value;
                this.OnPropertyChanged("MaSiJingGen");
            }
        }
        bool _isCompuate = false;
        
        [DataMember]
        /// <summary>
        /// 判断是否计算
        /// </summary>
        public bool IsCompute
        {
            get
            {
                return _isCompuate;
            }
            set
            {
                _isCompuate = value;
            }
        }
        /// <summary>
        /// 得到第一个断面的序号
        /// </summary>
        /// <returns>第一个断面所在列表中的序号</returns>
        public int GetFirstDuanMianIndex()
        {
            if (_lstDuanMian == null || _lstDuanMian.Count == 0)
                return -1;

            if (_lstDuanMian.Count == 1)
                return 0;

            // 先记录所有的断面的FID
            List<long> lstIndex = new List<long>();
            for (int i = 0; i < _lstDuanMian.Count; i++)
                lstIndex.Add(_lstDuanMian[i].Fid);

            int ntmp = 0;
            // 去除下断面为空的、所有下断面的对象
            for (int i = 0; i < _lstDuanMian.Count; i++)
            {
                if (string.IsNullOrEmpty(_lstDuanMian[i].DownStreamID))   // 最后一个断面
                {
                    lstIndex.Remove(_lstDuanMian[i].Fid);
                    continue;
                }

                if (int.TryParse(_lstDuanMian[i].DownStreamID, out ntmp))  // 有下断面，则去除下一个断面
                {
                    lstIndex.Remove(ntmp);
                    continue;
                }
            }

            // 假如仅剩下一个对象，则此为第一个断面，否则不存在
            if (lstIndex.Count == 1)
            {
                // 查找FID所对应的序号
                for (int i = 0; i < _lstDuanMian.Count; i++)
                {
                    if (_lstDuanMian[i].Fid == lstIndex[0])
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 根据断面的FID获取对应的断面
        /// </summary>
        /// <param name="fid">断面的FID</param>
        /// <returns>假如存在返回断面，否则返回空</returns>
        public DuanMian GetDuanMianByFID(long fid)
        {
            if (_lstDuanMian == null || _lstDuanMian.Count == 0)
                return null;

            for (int i = 0; i < _lstDuanMian.Count; i++)
            {
                if (_lstDuanMian[i].Fid == fid)
                    return _lstDuanMian[i];
            }

            return null;
        }

        /// <summary>
        /// 计算河道的最后一个断面
        /// </summary>
        /// <returns></returns>
        public DuanMian GetLastDuanMian()
        {
            if(_lstDuanMian == null )
                return null;

            for (int i = 0; i < _lstDuanMian.Count; i++)
            {
                if (string.IsNullOrEmpty(_lstDuanMian[i].DownStreamID))
                    return _lstDuanMian[i];
            }

            return null;
        }
        /// <summary>
        /// 子流域演进模型库
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public FloodingModelDatabase FloodingModelDB { get; set; }
    }
    /// <summary>
    /// 演进模型库
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(FloodingModelSet))]
    public class FloodingModelDatabase
    {
        /// <summary>
        /// 演进模型集合
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public List<FloodingModelSet> ModelCollection { get; set; }
    }
    /// <summary>
    /// 演进模型
    /// </summary>
    [DataContract]
    [Serializable]
    public class FloodingModelSet
    {
        /// <summary>
        /// 模型名称enum值
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public int ModelName { get; set; }
        /// <summary>
        /// 模型参数索引
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public int ModelParameterIndex { get; set; }
    }
}
