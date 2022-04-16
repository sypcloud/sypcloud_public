using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public interface ITSData
    {
        DateTime DT { get; set; }

        float Data { get; set; }
    }

    public interface IRelationData
    {
        object A { get; set; }

        object B { get; set; }
    }

    public interface ITSDataCollection
    {
        List<TSData> LstTSData { get; set; }
    }
}
