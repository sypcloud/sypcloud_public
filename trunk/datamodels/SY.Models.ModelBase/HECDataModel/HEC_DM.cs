using Sy.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class HMS_BASE_DM
    {
        public string Last_Modified_Date { get; set; }
        public string Last_Modified_Time { get; set; }
        public string Canvas_X { get; set; }
        public string Canvas_Y { get; set; }
        public string Label_X { get; set; }
        public string Label_Y { get; set; }
        public string Downstream { get; set; }

    }
    public class HMS_SUBBASIN_DM: HMS_BASE_DM
    {
        public string Subbasin { get; set; }        
        public string Area { get; set; }
        public string Canopy { get; set; }
        public string Plant_Uptake_Method { get; set; }
        public string Surface { get; set; }
        public string LossRate { get; set; }
        public string Percent_Impervious_Area { get; set; }
        public string Curve_Number {get;set;}
        public string Initial_Abstraction { get; set; }
        public string Transform { get; set; }
        public string Lag { get; set; }
        public string Unitgraph_Type { get; set; }
        public string Baseflow { get; set; }
    }
    public class HMS_JUNCTION_DM: HMS_BASE_DM
    {
        public string Junction { get; set; }
    }
    public class HMS_REACH_DM:HMS_BASE_DM
    {
        public string Reach { get; set; }
        public string From_Canvas_X { get; set; }
        public string From_Canvas_Y { get; set; }
        public string Route { get; set; }
        public string Channel_Loss { get; set; }

    }
    public class HMS_BASIN_DM
    {
        public List<HMS_SUBBASIN_DM> Subbasin { get; set; }
        public List<HMS_JUNCTION_DM> Junction { get; set; }
        public List<HMS_REACH_DM> Reach { get; set; }
    }
    public class HEC_DM
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public List<Junctor> JunctCollection { get; set; }
        public List<RiverReach> RiverReachCollection { get; set; }
        
    }
    public class Junctor
    {
        public string Name { get; set; }
        public string[] Desc { get; set; }
        public PointD Location { get; set; }
        public PointD TextLocation { get; set; }
        /// <summary>
        /// string[]--0为River name;1为Reach name
        /// </summary>
        public List<string[]> UpRiverReach { get; set; }
        /// <summary>
        /// string[]--0为River name;1为Reach name
        /// </summary>
        public List<string[]> DownRiverReach { get; set; }
        /// <summary>
        /// 跟上游河段个数一致,double[]--0为length;1为angle
        /// </summary>
        public List<double[]> JuctionLengthAndAngle { get; set; }
    }

    public class RiverReach : River
    {
        public string RiverName { get; set; }
        public string ReachName { get; set; }    
        
        public PointD TextLocation { get; set; }
        public List<HECCrossSection> CSCollection { get; set; }
    }

    public class HECCrossSection:Crosssection
    {
        /// <summary>
        /// 0-,1-桩号,
        /// </summary>
        public string[] Location { get; set; }
        public string Description { get; set; }
        public string LastEditedTime { get; set; }
        public string Manning { get; set; }
        public string ManningSta { get; set; }
        public string XS_HTab_Starting { get; set; }
    }
}
