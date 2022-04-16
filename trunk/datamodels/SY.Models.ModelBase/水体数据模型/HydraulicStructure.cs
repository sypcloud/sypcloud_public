using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class HydraulicStructure
    {
        public string RiverName { get; set; }
        public float Chainage { get; set; }
        public string ID { get; set; }
        public string TopID { get; set; }
        public List<float> Level { get; set; }
        public List<float> Width { get; set; }
        public int Type { get; set; }
        public int ControlStrategyType { get; set; }
    }
}
