using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    public class Source: ISource
    {
        public Source()
        {
            //实例化
            CaseLeakage_ListNuclideNames = new List<string>();
        }
        [DataMember]
        public string CaseLeakage_Name { get; set; }

        [DataMember]
        public float CaseLeakage_Longitude { get; set; }

        [DataMember]
        public float CaseLeakage_Latitude { get; set; }

        [DataMember]
        public List<string> CaseLeakage_ListNuclideNames { get; set; }


    }
}
