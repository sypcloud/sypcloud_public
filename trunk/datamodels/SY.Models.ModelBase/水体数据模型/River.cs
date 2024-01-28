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
    [KnownType(typeof(Crosssection))]
    public class River : RiverBase
    {
        [DataMember]
        /// <summary>
        /// 只存RvrMdCode
        /// </summary>
        public List<string> UpRvr { get; set; }

        [DataMember]
        /// <summary>
        /// 只存RvrMdCode
        /// </summary>
        public List<string> DnRvr { get; set; }

        [DataMember]
        public List<Crosssection> XSectoin { get; set; }
    }
}
