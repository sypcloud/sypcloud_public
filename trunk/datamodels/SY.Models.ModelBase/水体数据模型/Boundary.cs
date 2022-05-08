using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sy.Global;

namespace SY.Models.ModelBase
{
    [DataContract]
    [KnownType(typeof(TSData))]
    [KnownType(typeof(RelationData))]
    public class Boundary : BoundaryBase, IBoundary
    {

        public Boundary()
        {
            Value = new List<TSData>();
            RelationValue = new List<IRelationData>();
            Location3 = new RiverStation();
        }

        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// 边界序号
        /// </summary>
        [DataMember]
        public int BDIndex { get; set; }
        /// <summary>
        /// 相关点、线的名称（源项、边界等）
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Boundary的时间
        /// </summary>
        [DataMember]
        public int Time { get; set; }
        /// <summary>
        /// 水位
        /// </summary>
        [DataMember]
        public float WaterLever { get; set; }
        /// <summary>
        /// 流量
        /// </summary>
        [DataMember]
        public float Flow { get; set; }

        [DataMember]
        public float Concetration { get; set; }

        //*************tt-2018-9-20
        [DataMember]
        public float NuclideDoes { get; set; }
                
        [DataMember]
        public enumBoundaryDomainType DomainType { get; set; }

        [DataMember]
        public enumBoundaryDirectType Direction { get; set; }

        [DataMember]
        public enumHDBoundaryType HDType { get; set; }

        [DataMember]
        public enumWQBoundaryType WQType { get; set; }

        [DataMember]
        public enumRRBoundaryType RRType { get; set; }

        [DataMember]
        public List<PointD> Location { get; set; }

        [DataMember]
        public List<List<int>> Location2 { get; set; }
        /// <summary>
        /// 源汇所在的节点编号，用于有限元模型，如WASH123D模型
        /// </summary>
        [DataMember]
        public int NodeID { get; set; }

        [DataMember]
        public int ElementID { get; set; }

        [DataMember]
        public List<TSData> Value { get; set; }

        [DataMember]
        public IList<IRelationData> RelationValue { get; set; }

        [DataMember]
        public string SelectHDType { get { return HDType.ToString(); } set { HDType = (enumHDBoundaryType)Enum.Parse(typeof(enumHDBoundaryType), value); } }

        [DataMember]
        public List<string> HDTypeNames { get { return Enum.GetNames(typeof(enumHDBoundaryType)).ToList<string>(); } }

        [DataMember]
        public List<CIRPNuclideDM> CIRPNuclide  { get; set; }        

        [DataMember]
        public bool IsSelectCaseLocation { get; set; }
               
        [DataMember]
        public string SelectDirection { get { return Direction.ToString(); } set { Direction = (enumBoundaryDirectType)Enum.Parse(typeof(enumBoundaryDirectType), value); } }

        [DataMember]
        public List<string> DirectionNames { get { return Enum.GetNames(typeof(enumBoundaryDirectType)).ToList<string>(); } }

        [DataMember]
        public bool IsModified { get; set; }

        [DataMember]
        public float Longitude { get; set; }

        [DataMember]
        public float Latitude { get; set; }

        [DataMember]
        public string RiverName { get; set; }

        [DataMember]
        public float Chainage { get; set; }

        [DataMember]
        public float EndChainage { get; set; }

        [DataMember]
        public bool IncludeAD { get; set; }

        [DataMember]
        public List<string> Pollutants { get; set; }


        [DataMember]
        public RiverStation Location3 { get; set; }
    }
}
