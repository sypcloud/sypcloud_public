using Sy.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public class RiverBase
    {
        public string RvrMdCode { get; set; }
        public string TopoID { get; set; }
        /// <summary>
        /// 河流中文名称
        /// </summary>
        public string Name { get; set; }
        public string RvrName { get; set; }
        public string RchName { get; set; }
        public float StChainage { get; set; }
        public float EdChainage { get; set; }
        //public string UpRvrCode { get; set; }
        //public string UpChainage { get; set; }
        //public string DwRvrCode { get; set; }
        //public string DwChainage { get; set; }
        public List<PointD> Points { get; set; }
        /// <summary>
        /// 存储河段上的断面桩号
        /// </summary>
        public List<float> Stations { get; set; }
        public List<string> Stations2 { get; set; }
        /// <summary>
        /// 存储河段计算网格（线段）坐标wkt字符串
        /// </summary>
        public List<List<PointD>> SegmentPoints { get; set; }
    }
}
