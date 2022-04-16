using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class Gate : HydraulicStructure
    {
        public int NoGates { get; set; }
        public float GateWidth { get; set; }
        public float Silllevel { get; set; }
        public bool setInitialValue { get; set; }
        public float InitialValue { get; set; }
        public bool setMaxvaulue { get; set; }
        public float MaxValue { get; set; }
    }
}
