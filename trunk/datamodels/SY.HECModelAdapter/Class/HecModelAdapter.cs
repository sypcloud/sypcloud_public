﻿using System;
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

        public HecModelAdapter(bool configed)
        {
            var assemblyPath = this.GetType().Assembly.Location;
            var cfigfile = assemblyPath + ".config";
            config = ConfigurationManager.OpenExeConfiguration(assemblyPath);
            pythonEngine = config.AppSettings.Settings["PythonEngine"].Value;
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
                while(!isRunOk && File.Exists(logfile))
                {
                    using (FileStream fs = new FileStream(logfile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            if (OutputMsg != null)
                                OutputMsg(new MessageInfo() { Tag = 0, Message = sr.ReadToEnd() });

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
                    if (line == string.Empty) line = sr.ReadLine();

                    if (line.Trim().StartsWith("River Reach"))
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
                        for (int i = 0; i < ptsno * 2; i++)
                        {
                            if (i != 0 && i % 4 == 0)
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
                        for (int i = 0; i < ptsno * 2;)
                        {
                            rr.Points.Add(new Sy.Global.PointD(double.Parse(pts[i].Trim())
                                , double.Parse(pts[i + 1].Trim())));
                            i = i + 2;
                        }

                        line = sr.ReadLine();
                        line = sr.ReadLine();

                        st = line.Split('=')[1].Split(',');
                        rr.TextLocation = new Sy.Global.PointD(double.Parse(st[0].Trim()), double.Parse(st[1].Trim()));

                        line = sr.ReadLine();
                        line = sr.ReadLine();

                        rr.CSCollection = new List<HECCrossSection>();

                        while ((line = sr.ReadLine()) == "")
                            continue;

                        while (line.StartsWith("Type RM Length L Ch R"))
                        {
                            var cs = new HECCrossSection();
                            cs.Location = line.Split('=')[1].Split(',');
                            line = sr.ReadLine();
                            if (line.StartsWith("BEGIN DESCRIPTION:"))
                            { cs.Description = sr.ReadLine(); sr.ReadLine(); }
                            cs.LastEditedTime = sr.ReadLine();
                            cs.Data = new List<Sy.Global.PointD>();

                            line = sr.ReadLine();
                            st = line.Split('=');
                            ptsno = int.Parse(st[1]);
                            pts = new string[ptsno];
                            for (int i = 0; i < ptsno; i++)
                            {
                                if (i != 0 && i % 4 == 0)
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
                            for (int i = 0; i < ptsno; i++)
                            {
                                var regx = new System.Text.RegularExpressions.Regex("[ ]+");
                                var pt = regx.Split(pts[i].Trim());
                                cs.Data.Add(new Sy.Global.PointD(double.Parse(pt[0].Trim())
                                    , double.Parse(pt[1].Trim())));
                            }

                            line = sr.ReadLine();

                            if ((line = sr.ReadLine()).StartsWith("#Mann"))
                            {
                                cs.Manning = sr.ReadLine();
                                cs.ManningSta = sr.ReadLine();
                            }

                            line = sr.ReadLine();
                            cs.XS_HTab_Starting = sr.ReadLine();
                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            line = sr.ReadLine();

                            rr.CSCollection.Add(cs);

                            while ((line = sr.ReadLine()) == "")
                                continue;

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
                                    chainage = float.Parse(tmp[1].Split(':')[0].Trim())*-1; //根据情况自定义
                                    chainages.Add(chainage);

                                    while (!sr.EndOfStream)
                                    {
                                        line = sr.ReadLine();
                                        var pt = line.Trim().Split(',');
                                        if (pt.Count() == 1) pt = line.Trim().Split('，');

                                        if (pt[0] == "BEGIN")
                                        {
                                            chainage = float.Parse(pt[1].Split(':')[0].Trim())*-1;//根据情况自定义
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

                                for(int i=0;i<csc.Count;i++)
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
    }
}