using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    public class Crosssection :ICrosssection
    {
        [DataMember]
        public DataTable BranchTopoIdChainageTb { get; set; }

        [DataMember]
        public DataTable VerticalProfile { get; set; }
    }
}
