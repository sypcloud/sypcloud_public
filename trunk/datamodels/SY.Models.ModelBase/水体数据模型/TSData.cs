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
    public class TSData:ITSData
    {
        [DataMember]
        public DateTime DT { get; set; }

        [DataMember]
        public float Data { get; set; }
    }
    
    [DataContract]
    public class RelationData:IRelationData
    {
        [DataMember]
        public object A { get; set; }

        [DataMember]
        public object B { get; set; }
    }

    [DataContract]
    [KnownType(typeof(TSData))]
    public class TSDataCollection : ITSDataCollection
    {
        public TSDataCollection()
        {
            LstTSData = new List<TSData>();
        }

        [DataMember]
        public List<TSData> LstTSData { get; set; }
    }
}
