using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Hec.Dss;
using SY.Models.ModelBase;

namespace SY.HECModelAdapter
{
    public class HMSModelAdapter
    {
        private HmsModelResultsDM resultsDM;
        private string ProjectDir;

        public HMSModelAdapter()
        { }

        public HMSModelAdapter(string dir)
        {
            ProjectDir = dir;
        }

        public void ParseResults(string resultfile)
        {
            try
            {
                this.resultsDM = new HmsModelResultsDM();
                resultsDM.ReadXml(resultfile);
            }
            catch (Exception ex)
            {
                Common.CommonUtility.Log(ex.Message);
            }
        }
        public Dictionary<string, float> GetPeakFlow()
        {
            try
            {
                var res = new Dictionary<string, float>();
                foreach (HmsModelResultsDM.StatisticsRow statistic in this.resultsDM.Statistics)
                {
                    if (!res.ContainsKey(statistic.BasinElementRow.name))
                    {
                        var area = statistic.BasinElementRow.GetDrainageAreaRows()[0].area;
                        var statisticMeasures = statistic.GetStatisticMeasureRows();
                        foreach (HmsModelResultsDM.StatisticMeasureRow measure in statisticMeasures)
                        {
                            if (measure.type == "Outflow Maximum")
                                res.Add(statistic.BasinElementRow.name, float.Parse(measure.value));
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                Common.CommonUtility.Log(ex.Message);
                return null;
            }
        }
        
        public HMS_BASIN_DM GetBasin(string basinFile)
        {
            HMS_BASIN_DM basins = new HMS_BASIN_DM();
            basins.Subbasin = new List<HMS_SUBBASIN_DM>();
            basins.Junction = new List<HMS_JUNCTION_DM>();
            basins.Reach = new List<HMS_REACH_DM>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(basinFile);
                for (int iline = 0; iline < lines.Length; ++iline)
                {
                    if (lines[iline].Contains("Subbasin:"))
                    {
                        var basin = makeBasinLines(lines, iline);
                        if (basin != null) basins.Subbasin.Add(basin);
                    }
                    if (lines[iline].Contains("Junction:"))
                    {
                        var juction = makeJunctionLines(lines, iline);
                        if (juction != null) basins.Junction.Add(juction);
                    }
                    if (lines[iline].Contains("Reach:"))
                    {
                        var reach = makeReachLines(lines, iline);
                        if (reach != null) basins.Reach.Add(reach);
                    }
                }
                return basins;
            }
            catch (Exception ex)
            {
                SY.Common.CommonUtility.Log(ex.Message);
                return null;
            }
        }

        public void UpdateBasin(HMS_BASIN_DM basins, string basinfile)
        {
            try
            {
                var contents = new StringBuilder();
                var header =
@"Basin: WMS Watershed
     Last Modified Date: 2 July 2022
     Last Modified Time: 11:51:55
     Version: 4.2
     Filepath Separator: \
     Unit System: Metric
     Missing Flow To Zero: No
     Enable Flow Ratio: No
     Compute Local Flow At Junctions: No

     Enable Sediment Routing: No

     Enable Quality Routing: No
End:
";
                var footer =
@"Basin Schematic Properties:
     Last View N: 2050739.048
     Last View S: 2029135.048
     Last View W: 186333.9176
     Last View E: 208003.9176
     Maximum View N: 2050739.048
     Maximum View S: 2029135.048
     Maximum View W: 186333.9176
     Maximum View E: 208003.9176
     Extent Method: Elements Maps
     Buffer: 10
     Draw Icons: Yes
     Draw Icon Labels: Name
     Draw Map Objects: No
     Draw Gridlines: Yes
     Draw Flow Direction: No
     Fix Element Locations: No
     Fix Hydrologic Order: No
     Map: hec.map.image.ImageMap
     Map File Name: g-img.img
     Minimum Scale: -2147483648
     Maximum Scale: 2147483647
     Map Shown: Yes
     Map: hms.map.MapGeoMap
     Map File Name: wmsexport.map
     Minimum Scale: -2147483648
     Maximum Scale: 2147483647
     Map Shown: Yes
End:";
                contents.AppendLine(header);

                foreach (var subbasin in basins.Subbasin)
                {
                    contents.AppendLine("Subbasin: " + subbasin.Subbasin);
                    contents.AppendLine("     Last Modified Date: " + subbasin.Last_Modified_Date);
                    contents.AppendLine("     Last Modified Time: " + subbasin.Last_Modified_Time);
                    contents.AppendLine("     Canvas X: " + subbasin.Canvas_X);
                    contents.AppendLine("     Canvas Y: " + subbasin.Canvas_Y);
                    contents.AppendLine("     Label X: " + subbasin.Label_X);
                    contents.AppendLine("     Label Y: " + subbasin.Label_Y);
                    contents.AppendLine("     Area: " + subbasin.Area);
                    contents.AppendLine("     Downstream: " + subbasin.Downstream);
                    contents.AppendLine("");
                    contents.AppendLine("     Canopy: " + subbasin.Canopy);
                    contents.AppendLine("     Plant Uptake Method: " + subbasin.Plant_Uptake_Method);
                    contents.AppendLine("");
                    contents.AppendLine("     Surface: " + subbasin.Surface);
                    contents.AppendLine("");
                    contents.AppendLine("     LossRate: " + subbasin.LossRate);
                    contents.AppendLine("     Percent Impervious Area: " + subbasin.Percent_Impervious_Area);
                    contents.AppendLine("     Curve Number: " + subbasin.Curve_Number);
                    contents.AppendLine("     Initial Abstraction: " + subbasin.Initial_Abstraction);
                    contents.AppendLine("");
                    contents.AppendLine("     Transform: " + subbasin.Transform);
                    contents.AppendLine("     Lag: " + subbasin.Lag);
                    contents.AppendLine("     Unitgraph Type: " + subbasin.Unitgraph_Type);
                    contents.AppendLine("");
                    contents.AppendLine("     Baseflow: " + subbasin.Baseflow);

                    contents.AppendLine("End:");
                    contents.AppendLine("");
                }
                foreach (var junction in basins.Junction)
                {
                    contents.AppendLine("Junction: " + junction.Junction);
                    contents.AppendLine("     Last Modified Date: " + junction.Last_Modified_Date);
                    contents.AppendLine("     Last Modified Time: " + junction.Last_Modified_Time);
                    contents.AppendLine("     Canvas X: " + junction.Canvas_X);
                    contents.AppendLine("     Canvas Y: " + junction.Canvas_Y);
                    contents.AppendLine("     Label X: " + junction.Label_X);
                    contents.AppendLine("     Label Y: " + junction.Label_Y);
                    contents.AppendLine("     Downstream: " + junction.Downstream);
                    contents.AppendLine("End:");
                    contents.AppendLine("");
                }

                foreach (var reach in basins.Reach)
                {
                    contents.AppendLine("Reach: " + reach.Reach);
                    contents.AppendLine("     Last Modified Date: " + reach.Last_Modified_Date);
                    contents.AppendLine("     Last Modified Time: " + reach.Last_Modified_Time);
                    contents.AppendLine("     Canvas X: " + reach.Canvas_X);
                    contents.AppendLine("     Canvas Y: " + reach.Canvas_Y);
                    contents.AppendLine("     From Canvas X: " + reach.From_Canvas_X);
                    contents.AppendLine("     From Canvas Y: " + reach.From_Canvas_Y);
                    contents.AppendLine("     Label X: " + reach.Label_X);
                    contents.AppendLine("     Label Y: " + reach.Label_Y);
                    contents.AppendLine("     Downstream: " + reach.Downstream);
                    contents.AppendLine("");
                    contents.AppendLine("     Route: " + reach.Route);
                    contents.AppendLine("     Muskingum K: " + reach.Muskingum_K);
                    contents.AppendLine("     Muskingum x: " + reach.Muskingum_X);
                    contents.AppendLine("     Muskingum Steps: " + reach.Muskingum_X);
                    contents.AppendLine("     Channel Loss: " + reach.Channel_Loss);
                    contents.AppendLine("End:");
                    contents.AppendLine("");
                }

                contents.AppendLine(footer);

                System.IO.File.WriteAllText(basinfile, contents.ToString());
            }
            catch (Exception ex)
            {
                Common.CommonUtility.Log(ex.Message);
            }
        }

        public List<TSData> GetTimeSeries(string outlet,int tag)
        {
            var res = new List<TSData>();
            try
            {
                var basins = this.resultsDM.BasinElement;
                foreach(HmsModelResultsDM.BasinElementRow basin in basins)
                {
                    if (basin.name.Equals(outlet))
                    {
                        HmsModelResultsDM.HydrologyRow hy = basin.GetHydrologyRows()[0];
                        HmsModelResultsDM.TimeSeriesRow ts = hy.GetTimeSeriesRows()[tag];

                        using (var dssr = new DssReader(Path.Combine(ProjectDir, ts.DssFileName),
                                            DssReader.MethodID.MESS_METHOD_GENERAL_ID, DssReader.LevelID.MESS_LEVEL_CRITICAL))
                        {
                            var tsdata = dssr.GetTimeSeries(new DssPath(ts.DssPathname));
                            var idx = 0;
                            foreach (var t in tsdata.Times)
                            {
                                res.Add(new TSData()
                                {
                                    DT = t,
                                    Data = (float)tsdata.Values[idx]
                                });
                                idx++;
                            }
                        }
                        break;
                    }
                }

                
                return res;
            }
            catch (Exception ex)
            {
                Common.CommonUtility.Log(ex.Message);
                return res;
            }
        }

        public List<Tuple<string,float,float>> GetJuctionBasinArea(List<string> juctionName)
        {
            try
            {
                List<Tuple<string, float, float>> res=new List<Tuple<string, float, float>>();
                var qlist = GetPeakFlow();

                foreach (var jc in juctionName)
                {
                    var area = 0f;
                    var q = 0f;
                    getAreaAndQ(jc, qlist, ref area,ref q);
                    res.Add(Tuple.Create<string,float,float>(jc,area,q));
                }

                return res;
            }
            catch(Exception ex)
            {
                SY.Common.CommonUtility.Log(ex.Message);
                return null;
            }
        }
        private void getAreaAndQ(string jc, Dictionary<string, float> qlist, ref float area, ref float q)
        {
            var jcobj = (from r in this.resultsDM.BasinElement.AsEnumerable()
                         where r["name"].Equals(jc)
                         select (HmsModelResultsDM.BasinElementRow)r).FirstOrDefault();
            area += float.Parse(jcobj.GetDrainageAreaRows().FirstOrDefault().area);
            q += qlist[jc];
            var links = jcobj.GetFlowLinkRows();
            foreach (var lk in links)
            {
                var lkname = lk.upstreamName;
                if (lkname.EndsWith("R"))
                {
                    lkname = lkname.Replace("R", "C");
                    jcobj = (from r in this.resultsDM.BasinElement.AsEnumerable()
                             where r["name"].Equals(lkname)
                             select (HmsModelResultsDM.BasinElementRow)r).FirstOrDefault();
                    area += float.Parse(jcobj.GetDrainageAreaRows().FirstOrDefault().area);
                    q += qlist[lkname];
                    getAreaAndQ(lkname, qlist, ref area, ref q);
                }
            }
        }
        private HMS_SUBBASIN_DM makeBasinLines(string[] lines, int startOffset)
        {
            /*后面改造为已END标识结尾来循环判断*/
            HMS_SUBBASIN_DM subbasin = new HMS_SUBBASIN_DM();
            subbasin.Subbasin = lines[startOffset].Replace("Subbasin:", "").Trim();
            subbasin.Last_Modified_Date = lines[startOffset + 1].Replace("Last Modified Date:", "").Trim();
            subbasin.Last_Modified_Time = lines[startOffset + 2].Replace("Last Modified Time:", "").Trim();
            subbasin.Canvas_X = lines[startOffset + 3].Replace("Canvas X:", "").Trim();
            subbasin.Canvas_Y = lines[startOffset + 4].Replace("Canvas Y:", "").Trim();
            subbasin.Label_X = lines[startOffset + 5].Replace("Label X:", "").Trim();
            subbasin.Label_Y = lines[startOffset + 6].Replace("Label Y:", "").Trim();
            subbasin.Area = lines[startOffset + 7].Replace("Area:", "").Trim();
            subbasin.Downstream = lines[startOffset + 8].Replace("Downstream:", "").Trim();
            subbasin.Canopy = lines[startOffset + 10].Replace("Canopy:", "").Trim();
            startOffset++;
            subbasin.Plant_Uptake_Method = lines[startOffset + 11].Replace("Plant Uptake Method:", "").Trim();
            subbasin.Surface = lines[startOffset + 13].Replace("Surface:", "").Trim();
            subbasin.LossRate = lines[startOffset + 15].Replace("LossRate:", "").Trim();
            subbasin.Percent_Impervious_Area = lines[startOffset + 16].Replace("Percent Impervious Area:", "").Trim();
            subbasin.Curve_Number = lines[startOffset + 17].Replace("Curve Number:", "").Trim();
            subbasin.Initial_Abstraction = lines[startOffset + 18].Replace("Initial Abstraction:", "").Trim();
            subbasin.Transform = lines[startOffset + 20].Replace("Transform:", "").Trim();
            subbasin.Lag = lines[startOffset + 21].Replace("Lag:", "").Trim();
            subbasin.Unitgraph_Type = lines[startOffset + 22].Replace("Unitgraph Type:", "").Trim();
            subbasin.Baseflow = lines[startOffset + 24].Replace("Baseflow:", "").Trim();
            return subbasin;
        }
        private HMS_JUNCTION_DM makeJunctionLines(string[] lines, int startOffset)
        {
            var juction = new HMS_JUNCTION_DM();
            juction.Junction = lines[startOffset].Replace("Junction:", "").Trim();
            juction.Last_Modified_Date = lines[startOffset + 1].Replace("Last Modified Date:", "").Trim();
            juction.Last_Modified_Time = lines[startOffset + 2].Replace("Last Modified Time:", "").Trim();
            juction.Canvas_X = lines[startOffset + 3].Replace("Canvas X:", "").Trim();
            juction.Canvas_Y = lines[startOffset + 4].Replace("Canvas Y:", "").Trim();
            juction.Label_X = lines[startOffset + 5].Replace("Label X:", "").Trim();
            juction.Label_Y = lines[startOffset + 6].Replace("Label Y:", "").Trim();
            return juction;
        }
        private HMS_REACH_DM makeReachLines(string[] lines, int startOffset)
        {
            var reach = new HMS_REACH_DM();
            reach.Reach = lines[startOffset].Replace("Reach:", "").Trim();
            reach.Last_Modified_Date = lines[startOffset + 1].Replace("Last Modified Date:", "").Trim();
            reach.Last_Modified_Time = lines[startOffset + 2].Replace("Last Modified Time:", "").Trim();
            reach.Canvas_X = lines[startOffset + 3].Replace("Canvas X:", "").Trim();
            reach.Canvas_Y = lines[startOffset + 4].Replace("Canvas Y:", "").Trim();
            reach.From_Canvas_X = lines[startOffset + 5].Replace("From Canvas X:", "").Trim();
            reach.From_Canvas_Y = lines[startOffset + 6].Replace("From Canvas Y:", "").Trim();
            reach.Label_X = lines[startOffset + 7].Replace("Label X:", "").Trim();
            reach.Label_Y = lines[startOffset + 8].Replace("Label Y:", "").Trim();
            reach.Downstream = lines[startOffset + 9].Replace("Downstream:", "").Trim();
            reach.Route = lines[startOffset + 11].Replace("Route:", "").Trim();
            reach.Channel_Loss = lines[startOffset + 12].Replace("Channel Loss:", "").Trim();
            return reach;
        }
    }
}
