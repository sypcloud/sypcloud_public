namespace SY.Models.ModelBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Sy.Global;

    [DataContract]
    public class ModelDomain : IModelDomain
    {
        [DataMember]
        public Guid ScenarioID { get; set; }

        [DataMember]
        public enumSupportedGridType TypeName { get; set; }

        [DataMember]
        public string Projection { get; set; }

        [DataMember]
        public int NodeNo { get; set; }

        [DataMember]
        public List<PointD> Nodes { get; set; }

        [DataMember]
        public int ElementNo { get; set; }

        [DataMember]
        public int ElementType { get; set; }

        [DataMember]
        public List<List<int>> Elements { get; set; }

        [DataMember]
        public float CentralLogitude { get; set; }

        [DataMember]
        public int nCol { get; set; }

        [DataMember]
        public int nRow { get; set; }

        [DataMember]
        public bool IncludeMask { get; set; }
    }
}
