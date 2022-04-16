using SY.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    /// <summary>
    /// 蓄满产流
    /// </summary>
    public class XuManChanLiu : ViewModelBase
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        [DisplayName("序号")]
        [XmlIgnore]
        [DataMember]
        public int ID { get; set; }
        /// <summary>
        /// 流域名称
        /// </summary>
        [DisplayName("流域名称")]
        [XmlIgnore]
        [DataMember]
        public string LiuyuName { get; set; }
        /// <summary>
        /// 流域编码
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [DataMember]
        public string LiuyuCode { get; set; }
        /// <summary>
        /// 子流域名称
        /// </summary>
        [XmlIgnore]
        [DisplayName("子流域名称")]
        [DataMember]
        public string ZiliuyuName { get; set; }
        /// <summary>
        /// 子流域编码
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        [DataMember]
        public string ZiliuyuCode { get; set; }

        double _maxPointWater = 1.0;

        /// <summary>
        /// 各最大点蓄水容量
        /// </summary>
        [DataMember]
        [Browsable(false)]
        public double MaxPointWater
        {
            get
            {
                return _maxPointWater;
            }
            set
            {
                _maxPointWater = value;
            }
        }

        double _upSoilWater = 1.0;
        [DisplayName("上层土壤蓄水容量")]
        [DataMember]
        /// <summary>
        /// 上层土壤蓄水容量
        /// </summary>
        public double UpSoilWater
        {
            get
            {
                return _upSoilWater;
            }
            set
            {
                _upSoilWater = value;
                this.OnPropertyChanged("UpSoilWater");
            }
        }

        double _downSoilWater = 1.0;
        [DataMember]
        /// <summary>
        /// 下层土壤蓄水容量
        /// </summary>
        [DisplayName("下层土壤蓄水容量")]
        public double DownSoilWater
        {
            get
            {
                return _downSoilWater;
            }
            set
            {
                _downSoilWater = value;
            }
        }

        double _deepSoilWater = 1.0;
        [DataMember]
        /// <summary>
        /// 深层土壤蓄水容量
        /// </summary>
        [DisplayName("深层土壤蓄水容量")]
        public double DeepSoilWater
        {
            get
            {
                return _deepSoilWater;
            }
            set
            {
                _deepSoilWater = value;
            }
        }

        double _surfaceSoilWater = 1.0;
        [DataMember]
        /// <summary>
        /// 表层土壤蓄水容量
        /// </summary>
        [DisplayName("表层土壤蓄水容量")]
        public double SurfaceSoilWater
        {
            get
            {
                return _surfaceSoilWater;
            }
            set
            {
                _surfaceSoilWater = value;
            }
        }

        double _freeReservoirWater = 1.0;
        [DataMember]
        /// <summary>
        /// 自由水库蓄水量
        /// </summary>
        [DisplayName("自由水库蓄水量")]
        public double FreeReservoirWater
        {
            get
            {
                return _freeReservoirWater;
            }
            set
            {
                _freeReservoirWater = value;
            }
        }

        double _upPowerWater = 1.0;
        [DataMember]
        /// <summary>
        /// 上层张力水容量
        /// </summary>
        [DisplayName("上层张力水容量")]
        public double UpPowerWater
        {
            get
            {
                return _upPowerWater;
            }
            set
            {
                _upPowerWater = value;
            }
        }

        double _downPowerWater = 1.0;
        [DataMember]
        /// <summary>
        /// 下层张力水容量
        /// </summary>
        [DisplayName("下层张力水容量")]
        public double DownPowerWater
        {
            get
            {
                return _downPowerWater;
            }
            set
            {
                _downPowerWater = value;
            }
        }

        double _deepPowerWater = 1.0;
        [DataMember]
        /// <summary>
        /// 深层张力水容量
        /// </summary>
        [DisplayName("深层张力水容量")]
        public double DeepPowerWater
        {
            get
            {
                return _deepPowerWater;
            }
            set
            {
                _deepPowerWater = value;
            }
        }

        double _evaporationWord = 1.0;
        [DataMember]
        /// <summary>
        /// 蒸散发折算系数
        /// </summary>
        [DisplayName("蒸散发折算系数")]
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

        double _deepEvaporationWord = 0.1;
        [DataMember]
        /// <summary>
        /// 深层蒸散发系数
        /// </summary>
        [DisplayName("深层蒸散发系数")]
        public double DeepEvaporationWord
        {
            get
            {
                return _deepEvaporationWord;
            }
            set
            {
                _deepEvaporationWord = value;
            }
        }

        double _soilWaterOutWord = 0.2;
        [DataMember]
        /// <summary>
        /// 壤中流出流系数
        /// </summary>
        [DisplayName("壤中流出流系数")]
        public double SoilWaterOutWord
        {
            get
            {
                return _soilWaterOutWord;
            }
            set
            {
                _soilWaterOutWord = value;
            }
        }

        double _initBainFreeArea = 1;
        [DataMember]
        /// <summary>
        /// 自由水初始产流面积
        /// </summary>
        [DisplayName("自由水初始产流面积")]
        public double InitBainFreeArea
        {
            get
            {
                return _initBainFreeArea;
            }
            set
            {
                _initBainFreeArea = value;
            }
        }

        double _initBainFreeDeep = 1;
        [DataMember]
        /// <summary>
        /// 初始流域自由水深
        /// </summary>
        [DisplayName("初始流域自由水深")]
        public double InitBainFreeDeep
        {
            get
            {
                return _initBainFreeDeep;
            }
            set
            {
                _initBainFreeDeep = value;
            }
        }

        double _underLandRunoffWord = 0.1;
        [DataMember]
        /// <summary>
        /// 地下径流出流系数
        /// </summary>
        [DisplayName("地下径流出流系数")]
        public double UnderLandRunoffWord
        {
            get
            {
                return _underLandRunoffWord;
            }
            set
            {
                _underLandRunoffWord = value;
            }
        }

        double _freeWaterCurveWord = 0.1;
        [DataMember]
        /// <summary>
        /// 自由水蓄水容量曲线系数
        /// </summary>
        [DisplayName("自由水蓄水容量曲线系数")]
        public double FreeWaterCurveWord
        {
            get
            {
                return _freeWaterCurveWord;
            }
            set
            {
                _freeWaterCurveWord = value;
            }
        }

        double _waterParabolaWord = 0.1;
        [DataMember]
        /// <summary>
        /// 蓄水容量曲线抛物线系数
        /// </summary>
        [DisplayName("蓄水容量曲线抛物线系数")]
        public double WaterParabolaWord
        {
            get
            {
                return _waterParabolaWord;
            }
            set
            {
                _waterParabolaWord = value;
            }
        }

        string _realEvaporationFile;
        [DataMember]
        /// <summary>
        /// 各时段实测水面蒸发文件
        /// </summary>
        [Browsable(false)]
        public string RealEvaporationFile
        {
            get
            {
                return _realEvaporationFile;
            }
            set
            {
                _realEvaporationFile = value;
            }
        }
        float _imp;
        [DataMember]
        /// <summary>
        /// 不透水面积%
        /// </summary>
        [DisplayName("不透水面积%")]
        public float IMP
        {
            get { return _imp; }
            set { _imp = value; }
        }

        string _Jianyuwenjian;
        [DataMember]
        /// <summary>
        /// 降雨文件
        /// </summary>
        [Browsable(false)]
        public string JianYuWenJian
        {
            get
            {
                return _Jianyuwenjian;
            }
            set
            {
                _Jianyuwenjian = value;
            }
        }
    }
}
