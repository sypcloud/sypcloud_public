using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SY.Models.ModelBase;

namespace SY.Models.Scenario
{
    public interface IScenarionTime
    {
        DateTime CreateTime { get; set; }

        DateTime ModifyTime { get; set; }

        ModelTime ModelTimePara { get; set; }      
    }
}
