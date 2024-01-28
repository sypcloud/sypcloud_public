using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SY.Models.ModelBase;

namespace SY.Models.Scenario
{
    [DataContract]
    [KnownType(typeof(ModelTime))]
    public class ScenarionTime:IScenarionTime
    {
        public ScenarionTime()
        {
            //初始化
            ModelTimePara = new ModelTime();
            
        }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public DateTime ModifyTime { get; set; }

        [DataMember]
        public ModelTime ModelTimePara { get; set; }   
    }
}
