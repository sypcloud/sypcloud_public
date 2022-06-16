using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class River : RiverBase
    {
        /// <summary>
        /// 只存RvrMdCode
        /// </summary>
        public List<string> UpRvr { get; set; }
        /// <summary>
        /// 只存RvrMdCode
        /// </summary>
        public List<string> DnRvr { get; set; }
        public List<Crosssection> XSectoin { get; set; }
    }
}
