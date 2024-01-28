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
    [KnownType(typeof(Source))]
    public class CIRP_ReceiveSystemInfo : ICIRP_ReceiveSystemInfo
    {
        [DataMember]
        public string SystemTaskID { get; set; }  

        [DataMember]
        public bool IsSelectTask { get; set; }

        /// <summary>
        ///  接报时刻
        /// </summary>
        [DataMember]
        public DateTime ReceiveInfo_Time { get; set; }

        /// <summary>
        ///  接报状态枚举
        /// </summary>
        [DataMember]
        public enumReceiveInfo_StatuType ReceiveInfo_Status { get; set; }

        /// <summary>
        ///  任务标题
        /// </summary>
        [DataMember]
        public string ReceiveInfo_TaskTitle { get; set; }

        /// <summary>
        /// 任务开始时刻
        /// </summary>
        [DataMember]
        public DateTime ReceiveInfo_TaskStartTime { get; set; }

        /// <summary>
        /// 任务结束时刻
        /// </summary>
        [DataMember]
        public DateTime ReceiveInfo_TaskEndTime { get; set; }

        [DataMember]
        public DateTime ReceiveInfo_HappenTime { get; set; }

        /// <summary>
        ///  分析类型枚举
        /// </summary> 
        [DataMember]
        public eumScenarioType ReceiveInfo_AnalysisType { get; set; }

        /// <summary>
        /// 任务状态枚举
        /// </summary>
        [DataMember]
        public enumReceiveInfo_TaskStatus ReceiveInfo_TaskStatus { get; set; }

        /// <summary>
        /// 执行进度枚举
        /// </summary>
        [DataMember]
        public enumReceiveInfo_ExecutionProgress ReceiveInfo_ExecutionProgress { get; set; }

        /// <summary>
        /// 接报人员
        /// </summary>
        [DataMember]
        public string ReceiveInfo_People { get; set; }



        //接报状态
        [DataMember]
        public string SelectRe_SatusType
        {
            get { return ReceiveInfo_Status.ToString(); }
            set { ReceiveInfo_Status = (enumReceiveInfo_StatuType)Enum.Parse(typeof(enumReceiveInfo_StatuType), value); }
        }

        [DataMember]
        public List<string> Re_SatusTypes
        {
            get { return Enum.GetNames(typeof(enumReceiveInfo_StatuType)).ToList<string>(); }

        }


        //分析类型枚举
        [DataMember]
        public string SelectReceiveInfo_AnalysisType
        {
            get { return ReceiveInfo_AnalysisType.ToString(); }
            set { ReceiveInfo_AnalysisType = (eumScenarioType)Enum.Parse(typeof(eumScenarioType), value); }
        }

        [DataMember]
        public List<string> ReceiveInfo_AnalysisTypsTypes
        {
            get { return Enum.GetNames(typeof(eumScenarioType)).ToList<string>(); }

        }


        //任务状态枚举
        [DataMember]
        public string SelectReceiveInfo_TaskStatus
        {
            get { return ReceiveInfo_TaskStatus.ToString(); }
            set { ReceiveInfo_TaskStatus = (enumReceiveInfo_TaskStatus)Enum.Parse(typeof(enumReceiveInfo_TaskStatus), value); }
        }

        [DataMember]
        public List<string> ReceiveInfo_TaskStatusTypes
        {
            get { return Enum.GetNames(typeof(enumReceiveInfo_TaskStatus)).ToList<string>(); }

        }


        //执行进度枚举
        [DataMember]
        public string SelectReceiveInfo_ExecutionProgressType
        {
            get { return ReceiveInfo_ExecutionProgress.ToString(); }
            set { ReceiveInfo_ExecutionProgress = (enumReceiveInfo_ExecutionProgress)Enum.Parse(typeof(enumReceiveInfo_ExecutionProgress), value); }
        }

        [DataMember]
        public List<string> ReceiveInfo_ExecutionProgressTypes
        {
            get { return Enum.GetNames(typeof(enumReceiveInfo_ExecutionProgress)).ToList<string>(); }

        }

        [DataMember]
        public string DataFilename { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public double Lng { get; set; }

    }

}
