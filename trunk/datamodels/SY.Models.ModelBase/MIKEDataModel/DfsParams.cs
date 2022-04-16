using Sy.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase.MIKE
{
    public class DfsParams
    {
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public int NoElements { get; set; }
        [DataMember]
        public int NoNodes { get; set; }
        [DataMember]
        public int NoSteps { get; set; }
        [DataMember]
        public double GridSize { get; set; }
        [DataMember]
        public int GridXNum { get; set; }
        [DataMember]
        public int GridYNum { get; set; }
        [DataMember]
        public List<PointD> RegularGridsCoors { get; set; }
        [DataMember]
        public List<PointD> MeshNodesCoords { get; set; }
        [DataMember]
        public Extent MeshExtent { get; set; }
        [DataMember]
        public Extent MeshExtentWgs84 { get; set; }
        [DataMember]
        public DateTime[] ts { get; set; }
        [DataMember]
        public string[] items { get; set; }

        public object[][] ele { get; set; }

        [DataMember]
        public List<List<PointD>> BoundPts { get; set; }

        //public object[][] nd { get; set; }
        public List<float> Ptx { get; set; }
        public List<float> Pty { get; set; }
        public List<float> Ptz { get; set; }
        public List<int> Pta { get; set; }

        public int[][] vNeighborEle { get; set; }

        public List<PointD> centerCoor { get; set; }

        public float[][] nWeights { get; set; }
        [DataMember]
        public float[] Area { get; set; }
        [DataMember]
        public Dictionary<int, List<PointD>> Wgs84Nodes { get; set; }
        [DataMember]
        public List<PointD> Wgs84ElesCenterCoors { get; set; }
        [DataMember]
        public string Projection { get; set; }
        /// <summary>
        /// 当前Item的结果集
        /// </summary>
        public float[][] cc { get; set; }
        /// <summary>
        /// 当前item某步的结果集
        /// </summary>
        [DataMember]
        public List<float> StepValue { get; set; }
        /// <summary>
        /// 当前item某步段所有单元的结果集
        /// </summary>
        public List<float[]> StepsValue { get; set; }
        /// <summary>
        /// 当前item某步段内某个单元的结果集
        /// </summary>
        [DataMember]
        public List<float> StepsEleValue { get; set; }
        [DataMember]
        public float Max { get; set; }
        [DataMember]
        public float Min { get; set; }
        [DataMember]
        public float Avg { get; set; }
        [DataMember]
        public List<PointD> Points { get; set; }
        [DataMember]
        public List<PointD> GridPts { get; set; }
        [DataMember]
        public List<string> RiverNames { get; set; }
        [DataMember]
        public Dictionary<string, List<double>> Chainages { get; set; }
        [DataMember]
        public List<string> grdDescriptionEx { get; set; }
    }
}
