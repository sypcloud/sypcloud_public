using SY.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public interface IWeather
    {
        WeatherCondition WeatherC { get; set; }
    }
    [DataContract]
    [Serializable]
    public class WeatherCondition
    {
        private bool isconstant_Wind = true;
        private bool isfile_Wind;
        private bool isconstantPrecipitation = true;
        private bool isfile_Precipitation;
        private bool isconstantEvaporation = true;
        private bool isfile_Evaporation;

        [DataMember]
        public bool IsConstant_Wind
        {
            get { return isconstant_Wind; }
            set
            {
                isconstant_Wind = value;
                if (value)
                {
                    isfile_Wind = false;
                }
            }
        }

        [DataMember]
        public bool IsFile_Wind
        {
            get { return isfile_Wind; }
            set
            {
                isfile_Wind = value;
                if (value)
                {
                    isconstant_Wind = false;
                }
            }
        }

        [DataMember]
        public bool IsConstantPrecipitation
        {
            get { return isconstantPrecipitation; }
            set
            {
                isconstantPrecipitation = value;
                if (value)
                {
                    isfile_Precipitation = false;
                }
            }
        }

        [DataMember]
        public bool IsFile_Precipitation
        {
            get { return isfile_Precipitation; }
            set
            {
                isfile_Precipitation = value;
                if (value)
                {
                    isconstantPrecipitation = false;
                }
            }
        }

        [DataMember]
        public bool IsConstantEvaporation
        {
            get { return isconstantEvaporation; }
            set
            {
                isconstantEvaporation = value;
                if (value)
                {
                    isfile_Evaporation = false;
                }
            }
        }

        [DataMember]
        public bool IsFile_Evaporation
        {
            get { return isfile_Evaporation; }
            set
            {
                isfile_Evaporation = value;
                if (value)
                {
                    isconstantEvaporation = false;
                }
            }
        }

        [DataMember]
        public float ConstantSpeed_Wind { get; set; }

        [DataMember]
        public float ConstantDirection_Wind { get; set; }

        [DataMember]
        public float ConstantPrecipitation { get; set; }

        [DataMember]
        public float ConstantEvaporation { get; set; }

        [DataMember]
        public List<Rainfall> PrecipitationData { get; set; }
    }
}
