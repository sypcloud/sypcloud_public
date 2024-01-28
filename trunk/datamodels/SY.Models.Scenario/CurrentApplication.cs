using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{
    public static class CurrentApplication
    {        
        /// <summary>
        /// 小流域模型配置文件
        /// </summary>
        public static string XLY_Configfile { get; set; }
        /// <summary>
        /// 小流域对象
        /// </summary>
        public static SY.Models.ModelBase.XiaoLiuYu BasinObj { get; set; }
        /// <summary>
        /// 小流域模型方案对象
        /// </summary>
        public static XLYHSYBScenario XlyMlodeScenario { get; set; }
    }
}
