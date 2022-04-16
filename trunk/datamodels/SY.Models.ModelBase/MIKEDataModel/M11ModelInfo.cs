using Sy.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SY.Models.ModelBase.MIKE
{
    public class M11ModelInfo
    {
    }

    public class RiverInfo
    {
        public string Name { get; set; }
        public float Slope { get; set; }
        public float Length{get;set;}
        public int GridPtNo {get;set;}
        public List<double> Chainages { get; set; }
        public List<PointD> Coords { get; set; }
        public List<PointD> GridPoints { get; set; }
        public float MinDeltx { get; set; }
        public float MaxDeltx { get; set; }
        public List<float> GridLength { get; set; }
    }

    public class ModelInfo
    {
        private string desc = "暂时没有描述.";
        public string Description
        {
            get
            {
                return desc;
            }
            set { desc = value; }
        }
        private int ppSTNo;
        /// <summary>
        /// 雨量站个数
        /// </summary>
        public int PpSTNo
        {
            get { return ppSTNo; }
            set { ppSTNo = value; }
        }
        private int channelNo;
        /// <summary>
        /// 河段数
        /// </summary>
        public int ChannelNo
        {
            get { return channelNo; }
            set { channelNo = value; }
        }
        /// <summary>
        /// 河网最小空间步长
        /// </summary>
        public float MinDeltX
        {
            get
            {
                var q = (from r in RiversInfo
                         select r.MinDeltx).ToList<float>();
                return q.Min();
            }
        }
        /// <summary>
        /// 河网最大空间步长
        /// </summary>
        public float MaxDeltX
        {
            get
            {
                var q = (from r in RiversInfo
                         select r.MaxDeltx).ToList<float>();
                return q.Min();
            }
        }
        /// <summary>
        /// 当前模型的所有河网的基本信息
        /// </summary>
        public List<RiverInfo2> RiversInfo { get; set; }
        /// <summary>
        /// 河网的所有河段名称
        /// </summary>
        public List<string> ChannelNm { get; set; }
        /// <summary>
        /// 控制文件绝对路径
        /// </summary>
        public string ControlFile { get; set; }
        /// <summary>
        /// 模型投影信息
        /// </summary>
        public string Projection { get; set; }
        /// <summary>
        /// 模型方案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 结果文件
        /// </summary>
        public string resFile { get; set; }
    }

    [DataContract]
    /// <summary>
    /// 单个河段的基本信息
    /// </summary>
    public class RiverInfo2
    {
        [DataMember]
        /// <summary>
        /// 河段名称
        /// </summary>
        public string Name { get; set; }
        [DataMember]
        /// <summary>
        /// 河段平均坡度，算法：两两断面低高程求坡度再与总段数求平均。
        /// </summary>
        public float Slope
        {
            get
            {
                var lstMinZ = (from r in LstOfCs.Values
                               select (r.Select(e => e.Z)).ToList<double>().Min()).ToList<double>();
                var lstChainage = (from r in LstOfCs.Keys select r.Chainage).ToList<double>();
                double tmpSl = 0d;
                int idx = 0;//交点数
                for (int i = 0; i < lstMinZ.Count; i++)
                {
                    if (i + 1 == lstMinZ.Count) break;
                    if ((lstChainage[i + 1] - lstChainage[i]) == 0d) { idx++; continue; }//河段被支流截断后，交叉处重复存储了，在这里跳过
                    tmpSl += (lstMinZ[i] - lstMinZ[i + 1]) / (lstChainage[i + 1] - lstChainage[i]);
                }
                return (float)Math.Round(tmpSl * 1000 / (lstChainage.Count - 1 - idx), 5);
            }
            set
            {
                throw new Exception("Cannot be set");
            }
        }
        [DataMember]
        /// <summary>
        /// 河段长度
        /// </summary>
        public float Length
        {
            get
            {
                return (float)(Chainages.Max() - Chainages.Min());
            }
            set
            {
                throw new Exception("Cannot be set");
            }
        }
        [DataMember] //只读非泛型属性需要此标记以及引入set
        /// <summary>
        /// 河段计算点个数
        /// </summary>
        public int GridPtNo
        {
            get { return Chainages.Count; }
            set
            {
                throw new Exception("Cannot be set");
            }
        }
        [DataMember]
        /// <summary>
        /// 河段计算点里程值
        /// </summary>
        public List<double> Chainages { get; set; }
        [DataMember]
        /// <summary>
        /// 计算点，经纬度
        /// </summary>
        public List<PointD> GridPoints { get; set; }
        [DataMember]
        /// <summary>
        /// 点坐标，经纬度
        /// </summary>
        public List<PointD> Coords { get; set; }
        [DataMember]
        /// <summary>
        /// 河段最小空间步长
        /// </summary>
        public float MinDeltx
        {
            get { return GridLength.Min(); }
            set
            {
                throw new Exception("Cannot be set");
            }
        }
        [DataMember]
        /// <summary>
        /// 河段最大空间步长
        /// </summary>
        public float MaxDeltx
        {
            get { return GridLength.Max(); }
            set
            {
                throw new Exception("Cannot be set");
            }
        }
        [DataMember]
        /// <summary>
        /// 河段计算点之间的距离
        /// </summary>
        public List<float> GridLength
        {
            get
            {
                List<float> tmp = new List<float>();
                List<double> discChainages = Chainages.Distinct().ToList<double>();//清除河段支流交叉点重复记录的里程
                for (int i = 0; i < discChainages.Count; i++)
                {
                    if (i + 1 < discChainages.Count)
                    {
                        tmp.Add((float)(discChainages[i + 1] - Chainages[i]));
                    }
                }
                return tmp;
            }
            set
            {
                throw new Exception("Cannot be set");
            }
        }
        [DataMember]
        /// <summary>
        /// 河段的 里程&断面x,z,z
        /// </summary>
        public Dictionary<RouteLocationEx, List<PointD>> LstOfCs { get; set; }
    }
}
