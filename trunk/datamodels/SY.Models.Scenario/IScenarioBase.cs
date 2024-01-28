using SY.Common;
using SY.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{
    public enum enumRuningStatus
    {
        未计算=0,
        正在计算=1,
        计算成功=2,
        计算失败=3,
    }
    public enum enumParallelMode
    {
        OMP,
        MPI,
        GPU
    }
    public enum eumScenarioType
    {
        历史模拟,
        人工预报=2,
        自动预报=3,
        fast= 300100,//快速模型，即基于已有流场进行水质模拟,cirp project
        fine= 300101 //精细模型，水动力水质模型, cirp project
    }
    public interface IScenarioBase
    {
        /// <summary>
        /// 方案编码
        /// </summary>
        Guid OwnerID { get; set; }
        /// <summary>
        /// 方案名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 方案创建人
        /// </summary>
        string Creater { get; set; }
        /// <summary>
        /// 方案描述
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime ModifyTime { get; set; }
        /// <summary>
        /// 方案时间配置
        /// </summary>
        ScenarionTime ScenarioTime { get; set; }
        /// <summary>
        /// 方案类型
        /// </summary>
        eumScenarioType Type { get; set; }
        bool OneStepRun { get; set; }

        string ControlfileName { get; set; }

        string EngineFileName { get; set; }

        Dictionary<enumModelName, string> ModelEngine { get; set; }

        bool UploadedAsZip { get; set; }

        enumParallelMode ParallelMode { get; set; }

        eumApplications AppName { get; set; }
    }
}
