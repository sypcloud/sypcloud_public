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
    /// 汇点
    /// </summary>
    public class Hui
    {
        [DataMember]
        /// <summary>
        /// 汇的名称
        /// </summary>
        public string HuiName
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
        /// 汇的编码
        /// </summary>
        public string HuiCode
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
