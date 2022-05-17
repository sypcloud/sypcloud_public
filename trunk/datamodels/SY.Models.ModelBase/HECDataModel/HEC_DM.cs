using Sy.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
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

    public class RiverReach
    {
        public string RiverName { get; set; }
        public string ReachName { get; set; }
        
        public List<PointD> Points { get; set; }
        public PointD TextLocation { get; set; }
        public List<HECCrossSection> CSCollection { get; set; }
    }

    public class HECCrossSection
    {
        public string[] Location { get; set; }
        public string Description { get; set; }
        public string LastEditedTime { get; set; }
        public List<PointD> Data { get; set; }
        public string Manning { get; set; }
        public string ManningSta { get; set; }
        public string XS_HTab_Starting { get; set; }
    }
}
