using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class Tianditu_DistrictQuery_DM
    {
        public int yingjiType { get; set; }
        public int sourceType { get; set; }
        public string keyWord { get; set; }
        public int level { get; set; }
        public string mapBound { get; set; }
        public int queryType { get; set; }
        public int start { get; set; }
        public int count { get; set; }
        public int queryTerminal { get; set; }
    }

    public class Tianditu_DistrictQuery_Response_DM
    {
        public int count { get; set; }
        public string keyWord { get; set; }
        public string mclayer { get; set; }
        public int resultType { get; set; }
        public Tianditu_Area area { get; set; }
    }

    public class Tianditu_Area
    {
        public string level { get; set; }
        public string bound { get; set; }
        public string lonlat { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<Tianditu_Points> points { get; set; }
    }

    public class Tianditu_Points
    {
        public string region { get; set; }
    }
}
