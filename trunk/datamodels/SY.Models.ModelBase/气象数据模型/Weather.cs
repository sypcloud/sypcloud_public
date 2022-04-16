using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    [KnownType(typeof(WeatherCondition))]
    public class Weather : IWeather
    {
        public Weather()
        {
            WeatherC = new WeatherCondition();
        }

        [DataMember]
        public WeatherCondition WeatherC { get; set; }

    }
}
