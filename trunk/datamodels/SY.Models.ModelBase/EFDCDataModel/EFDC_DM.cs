using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class Elementslxlys
    {
        public int elementlxly_i;
        public int elementlxly_j;
        public double elementlxly_xx;
        public double elementlxly_yy;
    }

    public class Elementsdxdys
    {
        public int elementdxdy_i;
        public int elementdxdy_j;
        public double elementdxdy_dx;
        public double elementdxdy_dy;
        public double elementdxdy_depth;
        public double elementdxdy_elev;
        public double elementdxdy_zrough;
    }

    public class Elementcenters
    {
        public double center_xx;
        public double center_yy;
        public double ele_dx;
        public double ele_dy;
        public double ele_depth;
        public double ele_elev;
        public double ele_zrough;
        public double ele_bathy; 
    }

    public class Elementcorners
    {
        public double cor_xx;
        public double cor_yy;
    }

    public class Cellscorner
    {
        public int cellindex_i;
        public int cellindex_j;
        public double cellcorner1_xx;
        public double cellcorner1_yy;
        public double cellcorner2_xx;
        public double cellcorner2_yy;
        public double cellcorner3_xx;
        public double cellcorner3_yy;
        public double cellcorner4_xx;
        public double cellcorner4_yy;
        public int nodeIndex1;
        public int nodeIndex2;
        public int nodeIndex3;
        public int nodeIndex4;
    }

    public class Elementvertex
    {
        public int NO1;
        public int NO2;
        public int NO3;
        public int NO4;
    }


    public class WS_Out
    {
        public double time;
        public List<double> hs;

        public WS_Out()
        {
            hs = new List<double>();
        }
    }

    public class Layer_uvw
    {
        public double uu;
        public double vv;
        public double ww;
        public int lno;//层数
    }
    public class VEL_Out
    {
        public double time;
        public List<Layer_uvw> uvwlist;

        public VEL_Out()
        {
            uvwlist = new List<Layer_uvw>();
        }
    }

    public class FlowField
    {
        public double time;
        public List<double> hlist;
        public List<double> ulist;
        public List<double> vlist;
        public FlowField()
        {
            hlist = new List<double>();
            ulist = new List<double>();
            vlist = new List<double>();
        }
    }

    public class DYE_Conc
    {
        public double time;
        public List<double> dcon;

        public DYE_Conc()
        {
            dcon = new List<double>();
        }
    }

    public class PointD2
    {
        public double X;
        public double Y;
        public double Z;
    }

    //CIRP----------------------------------------------------------------------------------------\

    /// <summary>
    /// 流量时间序列
    /// </summary>
    public class FlowTSData
    {
        public FlowTSData()
        {
            Time = new List<double>();
            Flow = new List<double>();
        }
        public int InType;
        public int TimeCount;
        public int ReferenceTimePSec;
        public string FlowTSName;
        public List<double> Time;
        public List<double> Flow;
    }


}
