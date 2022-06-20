using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public enum enumRainfallType
    {
        StepAccumulate,
        Accumulate
    }
    public class Rainfall
    {
        public string StationName { get; set; }
        public string StationCode { get; set; }
        public enumRainfallType Type { get; set; }
        public List<TSData> Data { get; set; }
    }
}
