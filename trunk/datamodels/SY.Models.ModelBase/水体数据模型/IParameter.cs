using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{

    public interface IParameter
    {
        InitialCondition InitialC { get; set; }

        HDParameter HdPara { get; set; }

        WQParameter WqPara { get; set; }

    }


    [DataContract]
    public class InitialCondition
    {
        public InitialCondition()
        {
            IsGlobalWaterLevel = true;
        }
        
        [DataMember]
        public bool IsGlobalWaterLevel { get; set; }

        [DataMember]
        public bool IsGlobalWaterDepth { get; set; }

        [DataMember]
        public bool IsLocal { get; set; }

        [DataMember]
        public float InitialGlobalWaterDepth { get; set; }

        [DataMember]
        public float InitialGlobalWaterLevel { get; set; }

        /// <summary>
        /// 初始全局u速度
        /// </summary>
        [DataMember]
        public float InitialGlobalUSpeed { get; set; }
        /// <summary>
        /// 初始全局v速度
        /// </summary>
        [DataMember]
        public float InitialGlobalVSpeed { get; set; }
        //****************tt-2018-9-14

        [DataMember]
        public RiverStation Location3 { get; set; }

        [DataMember]
        public List<string> Pollutants { get; set; }

        [DataMember]
        public List<float> Concetration { get; set; }

        [DataMember]
        public List<float> Dispersion { get; set; }
    }

    [DataContract]
    public class HDParameter
    {
        public HDParameter()
        {
            IsGlobalRoughnessHeight = true;
            IsConstantHorizontalEddyViscosity = true;
            Layers = new List<float>() { 1f };
        }

        [DataMember]
        public List<float> Layers { get; set; }

        [DataMember]
        public bool IsGlobalRoughnessHeight { get; set; }

        [DataMember]
        public bool IsGlobalManningsFactor { get; set; }

        [DataMember]
        public bool IsConstantHorizontalEddyViscosity { get; set; }

        [DataMember]
        public bool IsConstantSmagorinskyCoefficient { get; set; }

        [DataMember]
        public float GlobalRoughnessHeight { get; set; }

        [DataMember]
        public float GlobalManningsFactor { get; set; }

        [DataMember]
        public float ConstantHorizontalEddyViscosity { get; set; }

        [DataMember]
        public float ConstantSmagorinskyCoefficient { get; set; }

        [DataMember]
        public float ConstantVerticalEddyViscosity { get; set; }

        [DataMember]
        public float ConstantVerticalDiffusivity { get; set; }
        /// <summary>
        /// 全局干水深
        /// </summary>
        [DataMember]
        public float GlobalDryDepth { get; set; }
        /// <summary>
        /// 全局湿水深
        /// </summary>
        [DataMember]
        public float GlobalWetDepth { get; set; }
    }
    //*****************tt-2018-9-14

    public class WQParameter
    {
        [DataMember]
        public RiverStation Location3 { get; set; }

        [DataMember]
        public List<string> Pollutants { get; set; }

        [DataMember]
        public List<float> Dispersion { get; set; }
    }
}
