using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    /// <summary>
    /// 超渗产流
    /// </summary>
    public class ChaoShenChanLiu
    {
        string _realWaterEvaporation;
        [DataMember]
        /// <summary>
        /// 各时段实际/实测水面蒸发文件
        /// </summary>
        public string RealWaterEvaporation
        {
            get
            {
                return _realWaterEvaporation;
            }
            set
            {
                _realWaterEvaporation = value;
            }
        }

        string _soilEvaporation;
        [DataMember]
        /// <summary>
        /// 降雨文件
        /// </summary>
        public string SoilEvaporation
        {
            get
            {
                return _soilEvaporation;
            }
            set
            {
                _soilEvaporation = value;
            }
        }

        double _evaporationWord = 0.2;
        [DataMember]
        /// <summary>
        /// 蒸散发折算系数
        /// </summary>
        public double EvaporationWord
        {
            get
            {
                return _evaporationWord;
            }
            set
            {
                _evaporationWord = value;
            }
        }

        double _initWaterInSoil = 1.0;
        [DataMember]
        /// <summary>
        /// 流域初始土壤含水量
        /// </summary>
        public double InitWaterInSoil
        {
            get
            {
                return _initWaterInSoil;
            }
            set
            {
                _initWaterInSoil = value;
            }
        }

        double _philipsCurveParameter1 = 0.1;
        [DataMember]
        /// <summary>
        /// 菲利普下渗曲线参数1
        /// </summary>
        public double PhilipsCurveParameter1
        {
            get
            {
                return _philipsCurveParameter1;
            }
            set
            {
                _philipsCurveParameter1 = value;
            }
        }

        double _philipsCurveParameter2 = 0.1;
        [DataMember]
        /// <summary>
        /// 菲利普下渗曲线参数2
        /// </summary>
        public double PhilipsCurveParameter2
        {
            get
            {
                return _philipsCurveParameter2;
            }
            set
            {
                _philipsCurveParameter2 = value;
            }
        }

        double _infiltrationDistributionCurve = 0.1;
        [DataMember]
        /// <summary>
        /// 下渗能力分布曲线参数
        /// </summary>
        public double InfiltrationDistributionCurve
        {
            get
            {
                return _infiltrationDistributionCurve;
            }
            set
            {
                _infiltrationDistributionCurve = value;
            }
        }

    }
}
