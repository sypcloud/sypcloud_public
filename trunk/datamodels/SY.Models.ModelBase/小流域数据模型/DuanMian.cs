using System;
using System.Collections;//在C#中使用ArrayList必须引用Collections类
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    /// <summary>
    /// 断面
    /// </summary>
    public class DuanMian
    {
        string _duanMianName;
        [DataMember]
        /// <summary>
        /// 断面名称
        /// </summary>
        public string DuanMianName
        {
            get
            {
                return _duanMianName;
            }
            set
            {
                _duanMianName = value;
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

        string _duanMianCode;
        [DataMember]
        /// <summary>
        /// 断面编码
        /// </summary>
        public string DuanMianCode
        {
            get
            {
                return _duanMianCode;
            }
            set
            {
                _duanMianCode = value;
            }
        }

        string _belongID;
        [DataMember]
        /// <summary>
        /// 所属单元
        /// </summary>
        public string BelongID
        {
            get
            {
                return _belongID;
            }
            set
            {
                _belongID = value;
            }
        }

        string _downStreamID;
        [DataMember]
        /// <summary>
        /// 下游断面编码
        /// </summary>
        public string DownStreamID
        {
            get
            {
                return _downStreamID;
            }
            set
            {
                _downStreamID = value;
            }
        }

        bool _isHaveKX = true;
        [DataMember]
        /// <summary>
        /// 断面是否有KX值
        /// </summary>
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
        [DefaultValue(typeof(string),"1")]
        [DataMember]
        /// <summary>
        /// 断面形状,"1"代表抛物型
        /// </summary>
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

        float _distance;
        [DataMember]
        /// <summary>
        /// 断面间距
        /// </summary>
        public float Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                _distance = value;
            }
        }

        DataTable _tabCoordinate;
        [DataMember]
        /// <summary>
        /// 坐标集合表
        /// </summary>
        public DataTable TabCoordinate
        {
            get
            {
                return _tabCoordinate;
            }
            set
            {
                _tabCoordinate = value;
            }
        }

        //ArrayList _Coordinate;
        ///// <summary>
        ///// 坐标集合表
        ///// </summary>
        //public ArrayList Coordinate
        //{
        //    get
        //    {
        //        return _Coordinate;
        //    }
        //    set
        //    {
        //        _Coordinate = value;
        //    }
        //}

        public float _Downlevel;
        [DataMember]
        /// <summary>
        /// 下垫面高程
        /// </summary>
        public float DowmLevel
        {
            get { return this._Downlevel; }
            set { this._Downlevel = value; }
        }


        List<float> _result;
        
        [DataMember]
        /// <summary>
        /// 计算结果
        /// </summary>
        public List<float> Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }
        /// <summary>
        /// 指明是否为全流域出口断面
        /// </summary>
        [DataMember]
        public bool IsOutlet { get; set; }

        public override string ToString()
        {
            return this._duanMianName;
        }
    }
}
