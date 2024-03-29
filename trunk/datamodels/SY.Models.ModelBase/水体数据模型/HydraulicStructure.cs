﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sy.Global;

namespace SY.Models.ModelBase
{
    public enum eumStructureSectionType
    {
        Rectangular=0,
        Cicular=1,
        Irregular =2,
    }
    public enum eumStructureFlowDir
    {
        Positive=0,
        Negative=-1,
        Bidirection=1,
        Noflow=2
    }
    public class HydraulicStructure
    {        
        public string RiverName { get; set; }
        public float Chainage { get; set; }
        public List<PointD> Location { get; set; }
        public string ID { get; set; }
        public string TopID { get; set; }
        public int Type { get; set; }
        public int ControlStrategyType { get; set; }
        public string coordinate_type { get; set; }
        public float Diameter { get; set; }

        public float FlowWidth { get; set; }

        public float Height { get; set; }

        public float Length { get; set; }

        public List<float> Level { get; set; }
        public List<float> Width { get; set; }

        public eumStructureSectionType SectionType { get; set; }

        public eumStructureFlowDir FlowDir { get; set; }
    }
}
