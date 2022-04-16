using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    /// <summary>
    /// 水库
    /// </summary>
    public class ShuiKu:ModelObject
    {
        string _shuiKuName;
        [DataMember]
        /// <summary>
        /// 水库名称
        /// </summary>
        public string ShuiKuName
        {
            get
            {
                return _shuiKuName;
            }
            set
            {
                _shuiKuName = value;
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

        string _shuiKucode;
        [DataMember]
        /// <summary>
        /// 水库编码
        /// </summary>
        public string ShuiKuCode
        {
            get
            {
                return _shuiKucode;
            }
            set
            {
                _shuiKucode = value;
            }
        }

        string _DownStreamType;
        [DataMember]
        /// <summary>
        /// 水库下游单元类型
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

        DataTable _tabEffusionAbility;
        [DataMember]
        /// <summary>
        /// 泄流能力表
        /// </summary>
        public DataTable TabEffusionAbility
        {
            get
            {
                return _tabEffusionAbility;
            }
            set
            {
                _tabEffusionAbility = value;
            }
        }
        [DataMember]
        /// <summary>
        /// 库容
        /// </summary>
        public float Capacity
        {
            get;
            set;
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
        private ShuiKuResult _SKResult;
        [DataMember]
        public ShuiKuResult SKResult
        {
            get { return this._SKResult; }
            set { this._SKResult = value; }
        }
    }
}
