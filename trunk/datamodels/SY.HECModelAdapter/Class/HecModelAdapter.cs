using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SY.Models.ModelBase;
using SY.Common;
using System.IO;
using SY.Utility;
using System.Data;
using System.Configuration;
using Sy.Global;
using RAS505;
using System.ComponentModel;
using System.Threading;
using SY.CommonDataModel;
using Hec.Dss;

namespace SY.HECModelAdapter
{
    public class HecModelAdapter
    {
        #region 字段
        private static Configuration config = null;
        private string pythonEngine;
        private static bool isRunOk = false;
        private string currentPlanFilepath;
        //private static List<string> calmsg = new List<string>();
        #endregion

        #region 构造体
        public HecModelAdapter()
        {
        }

        public HecModelAdapter(bool configed)
        {
            var assemblyPath = this.GetType().Assembly.Location;
            var cfigfile = assemblyPath + ".config";
            config = ConfigurationManager.OpenExeConfiguration(assemblyPath);
            pythonEngine = config.AppSettings.Settings["PythonEngine"].Value;
        }

        public HecModelAdapter(string folderPath, string prjName = "MZH")
        {
            ProjectDir = folderPath;
            //wf （1）在Boundary类里扩展
            //    (2) 实现一个HecModelAdapter构造函数，增加一个属性 List<Boundary>
            //    (3) 传入一个目录，首先读取prj文件，然后确认u0x和p0x文件，读取数据填充到List<Boundary>属性中，供贾总调用。
            var boundaryList = new List<Boundary>();
            var intConditionList = new List<InitialCondition>();
            var wqParaList = new List<WQParameter>();

            try
            {
                #region//获取文件配置信息
                // 遍历目录下全部文件，找到第一个*.prj工程文件路径
                DirectoryInfo tDir = new DirectoryInfo(folderPath);
                FileInfo tPrjFile1 = null;
                foreach (FileInfo tfi in tDir.GetFiles(prjName + ".prj"))
                {
                    tPrjFile1 = tfi;
                    break;
                }

                //step1 读取prj文件，获取Current Plan的值
                {
                    string[] lines = System.IO.File.ReadAllLines(tPrjFile1.FullName);
                    foreach (string line1 in lines)
                    {
                        if (line1.Contains("Proj Title="))
                        {
                            ProjTitle = line1.Replace("Proj Title=", "");
                            DSSFile = ProjTitle + "." + "dss";
                        }
                        if (line1.Contains("Current Plan="))
                        {
                            PlanFile = ProjTitle + "." + line1.Replace("Current Plan=", "");
                        }
                        if (line1.Contains("Geom File="))
                        {
                            Geofile = ProjTitle + "." + line1.Replace("Geom File=", "");
                        }
                        /// boundaryFile
                        /// 赋值 BoundaryFile 属性
                        if (line1.Contains("Unsteady File="))
                        {
                            UnsteadyBoundaryFile = ProjTitle + "." + line1.Replace("Unsteady File=", "");
                        }
                        if (line1.Contains("Water Quality File="))
                        {
                            WqFile = ProjTitle + "." + line1.Replace("Water Quality File=", "");
                        }
                    }
                }
                currentPlanFilepath = tPrjFile1.DirectoryName + @"\" + PlanFile;
                #endregion

                #region//解析拓扑信息
                ModelTopo = GetGeometry(Path.Combine(ProjectDir, Geofile));
                #endregion

                #region//解析模拟方案信息
                //step2 读取Current Plan 的 Simulation Date
                //如果plan文件模拟时间为空，默认赋值2022-1-1 00:00:00
                //DateTime startDt = new DateTime(2022, 1, 1, 0, 0, 0);
                //DateTime stopDt = new DateTime(2022, 1, 1, 0, 0, 0);
                string flowFileExtentsion = "";
                {
                    string[] lines = System.IO.File.ReadAllLines(currentPlanFilepath);
                    foreach (string line1 in lines)
                    {
                        if (line1.Contains("Simulation Date="))
                        {
                            string tempSD = line1.Replace("Simulation Date=", "");
                            //if (tempSD.Length >= 37)
                            {
                                string[] tempParts = tempSD.Split(',');
                                if (tempParts.Length == 4)
                                {
                                    int startYear = 0, startMon = 0, startDay = 0, stopYear = 0, stopMon = 0, stopDay = 0;
                                    int startHour = 0, startMinu = 0, startSec = 0, stopHour = 0, stopMinu = 0, stopSec = 0;
                                    ConvertDateStr2YMD(tempParts[0], out startYear, out startMon, out startDay);
                                    ConvertDateStr2YMD(tempParts[2], out stopYear, out stopMon, out stopDay);
                                    //if (tempParts[1].Length != 8)
                                    //{
                                    //    throw new Exception("无效的开始时间");
                                    //}
                                    //if (tempParts[3].Length != 8)
                                    //{
                                    //    throw new Exception("无效的结束时间");
                                    //}
                                    if (tempParts[1] != "0000")
                                    {
                                        startHour = int.Parse(tempParts[1].Split(':')[0]);
                                        startMinu = int.Parse(tempParts[1].Split(':')[1]);
                                        startSec = int.Parse(tempParts[1].Split(':')[2]);
                                    }
                                    if (tempParts[3] != "0000")
                                    {
                                        stopHour = int.Parse(tempParts[3].Split(':')[0]);
                                        stopMinu = int.Parse(tempParts[3].Split(':')[1]);
                                        stopSec = int.Parse(tempParts[3].Split(':')[2]);
                                    }

                                    SimulationStartTime = new DateTime(startYear, startMon, startDay, startHour, startMinu, startSec);
                                    SimulationEndTime = new DateTime(stopYear, stopMon, stopDay, stopHour, stopMinu, stopSec);
                                }
                            }
                        }
                        else if (line1.Contains("Flow File="))
                        {
                            flowFileExtentsion = line1.Replace("Flow File=", "");
                        }
                        else if (line1.StartsWith("Output Interval="))
                        {
                            var tmp = line1.Replace("Output Interval=", "");
                            if (tmp.Contains("HOUR"))
                                OutputTimeInterval = int.Parse(tmp.Substring(0, tmp.Length - 4)) * 3600;
                            else if (tmp.ToUpper().Contains("DAY"))
                                OutputTimeInterval = int.Parse(tmp.Substring(0, tmp.Length - 3)) * 24 * 3600;
                            else if (tmp.ToUpper().Contains("WEEK"))
                                OutputTimeInterval = int.Parse(tmp.Substring(0, tmp.Length - 4)) * 7 * 24 * 3600;
                            //else if (tmp.ToUpper().Contains("MONTH"))
                            //    OutputTimeInterval = int.Parse(tmp.Substring(0, tmp.Length - 5)) * 24 * 3600;
                            OutputTimeStepNo = (int)((SimulationEndTime - SimulationStartTime).TotalSeconds / OutputTimeInterval);
                        }
                    }
                }
                #endregion

                #region//解析边界信息

                //step3 读取Unsteady File 文件中的 Boundary数据
                var flowFilePath = tPrjFile1.DirectoryName + @"\" + UnsteadyBoundaryFile;
                if (File.Exists(flowFilePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(flowFilePath);
                    for (int iline = 0; iline < lines.Length; ++iline)
                    {
                        if (lines[iline].Contains("Boundary Location="))
                        {
                            Boundary tBoundary = makeUnsteadyBoundaryByLines(lines, iline, SimulationStartTime);
                            if (tBoundary != null)
                                boundaryList.Add(tBoundary);
                        }
                    }
                }

                //step4 读取water quality边界文件中的水质边界信息
                var wqFilePath = tPrjFile1.DirectoryName + @"\" + this.WqFile;
                if (File.Exists(wqFilePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(wqFilePath);
                    for (int iline = 0; iline < lines.Length; iline++)
                    {
                        if (lines[iline].Contains("Constituent="))
                        {
                            if (lines[iline + 1].Contains("BC="))
                            {
                                makeWqBoundaryByLines(lines, ref iline, SimulationStartTime, SimulationEndTime,
                                ref boundaryList);
                                iline--;
                            }

                            if (lines[iline + 1].Contains("IC="))
                            {
                                //遍历该水质组成所有边界初始
                                makeWqIcSection(lines, ref iline, SimulationStartTime, SimulationEndTime, ref intConditionList);
                                iline--;
                            }
                        }
                        while (lines[iline].StartsWith("Dispersion="))
                        {
                            makeWqDcSection(lines, ref iline, "", ref wqParaList);
                        }
                    }
                }

                BoundaryList = boundaryList;
                IntConditionList = intConditionList;
                WqParamList = wqParaList;

                #endregion
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
            }
        }

        #endregion

        #region 方法

        public bool RunModel(string controlfile)
        {
            try
            {
                var dir = Path.GetDirectoryName(controlfile);
                var logfile = Directory.GetFiles(dir, "*.computeMsgs.txt").FirstOrDefault();
                if (File.Exists(logfile)) File.Delete(logfile);
                // 启用背景线程计算模型
                RunModelInBackgroud(controlfile);
                System.Threading.Thread.Sleep(100);

                // 读取日志
                while (!isRunOk)
                {
                    logfile = Directory.GetFiles(dir, "*.computeMsgs.txt").FirstOrDefault();

                    while (!isRunOk && File.Exists(logfile))
                    {
                        var bakfile = logfile + ".bak";
                        File.Copy(logfile, bakfile, true);

                        using (FileStream fs = new FileStream(bakfile, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                var prgs = new List<string>();
                                var msg = string.Empty;
                                while (!sr.EndOfStream)
                                {
                                    msg = sr.ReadLine();
                                    //if (msg.StartsWith("Progress"))
                                    {
                                        prgs.Add(msg);
                                    }
                                }
                                //prgs.Add(msg);

                                if (prgs.Count > 0)
                                {
                                    //CommonUtility.Log(prgs.Last());
                                    if (OutputMsg != null)
                                        OutputMsg(new MessageInfo() { Tag = 0, Message = prgs.Last() });
                                }
                                if (msg.Contains("Finished Post Processing")) isRunOk = true;
                            }
                            //fs.Close();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                return false;
            }
        }
        /// <summary>
        /// 进一步优化为每次只返回上次未返回的信息
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public string GetCalMsg(string dir)
        {
            var data = string.Empty;
            isRunOk = false;

            //while (!isRunOk)
            {
                var logfile = Directory.GetFiles(dir, "*.computeMsgs.txt", SearchOption.AllDirectories).FirstOrDefault();
                CommonUtility.Log(logfile);
                //while (!isRunOk && File.Exists(logfile))
                while (File.Exists(logfile))
                {
                    //var bakfile = logfile + ".bak2";
                    //File.Copy(logfile, bakfile, true);

                    using (FileStream fs = new FileStream(logfile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            data = sr.ReadToEnd();
                            //isRunOk = true;
                        }
                    }
                    break;
                }
            }

            //if (isRunOk) data = "计算已结束！";

            return data;
        }

        public void RunModelInBackgroud(string controlfile)
        {
            //var bgw = new BackgroundWorker();
            //bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
            //bgw.DoWork += (sender, e) =>
            //{
            new Thread(() =>
            {
                HECRASController heccore = new HECRASController();
                int nmsg = 0;
                bool block = true;
                Array sa = null;
                heccore.Project_Open(controlfile);
                heccore.Compute_ShowComputationWindow();

                heccore.Compute_CurrentPlan(ref nmsg, sa, ref block);

                heccore.Project_Save();
                heccore.Project_Close();
                heccore.QuitRas();
                isRunOk = true;
            }).Start();
            //};
        }

        public void GenerateGemetryFileByShp(string shpfile, string geoFile, double tolerence)
        {
            try

            {
                var hecModel = GenerateGemetryByShp(shpfile, tolerence);
                //write geofile
                WriteGeoFile(hecModel, geoFile);
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
            }

        }

        public HEC_DM GenerateGemetryByShp(string shpfile, double tolerence)
        {
            try
            {
                //read shp file get info to hec_dm
                var u = new Utility.Utility();
                //增加FromNode、ToNode字段
                //var fields = new Dictionary<string, Type>();
                //fields.Add("FromNode", typeof(int));
                //fields.Add("ToNode", typeof(int));
                //u.AddShpfield(shpfile, fields);
                //调用arc python计算河网连接关系
                //u.CalLineConnectivity(pythonEngine, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gis\\lineNodeID.py"),
                //    shpfile, tolerence);

                //获取河网属性及空间信息
                var dt = u.GetAttibutesFromShp(shpfile);
                var geo = u.GetPolylineVertices(shpfile);

                HEC_DM hecModel = new HEC_DM();
                hecModel.JunctCollection = new List<Junctor>();
                hecModel.RiverReachCollection = new List<RiverReach>();

                int idx = 0;
                //增加一个checkstate字段
                dt.Columns.Add("F_check", typeof(int));
                dt.Columns.Add("T_check", typeof(int));

                foreach (DataRow r in dt.Rows)
                {
                    var fid = idx;
                    var rr = new RiverReach();
                    rr.RiverName = r["RiverCode"].ToString();
                    rr.ReachName = r["ReachCode"].ToString();
                    rr.Points = geo[fid];
                    var avg_x = (from q in rr.Points select q.X).Average();
                    var avg_y = (from q in rr.Points select q.Y).Average();
                    rr.TextLocation = new Sy.Global.PointD(avg_x - 50d, avg_y - 50d);

                    hecModel.RiverReachCollection.Add(rr);

                    Junctor jc = null;

                    if (r["F_check"] != null && r["F_check"].ToString() != "" && int.Parse(r["F_check"].ToString()) == 1
                        &&
                        r["T_check"] != null && r["T_check"].ToString() != "" && int.Parse(r["T_check"].ToString()) == 1)
                        continue;
                    //找上游
                    else if (r["F_check"] == null && r["F_check"].ToString() == "")
                    {
                        var upNodesFID = new List<object>();
                        var dwnNodesFID = new List<object>();

                        var fNode = r["FromNode"];

                        int idx2 = 0;
                        foreach (DataRow r2 in dt.Rows)
                        {
                            var fromNode2 = r2["FromNode"];
                            var toNode2 = r2["ToNode"];
                            //find upstream node
                            if (fNode.Equals(toNode2))
                            {
                                upNodesFID.Add(idx2);
                            }
                            //find dwnstream node
                            if (fNode.Equals(fromNode2))
                            {
                                dwnNodesFID.Add(idx2);
                            }
                            idx2++;
                        }
                        if (upNodesFID.Count > 0)
                        {
                            var stapt = geo[fid].FirstOrDefault();

                            jc = new Junctor();
                            jc.Name = "jc_" + r["ReachCode"].ToString();
                            jc.Location = stapt;
                            jc.TextLocation = new Sy.Global.PointD(stapt.X - 50d, stapt.Y - 50d);
                            jc.JuctionLengthAndAngle = new List<double[]>();
                            jc.UpRiverReach = new List<string[]>();
                            jc.DownRiverReach = new List<string[]>();

                            foreach (var n in upNodesFID)
                            {
                                var fidtmp = int.Parse(n.ToString());
                                jc.UpRiverReach.Add(new string[] {
                                dt.Rows[fidtmp]["RiverCode"].ToString(),
                            dt.Rows[fidtmp]["ReachCode"].ToString() });
                                jc.JuctionLengthAndAngle.Add(new double[] { 0d, 0d });

                                dt.Rows[fidtmp]["T_check"] = 1;
                            }
                            foreach (var n in dwnNodesFID)
                            {
                                var fidtmp = int.Parse(n.ToString());
                                jc.DownRiverReach.Add(new string[] {
                                dt.Rows[fidtmp]["RiverCode"].ToString(),
                            dt.Rows[fidtmp]["ReachCode"].ToString() });
                            }

                            hecModel.JunctCollection.Add(jc);
                        }
                        idx++;
                    }
                    else
                    {
                        var upNodesFID = new List<object>();
                        var dwnNodesFID = new List<object>();

                        var fNode = r["ToNode"];

                        int idx2 = 0;
                        foreach (DataRow r2 in dt.Rows)
                        {
                            var fromNode2 = r2["FromNode"];
                            var toNode2 = r2["ToNode"];
                            //find upstream node
                            if (fNode.Equals(toNode2))
                            {
                                upNodesFID.Add(idx2);
                            }
                            //find dwnstream node
                            if (fNode.Equals(fromNode2))
                            {
                                dwnNodesFID.Add(idx2);
                            }
                            idx2++;
                        }
                        if (dwnNodesFID.Count > 0)
                        {
                            var endpt = geo[fid].LastOrDefault();

                            jc = new Junctor();
                            jc.Name = "jc_" + r["ReachCode"].ToString();
                            jc.Location = endpt;
                            jc.TextLocation = new Sy.Global.PointD(endpt.X - 50d, endpt.Y - 50d);
                            jc.JuctionLengthAndAngle = new List<double[]>();
                            jc.UpRiverReach = new List<string[]>();
                            jc.DownRiverReach = new List<string[]>();

                            foreach (var n in upNodesFID)
                            {
                                var fidtmp = int.Parse(n.ToString());
                                jc.UpRiverReach.Add(new string[] {
                                dt.Rows[fidtmp]["RiverCode"].ToString(),
                            dt.Rows[fidtmp]["ReachCode"].ToString() });
                                jc.JuctionLengthAndAngle.Add(new double[] { 0d, 0d });
                            }
                            foreach (var n in dwnNodesFID)
                            {
                                var fidtmp = int.Parse(n.ToString());
                                jc.DownRiverReach.Add(new string[] {
                                dt.Rows[fidtmp]["RiverCode"].ToString(),
                            dt.Rows[fidtmp]["ReachCode"].ToString() });

                                dt.Rows[fidtmp]["F_check"] = 1;
                            }

                            hecModel.JunctCollection.Add(jc);
                        }
                        idx++;
                    }
                }

                return hecModel;
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                return null;
            }

        }

        public void WriteGeoFile(HEC_DM hecModel, string geoFile)
        {
            try
            {
                var sw = new StreamWriter(geoFile, false, Encoding.Default);
                sw.WriteLine(string.Format("Geom Title={0}", hecModel.Title));
                sw.WriteLine(string.Format("Program Version={0}", hecModel.Version));
                sw.WriteLine("");

                foreach (var juc in hecModel.JunctCollection)
                {
                    sw.WriteLine(string.Format("Junct Name={0}", juc.Name));
                    //sw.WriteLine(string.Format("Junct Desc={0},{1},{2},{3},{4}", juc.Desc[0], juc.Desc[1],
                    //    juc.Desc[2], juc.Desc[3], juc.Desc[4]));
                    sw.WriteLine("Junct Desc=, 0 , 0 ,-1 ,0");
                    sw.WriteLine(string.Format("Junct X Y & Text X Y={0},{1},{2},{3}", juc.Location.X,
                        juc.Location.Y, juc.TextLocation.X, juc.TextLocation.Y));
                    if (juc.UpRiverReach != null)
                        foreach (var ur in juc.UpRiverReach)
                            sw.WriteLine(string.Format("Up River,Reach={0},{1}", ur[0], ur[1]));

                    if (juc.DownRiverReach != null)
                        foreach (var dr in juc.DownRiverReach)
                            sw.WriteLine(string.Format("Dn River,Reach={0},{1}", dr[0], dr[1]));

                    if (juc.JuctionLengthAndAngle != null)
                        foreach (var la in juc.JuctionLengthAndAngle)
                            sw.WriteLine(string.Format("Junc L&A={0},{1}", la[0], la[1]));

                    sw.WriteLine("");
                }

                foreach (var river in hecModel.RiverReachCollection)
                {
                    sw.WriteLine(string.Format("River Reach={0},{1}", river.RiverName, river.ReachName));
                    sw.WriteLine(string.Format("Reach XY= {0}", river.Points.Count));
                    for (int i = 0; i < river.Points.Count; i++)
                    {
                        if (i != 0 && i % 2 == 0)
                        {
                            sw.Write("\r\n");
                            sw.Write(string.Format("{0,16}{1,16}", river.Points[i].X, river.Points[i].Y));
                        }
                        else
                            sw.Write(string.Format("{0,16}{1,16}", river.Points[i].X, river.Points[i].Y));
                    }
                    sw.Write("\r\n");
                    sw.WriteLine(string.Format("Rch Text X Y={0},{1}", river.TextLocation.X, river.TextLocation.Y));
                    sw.WriteLine("Reverse River Text= 0 ");
                    sw.WriteLine("");

                    foreach (var cs in river.CSCollection)
                    {
                        string stp = string.Empty;
                        foreach (var s in cs.Location)
                            stp += s + ",";
                        stp = stp.Substring(0, stp.Length - 1);
                        sw.WriteLine(string.Format("Type RM Length L Ch R = {0}", stp));
                        if (cs.Description != null)
                        {
                            sw.WriteLine("BEGIN DESCRIPTION:");
                            sw.WriteLine(cs.Description);
                            sw.WriteLine("END DESCRIPTION:");
                        }
                        sw.WriteLine(cs.LastEditedTime);
                        sw.WriteLine(string.Format("#Sta/Elev= {0}", cs.Data.Count));

                        for (int i = 0; i < cs.Data.Count; i++)
                        {
                            if (i != 0 && i % 5 == 0)
                            {
                                //sw.Write("\r\n");
                                sw.Write(Environment.NewLine);
                                sw.Write(string.Format("{0,8}{1,8}", cs.Data[i].X, cs.Data[i].Y));
                            }
                            else
                                sw.Write(string.Format("{0,8}{1,8}", cs.Data[i].X, cs.Data[i].Y));
                        }
                        sw.Write("\r\n");
                        sw.WriteLine("#Mann= 3 , 0 , 0");
                        sw.WriteLine(cs.Manning);
                        sw.WriteLine(cs.ManningSta);
                        sw.WriteLine("XS Rating Curve= 0 ,0");
                        sw.WriteLine(cs.XS_HTab_Starting);
                        sw.WriteLine("XS HTab Horizontal Distribution= 5 , 5 , 5");
                        sw.WriteLine("Exp/Cntr=0.3,0.1");
                        sw.WriteLine();
                    }

                }

                sw.WriteLine("LCMann Time=Dec/30/1899 00:00:00");
                sw.WriteLine("LCMann Region Time=Dec/30/1899 00:00:00");
                sw.WriteLine("LCMann Table=0");
                sw.WriteLine("Chan Stop Cuts=-1");
                sw.WriteLine("");
                sw.WriteLine("Use User Specified Reach Order=0");
                sw.WriteLine("GIS Ratio Cuts To Invert=-1");
                sw.WriteLine("GIS Limit At Bridges=0");
                sw.WriteLine("Composite Channel Slope=5");
                sw.Close();
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
            }
        }

        public HEC_DM GetGeometry(string geofile)
        {
            try
            {
                HEC_DM hecModel = new HEC_DM();
                hecModel.JunctCollection = new List<Junctor>();
                hecModel.RiverReachCollection = new List<RiverReach>();

                var sr = new StreamReader(geofile, Encoding.Default);

                string line = string.Empty;
                while (!sr.EndOfStream)
                {
                    //读取连接点
                    if (line.Trim().StartsWith("Junct Name"))
                    {
                        var jc = new Junctor();
                        var st = line.Split('=')[1];
                        jc.Name = st.Trim();
                        jc.Desc = sr.ReadLine().Split('=')[1].Split(',');
                        var loc = sr.ReadLine().Split('=')[1].Split(',');
                        jc.Location = new PointD(double.Parse(loc[0]), double.Parse(loc[1]));
                        jc.TextLocation = new PointD(double.Parse(loc[2]), double.Parse(loc[3]));
                        jc.UpRiverReach = new List<string[]>();
                        jc.DownRiverReach = new List<string[]>();
                        jc.JuctionLengthAndAngle = new List<double[]>();
                        line = sr.ReadLine();
                        while ((line.Trim()).StartsWith("Up River"))
                        {
                            jc.UpRiverReach.Add(line.Split('=')[1].Split(','));
                            line = sr.ReadLine();
                        }
                        while ((line.Trim()).StartsWith("Dn River"))
                        {
                            jc.DownRiverReach.Add(line.Split('=')[1].Split(','));
                            line = sr.ReadLine();
                        }
                        while ((line.Trim()).StartsWith("Junc L&A"))
                        {
                            jc.JuctionLengthAndAngle.Add((line.TrimEnd(',').Split('=')[1].Split(',')).Select(r => double.Parse(r)).ToArray());
                            line = sr.ReadLine();
                        }
                        hecModel.JunctCollection.Add(jc);
                    }
                    //读取河段及断面
                    else if (line.Trim().StartsWith("River Reach"))
                    {
                        var rr = new RiverReach();
                        var st = line.Split('=')[1].Split(',');
                        rr.RiverName = st[0].Trim();
                        rr.ReachName = st[1].Trim();

                        line = sr.ReadLine();
                        st = line.Split('=');
                        var ptsno = int.Parse(st[1].Trim());
                        var pts = new string[ptsno * 2];
                        rr.Points = new List<Sy.Global.PointD>();

                        pts = readHecDataTable(ref sr, ptsno * 2);

                        for (int i = 0; i < ptsno * 2;)
                        {
                            rr.Points.Add(new Sy.Global.PointD(double.Parse(pts[i].Trim())
                                , double.Parse(pts[i + 1].Trim())));
                            i = i + 2;
                        }

                        JumpUnuseLine(ref sr, ref line);

                        if (line.StartsWith("Rch Text X Y"))
                        {
                            st = line.Split('=')[1].Split(',');
                            rr.TextLocation = new Sy.Global.PointD(double.Parse(st[0].Trim()), double.Parse(st[1].Trim()));
                        }

                        JumpUnuseLine(ref sr, ref line);

                        rr.CSCollection = new List<HECCrossSection>();

                        JumpUnuseLine(ref sr, ref line);

                        while (line.StartsWith("Type RM Length L Ch R"))
                        {
                            var cs = new HECCrossSection();
                            cs.Location = line.Split('=')[1].Split(',');

                            JumpUnuseLine(ref sr, ref line);

                            if (line.StartsWith("XS GIS Cut Line")) // jump XS GIS Cut Line
                            {
                                st = line.Split('=');
                                ptsno = int.Parse(st[1]);
                                pts = new string[ptsno * 2];
                                pts = readHecDataTable(ref sr, ptsno * 2);

                                JumpUnuseLine(ref sr, ref line);
                            }

                            if (line.StartsWith("BEGIN DESCRIPTION:"))
                            {
                                cs.Description = sr.ReadLine();

                                JumpUnuseLine(ref sr, ref line);
                            }
                            if (line.StartsWith("Node Last Edited Time="))
                            {
                                cs.LastEditedTime = line;

                                JumpUnuseLine(ref sr, ref line);
                            }
                            if (line.StartsWith("#Sta/Elev"))
                            {
                                cs.Data = new List<Sy.Global.PointD>();
                                st = line.Split('=');
                                ptsno = int.Parse(st[1]);

                                pts = new string[ptsno];
                                pts = readHecDataTable(ref sr, ptsno, 5);
                                for (int i = 0; i < ptsno; i++)
                                {
                                    var regx = new System.Text.RegularExpressions.Regex("[ ]+|\r\n");
                                    var pt = regx.Split(pts[i].Trim());
                                    cs.Data.Add(new Sy.Global.PointD(double.Parse(pt[0].Trim())
                                        , double.Parse(pt[1].Trim())));
                                }

                                JumpUnuseLine(ref sr, ref line);
                            }

                            if (line.StartsWith("#Mann"))
                            {
                                cs.Manning = sr.ReadLine();
                                cs.ManningSta = sr.ReadLine();

                                JumpUnuseLine(ref sr, ref line);
                            }

                            if (line.StartsWith("XS Rating Curve"))
                            {
                                JumpUnuseLine(ref sr, ref line);
                            }

                            if (line.StartsWith("XS HTab Starting El and Incr"))
                            {
                                cs.XS_HTab_Starting = line;

                                JumpUnuseLine(ref sr, ref line);
                            }

                            if (line.StartsWith("XS HTab Horizontal Distribution"))
                            {
                                JumpUnuseLine(ref sr, ref line);
                            }

                            if (line.StartsWith("Exp/Cntr(USF)"))
                            {
                                JumpUnuseLine(ref sr, ref line);
                            }

                            if (line.StartsWith("Exp/Cntr"))
                            {
                                JumpUnuseLine(ref sr, ref line);
                            }

                            rr.CSCollection.Add(cs);
                        }

                        hecModel.RiverReachCollection.Add(rr);
                    }
                    else line = sr.ReadLine();
                }

                sr.Close();

                return hecModel;
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hecModel">由shp生成的河网及连接关系</param>
        /// <param name="hecModel2">河网及断面信息</param>
        /// <param name="geoFile">根据河网关联将hecmodel2中的断面信息合并到hecmodel中</param>
        public void MergeGeometry(HEC_DM hecModel, HEC_DM hecModel2, string geoFile)
        {
            try
            {
                var sw = new StreamWriter(geoFile, false, Encoding.Default);
                sw.WriteLine(string.Format("Geom Title={0}", hecModel.Title));
                sw.WriteLine(string.Format("Program Version={0}", hecModel.Version));
                sw.WriteLine("");

                foreach (var juc in hecModel.JunctCollection)
                {
                    sw.WriteLine(string.Format("Junct Name={0}", juc.Name));
                    //sw.WriteLine(string.Format("Junct Desc={0},{1},{2},{3},{4}", juc.Desc[0], juc.Desc[1],
                    //    juc.Desc[2], juc.Desc[3], juc.Desc[4]));
                    sw.WriteLine("Junct Desc=, 0 , 0 ,-1 ,0");
                    sw.WriteLine(string.Format("Junct X Y & Text X Y={0},{1},{2},{3}", juc.Location.X,
                        juc.Location.Y, juc.TextLocation.X, juc.TextLocation.Y));
                    if (juc.UpRiverReach != null)
                        foreach (var ur in juc.UpRiverReach)
                            sw.WriteLine(string.Format("Up River,Reach={0},{1}", ur[0], ur[1]));

                    if (juc.DownRiverReach != null)
                        foreach (var dr in juc.DownRiverReach)
                            sw.WriteLine(string.Format("Dn River,Reach={0},{1}", dr[0], dr[1]));

                    if (juc.JuctionLengthAndAngle != null)
                        foreach (var la in juc.JuctionLengthAndAngle)
                            sw.WriteLine(string.Format("Junc L&A={0},{1}", la[0], la[1]));

                    sw.WriteLine("");
                }

                foreach (var river in hecModel.RiverReachCollection)
                {
                    sw.WriteLine(string.Format("River Reach={0},{1}", river.RiverName, river.ReachName));
                    sw.WriteLine(string.Format("Reach XY= {0}", river.Points.Count));
                    for (int i = 0; i < river.Points.Count; i++)
                    {
                        if (i != 0 && i % 2 == 0)
                        {
                            sw.Write("\r\n");
                            sw.Write(string.Format("{0,16}{1,16}", river.Points[i].X, river.Points[i].Y));
                        }
                        else
                            sw.Write(string.Format("{0,16}{1,16}", river.Points[i].X, river.Points[i].Y));
                    }
                    sw.Write("\r\n");
                    sw.WriteLine(string.Format("Rch Text X Y={0},{1}", river.TextLocation.X, river.TextLocation.Y));
                    sw.WriteLine("Reverse River Text= 0 ");
                    sw.WriteLine("");

                    var rr = (from r in hecModel2.RiverReachCollection
                              where r.RiverName.Equals(river.ReachName) && r.ReachName.Equals(river.ReachName)
                              select r).FirstOrDefault();
                    if (rr != null)
                    {
                        foreach (var cs in rr.CSCollection)
                        {
                            string stp = string.Empty;
                            foreach (var s in cs.Location)
                                stp += s + ",";
                            stp = stp.Substring(0, stp.Length - 1);
                            sw.WriteLine(string.Format("Type RM Length L Ch R = {0}", stp));
                            sw.WriteLine(cs.LastEditedTime);
                            sw.WriteLine(string.Format("#Sta/Elev= {0}", cs.Data.Count));

                            for (int i = 0; i < cs.Data.Count; i++)
                            {
                                if (i != 0 && i % 4 == 0)
                                {
                                    sw.Write("\r\n");
                                    sw.Write(string.Format("{0,8}{1,8}", cs.Data[i].X, cs.Data[i].Y));
                                }
                                else
                                    sw.Write(string.Format("{0,8}{1,8}", cs.Data[i].X, cs.Data[i].Y));
                            }
                            sw.Write("\r\n");
                            sw.WriteLine("#Mann= 3 , 0 , 0");
                            sw.WriteLine(cs.Manning);
                            sw.WriteLine(cs.ManningSta);
                            sw.WriteLine("XS Rating Curve= 0 ,0");
                            sw.WriteLine(cs.XS_HTab_Starting);
                            sw.WriteLine("XS HTab Horizontal Distribution= 5 , 5 , 5");
                            sw.WriteLine("Exp/Cntr=0.3,0.1");
                            sw.WriteLine();
                        }
                    }
                }

                sw.WriteLine("LCMann Time=Dec/30/1899 00:00:00");
                sw.WriteLine("LCMann Region Time=Dec/30/1899 00:00:00");
                sw.WriteLine("LCMann Table=0");
                sw.WriteLine("Chan Stop Cuts=-1");
                sw.WriteLine("");
                sw.WriteLine("Use User Specified Reach Order=0");
                sw.WriteLine("GIS Ratio Cuts To Invert=-1");
                sw.WriteLine("GIS Limit At Bridges=0");
                sw.WriteLine("Composite Channel Slope=5");
                sw.Close();
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
            }
        }
        /// <summary>
        /// 对已有河网文件批量导入断面文件，hdm格式
        /// </summary>
        /// <param name="hdmfodler">各河段断面文件存储文件夹：cs\riverreach1;cs\riverreach2\。。。</param>
        /// <param name="geometryFile">原始河网文件</param>
        public void ImportHdm2Gemetry(string hdmfodler, string geometryFile, int tag)
        {
            try
            {
                //get geometry
                var hec1 = GetGeometry(geometryFile);
                switch (tag)
                {
                    case 0:
                        {
                            foreach (var rr in hec1.RiverReachCollection)
                            {
                                //get chainages
                                var files = Directory.GetFiles(Path.Combine(hdmfodler, rr.ReachName), "*.hdm");
                                var chainages = new List<float>();
                                foreach (var file in files)
                                {
                                    var filename = Path.GetFileNameWithoutExtension(file);
                                    if (filename.StartsWith("横断面"))
                                        filename = filename.Remove(0, "横断面".Length);
                                    if (filename.Contains("+"))
                                    {
                                        var tmp = filename.Split('+');
                                        filename = string.Empty;
                                        foreach (var s in tmp)
                                            filename += s.Trim();
                                        chainages.Add(float.Parse(filename.Trim()) * -1); //桩号倒排时乘以-1处理
                                    }
                                    else if (filename.Contains("-"))
                                    {
                                        var tmp = filename.Split('-');
                                        filename = string.Empty;
                                        foreach (var s in tmp)
                                            filename += s.Trim();
                                        chainages.Add(float.Parse(filename.Trim()));
                                    }
                                    else
                                    {
                                        chainages.Add(float.Parse(filename.Trim()));
                                    }
                                }
                                chainages = chainages.OrderByDescending(x => x).ToList<float>();

                                //get crosssection
                                var csc = new List<HECCrossSection>();
                                int idx = 0;
                                foreach (var file in files)
                                {
                                    var cspts = new List<PointD>();
                                    var cs = new HECCrossSection();
                                    cs.Description = Path.GetFileNameWithoutExtension(file);
                                    //read each hdm file
                                    var sr = new StreamReader(file, Encoding.Default);
                                    string line = sr.ReadLine();
                                    while (!sr.EndOfStream)
                                    {
                                        line = sr.ReadLine();
                                        var pt = line.Trim().Split(',');
                                        if (pt.Count() == 1) pt = line.Trim().Split('，');
                                        cspts.Add(new PointD() { X = double.Parse(pt[0]), Y = double.Parse(pt[1]) });
                                    }
                                    sr.Close();

                                    cs.Data = cspts;
                                    cs.Location = new string[5];
                                    cs.Location[0] = "1";
                                    cs.Location[1] = chainages[idx].ToString();

                                    if (idx == files.Count() - 1)
                                    {
                                        cs.Location[2] = "0";
                                        cs.Location[3] = cs.Location[2];
                                        cs.Location[4] = cs.Location[2];
                                    }
                                    else
                                    {
                                        cs.Location[2] = (chainages[idx] - chainages[idx + 1]).ToString();
                                        cs.Location[3] = cs.Location[2];
                                        cs.Location[4] = cs.Location[2];
                                    }
                                    var xmin = (from r in cs.Data select r.X).Min();
                                    var xminright = (from r in cs.Data where r.X > xmin select r.X).FirstOrDefault();
                                    var xmax = (from r in cs.Data select r.X).Max();
                                    var xmaxleft = (from r in cs.Data where r.X < xmax select r.X).LastOrDefault();
                                    cs.Manning = string.Format("{0,8}{1,8}{2,8}{3,8}{4,8}{5,8}{6,8}{7,8}",
                                        xmin, 0.0275, 0, xminright, 0.0225, 0, xmaxleft, 0.0275, 0);
                                    cs.ManningSta = string.Format("Bank Sta={0},{1}", xminright, xmaxleft);
                                    cs.LastEditedTime = DateTime.Now.ToShortDateString();
                                    cs.XS_HTab_Starting = string.Format("XS HTab Starting El and Incr={0},{1},{2}", 175.5, 0.04, 20);
                                    csc.Add(cs);

                                    idx++;
                                }

                                rr.CSCollection = csc;
                            }
                        }
                        break;
                    case 1:
                        {
                            foreach (var rr in hec1.RiverReachCollection)
                            {
                                var files = Directory.GetFiles(Path.Combine(hdmfodler, rr.ReachName), "*.hdm", SearchOption.AllDirectories);

                                //get crosssection
                                var csc = new List<HECCrossSection>();
                                var chainages = new List<float>();
                                HECCrossSection cs = new HECCrossSection();
                                int idx = 0;

                                foreach (var file in files)
                                {
                                    var chainage = 0f;

                                    var cspts = new List<PointD>();
                                    //read each hdm file
                                    var sr = new StreamReader(file, Encoding.Default);
                                    string line = sr.ReadLine();

                                    var tmp = line.Trim().Split(',');
                                    if (tmp.Count() == 1) tmp = line.Trim().Split('，');
                                    chainage = float.Parse(tmp[1].Split(':')[0].Trim()) * -1; //根据情况自定义
                                    chainages.Add(chainage);

                                    while (!sr.EndOfStream)
                                    {
                                        line = sr.ReadLine();
                                        var pt = line.Trim().Split(',');
                                        if (pt.Count() == 1) pt = line.Trim().Split('，');

                                        if (pt[0] == "BEGIN")
                                        {
                                            chainage = float.Parse(pt[1].Split(':')[0].Trim()) * -1;//根据情况自定义
                                            chainages.Add(chainage);

                                            cs.Data = cspts;

                                            var xmin = (from r in cs.Data select r.X).Min();
                                            var xminright = (from r in cs.Data where r.X > xmin select r.X).FirstOrDefault();
                                            var xmax = (from r in cs.Data select r.X).Max();
                                            var xmaxleft = (from r in cs.Data where r.X < xmax select r.X).LastOrDefault();
                                            cs.Manning = string.Format("{0,8}{1,8}{2,8}{3,8}{4,8}{5,8}{6,8}{7,8}",
                                                xmin, 0.0275, 0, xminright, 0.0225, 0, xmaxleft, 0.0275, 0);
                                            cs.ManningSta = string.Format("Bank Sta={0},{1}", xminright, xmaxleft);
                                            cs.LastEditedTime = DateTime.Now.ToShortDateString();
                                            cs.XS_HTab_Starting = string.Format("XS HTab Starting El and Incr={0},{1},{2}", 175.5, 0.04, 20);
                                            csc.Add(cs);

                                            cs = new HECCrossSection();
                                            cspts = new List<PointD>();
                                        }
                                        else
                                            cspts.Add(new PointD() { X = double.Parse(pt[0].Trim()), Y = double.Parse(pt[1].Trim()) });
                                    }
                                    sr.Close();
                                }

                                for (int i = 0; i < csc.Count; i++)
                                {
                                    csc[i].Location = new string[5];
                                    csc[i].Location[0] = "1";
                                    csc[i].Location[1] = chainages[i].ToString();

                                    if (i == csc.Count - 1)
                                    {
                                        csc[i].Location[2] = "0";
                                        csc[i].Location[3] = csc[i].Location[2];
                                        csc[i].Location[4] = csc[i].Location[2];
                                    }
                                    else
                                    {
                                        csc[i].Location[2] = (chainages[i] - chainages[i + 1]).ToString();
                                        csc[i].Location[3] = csc[i].Location[2];
                                        csc[i].Location[4] = csc[i].Location[2];
                                    }
                                }

                                rr.CSCollection = csc;
                            }
                        }
                        break;
                }

                hec1.Version = "5.05";
                WriteGeoFile(hec1, geometryFile);
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
            }
        }

        public string Geo2Json(string geofile)
        {
            try
            {
                var hecdm = GetGeometry(geofile);
                var shp = Path.Combine(Path.GetDirectoryName(geofile),
                    Path.GetFileNameWithoutExtension(geofile) + "-shp.shp");
                Utility.Utility.CreatePolylineShp(hecdm, shp);
                Utility.Utility.ConvertShp2JsonFileEx3(shp);
                return "";
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                return null;
            }
        }

        public string Geo2Json(HEC_DM hecdm, string jsonfile)
        {
            try
            {
                var shp = Path.Combine(Path.GetDirectoryName(jsonfile),
                    Path.GetFileNameWithoutExtension(jsonfile) + "-shp.shp");
                Utility.Utility.CreatePolylineShp(hecdm, shp);

                var json = Utility.Utility.ConvertShp2JsonFileEx4(shp);
                File.WriteAllText(jsonfile, json);
                return json;
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                return null;
            }
        }

        public string Geo2Json(List<River> riverdm, string jsonfile)
        {
            try
            {
                var shp = Path.Combine(Path.GetDirectoryName(jsonfile),
                    Path.GetFileNameWithoutExtension(jsonfile) + "-shp.shp");
                Utility.Utility.CreateRiverNetPolylineShp(riverdm, shp);

                Utility.Utility.ConvertShp2GeoJson(shp, jsonfile);

                //var json = Utility.Utility.ConvertShp2JsonFileEx4(shp);
                //File.WriteAllText(jsonfile, json);
                return File.ReadAllText(jsonfile).Replace("\r\n", "").Replace("\n", "").Trim();
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                CommonUtility.Log("Geo2Json:" + ex.Message);
                return null;
            }
        }

        public string Crossection2Json(List<River> riverdm, string jsonfile)
        {
            try
            {
                var shp = Path.Combine(Path.GetDirectoryName(jsonfile),
                    Path.GetFileNameWithoutExtension(jsonfile) + "-shp.shp");
                Utility.Utility.CreateRiverXSPolylineShp(riverdm, shp);

                //var json = Utility.Utility.ConvertShp2JsonFileEx4(shp);
                //File.WriteAllText(jsonfile, json);

                Utility.Utility.ConvertShp2GeoJson(shp, jsonfile);

                return File.ReadAllText(jsonfile).Replace("\r\n", "").Replace("\n", "").Trim();
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                CommonUtility.Log("Crossection2Json:" + ex.Message);
                return null;
            }
        }

        public List<River> GetRiverInfo(HEC_DM hecdm)
        {
            try
            {
                var res = new List<River>();
                foreach (var rv in hecdm.RiverReachCollection)
                {
                    var rvr = new River();
                    rvr.RvrName = rv.RiverName;
                    rvr.RchName = rv.ReachName;
                    rvr.Points = rv.Points;
                    rvr.RvrMdCode = rv.RiverName + "&" + rv.ReachName;
                    rvr.StChainage = float.Parse(rv.CSCollection.Last().Location[1].Replace("*", ""));
                    rvr.EdChainage = float.Parse(rv.CSCollection.Last().Location[0].Replace("*", ""));
                    rvr.XSectoin = new List<Crosssection>();
                    //断面处理
                    var origin = float.Parse(rv.CSCollection.Last().Location[1].Replace("*", ""));
                    foreach (var cs in rv.CSCollection)
                    {
                        //计算断面测量点空间坐标
                        var pts = cs.Data;//断面测量点
                        var branchPts = rv.Points;//河段点坐标集合
                        var distance = float.Parse(cs.Location[1].Replace("*", "")) - origin;//距离
                        //将断面测量点分左右
                        var ld = new List<float>();
                        var rd = new List<float>();
                        var lpts = new List<PointD>();
                        var rpts = new List<PointD>();
                        //最低点
                        var zmin = (from r in pts select r.Y).Min();
                        var zminPts = (from r in pts where r.Y == zmin select r);
                        var cidx = -1;
                        PointD cpt;
                        if (zminPts.Count() == 1)
                        {
                            cidx = pts.IndexOf(zminPts.First());
                            cpt = pts[cidx];
                        }
                        else
                        {
                            if (zminPts.Count() % 2 == 0)
                            {
                                //增加一个点
                                var insetIdx = pts.IndexOf(zminPts.First()) + zminPts.Count() / 2;
                                cpt = new PointD(zminPts.First().X + (zminPts.Last().X - zminPts.First().X) / 2, zmin);
                                pts.Insert(insetIdx - 1, cpt);
                                cidx = insetIdx;
                            }
                            else
                            {
                                cidx = pts.IndexOf(zminPts.First()) + (int)Math.Floor(zminPts.Count() / 2.0);
                                cpt = pts[cidx];
                            }
                        }
                        ld = pts.Take(cidx).Select(x => (float)x.X).ToList<float>();
                        var ldz = pts.Take(cidx).Select(x => (float)x.Y).ToList<float>();
                        rd = pts.Skip(cidx + 1).Select(x => (float)x.X).ToList<float>();
                        var rdz = pts.Skip(cidx + 1).Select(x => (float)x.Y).ToList<float>();
                        var cptz = cpt.Y;
                        //计算坐标
                        Utility.Utility.GetSectionCoords(branchPts, distance, ld, rd, ref lpts, ref rpts, ref cpt.X, ref cpt.Y);
                        for (int i = 0; i < ld.Count; i++)
                            lpts[i] = new PointD(lpts[i].X, lpts[i].Y, ldz[i]);
                        for (int i = 0; i < rd.Count; i++)
                            rpts[i] = new PointD(rpts[i].X, rpts[i].Y, rdz[i]);
                        cpt = new PointD(cpt.X, cpt.Y, cptz);

                        cs.Points = lpts;
                        if (cidx > 0)
                            cs.Points.Add(cpt);
                        cs.Points.AddRange(rpts);
                        rvr.XSectoin.Add(cs);
                    }
                    res.Add(rvr);
                }
                foreach (var rv in res)
                {
                    //找上游河段
                    var qup = (from r in hecdm.JunctCollection
                               from x in r.DownRiverReach
                               where rv.RvrName == x[0].Trim() && rv.RchName == x[1].Trim()
                               select r);
                    if (qup.Count() > 0) rv.UpRvr = new List<string>();
                    //foreach (var q in qup)
                    //{
                    ////找连接桩号
                    //var qdir = (from r in hecdm.JunctCollection
                    //            from x in r.DownRiverReach
                    //            where q[0].Trim() == x[0].Trim() && q[1].Trim() == x[1].Trim()
                    //            select x).ToList<string[]>();
                    //if (qdir.Count > 0)
                    //{
                    //    var qc = from r in qdir
                    //             where rv.RvrName == r[0].Trim() && rv.RchName == r[1].Trim()
                    //             select r;
                    //    if (qc.Count() > 0)//下游相连
                    //    {

                    //    }
                    //}
                    foreach (var qd in qup)
                    {
                        foreach (var q in qd.UpRiverReach)
                        {
                            rv.UpRvr.Add((from r in res where r.RvrName == q[0].Trim() && r.RchName == q[1].Trim() select r.RvrMdCode).FirstOrDefault());
                        }
                    }
                    //rv.UpRvr.Add((from r in res where r.RvrName == q[0].Trim() && r.RchName == q[1].Trim() select r).FirstOrDefault());
                    //}
                    //找下游河段
                    var qdwn = (from r in hecdm.JunctCollection
                                from x in r.UpRiverReach
                                where rv.RvrName == x[0].Trim() && rv.RchName == x[1].Trim()
                                select r);
                    if (qdwn.Count() > 0) rv.DnRvr = new List<string>();
                    foreach (var qd in qdwn)
                    {
                        foreach (var q in qd.DownRiverReach)
                        {
                            rv.DnRvr.Add((from r in res where r.RvrName == q[0].Trim() && r.RchName == q[1].Trim() select r.RvrMdCode).FirstOrDefault());
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                CommonUtility.Log("GetRiverInfo:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 将边界条件数据写入边界文件（类似u01或者u02文件）,注意只写入Boundary Location数据，其他数据从源文件保留 2022-5-28
        /// </summary>
        /// <param name="boundaryList"></param>
        public void SetBoundary(List<Boundary> boundaryList)
        {
            /// boundaryList 写到 BoundaryFile 文件里。
            /// 

            if (this.UnsteadyBoundaryFile == null || this.UnsteadyBoundaryFile.Equals(""))
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = "BoundaryFile文件路径无效" });
                }
            }

            if (this.BoundaryList != boundaryList)
            {
                this.BoundaryList = boundaryList;
            }

            try
            {
                //step1
                string tBoundaryFilepath = ProjectDir + @"\" + UnsteadyBoundaryFile;
                string[] lines = System.IO.File.ReadAllLines(tBoundaryFilepath);
                List<string> newFileLines = new List<string>();
                int stepTwoLineOffset = -1;
                for (int iline = 0; iline < lines.Length; ++iline)
                {
                    if (lines[iline].Contains("Boundary Location="))
                    {
                        //第一次出现 Boundary Location= 跳出循环，进入下一步写入 BoundaryList数据
                        stepTwoLineOffset = iline;
                        break;
                    }
                    else
                    {
                        newFileLines.Add(lines[iline]);
                    }
                }

                if (stepTwoLineOffset < 0)
                {
                    throw new Exception("没有找到Boundary Location");
                }

                //step2 
                for (int iboundary = 0; iboundary < boundaryList.Count; ++iboundary)
                {
                    string[] boundaryLines1 = writeBoundaryToLines(boundaryList[iboundary]);
                    for (int il = 0; il < boundaryLines1.Length; ++il)
                    {
                        newFileLines.Add(boundaryLines1[il]);
                    }
                }

                //step3 写入老文件
                System.IO.File.WriteAllLines(tBoundaryFilepath, newFileLines);
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
            }
        }

        public void SetWqBoundary(List<Boundary> boundaryList)
        {
            try
            {
                //step1
                string tBoundaryFilepath = ProjectDir + @"\" + WqFile;
                string[] lines = System.IO.File.ReadAllLines(tBoundaryFilepath);
                List<string> newFileLines = new List<string>();
                int stepTwoLineOffset = -1;
                for (int iline = 0; iline < lines.Length; ++iline)
                {
                    if (lines[iline].Contains("Constituent="))
                    {
                        //第一次出现 Boundary Location= 跳出循环，进入下一步写入 BoundaryList数据
                        stepTwoLineOffset = iline;
                        break;
                    }
                    else
                    {
                        newFileLines.Add(lines[iline]);
                    }
                }
                //step2
                var wqBdlist = boundaryList.Where(x => x.Pollutants != null).Select(x => x).ToList();
                for (int icomponent = 0; icomponent < wqBdlist[0].Pollutants.Count; ++icomponent)
                {
                    string[] boundaryLines1 = writeWqBoundaryToLines(wqBdlist[0].Pollutants[icomponent],
                        wqBdlist);
                    for (int il = 0; il < boundaryLines1.Length; ++il)
                    {
                        newFileLines.Add(boundaryLines1[il]);
                    }
                }
                //step3
                var lastlines = lines.Skip(newFileLines.Count);
                newFileLines.AddRange(lastlines);
                System.IO.File.WriteAllLines(tBoundaryFilepath, newFileLines);
            }
            catch(Exception ex)
            {
                CommonUtility.Log(ex.Message);
            }
        }

        public void SetSimulationTime(ModelTime modeltime)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(currentPlanFilepath);
                foreach (string line1 in lines)
                {
                    if (line1.Contains("Simulation Date="))
                    {
                        var st1 = ConvertYMD2DateStr(modeltime.StartTime.Year, modeltime.StartTime.Month, modeltime.StartTime.Day);
                        var st2 = ConvertYMD2DateStr(modeltime.EndTime.Year, modeltime.EndTime.Month, modeltime.EndTime.Day);
                        var ls = "Simulation Date=" + st1 + "," +
                            modeltime.StartTime.Hour + ":" + modeltime.StartTime.Minute + ":" + modeltime.StartTime.Second + "," +
                            st2 + "," +
                            modeltime.EndTime.Hour + ":" + modeltime.EndTime.Minute + ":" + modeltime.EndTime.Second;
                        lines[lines.ToList().IndexOf(line1)] = ls;
                    }
                }
                File.WriteAllLines(currentPlanFilepath, lines);
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
            }
        }

        public List<RiverSegModelResults> GetResults(DateTime startTime, DateTime endTime,
            string Projection, int centralLgtd)
        {
            try
            {
                List<RiverSegModelResults> res = new List<RiverSegModelResults>();

                using (var dssr = new DssReader(Path.Combine(ProjectDir, DSSFile),
                    DssReader.MethodID.MESS_METHOD_GENERAL_ID, DssReader.LevelID.MESS_LEVEL_CRITICAL))
                {
                    var rivers = GetRiverInfo(ModelTopo);
                    foreach (var river in rivers)
                    {
                        var listWl = new List<double[]>();
                        var listQ = new List<double[]>();
                        var stations = new List<double>();

                        //直接在这里根据时间、结果保存步长、特征值标识写全path字符串
                        //按时间遍历取值，赋给RiverSegStatisctResults对象
                        int i = 0;
                        var currTime = startTime;
                        while (currTime < endTime)
                        {
                            currTime = currTime.AddSeconds(OutputTimeInterval);
                            var stime = ConvertYMD2DateStr(currTime.Year, currTime.Month, currTime.Day);
                            var wl_path = "/" + river.RvrName.ToUpper() + " " + river.RchName.ToUpper() + "//LOCATION-ELEV//" +
                                stime + " 0000/SINGLERIVER/";
                            var q_path = "/" + river.RvrName.ToUpper() + " " + river.RchName.ToUpper() + "//LOCATION-FLOW//" +
                                stime + " 0000/SINGLERIVER/";

                            DssPathCollection paths = dssr.GetCatalog();
                            //var elevpath = new List<DssPath>();
                            for (int j = 0; j < paths.Count; j++)
                            {
                                if (paths[j].FullPath.Contains(wl_path))
                                {
                                    //elevpath.Add(paths[j]);
                                    var pd = dssr.GetPairedData(paths[j].FullPath);
                                    if (i == 0) stations = pd.Ordinates.ToList(); //桩号
                                    listWl.Add(pd.Values[0]); //水位
                                }
                                else if (paths[j].FullPath.Contains(q_path))
                                {
                                    var pd = dssr.GetPairedData(paths[j].FullPath);
                                    //var stations = pd.Ordinates; //桩号
                                    listQ.Add(pd.Values[0]); //流量
                                }
                            }

                            i++;
                        }

                        //根据桩号（也即距离，计算河段的坐标）
                        var riverpts = river.Points;

                        riverpts.Reverse();
                        stations.Sort();
                        var uitl = new Utility.Utility();
                        for (int m = 0; m < stations.Count; m++)
                        {
                            List<PointD> segpts = new List<PointD>();
                            var endDistIdx = -1;

                            uitl.GetSegmentPoints(riverpts,
                                (float)(m == 0 ?
                                (stations[m] - stations[0]) :
                                stations[m] - stations[0] - (stations[m] - stations[m - 1]) / 2),
                                (float)(m + 1 == stations.Count ?
                                stations[m] - stations[0] :
                                stations[m] - stations[0] + (stations[m + 1] - stations[m]) / 2), out segpts, out endDistIdx);

                            //如果最后一个桩号不在reach的端点上，将点补全
                            if (m + 1 == stations.Count && endDistIdx < riverpts.Count - 1)
                            {
                                segpts.AddRange(riverpts.Skip(endDistIdx + 1).Take(riverpts.Count - 1 - endDistIdx));
                            }

                            //转换为cgcs2000坐标系
                            //根据投影坐标系及中央经度找到坐标编码
                            var crs_in = int.Parse(Utility.Utility.GetPrjSysWKID(centralLgtd, Projection));
                            var crs_out = 4490;
                            var pts = segpts;
                            segpts = Utility.Utility.CoordTransformPoints(pts, crs_in, crs_out);

                            var seg = new RiverSegModelResults();
                            seg.TimeInterval = (int)OutputTimeInterval / 3600;
                            seg.RvrMdCode = river.RvrMdCode;
                            seg.Chainage = (float)stations[m];
                            seg.WaterLevel = listWl.Select(x => x[m]).Select(y => (float)y).ToArray<float>();
                            seg.Discharge = listQ.Select(x => x[m]).Select(y => (float)y).ToArray<float>();

                            seg.MaxDischarge = seg.Discharge.Max();
                            seg.MinDischarge = seg.Discharge.Min();
                            seg.AvgDischarge = seg.Discharge.Average();

                            seg.MaxWaterLevel = seg.WaterLevel.Max();
                            seg.MinWaterLevel = seg.WaterLevel.Min();
                            seg.AvgWaterLevel = seg.WaterLevel.Average();

                            segpts.ForEach(x => seg.LineGeoString += (x.X.ToString() + " " + x.Y.ToString() + ","));
                            seg.LineGeoString = "LINESTRING(" + seg.LineGeoString.TrimEnd(',') + ")";

                            res.Add(seg);
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                CommonUtility.Log("GetResults:" + ex.Message);
                return null;
            }
        }

        public void GenerateResultsStatisticShp(List<RiverSegModelResults> resDM, string shpfile)
        {
            try
            {
                Utility.Utility.CreateRiverStatisticResultsShp(resDM, shpfile);
            }
            catch (Exception ex)
            {
                CommonUtility.Log("GenerateResultsStatisticShp:" + ex.Message);
            }
        }
        #endregion

        #region 属性

        /// <summary>
        /// 发送信息数据
        /// </summary>
        public event OutputMsgHandler OutputMsg;
        public List<Boundary> BoundaryList { get; set; }
        public List<InitialCondition> IntConditionList { get; set; }
        public List<WQParameter> WqParamList { get; set; }
        public string ProjectDir { get; set; }
        public string Geofile { get; set; }
        public string ProjTitle { get; set; }
        public string PlanFile { get; set; }

        //wf
        public string UnsteadyBoundaryFile { get; set; }
        public string SteadyBoundaryFile { get; set; }
        public string DSSFile { get; set; }
        public string WqFile { get; set; }

        public DateTime SimulationStartTime { get; set; }
        public DateTime SimulationEndTime { get; set; }
        /// <summary>
        /// 单位：秒；计算步长
        /// </summary>
        public float ComputationTimeInterval { get; set; }
        /// <summary>
        /// 单位：秒；dss结果输出时间步长
        /// </summary>
        public float OutputTimeInterval { get; set; }
        /// <summary>
        /// 结果输出时间步数
        /// </summary>
        public int OutputTimeStepNo { get; set; }

        public HEC_DM ModelTopo { get; set; }

        #endregion

        #region 私有方法
        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        /// <summary>
        /// 转换01APR2022日期字符串为年，月，日 by wf
        /// </summary>
        /// <param name="str"></param>
        /// <param name="year"></param>
        /// <param name="mon"></param>
        /// <param name="day"></param>
        private void ConvertDateStr2YMD(string str, out int year, out int mon, out int day)
        {//01APR2022 -> 2022, 4 ,1 
            year = 0;
            mon = 0;
            day = 0;
            if (str.Length != 9)
            {
                throw new Exception("无效的日期字符串");
            }
            day = int.Parse(str.Substring(0, 2));
            string tmon = str.Substring(2, 3);
            string[] monstr = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            for (int im = 0; im < monstr.Length; ++im)
            {
                if (monstr[im].Equals(tmon))
                {
                    mon = im + 1;
                    break;
                }
            }
            if (mon == 0)
            {
                throw new Exception("无效的月份缩写'" + tmon + "'");
            }
            year = int.Parse(str.Substring(5, 4));
        }
        private string ConvertYMD2DateStr(int year, int mon, int day)
        {//01APR2022 <-> 2022, 4 ,1 

            var sday = day.ToString().PadLeft(2, '0');
            string[] monstr = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            var sm = monstr[mon - 1];
            var syear = year.ToString();
            return sday + sm + syear;
        }

        private Boundary makeUnsteadyBoundaryByLines(string[] lines, int startOffset, DateTime startDt)
        {
            try
            {
                Boundary boundary = new Boundary();

                var ifline = lines[startOffset + 1];
                if (ifline.StartsWith("Interval="))
                {
                    string intervalStr = lines[startOffset + 1].Replace("Interval=", "");
                    TimeSpan timeSpan = new TimeSpan();
                    {
                        int n = 0;
                        string unit = "";
                        if (intervalStr.StartsWith("Gate Name=") || intervalStr.StartsWith("Elev Controlled Gate=")) return null;
                        if (Char.IsDigit(intervalStr[1]))
                        {
                            n = int.Parse(intervalStr.Substring(0, 2));
                            unit = intervalStr.Substring(2);
                        }
                        else
                        {
                            n = int.Parse(intervalStr.Substring(0, 1));
                            unit = intervalStr.Substring(1);
                        }
                        if (unit.Contains("SEC"))
                        {
                            timeSpan = new TimeSpan(0, 0, 0, n);
                        }
                        else if (unit.Contains("MIN"))
                        {
                            timeSpan = new TimeSpan(0, 0, n, 0);
                        }
                        else if (unit.Contains("HOUR"))
                        {
                            timeSpan = new TimeSpan(0, n, 0, 0);
                        }
                        else if (unit.Contains("DAY"))
                        {
                            timeSpan = new TimeSpan(n, 0, 0, 0);
                        }
                        else if (unit.Contains("WEE"))
                        {
                            timeSpan = new TimeSpan(7, 0, 0, 0);
                        }
                        else if (unit.Contains("MON"))
                        {
                            timeSpan = new TimeSpan(30, 0, 0, 0);
                        }
                        else if (unit.Contains("YEA"))
                        {
                            timeSpan = new TimeSpan(365, 0, 0, 0);
                        }
                        else
                        {
                            throw new Exception("无效的时间间隔字符串'" + intervalStr + "'");
                        }
                    }
                    boundary.IntervalStr = intervalStr;

                    int numVal = 0;
                    string line3 = lines[startOffset + 2];
                    if (line3.Contains("Flow Hydrograph="))
                    {
                        numVal = int.Parse(line3.Replace("Flow Hydrograph=", ""));
                        boundary.HDType = enumHDBoundaryType.流量;
                    }
                    else if (line3.Contains("Stage Hydrograph="))
                    {
                        numVal = int.Parse(line3.Replace("Stage Hydrograph=", ""));
                        boundary.HDType = enumHDBoundaryType.水位;
                    }
                    else if (line3.Contains("Uniform Lateral Inflow Hydrograph="))
                    {
                        numVal = int.Parse(line3.Replace("Uniform Lateral Inflow Hydrograph=", ""));
                        boundary.HDType = enumHDBoundaryType.侧向流量;
                        boundary.BndDescription = enumBoundaryDescription.DistributedSource;
                    }
                    else if (line3.Contains("Lateral Inflow Hydrograph="))
                    {
                        numVal = int.Parse(line3.Replace("Lateral Inflow Hydrograph=", ""));
                        boundary.HDType = enumHDBoundaryType.侧向流量;
                        boundary.BndDescription = enumBoundaryDescription.PointSource;
                    }
                    else
                    {
                        //throw new Exception("不支持的边界类型'" + line3 + "'");
                        return null;
                    }

                    string line1 = lines[startOffset].Replace("Boundary Location=", "");
                    {
                        string[] parts = line1.Split(',');
                        if (parts.Length < 3)
                        {
                            throw new Exception("无效的Boundary Location");
                        }
                        boundary.Location3.riverName = parts[0].Trim();
                        boundary.Location3.reachName = parts[1].Trim();
                        boundary.Location3.station = parts[2].Trim();
                        if (boundary.HDType == enumHDBoundaryType.侧向流量)
                            boundary.Location3.station2 = parts[3].Trim();
                    }

                    DateTime currDt = startDt;
                    int numLinesOfValue = (int)Math.Ceiling(numVal / 10.0);
                    for (int iline = startOffset + 3; iline < startOffset + 3 + numLinesOfValue; ++iline)
                    {
                        int numValInLine = lines[iline].Length / 8;
                        for (int ival = 0; ival < numValInLine; ++ival)
                        {
                            TSData tData = new TSData();
                            tData.DT = currDt;
                            tData.Data = float.Parse(lines[iline].Substring(ival * 8, 8));
                            boundary.Value.Add(tData);
                            currDt += timeSpan;
                        }
                    }
                }

                if(ifline.StartsWith("Gate Name="))
                {
                    boundary.HDType = enumHDBoundaryType.时间控制闸门;
                    boundary.StuctureBnd = new List<string>();
                    boundary.StuctureBnd.Add(lines[startOffset]);
                    boundary.StuctureBnd.Add(lines[startOffset+1]);
                    boundary.StuctureBnd.Add(lines[startOffset+2]);
                    boundary.StuctureBnd.Add(lines[startOffset+3]);
                    boundary.StuctureBnd.Add(lines[startOffset+4]);
                    boundary.StuctureBnd.Add(lines[startOffset+5]);
                    boundary.StuctureBnd.Add(lines[startOffset+6]);
                    boundary.StuctureBnd.Add(lines[startOffset+7]);
                    boundary.StuctureBnd.Add(lines[startOffset+8]);
                }

                if (ifline.StartsWith("Elev Controlled Gate="))
                {
                    boundary.HDType = enumHDBoundaryType.水位控制闸门;
                    boundary.StuctureBnd = new List<string>();
                    boundary.StuctureBnd.Add(lines[startOffset]);
                    boundary.StuctureBnd.Add(lines[startOffset + 1]);
                    boundary.StuctureBnd.Add(lines[startOffset + 2]);
                    boundary.StuctureBnd.Add(lines[startOffset + 3]);
                }
                return boundary;
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
                return null;
            }
        }

        private void makeWqBoundaryByLines(string[] lines, ref int startOffset,
            DateTime startDt, DateTime endDt,
            ref List<Boundary> boundaryList)
        {
            try
            {
                //水质组分名称
                var componentName = lines[startOffset].Replace("Constituent=", "").Trim();
                //遍历该水质组分所有边界
                while (lines[++startOffset].Contains("BC="))
                    makeWqBcSection(lines, ref startOffset, startDt, endDt, componentName, ref boundaryList);
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
            }
        }

        private void makeWqBcSection(string[] lines, ref int startOffset,
            DateTime startDt, DateTime endDt, string componentName, ref List<Boundary> boundaryList)
        {
            try
            {
                var location = lines[startOffset].Replace("BC=", "").Trim().Split(',');
                var bd = (from r in boundaryList
                          where r !=null
                          && r.Location3.riverName != null
                          && r.Location3.riverName.Equals(location[0])
                          && r.Location3.reachName.Equals(location[1])
                          && r.Location3.station.Equals(location[2])
                          select r).FirstOrDefault();
                if (bd != null)
                {
                    if (bd.Pollutants == null)
                    {
                        bd.Pollutants = new List<string>();
                        bd.Pollutants.Add(componentName);
                        bd.Concetration = new List<List<TSData>>();
                        bd.ObsConcetration = new List<List<TSData>>();
                        bd.Concetration.Add(new List<TSData>());
                        bd.ObsConcetration.Add(new List<TSData>());
                    }
                    else
                    {
                        if (!bd.Pollutants.Contains(componentName))
                        { bd.Pollutants.Add(componentName);
                            bd.Concetration.Add(new List<TSData>());
                            bd.ObsConcetration.Add(new List<TSData>());
                        }
                    }
                    var pindex = bd.Pollutants.IndexOf(componentName);
                    var timeMode = int.Parse(lines[startOffset + 2].Replace("Time Series Mode=", "").Trim());
                    switch (timeMode)
                    {
                        case 1:
                            var obsConcetration = float.Parse(lines[startOffset + 3].Replace("Observed Data=", "").Trim());
                            //if (bd.ObsConcetration == null)
                            //{
                            //    bd.ObsConcetration = new List<List<TSData>>();
                            //    bd.ObsConcetration.Add(new List<TSData>()
                            //    {
                            //        new TSData(){ DT=startDt,Data=obsConcetration },
                            //        new TSData(){ DT=endDt,Data=obsConcetration },
                            //    });
                            //}
                            //else
                            //{
                                bd.ObsConcetration[pindex].AddRange(new List<TSData>()
                                {
                                    new TSData(){ DT=startDt,Data=obsConcetration },
                                    new TSData(){ DT=endDt,Data=obsConcetration },
                                });
                            //}
                            startOffset = startOffset + 4;
                            break;
                        case 4:
                            obsConcetration = float.Parse(lines[startOffset + 3].Replace("Observed Data=", "").Trim());
                            var concetration = float.Parse(lines[startOffset + 4].Replace("Constant Value=", "").Trim());
                            //if (bd.ObsConcetration == null)
                            //{
                            //    bd.ObsConcetration = new List<List<TSData>>();
                            //    bd.ObsConcetration.Add(new List<TSData>()
                            //    {
                            //        new TSData(){ DT=startDt,Data=obsConcetration },
                            //        new TSData(){ DT=endDt,Data=obsConcetration },
                            //    });
                            //}
                            //else
                            //{
                                bd.ObsConcetration[pindex].AddRange(new List<TSData>()
                                {
                                    new TSData(){ DT=startDt,Data=obsConcetration },
                                    new TSData(){ DT=endDt,Data=obsConcetration },
                                });
                            //}
                            //if (bd.Concetration == null)
                            //{
                            //    bd.Concetration = new List<List<TSData>>();
                            //    bd.Concetration.Add(new List<TSData>()
                            //    {
                            //        new TSData(){ DT=startDt,Data=concetration },
                            //        new TSData(){ DT=endDt,Data=concetration },
                            //    });
                            //}
                            //else
                            //{
                                bd.Concetration[pindex].AddRange(new List<TSData>()
                                {
                                    new TSData(){ DT=startDt,Data=concetration },
                                    new TSData(){ DT=endDt,Data=concetration },
                                });
                            //}
                            startOffset = startOffset + 6;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
            }
        }

        private void makeWqIcSection(string[] lines, ref int startOffset,
            DateTime startDt, DateTime endDt, ref List<InitialCondition> intConditionList)
        {
            try
            {
                //水质组分名称
                var componentName = lines[startOffset].Replace("Constituent=", "").Trim();
                while (lines[++startOffset].Contains("IC="))
                {
                    var location = lines[startOffset].Replace("IC=", "").Trim().Split(',');
                    var bd = (from r in intConditionList
                              where r.Location3.riverName != null
                              && r.Location3.riverName.Equals(location[0])
                              && r.Location3.reachName.Equals(location[1])
                              && r.Location3.station.Equals(location[2])
                              select r).FirstOrDefault();
                    if (bd != null)
                    {
                        if (bd.Pollutants == null)
                        {
                            bd.Pollutants = new List<string>();
                            bd.Pollutants.Add(componentName);
                        }
                        else
                        {
                            if (!bd.Pollutants.Contains(componentName))
                            {
                                bd.Pollutants.Add(componentName);
                                bd.Concetration.Add(0f);
                            }
                        }

                        var pindex = bd.Pollutants.IndexOf(componentName);

                        var intConcetration = float.Parse(lines[startOffset + 1].Replace("IC Value=", "").Trim());
                        if (bd.Concetration == null)
                        {
                            bd.Concetration = new List<float>();
                            bd.Concetration.Add(intConcetration);
                        }
                        else
                        {
                            bd.Concetration[pindex] = intConcetration;
                        }
                    }
                    else
                    {
                        location = lines[startOffset].Replace("IC=", "").Trim().Split(',');
                        var newbd = new InitialCondition();
                        newbd.Location3 = new RiverStation();
                        newbd.Location3.riverName = location[0];
                        newbd.Location3.reachName = location[1];
                        newbd.Location3.station = location[2];
                        newbd.Pollutants = new List<string>();
                        newbd.Pollutants.Add(componentName);
                        newbd.Concetration = new List<float>() { float.Parse(lines[startOffset + 1].Replace("IC Value=", "").Trim()) };

                        intConditionList.Add(newbd);
                    }
                    startOffset = startOffset + 1;
                }
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
            }
        }

        private void makeWqDcSection(string[] lines, ref int startOffset,
            string componentName, ref List<WQParameter> wqParamList)
        {
            try
            {
                var location = lines[startOffset].Replace("Dispersion=", "").Trim().Split(',');
                var bd = (from r in wqParamList
                          where r.Location3.riverName != null
                          && r.Location3.riverName.Equals(location[0])
                          && r.Location3.reachName.Equals(location[1])
                          && r.Location3.station.Equals(location[2])
                          select r).FirstOrDefault();
                if (bd != null)
                {
                    var dc = float.Parse(lines[startOffset + 1].Replace("Dispersion Value=", "").Trim());
                    bd.Dispersion = new List<float>() { dc };
                }
                else
                {
                    location = lines[startOffset].Replace("IC=", "").Trim().Split(',');
                    var newbd = new WQParameter();
                    newbd.Location3 = new RiverStation();
                    newbd.Location3.riverName = location[0];
                    newbd.Location3.reachName = location[1];
                    newbd.Location3.station = location[2];
                    newbd.Pollutants = new List<string>();
                    newbd.Dispersion = new List<float>() { float.Parse(lines[startOffset + 1].Replace("Dispersion Value=", "").Trim()) };

                    wqParamList.Add(newbd);
                }

                startOffset = startOffset + 2;
            }
            catch (Exception ex)
            {
                CommonUtility.Log(ex.Message);
            }
        }

        /// <summary>
        /// 小于16字符填补到16个字符，反之完整输出
        /// </summary>
        /// <param name="tstr"></param>
        /// <returns></returns>
        private string padding16(string tstr)
        {
            if (tstr.Length < 16)
            {
                string newStr = tstr;
                for (int i = tstr.Length; i < 16; ++i)
                {
                    newStr += " ";
                }
                return newStr;
            }
            else
            {
                return tstr;
            }
        }

        private string padding8(string tstr)
        {
            if (tstr.Length < 8)
            {
                string newStr = tstr;
                for (int i = tstr.Length; i < 8; ++i)
                {
                    newStr += " ";
                }
                return newStr;
            }
            else
            {
                return tstr;
            }
        }

        private string padding32(string tstr)
        {
            if (tstr.Length < 32)
            {
                string newStr = tstr;
                for (int i = tstr.Length; i < 32; ++i)
                {
                    newStr += " ";
                }
                return newStr;
            }
            else
            {
                return tstr;
            }
        }

        private string paddingLeft8(float val)
        {
            return string.Format("{0,8}", val);
        }

        private string[] writeBoundaryToLines(Boundary theBoundary)
        {
            List<string> tNewLines = new List<string>();

            if (theBoundary.HDType == enumHDBoundaryType.时间控制闸门
                || theBoundary.HDType == enumHDBoundaryType.水位控制闸门)
            {
                tNewLines = theBoundary.StuctureBnd;
                return tNewLines.ToArray(); 
            }
            tNewLines.Add("Boundary Location="
                + padding16(theBoundary.Location3.riverName) + ","
                + padding16(theBoundary.Location3.reachName) + ","
                + padding8(theBoundary.HDType == enumHDBoundaryType.侧向流量 ?
                theBoundary.Location3.station + "," + theBoundary.Location3.station2
                : theBoundary.Location3.station) + ","
                + padding8("") + ","
                + padding16("") + ","
                + padding16("") + ","
                + padding16("") + ","
                + padding32("")
                );
            tNewLines.Add("Interval=" + theBoundary.IntervalStr);
            if (theBoundary.HDType == enumHDBoundaryType.流量)
            {
                tNewLines.Add("Flow Hydrograph=" + theBoundary.Value.Count);
            }
            else if (theBoundary.HDType == enumHDBoundaryType.水位)
            {
                tNewLines.Add("Stage Hydrograph=" + theBoundary.Value.Count);
            }
            else if (theBoundary.HDType == enumHDBoundaryType.侧向流量 
                && theBoundary.BndDescription == enumBoundaryDescription.DistributedSource)
            {
                tNewLines.Add("Uniform Lateral Inflow Hydrograph=" + theBoundary.Value.Count);
            }
            else if (theBoundary.HDType == enumHDBoundaryType.侧向流量 
                && theBoundary.BndDescription == enumBoundaryDescription.PointSource)
            {
                tNewLines.Add("Lateral Inflow Hydrograph=" + theBoundary.Value.Count);
            }
            else
            {
                throw new Exception("不支持的边界类型'" + theBoundary.HDType.ToString() + "'");
            }
            int nlines = (int)Math.Ceiling(theBoundary.Value.Count * 1.0 / 10.0);
            for (int iline = 0; iline < nlines; ++iline)
            {
                string tline1 = "";
                for (int iv = 0; iv < 10; ++iv)
                {
                    if (iline * 10 + iv < theBoundary.Value.Count)
                    {
                        tline1 += paddingLeft8(theBoundary.Value[iline * 10 + iv].Data);
                    }

                }
                tNewLines.Add(tline1);
            }
            tNewLines.Add("DSS Path=");
            tNewLines.Add("Use DSS=False");
            tNewLines.Add("Use Fixed Start Time=False");
            tNewLines.Add("Fixed Start Date/Time=,");
            tNewLines.Add("Is Critical Boundary=False");
            tNewLines.Add("Critical Boundary Flow=");

            return tNewLines.ToArray();
        }

        private string[] writeWqBoundaryToLines(string theComponent,List<Boundary> boundaryList)
        {
            try
            {
                List<string> tNewLines = new List<string>();
                tNewLines.Add("Constituent= " + theComponent);
                for (int iboundary = 0; iboundary < boundaryList.Count; iboundary++)
                {
                    if (!boundaryList[iboundary].Pollutants.Contains(theComponent)) continue;
                    var icomponent = boundaryList[iboundary].Pollutants.IndexOf(theComponent);

                    tNewLines.Add("BC=" + boundaryList[iboundary].Location3.riverName + ","
                        + boundaryList[iboundary].Location3.reachName + ","
                        + boundaryList[iboundary].Location3.station + ",,,,,");
                    tNewLines.Add("BC Time Series");
                    if (theComponent == "Water Temperature")
                    {
                        tNewLines.Add("Time Series Mode= 1");
                        tNewLines.Add("Observed Data=" + boundaryList[iboundary].ObsConcetration[icomponent][0].Data);
                    }
                    else
                    {
                        tNewLines.Add("Time Series Mode= 4");
                        tNewLines.Add("Observed Data=" + boundaryList[iboundary].ObsConcetration[icomponent][0].Data);
                        tNewLines.Add("Constant Value=" + boundaryList[iboundary].Concetration[icomponent][0].Data);
                        tNewLines.Add("Constant Units= 1");
                    }
                    tNewLines.Add("End Time Series Data");
                }

                return tNewLines.ToArray();
            }
            catch(Exception ex)
            {
                CommonUtility.Log(ex.Message);
                return null;
            }
        }

        private string[] readHecDataTable(ref StreamReader sr, int ptsno, int colums = 4)
        {
            var pts = new string[ptsno];
            for (int i = 0; i < ptsno; i++)
            {
                if (i != 0 && i % colums == 0)
                {
                    char[] buffer = new char[16];
                    sr.Read(buffer, 0, 2);
                    sr.Read(buffer, 0, 16);
                    pts[i] = new string(buffer);
                }
                else
                {
                    char[] buffer = new char[16];
                    sr.Read(buffer, 0, 16);
                    pts[i] = new string(buffer);
                }
            }
            return pts;
        }

        private void JumpUnuseLine(ref StreamReader sr, ref string line)
        {
            line = sr.ReadLine();
            while (line == "" || line == "\r\n" || line.Length == 0)
            {
                line = sr.ReadLine();
                continue;
            }
        }

        #endregion
    }
}
