using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase
{
    public enum enumProjectTyep
    {
        污染影响型,
        水文要素影响型,
        复合型
    }

    public enum enumAssessmentType
    {
        一级 = 0,
        二级 = 1,
        三级 = 3
    }

    public enum enumWaterBodyType
    {
        河流,
        湖泊,
        水库,
        入海河口,
        近岸海域
    }

    public enum enumWaterQualityComplexity
    {
        复杂,
        中等,
        简单
    }

    public enum enumWaterQualityRequirement
    {
        I,
        II,
        III,
        IV,
        V
    }

    public enum enumOceanType
    {
        F,
        G,
        O
    }

    public enum enumOceanQualityRequirement
    {
        一,
        二,
        三,
        四
    }

    public enum enumRiverScale
    {
        大型河流,
        中等河流,
        小型河流
    }

    public enum enumLakeReservoirScale
    {
        大型,
        中型,
        小型
    }

    public enum enumPollutantType
    {
        持久性,
        非持久性,
        酸和碱,
        水温影响
    }
    public enum enumHydrologicSituation
    {
        年径流量与总流量之比,
        调水流量占多年平均流量之比,
        水动力条件影响
        
    }
    /// <summary>
    /// 地表水环评导则等级确定数据模型
    /// </summary>
    [DataContract]
    public class WEIAClassifyDM   
    {
        /// <summary>
        /// 建设项目分类
        /// Ⅰ类：主要因排放水污染物对受纳水域水质造成污染的建设项目，即污染影响型项目。
        /// Ⅱ类：主要引起水域水文要素变化的建设项目，即水文要素影响型项目。
        /// Ⅲ类：指同时具备Ⅰ类和Ⅱ类建设项目环境影响特征的建设项目。
        /// </summary>
        [DataMember]
        public enumProjectTyep ProjectType { get; set; }
        /// <summary>
        /// 评价等级
        /// </summary>
        [DataMember]
        public enumAssessmentType AssessmentType { get; set; }
        /// <summary>
        /// 水体类别
        /// </summary>
        [DataMember]
        public enumWaterBodyType WaterBodyType { get; set; }
        /// <summary>
        /// 海域类型
        /// F：入海河口和半封闭海湾；G：半开敞式海湾；O：其他海域和开敞式海湾；
        /// </summary>
        [DataMember]
        public enumOceanType OceanType { get; set; }
        /// <summary>
        /// 污水水质复杂程度
        /// </summary>
        [DataMember]
        public enumWaterQualityComplexity WaterQualityComplexity { get; set; }
        /// <summary>
        /// 河流水域规模
        /// </summary>
        [DataMember]
        public enumRiverScale RiverWaterAreaScale { get; set; }
        /// <summary>
        /// 湖泊水库水域规模
        /// </summary>
        [DataMember]
        public enumLakeReservoirScale LakeReservoirAreaScale { get; set; }
        /// <summary>
        /// 受纳水体水质要求
        /// </summary>
        [DataMember]
        public enumWaterQualityRequirement WaterQualityRequirement { get; set; }
        /// <summary>
        /// 受纳海域水质要求
        /// </summary>
        [DataMember]
        public enumOceanQualityRequirement OceanQualityRequirement { get; set; }
        /// <summary>
        /// 污染物类型数
        /// </summary>
        [DataMember]
        public int PollutantsTypeNo { get; set; }
        /// <summary>
        /// 需预测的水质因子数目
        /// </summary>
        [DataMember]
        public int RequiredForcastingPollutantsNo { get; set; }
        /// <summary>
        /// 含有GB 8978中的一类污染物、GB 3838中未包括的水质因子时（悬浮物除外）
        /// </summary>
        [DataMember]
        public bool IsIncludeGB8978CatalogOneAndOutofGB3838 { get; set; }
        /// <summary>
        /// 多年平均流量或平水期的平均流
        /// </summary>
        [DataMember]
        public float AverageDischargeOr { get; set; }
        /// 多年平均流量
        /// </summary>
        [DataMember]
        public float AverageDischarge { get; set; }
        /// <summary>
        /// 枯水期湖泊或水库的平均水深
        /// </summary>
        [DataMember]
        public float AverageDepth { get; set; }
        /// <summary>
        /// 枯水期湖泊或水库的水域面积,相应于水深
        /// </summary>
        [DataMember]
        public float WaterArea { get; set; }
        /// <summary>
        /// 湾口宽度
        /// </summary>
        [DataMember]
        public float EstuaryWidth { get; set; }
        /// <summary>
        /// 海岸线长度
        /// </summary>
        [DataMember]
        public float CoastlineLength { get; set; }
        /// <summary>
        /// 海湾的面积
        /// </summary>
        [DataMember]
        public float EstuaryArea { get; set; }
        /// <summary>
        /// 污水排放量（m3/d)
        /// </summary>
        [DataMember]
        public float PollutionDischarge { get; set; }
        /// <summary>
        /// 年径流量
        /// </summary>
        [DataMember]
        public float AnnualDischarge { get; set; }
        /// <summary>
        /// 总库容
        /// </summary>
        [DataMember]
        public float TotalReservoirVolumn { get; set; }
        /// <summary>
        /// 调水流量
        /// </summary>
        [DataMember]
        public float TransferDischarge { get; set; }
        /// <summary>
        /// 工程占用水域面积
        /// </summary>
        [DataMember]
        public float TakeupWaterArea { get; set; }
        /// <summary>
        /// 工程扰动水底面积
        /// </summary>
        [DataMember]
        public float DisturbanceWaterBottomArea { get; set; }
        /// <summary>
        /// 涉及划定的饮用水源保护区、重点保护与珍稀水生生物的栖息地、重要水生生物的自然产卵场等敏感目标
        /// </summary>
        [DataMember]
        public bool IsIncludeSensitiveTargets { get; set; }
        /// <summary>
        /// 评价工作等级应不低于二级
        /// </summary>
        [DataMember]
        public enumAssessmentType MinAssessmentType { get; set; }
        /// <summary>
        /// 水文影响型判定类型
        /// </summary>
        [DataMember]
        public enumHydrologicSituation HydrologicSituation { get; set; }
        /// <summary>
        /// 评价工作等级提高一级
        /// </summary>
        [DataMember]
        public bool IsNeedUpgrade { get; set; }
        /// <summary>
        /// 是否含有引调水
        /// </summary>
        [DataMember]
        public bool IsIncludeWaterTransfer { get; set; }
        /// <summary>
        /// 是否含特殊情况
        /// </summary>
        [DataMember]
        public bool IsExceptionalCase { get; set; }
        /// <summary>
        /// 海水是否当做温度介质
        /// </summary>
        [DataMember]
        public bool IsTemperatureMedium { get; set; }
        /// <summary>
        /// 年径流量与总库容之比计算判断
        /// </summary>
        [DataMember]
        public bool IsAnnualDischarge { get; set; }
        /// <summary>
        /// 调水流量计算判断
        /// </summary>
        [DataMember]
        public bool IsWaterTransfer { get; set; }
        /// <summary>
        /// 海域类型判断方法
        /// </summary>
        [DataMember]
        public bool MethodOceanType { get; set; }
        /// <summary>
        /// 工程面积计算判断
        /// </summary>
        [DataMember]
        public bool IsTakeupWaterArea { get; set; }

    }
}
