using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sy.Global;

namespace SY.Models.ModelBase
{
    //
    // Summary:
    //     Boundary condition categories.
    public enum enumBoundaryDescription
    {
        //
        // Summary:
        //     Open boundary condition
        Open = 0,
        //
        // Summary:
        //     Point source
        PointSource = 1,
        //
        // Summary:
        //     Distributed source
        DistributedSource = 2,
        //
        // Summary:
        //     Global boundary condition
        Global = 3,
        //
        // Summary:
        //     Structures
        Structures = 4,
        //
        // Summary:
        //     Closed
        Closed = 5,

        Rainfall=6
    }
    public enum enumBoundaryDirectType
    {
        北,
        南,
        东,
        西
    }

    public enum enumBoundaryDomainType
    {
        OneD,
        OneHD,
        OneWQ,
        TwoD,
        ThreeD,
        管网
    }

    public enum enumHDBoundaryType
    {
        陆地边界,
        水位,
        流量,        
        水位流量关系曲线,
        侧向流量,
        时间控制闸门,
        水位控制闸门,
    }

    public enum enumWQBoundaryType
    {
        浓度,
        剂量
    }

    public enum enumRRBoundaryType
    {
        降雨
    }

    public interface IBoundary
    {
        int BDIndex { get; set; }

        int Time { get; set; }

        string Name { get; set; }

        float WaterLever { get; set; }

        float Flow { get; set; }

        //float Concetration { get; set; }

        float NuclideDoes { get; set; }

        //*************tt-2018-9-20
        enumBoundaryDomainType DomainType { get; set; }

        enumBoundaryDirectType Direction { get; set; }

        enumHDBoundaryType HDType { get; set; }

        enumWQBoundaryType WQType { get; set; }

        enumRRBoundaryType RRType { get; set; }

        List<PointD> Location { get; set; }

        List<List<int>> Location2 { get; set; }

        List<TSData> Value { get; set; }

        IList<IRelationData> RelationValue { get; set; }

        List<string> HDTypeNames { get; }

        bool IsModified { get; set; }

    }
}
