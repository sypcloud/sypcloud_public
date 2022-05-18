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
using RAS620;
using System.ComponentModel;

namespace SY.HECModelAdapter
{
    public class HecModelAdapter
    {
        private static Configuration config = null;
        private string pythonEngine;
        private bool isRunOk = false;

        /// <summary>
        /// 发送信息数据
        /// </summary>
        public event OutputMsgHandler OutputMsg;

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

        public HecModelAdapter(string folderPath)
        {
            ProjectDir = folderPath;
            //wf （1）在Boundary类里扩展
            //    (2) 实现一个HecModelAdapter构造函数，增加一个属性 List<Boundary>
            //    (3) 传入一个目录，首先读取prj文件，然后确认u0x和p0x文件，读取数据填充到List<Boundary>属性中，供贾总调用。
            BoundaryList = new List<Boundary>();
            try
            {
                #region//获取文件配置信息
                // 遍历目录下全部文件，找到第一个*.prj工程文件路径
                DirectoryInfo tDir = new DirectoryInfo(folderPath);
                FileInfo tPrjFile1 = null;
                foreach (FileInfo tfi in tDir.GetFiles("*.prj"))
                {
                    tPrjFile1 = tfi;
                    break;
                }

                if (tPrjFile1 == null)
                {
                    throw new Exception("没有找到prj文件");
                }

                string projectRootNameWithoutExtension = tPrjFile1.Name.Replace(".prj", "");

                //step1 读取prj文件，获取Current Plan的值
                string currentPlanExtension = "";
                {

                    string[] lines = System.IO.File.ReadAllLines(tPrjFile1.FullName);
                    foreach (string line1 in lines)
                    {
                        if (line1.Contains("Proj Title="))
                        {
                            ProjTitle = line1.Replace("Proj Title=", "");
                        }
                        if (line1.Contains("Current Plan="))
                        {
                            PlanFile = ProjTitle + "." + line1.Replace("Current Plan=", "");
                        }
                        if (line1.Contains("Geom File="))
                        {
                            Geofile = ProjTitle + "." + line1.Replace("Geom File=", "");
                        }
                    }
                }
                string currentPlanFilepath = tPrjFile1.DirectoryName + @"\" + PlanFile;
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
                            if (tempSD.Length >= 37)
                            {
                                string[] tempParts = tempSD.Split(',');
                                if (tempParts.Length == 4)
                                {
                                    int startYear, startMon, startDay, stopYear, stopMon, stopDay;
                                    int startHour, startMinu, startSec, stopHour, stopMinu, stopSec;
                                    ConvertDateStr2YMD(tempParts[0], out startYear, out startMon, out startDay);
                                    ConvertDateStr2YMD(tempParts[2], out stopYear, out stopMon, out stopDay);
                                    if (tempParts[1].Length != 8)
                                    {
                                        throw new Exception("无效的开始时间");
                                    }
                                    if (tempParts[3].Length != 8)
                                    {
                                        throw new Exception("无效的结束时间");
                                    }
                                    startHour = int.Parse(tempParts[1].Substring(0, 2));
                                    startMinu = int.Parse(tempParts[1].Substring(3, 2));
                                    startSec = int.Parse(tempParts[1].Substring(6, 2));
                                    stopHour = int.Parse(tempParts[3].Substring(0, 2));
                                    stopMinu = int.Parse(tempParts[3].Substring(3, 2));
                                    stopSec = int.Parse(tempParts[3].Substring(6, 2));

                                    SimulationStartTime = new DateTime(startYear, startMon, startDay, startHour, startMinu, startSec);
                                    SimulationEndTime = new DateTime(stopYear, stopMon, stopDay, stopHour, stopMinu, stopSec);
                                }
                            }
                        }
                        else if (line1.Contains("Flow File="))
                        {
                            flowFileExtentsion = line1.Replace("Flow File=", "");
                        }
                    }
                }
                #endregion

                #region//解析边界信息

                //step3 读取Unsteady File 文件中的 Boundary数据
                if (flowFileExtentsion.Equals(""))
                {
                    throw new Exception("Flow File不能为空，请检查plan文件");
                }
                string flowFilePath = tPrjFile1.DirectoryName + @"\" + projectRootNameWithoutExtension + "." + flowFileExtentsion;
                {
                    string[] lines = System.IO.File.ReadAllLines(flowFilePath);
                    for (int iline = 0; iline < lines.Length; ++iline)
                    {
                        if (lines[iline].Contains("Boundary Location="))
                        {
                            Boundary tBoundary = makeBoundaryByLines(lines, iline, SimulationStartTime);
                            if (tBoundary != null)
                                BoundaryList.Add(tBoundary);
                        }
                    }
                }

                #endregion


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool RunModel(string controlfile)
        {
            try
            {
                // 启用背景线程计算模型
                runModelInBackgroud(controlfile);
                System.Threading.Thread.Sleep(100);
                // 读取日志
                var dir = Path.GetDirectoryName(controlfile);
                var logfile = Directory.GetFiles(dir, ".computeMsgs.txt").FirstOrDefault();
                while (!isRunOk && File.Exists(logfile))
                {
                    using (FileStream fs = new FileStream(logfile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            var msg = sr.ReadToEnd();
                            CommonUtility.Log(msg);
                            if (OutputMsg != null)
                                OutputMsg(new MessageInfo() { Tag = 0, Message = msg });

                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (OutputMsg != null)
                {
                    OutputMsg(new MessageInfo() { Tag = 0, Message = ex.Message });
                }
                return false;
            }
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

        public string Crossection2Json(List<River> riverdm, string jsonfile)
        {
            try
            {
                var shp = Path.Combine(Path.GetDirectoryName(jsonfile),
                    Path.GetFileNameWithoutExtension(jsonfile) + "-shp.shp");
                Utility.Utility.CreateRiverXSPolylineShp(riverdm, shp);

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
                    var origin =float.Parse( rv.CSCollection.Last().Location[1].Replace("*", ""));
                    foreach (var cs in rv.CSCollection)
                    {
                        //计算断面测量点空间坐标
                        var pts = cs.Data;//断面测量点
                        var branchPts = rv.Points;//河段点坐标集合
                        var distance = float.Parse(cs.Location[1].Replace("*","")) - origin;//距离
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
                        if(zminPts.Count()==1)
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
                                cidx = pts.IndexOf(zminPts.First())+(int) Math.Floor(zminPts.Count() / 2.0);
                                cpt = pts[cidx];
                            }
                        }
                        ld = pts.Take(cidx).Select(x => (float)x.X).ToList<float>();
                        var ldz = pts.Take(cidx).Select(x => (float)x.Y).ToList<float>();
                        rd = pts.Skip(cidx+1).Select(x => (float)x.X).ToList<float>();
                        var rdz = pts.Skip(cidx+1).Select(x => (float)x.Y).ToList<float>();
                        var cptz = cpt.Y;
                        //计算坐标
                        Utility.Utility.GetSectionCoords(branchPts, distance, ld, rd, ref lpts, ref rpts, ref cpt.X, ref cpt.Y);
                        for (int i = 0; i < ld.Count; i++)
                            lpts[i] = new PointD(lpts[i].X, lpts[i].Y,ldz[i]);
                        for (int i = 0; i < rd.Count; i++)
                            rpts[i] = new PointD(rpts[i].X, rpts[i].Y, rdz[i]);
                        cpt = new PointD(cpt.X,cpt.Y,cptz);

                        cs.Points = lpts;
                        if(cidx>0)
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
                    if (qup.Count() > 0) rv.UpRvr = new List<River>();
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
                            rv.UpRvr.Add((from r in res where r.RvrName == q[0].Trim() && r.RchName == q[1].Trim() select r).FirstOrDefault());
                        }
                    }
                    //rv.UpRvr.Add((from r in res where r.RvrName == q[0].Trim() && r.RchName == q[1].Trim() select r).FirstOrDefault());
                    //}
                    //找下游河段
                    var qdwn = (from r in hecdm.JunctCollection
                                from x in r.UpRiverReach
                                where rv.RvrName == x[0].Trim() && rv.RchName == x[1].Trim()
                                select r);
                    if (qdwn.Count() > 0) rv.DnRvr = new List<River>();
                    foreach (var qd in qdwn)
                    {
                        foreach (var q in qd.DownRiverReach)
                        {
                            rv.DnRvr.Add((from r in res where r.RvrName == q[0].Trim() && r.RchName == q[1].Trim() select r).FirstOrDefault());
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

        public List<Boundary> BoundaryList { get; set; }
        public string ProjectDir { get; set; }
        public string Geofile { get; set; }
        public string ProjTitle { get; set; }
        public string PlanFile { get; set; }

        public DateTime SimulationStartTime { get; set; }
        public DateTime SimulationEndTime { get; set; }
        /// <summary>
        /// 单位：秒
        /// </summary>
        public float TimeInterval { get; set; }
        public HEC_DM ModelTopo { get; set; }

        private void runModelInBackgroud(string controlfile)
        {
            var bgw = new BackgroundWorker();
            bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
            bgw.DoWork += (sender, e) =>
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
            };
        }

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

        private Boundary makeBoundaryByLines(string[] lines, int startOffset, DateTime startDt)
        {
            Boundary boundary = new Boundary();
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
            }

            string intervalStr = lines[startOffset + 1].Replace("Interval=", "");
            TimeSpan timeSpan = new TimeSpan();
            {
                int n = 0;
                string unit = "";
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
            else
            {
                //throw new Exception("不支持的边界类型'" + line3 + "'");
                return null;
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
            return boundary;
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

    }
}
