using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sy.Global;

namespace SY.Models.ModelBase
{
    public enum enumScenarioFeild
    {
        //洪水影响评价洪水影响分析计算,
        //洪水风险图编制洪水影响分析计算,
        石化行业环境影响评价排河环境影响预测,
        石化行业环境影响评价海域环境影响预测,
        水电行业环境影响评价水库水温环境影响预测,
        防洪,
        水资源调度,
        水环境
    }
    public enum enumMethodType
    {
        解析解,
        数值解
    }
    public enum enumModelDomainType
    {
        平面一维,
        平面二维,
        一二维耦合,
        分层三维,
        立面二维
    }

    public enum enumModelScale
    {
        CIRP_SeaModel,
        CIRP_PortModel
    }

    public enum enumGridType
    {
        六点Abort,
        中心差分,
        正交,
        正交曲线,
        三角网,
        混合网格
    }
    public enum enumProcessType
    {
        水动力,
        水质,
        水温,
        水动力水质
    }
    public enum enumModelName
    {
        MIKE11,
        HECRAS,
        EFDC=1000,
        Delft3D=1001,
        MIKE21=1002,
        MIKE21FM=1003,
        MIKEFLOOD=1004,//11&21fm
        MIKEFLOODUrban=10041,//11&21&urban
        MIKE3=1005,
        WASH123D=1006,
        SYHD=1007,
        WESC2D=1008,
        EFDC2WASH=1009,
        MIKE2EFDC2WASH=1010,
        蓄满产流=2001,
        超渗产流=2002,
        单位线 =2011,
        马斯京根=2021,
        SWMMMIKE11=2022,
        暂未集成
    }
    public enum enumGridGenerateType
    {
        导入网格,
        在线剖分
    }

    public enum enumGridGenerateSteps
    {
        描绘边界,
        导入边界,
        网格剖分,
        导入散点,
        地形插值
    }

    public enum enumComponentType
    {
        持久性污染物,
        非持久性污染物,
        热排放
    }
    public enum enumComponent
    {
        化学需氧量COD,
        生化需氧量BOD5,
        溶解氧DO,
        氮循环,
        总氮TN,
        磷循环,
        总磷TP,
        叶绿素Chla,
        水温,
        余氯,
        重金属,
        泥沙
    }
    public enum enumSupportedGridType
    {
        MIKE非结构网格,
        Delft3D正交曲线网格,
        EFDC正交曲线网格,
        SMS_RMA2非结构网格,
        暂未集成
    }

    public interface IModelDomain
    {
        Guid ScenarioID { get; set; }

        enumSupportedGridType TypeName { get; set; }

        string Projection { get; set; }

        int NodeNo { get; set; }

        List<PointD> Nodes { get; set; }

        int ElementNo { get; set; }

        int ElementType { get; set; }

        List<List<int>> Elements { get; set; }

        float CentralLogitude { get; set; }

        int nCol { get; set; }

        int nRow { get; set; }

        bool IncludeMask { get; set; }
    }
}
