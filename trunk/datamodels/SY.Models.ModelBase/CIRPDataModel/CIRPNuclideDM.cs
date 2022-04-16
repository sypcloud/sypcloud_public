using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    /// <summary>
    /// 接口ICIRPNuclideDM的实现
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(CIRPNuclideParaInfo))]
    public class CIRPNuclideDM : ICIRPNuclideDM
    {

        public CIRPNuclideDM()
        {
            CIRPNuclidePI =  new CIRPNuclideParaInfo();
            CIRPNuclidePI2 = new List<CIRPNuclideParaInfo>();
        }

        //获取核素信息
        [DataMember]
        public CIRPNuclideParaInfo CIRPNuclidePI { get; set; }

        //获取时间序列
        [DataMember]
        public List<CIRPNuclideParaInfo> CIRPNuclidePI2 { get; set; }

        /// <summary>
        /// 边界核素名称
        /// </summary>
        [DataMember]
        public enumBDNuclideName BDNuclideName { get; set; }

        /// <summary>
        /// 浓度、剂量、核素排放量枚举——源项
        /// </summary>
        [DataMember]
        public enumNuclideLeakageShowType NuclideShowType { get; set; }

        /// <summary>
        /// 浓度、剂量枚举——边界
        /// </summary>
        [DataMember]
        public enumNuclideBDShowType NuclideBDShowType { get; set; }

        [DataMember]
        public string SelectBDNuclideName { get { return BDNuclideName.ToString(); } set { BDNuclideName = (enumBDNuclideName)Enum.Parse(typeof(enumBDNuclideName), value); } }
        [DataMember]
        public List<string> BDNuclideNames { get { return Enum.GetNames(typeof(enumBDNuclideName)).ToList<string>(); } }

        [DataMember]
        public string SelectNuclideLeakageShowType { get { return NuclideShowType.ToString(); } set { NuclideShowType = (enumNuclideLeakageShowType)Enum.Parse(typeof(enumNuclideLeakageShowType), value); } }
        [DataMember]
        public List<string> NuclideLeakageShowTypeNames { get { return Enum.GetNames(typeof(enumNuclideLeakageShowType)).ToList<string>(); } }

        [DataMember]
        public string SelectNuclideBDShowType { get { return NuclideBDShowType.ToString(); } set { NuclideBDShowType = (enumNuclideBDShowType)Enum.Parse(typeof(enumNuclideBDShowType), value); } }
        [DataMember]
        public List<string> NuclideBDShowTypeNames { get { return Enum.GetNames(typeof(enumNuclideBDShowType)).ToList<string>(); } }
    }
}
