using SY.Common;
using SY.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.Scenario
{
    [DataContract]    
    [KnownType(typeof(ScenarionTime))]
    [KnownType(typeof(XiaoLiuYu))]
    [KnownType(typeof(Outlet))]
    [KnownType(typeof(ViewModelBase))]
    [KnownType(typeof(eumScenarioType))]
    public class XLYHSYBScenario:ScenarioBase,IXLYHSYBScenario
    {
        /// <summary>
        /// 流域模型输入及计算输出的对象
        /// </summary>
        [DataMember]
        public XiaoLiuYu ScenarioInput { get; set; }
        /// <summary>
        /// 方案的分类标志，这里是分类历史洪水模拟、人工预报、自动预报
        /// </summary>
        [DataMember]
        public string ScenarioTag { get; set; }
    }
}
