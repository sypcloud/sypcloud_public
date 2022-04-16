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
    /// 交汇点
    /// </summary>
    public class JiaoHuiDian
    {
        string _jiaoHuiDianName;
        [DataMember]
        /// <summary>
        /// 交汇点名称
        /// </summary>
        public string JiaoHuiDianName
        {
            get
            {
                return _jiaoHuiDianName;
            }
            set
            {
                _jiaoHuiDianName = value;
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

        string _jiaoHuiDiancode;
        [DataMember]
        /// <summary>
        /// 交汇点编码
        /// </summary>
        public string JiaoHuiDianCode
        {
            get
            {
                return _jiaoHuiDiancode;
            }
            set
            {
                _jiaoHuiDiancode = value;
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
    }
}
