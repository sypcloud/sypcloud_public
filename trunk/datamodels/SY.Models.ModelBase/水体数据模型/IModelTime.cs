using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public interface IModelTime
    {
        DateTime StartTime { get; set; }

        DateTime EndTime { get; set; }

        DateTime ForecastingTime { get; set; }
        /// <summary>
        /// 时间步长，以秒为单位
        /// </summary>
        float TimeStep { get; set; }

        float MaxTimeStep { get; set; }

        float MinTimeStep { get; set; }
        /// <summary>
        /// 时间序列的步长（以小时为单位）
        /// </summary>
        float TimeValueStep { get; set; }
        /// <summary>
        /// 输出步长（以秒为单位）
        /// </summary>
        int OutputStep { get; set; }
    }
}
