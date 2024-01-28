using SY.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{

    /// <summary>
    ///  接报状态枚举
    /// </summary>
    public enum enumReceiveInfo_StatuType
    {
        未接收,
        已接收
    }


    /// <summary>
    ///  分析类型枚举
    /// </summary>
    public enum enumReceiveInfo_AnalysisType
    {
        港区模式,
        海域模式
    }


    /// <summary>
    ///  任务状态枚举
    /// </summary>
    public enum enumReceiveInfo_TaskStatus
    {
        未计算,
        正在计算,
        计算完成,
        取消计算,
        计算异常
    }


    /// <summary>
    ///  执行进度枚举
    /// </summary>
    public enum enumReceiveInfo_ExecutionProgress
    {
        未完成,
        已完成
    }




    public interface ICIRP_ReceiveSystemInfo
    {

        string SystemTaskID { get; set; }

        bool IsSelectTask { get; set; }

        /// <summary>
        ///  接报时刻
        /// </summary>
        DateTime ReceiveInfo_Time { get; set; }

        /// <summary>
        ///  接报状态枚举
        /// </summary>
        enumReceiveInfo_StatuType ReceiveInfo_Status { get; set; }

        /// <summary>
        ///  任务标题
        /// </summary>
        string ReceiveInfo_TaskTitle { get; set; }

        /// <summary>
        /// 任务开始时刻
        /// </summary>
        DateTime ReceiveInfo_TaskStartTime { get; set; }

        /// <summary>
        /// 任务结束时刻
        /// </summary>
        DateTime ReceiveInfo_TaskEndTime { get; set; }

        /// <summary>
        /// 任务事故计算时刻
        /// </summary>
        DateTime ReceiveInfo_HappenTime { get; set; }

        /// <summary>
        ///  分析类型枚举
        /// </summary> 
        eumScenarioType ReceiveInfo_AnalysisType { get; set; }

        /// <summary>
        /// 任务状态枚举
        /// </summary>
        enumReceiveInfo_TaskStatus ReceiveInfo_TaskStatus { get; set; }

        /// <summary>
        /// 执行进度枚举
        /// </summary>
        enumReceiveInfo_ExecutionProgress ReceiveInfo_ExecutionProgress { get; set; }

        /// <summary>
        /// 接报人员
        /// </summary>
        string ReceiveInfo_People { get; set; }



    }
}
