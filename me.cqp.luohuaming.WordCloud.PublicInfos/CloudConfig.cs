﻿using me.cqp.luohuaming.WordCloud.Tool.IniConfig.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PublicInfos
{
    public enum MatchMode
    {
        Regex = 0,

        Contain = 1,

        Full = 2
    }

    public enum CycleMode
    {
        Today = 1,

        Yesterday = 0
    }

    public static class CloudConfig
    {
        public static int ImageWidth
        {
            get
            {
                if (MainSave.ConfigMain.Object.ContainsKey("Config") is false)
                {
                    MainSave.IniChangeFlag = true;
                    MainSave.ConfigMain.Object.Add(new ISection("Config"));
                    MainSave.ConfigMain.Save();
                }
                MainSave.IniChangeFlag = false;
                int? b = MainSave.ConfigMain.Object["Config"]["ImageWidth"];
                if (b.HasValue is false)
                {
                    b = 500;
                }

                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Config"]["ImageWidth"] = value; MainSave.ConfigMain.Save(); }
        }

        public static int ImageHeight
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["Config"]["ImageHeight"];
                if (b.HasValue is false)
                {
                    b = 500;
                }

                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Config"]["ImageHeight"] = value; MainSave.ConfigMain.Save(); }
        }

        public static int WordNum
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["Config"]["WordNum"];
                if (b.HasValue is false)
                {
                    b = 50;
                }

                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Config"]["WordNum"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string MaskPath
        {
            get
            {
                string c = MainSave.ConfigMain.Object["Config"]["MaskPath"]?.ToString() ?? "";
                return File.Exists(c) is false ? Path.Combine(MainSave.AppDirectory, c) : c;
            }
            set { MainSave.ConfigMain.Object["Config"]["MaskPath"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string Font
        {
            get
            {
                string c = MainSave.ConfigMain.Object["Config"]["Font"]?.ToString();
                if (string.IsNullOrEmpty(c))
                {
                    return "微软雅黑";
                }
                return c.EndsWith("ttf") || c.EndsWith("ttc") ? Path.Combine(MainSave.AppDirectory, c) : c;
            }
            set { MainSave.ConfigMain.Object["Config"]["Font"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string FilterWord
        {
            get
            {
                return MainSave.ConfigMain.Object["Config"]["FilterWord"]?.ToString();
            }
            set { MainSave.ConfigMain.Object["Config"]["FilterWord"] = value; MainSave.ConfigMain.Save(); }
        }

        public static bool WhiteListSwitch
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["WhiteList"]["Switch"];
                if (b.HasValue is false)
                {
                    b = 0;
                }

                return b == 1;
            }
            set { MainSave.ConfigMain.Object["WhiteList"]["Switch"] = value ? 1 : 0; MainSave.ConfigMain.Save(); }
        }

        public static List<long> WhiteList
        {
            get
            {
                var b = MainSave.ConfigMain.Object["WhiteList"]["Groups"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? new List<long>() : Array.ConvertAll(b.Split('|'), (input) => Convert.ToInt64(input)).ToList();
            }
            set
            {
                string b = "";
                value.ForEach(x => b += x.ToString() + "|");
                b.Remove(b.Length - 1);
                MainSave.ConfigMain.Object["WhiteList"]["Groups"] = b; MainSave.ConfigMain.Save();
            }
        }

        public static List<long> BlockList
        {
            get
            {
                var b = MainSave.ConfigMain.Object["BlockList"]["QQID"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? new List<long>() : Array.ConvertAll(b.Split('|'), (input) => Convert.ToInt64(input)).ToList();
            }
            set
            {
                string b = "";
                value.ForEach(x => b += x.ToString() + "|");
                b.Remove(b.Length - 1);
                MainSave.ConfigMain.Object["BlockList"]["QQID"] = b; MainSave.ConfigMain.Save();
            }
        }

        public static bool BlackListSwitch
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["BlackList"]["Switch"];
                if (b.HasValue is false)
                {
                    b = 0;
                }

                return b == 1;
            }
            set { MainSave.ConfigMain.Object["BlackList"]["Switch"] = value ? 1 : 0; MainSave.ConfigMain.Save(); }
        }

        public static List<long> BlackList
        {
            get
            {
                var b = MainSave.ConfigMain.Object["BlackList"]["Groups"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? new List<long>() : Array.ConvertAll(b.Split('|'), (input) => Convert.ToInt64(input)).ToList();
            }
            set
            {
                string b = "";
                value.ForEach(x => b += x.ToString() + "|");
                b.Remove(b.Length - 1);
                MainSave.ConfigMain.Object["BlackList"]["Groups"] = b; MainSave.ConfigMain.Save();
            }
        }

        public static string SendTmpMsg
        {
            get
            {
                return MainSave.ConfigMain.Object["Config"]["SendTmpMsg"]?.ToString();
            }
            set { MainSave.ConfigMain.Object["Config"]["SendTmpMsg"] = value; MainSave.ConfigMain.Save(); }
        }

        public static bool CycleSwitch
        {
            get
            {
                if (MainSave.ConfigMain.Object.ContainsKey("Cycle") is false)
                {
                    MainSave.IniChangeFlag = true;
                    MainSave.ConfigMain.Object.Add(new ISection("Cycle"));
                    MainSave.ConfigMain.Save();
                }
                MainSave.IniChangeFlag = false;

                int? b = MainSave.ConfigMain.Object["Cycle"]["CycleSwitch"];
                if (b.HasValue is false)
                {
                    b = 0;
                }

                return b == 1;
            }
            set { MainSave.ConfigMain.Object["Cycle"]["CycleSwitch"] = value ? 1 : 0; MainSave.ConfigMain.Save(); }
        }

        public static int CycleInterval
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["Cycle"]["Interval"];
                if (b.HasValue is false || b < 10 * 1000)
                {
                    b = 10 * 1000;
                }

                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Cycle"]["Interval"] = value; MainSave.ConfigMain.Save(); }
        }

        public static int CycleDelay
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["Cycle"]["Delay"];
                if (b.HasValue is false || b < 10 * 1000)
                {
                    b = 60 * 1000;
                }

                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Cycle"]["Delay"] = value; MainSave.ConfigMain.Save(); }
        }

        public static DateTime CycleTime
        {
            get
            {
                DateTime? b = MainSave.ConfigMain.Object["Cycle"]["CycleTime"];
                if (b.HasValue is false)
                {
                    b = new DateTime();
                }

                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Cycle"]["CycleTime"] = value; MainSave.ConfigMain.Save(); }
        }

        public static CycleMode CycleMode
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["Cycle"]["CycleMode"];
                if (b.HasValue is false)
                {
                    b = 0;
                }

                return (CycleMode)b;
            }
            set { MainSave.ConfigMain.Object["Cycle"]["CycleMode"] = (int)value; MainSave.ConfigMain.Save(); }
        }

        public static string CycleText
        {
            get
            {
                return MainSave.ConfigMain.Object["Cycle"]["CycleText"]?.ToString();
            }
            set { MainSave.ConfigMain.Object["Cycle"]["CycleText"] = value; MainSave.ConfigMain.Save(); }
        }

        public static MatchMode MatchMode
        {
            get
            {
                int? b = MainSave.ConfigMain.Object["Config"]["MatchMode"];
                if (b.HasValue is false)
                {
                    b = 0;
                }

                return (MatchMode)b;
            }
            set { MainSave.ConfigMain.Object["Config"]["MatchMode"] = (int)value; MainSave.ConfigMain.Save(); }
        }

        public static string TodayCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["TodayCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^今[日|天]词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["TodayCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string YesterdayCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["YesterdayCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^昨[日|天]词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["YesterdayCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string LastWeekCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["LastWeekCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^上个?周词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["LastWeekCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string WeekCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["WeekCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^[这|本]个?周词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["WeekCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string LastMonthCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["LastMonthCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^上个?月词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["LastMonthCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string MonthCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["MonthCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^[这|本]个?月词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["MonthCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string YearCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["YearCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^[这|本|今]个?年词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["YearCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string LastYearCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["LastYearCloudOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^去年词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["LastYearCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalWeekOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalWeekOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^(我的)?(个人)?本周词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalWeekOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalTodayOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalTodayOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^(我的)?(个人)?今[日|天]词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalTodayOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalMonthOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalMonthOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^(我的)?(个人)?[这|本]个?月词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalMonthOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalYearOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalYearOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^(我的)?(个人)?[这|本]个?年词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalYearOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalLastYearOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalLastYearOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^我的去年词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalLastYearOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalLastWeekOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalLastWeekOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^(我的)?(个人)?上个?周词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalLastWeekOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalYesterdayOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalYesterdayOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^(我的)?(个人)?昨[日|天]词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalYesterdayOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string PersonalLastMonthOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalLastMonthOrder"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^(我的)?(个人)?上个?月词云$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalLastMonthOrder"] = value; MainSave.ConfigMain.Save(); }
        }

        public static int GroupRankMaxCount
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["TodayRank"]?.ToString();
                return int.TryParse(b, out int value) ? value : 10;
            }
            set { MainSave.ConfigMain.Object["Config"]["TodayRank"] = value; MainSave.ConfigMain.Save(); }
        }

        public static string TodayRank
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["TodayRank"]?.ToString();
                return string.IsNullOrWhiteSpace(b) ? "^今日发言排行$" : b;
            }
            set { MainSave.ConfigMain.Object["Config"]["TodayRank"] = value; MainSave.ConfigMain.Save(); }
        }

        public static bool PieWord
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["DrawPieWord"]?.ToString();
                return bool.TryParse(b, out bool value) && value;
            }
            set { MainSave.ConfigMain.Object["Config"]["DrawPieWord"] = value; MainSave.ConfigMain.Save(); }
        }

        public static int ChartWidth { get; set; }

        public static int ChartHeight { get; set; }

        public static int YearShowWordCount
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["YearShowWordCount"]?.ToString();
                return int.TryParse(b, out int value) ? value : 10;
            }
            set { MainSave.ConfigMain.Object["Config"]["YearShowWordCount"] = value; MainSave.ConfigMain.Save(); }
        }

        public static bool YearShowWordListMode
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["YearShowWordListMode"]?.ToString();
                return !bool.TryParse(b, out bool value) || value;
            }
            set { MainSave.ConfigMain.Object["Config"]["YearShowWordListMode"] = value; MainSave.ConfigMain.Save(); }
        }
    }
}