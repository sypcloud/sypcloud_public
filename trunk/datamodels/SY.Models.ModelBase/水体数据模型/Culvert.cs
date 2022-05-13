using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class Culvert:HydraulicStructure
    {
        public float UpsInvertLevel { get; set; }
        public float DwnInvertLevel { get; set; }
    }
}
