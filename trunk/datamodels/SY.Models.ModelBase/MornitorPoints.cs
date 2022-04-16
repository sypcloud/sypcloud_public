using Sy.Global;
using SY.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    [KnownType(typeof(PointD))]
    [KnownType(typeof(TSData))]
    [KnownType(typeof(Location))]
    public class MornitorPoint
    {
        private List<PointD> location=new List<PointD>();
        private List<TSData> obsTimeSeriers;
        private List<TSData> simTimeSeriers;
        private List<int> elements=new List<int>();
        private List<Location> location2=new List<ModelBase.Location>();

        public MornitorPoint()
        {
            Location2 = new List<ModelBase.Location>();
        }
        /// <summary>
        /// 监控点名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 监控点数据类型（水位、流量、流速...)
        /// </summary>
        [DataMember]
        public eumDataType DataType { get; set; }
        /// <summary>
        /// 监控点位置
        /// </summary>
        [DataMember]
        public List<PointD> Location { get { return location; } set { value = location; } }

        [DataMember]
        public List<Location> Location2 { get { return location2; } set { value = location2; } }

        [DataMember]
        public List<int> Elements { get { return elements; } set { value = elements; } }
        /// <summary>
        /// 监控点实测时间序列
        /// </summary>
        [DataMember]
        public List<TSData> ObsTimeSeriers { get { return obsTimeSeriers; } set { value = obsTimeSeriers; } }
        /// <summary>
        /// 监控点模拟时间序列
        /// </summary>
        [DataMember]
        public List<TSData> SimTimeSeriers { get { return simTimeSeriers; } set { value = simTimeSeriers; } }

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Location
    {
        [DataMember]
        public string Atribute;
        [DataMember]
        public double X;
        [DataMember]
        public double Y;
    }
}
