using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public interface ICrosssection
    {
        DataTable BranchTopoIdChainageTb { get; set; }

        DataTable VerticalProfile { get; set; }
    }
}
