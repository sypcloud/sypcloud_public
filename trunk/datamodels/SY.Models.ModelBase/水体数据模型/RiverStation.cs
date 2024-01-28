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
    /// <summary>
    /// 移除，归到riverbase类
    /// </summary>
    public class RiverStation
    {
        [DataMember]
        public string riverName { get; set; }

        [DataMember]
        public string reachName { get; set; }

        [DataMember]
        public string station { get; set; }

        [DataMember]
        public string station2 { get; set; }
    }
}
