using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    public class TSData2D:ITSData2D
    {
        [DataMember]
        public DateTime DT { get; set; }

        [DataMember]
        public float[] Data { get; set; }

        [DataMember]
        public float[] Data1 { get; set; }

        [DataMember]
        public float[] Data2 { get; set; }

        [DataMember]
        public float[] Data3 { get; set; }
    }

    [DataContract]
    [KnownType(typeof(TSData2D))]
    public class TSData2DCollection : ITSData2DCollection
    {
        public TSData2DCollection()
        {
            LstTSData2D = new List<TSData2D>();
        }

        [DataMember]
        public List<TSData2D> LstTSData2D { get; set; }
    }
}
