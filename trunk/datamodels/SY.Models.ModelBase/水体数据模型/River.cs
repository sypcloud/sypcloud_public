using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class River : RiverBase
    {
        public List<River> UpRvr { get; set; }
        public List<River> DnRvr { get; set; }
    }
}
