using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public interface ITSData2D
    {
        DateTime DT { get; set; }

        float[] Data { get; set; }

        float[] Data1 { get; set; }

        float[] Data2 { get; set; }

        float[] Data3 { get; set; }
    }

    public interface ITSData2DCollection
    {
        List<TSData2D> LstTSData2D { get; set; }
    }
}
