using SY.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public enum enumBDNuclideName
    {
        Co60,
        I131
    }

    public enum enumNuclideLeakageShowType
    {
        浓度,
        剂量,
        核素排放量
    }

    public enum enumNuclideBDShowType
    {
        浓度,
        剂量,
    }

    //核素数据模型接口
    public interface ICIRPNuclideDM
    {
        //定义接口成员
        [DataMember]
        CIRPNuclideParaInfo CIRPNuclidePI { get; set; }

        [DataMember]
        List<CIRPNuclideParaInfo> CIRPNuclidePI2 { get; set; }

        [DataMember]
        enumBDNuclideName BDNuclideName { get; set; }

        [DataMember]
        enumNuclideLeakageShowType NuclideShowType { get; set; }
    }
    [DataContract]
    [Serializable]
    public class CIRPNuclideParaInfo
    {
        private bool isconstant_InitialNuclide = true;
        private bool isfile_InitialNuclide;
        private bool isuserSetLeakageNuclide = true;
        private bool isbatchImportLeakageNuclide;
        private bool isSelectNuclide=true ;

        /// <summary>
        /// 初始全局核素浓度
        /// </summary>
        [DataMember]
        public double InitialGlobalConcentration_Nuclide { get; set; }
        /// <summary>
        /// radio初始全局常数核素浓度
        /// </summary>
        [DataMember]
        public bool IsConstant_InitialNuclide
        {
            get { return isconstant_InitialNuclide; }
            set
            {
                isconstant_InitialNuclide = value;
                if (value)
                {
                    isfile_InitialNuclide = false;
                }
            }
        }
        /// <summary>
        /// radio初始全局文件核素浓度
        /// </summary>
        [DataMember]
        public bool IsFile_InitialNuclide
        {
            get { return isfile_InitialNuclide; }
            set
            {
                isfile_InitialNuclide = value;
                if (value)
                {
                    isconstant_InitialNuclide = false;
                }
            }
        }
        /// <summary>
        /// 事故发生时间
        /// </summary>
        [DataMember]
        public DateTime CaseLeakageOccurTime{ get; set; }
        /// <summary>
        /// 用户设定核素
        /// </summary>
        [DataMember]
        public bool IsUserSetLeakageNuclide
        {
            get { return isuserSetLeakageNuclide; }
            set
            {
                isuserSetLeakageNuclide = value;
                if (value)
                {
                    isbatchImportLeakageNuclide = false;
                }
            }
        }
        /// <summary>
        /// 批量导入核素
        /// </summary>
        [DataMember]
        public bool IsBatchImportLeakageNuclide
        {
            get { return isbatchImportLeakageNuclide; }
            set
            {
                isbatchImportLeakageNuclide = value;
                if (value)
                {
                    isuserSetLeakageNuclide = false;
                }
            }

        }
        /// <summary>
        /// 默认选择选中核素种类
        /// </summary>
        [DataMember]
        public bool IsSelectNuclide
        {
            get { return isSelectNuclide; }
            set { isSelectNuclide = value; }

        }

        /// <summary>
        /// 核素数量
        /// </summary>
        [DataMember]
        public int numNuclide { get; set; }

        /// <summary>
        /// 核素序号，第几个核素
        /// </summary>
        [DataMember]
        public int orderNuclide { get; set; }

        /// <summary>
        /// 核素名称
        /// </summary>
        [DataMember]
        public string NuclideName { get; set; }
        /// <summary>
        /// 核素扩散系数
        /// </summary>
        [DataMember]
        public double NuclideDispersionCoefficient { get; set; }
        /// <summary>
        /// 核素A半衰期
        /// </summary>
        [DataMember]
        public double NuclideHalfTime { get; set; }
        /// <summary>
        /// 核素B衰减系数
        /// </summary>
        [DataMember]
        public double NuclideDecay { get; set; }
        /// <summary>
        /// 核素核素转化因子
        /// </summary>
        [DataMember]
        public double NuclideConversionFactor { get; set; }
        /// <summary>
        /// 核素泄漏时间
        /// </summary>
        [DataMember]
        public DateTime NuclideLeakageTime { get; set; }
        /// <summary>
        /// 核素泄漏浓度
        /// </summary>
        [DataMember]
        public double NuclideLeakageCon { get; set; }
        /// <summary>
        /// 核素泄漏量、排放量
        /// </summary>
        [DataMember]
        public double NuclideLeakageDischarge { get; set; }
        /// <summary>
        /// 核素排放剂量
        /// </summary>
        [DataMember]
        public double NuclideLeakageDoes { get; set; }
        /// <summary>
        /// 核素排放质量
        /// </summary>
        [DataMember]
        public double NuclideMass { get; set; }


        //*********tt-2018-9-18


    }
}
