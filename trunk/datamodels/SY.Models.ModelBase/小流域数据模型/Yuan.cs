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
    /// 源点
    /// </summary>
    public class Yuan
    {
        [DataMember]        
        /// <summary>
        /// 源的名称
        /// </summary>
        public string YuanName
        {
            get;
            set;
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

        [DataMember]
        /// <summary>
        /// 源的编码
        /// </summary>
        public string YuanCode
        {
            get;
            set;
        }

        string _belongId;
        [DataMember]
        /// <summary>
        /// 所属单元
        /// </summary>
        public string BelongID
        {
            get
            {
                return _belongId;
            }
            set
            {
                _belongId = value;
            }
        }

        DataTable _tabFlowProcess;
        [DataMember]
        /// <summary>
        /// 出流过程表
        /// </summary>
        public DataTable TabFlowProcess
        {
            get
            {
                return _tabFlowProcess;
            }
            set
            {
                _tabFlowProcess = value;
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
    }
}
