using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
     [DataContract]
    public class AssessmentResult
    {
         /// <summary>
         /// 平均绝对误差
         /// </summary>
         [DataMember]
         public float AverageAbsoluteError { get; set; }
         /// <summary>
         /// 平均相对误差
         /// </summary>
         [DataMember]
         public float AverageRelativeError { get; set; }
         /// <summary>
         /// 相关系数
         /// </summary>
         [DataMember]
         public float DeterministicCoefficient { get; set; }
    }
}
