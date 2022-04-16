using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{   
    [DataContract]
    [KnownType(typeof(InitialCondition))]
    [KnownType(typeof(HDParameter))]
    public class Parameter:IParameter
    {
        public Parameter()
        {
            InitialC = new InitialCondition();
            HdPara = new HDParameter();

            HdPara.ConstantHorizontalEddyViscosity = 0.01f;
            HdPara.ConstantSmagorinskyCoefficient = 0.18f;
            HdPara.ConstantVerticalDiffusivity = 0.001f;
            HdPara.ConstantVerticalEddyViscosity = 0.001f;
            HdPara.GlobalManningsFactor = 0.02f;
            HdPara.GlobalRoughnessHeight = 0.02f;          
        }
        
        [DataMember]
        public InitialCondition InitialC { get; set; }

        [DataMember]
        public HDParameter HdPara { get; set; }

        //*****************tt-2018-9-14

    }
}
