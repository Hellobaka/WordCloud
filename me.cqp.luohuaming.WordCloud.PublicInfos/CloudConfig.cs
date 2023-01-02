using me.cqp.luohuaming.WordCloud.Tool.IniConfig.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    b = 500;
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
                    b = 500;
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
                    b = 50;
                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Config"]["WordNum"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string MaskPath
        {
            get
            {
                string c = MainSave.ConfigMain.Object["Config"]["MaskPath"]?.ToString() ?? "";
                if (File.Exists(c) is false)
                    return Path.Combine(MainSave.AppDirectory, c);
                return c;
            }
            set { MainSave.ConfigMain.Object["Config"]["MaskPath"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string Font
        {
            get
            {
                string c = MainSave.ConfigMain.Object["Config"]["Font"]?.ToString();
                if (c.EndsWith("ttf") || c.EndsWith("ttc"))
                    return Path.Combine(MainSave.AppDirectory, c);
                return c;
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
                    b = 0;
                return b == 1;
            }
            set { MainSave.ConfigMain.Object["WhiteList"]["Switch"] = value ? 1 : 0; MainSave.ConfigMain.Save(); }
        }
        public static List<long> WhiteList
        {
            get
            {
                var b = MainSave.ConfigMain.Object["WhiteList"]["Groups"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return new List<long>();
                return Array.ConvertAll(b.Split('|'), (input) => Convert.ToInt64(input)).ToList();
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
                if (string.IsNullOrWhiteSpace(b))
                    return new List<long>();
                return Array.ConvertAll(b.Split('|'), (input) => Convert.ToInt64(input)).ToList();
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
                    b = 0;
                return b == 1;
            }
            set { MainSave.ConfigMain.Object["BlackList"]["Switch"] = value ? 1 : 0; MainSave.ConfigMain.Save(); }
        }
        public static List<long> BlackList
        {
            get
            {
                var b = MainSave.ConfigMain.Object["BlackList"]["Groups"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return new List<long>();
                return Array.ConvertAll(b.Split('|'), (input) => Convert.ToInt64(input)).ToList();
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
                    b = 0;
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
                    b = 10 * 1000;
                return b.Value;
            }
            set { MainSave.ConfigMain.Object["Cycle"]["Interval"] = value; MainSave.ConfigMain.Save(); }
        }
        public static DateTime CycleTime
        {
            get
            {
                DateTime? b = MainSave.ConfigMain.Object["Cycle"]["CycleTime"];
                if (b.HasValue is false)
                    b = new DateTime();
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
                    b = 0;
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
                    b = 0;
                return (MatchMode)b;
            }
            set { MainSave.ConfigMain.Object["Config"]["MatchMode"] = (int)value; MainSave.ConfigMain.Save(); }
        }

        public static string TodayCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["TodayCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^今[日|天]词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["TodayCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string YesterdayCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["YesterdayCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^昨[日|天]词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["YesterdayCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string LastWeekCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["LastWeekCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^上个?周词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["LastWeekCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string WeekCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["WeekCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^[这|本]个?周词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["WeekCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string LastMonthCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["LastMonthCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^上个?月词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["LastMonthCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string MonthCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["MonthCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^[这|本]个?月词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["MonthCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string YearCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["YearCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^[这|本|今]个?年词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["YearCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string LastYearCloudOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["YearCloudOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^去年词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["YearCloudOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalWeekOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalWeekOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^(我的)?(个人)?本周词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalWeekOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalTodayOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalTodayOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^(我的)?(个人)?今[日|天]词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalTodayOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalMonthOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalMonthOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^(我的)?(个人)?[这|本]个?月词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalMonthOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalYearOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalYearOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^(我的)?(个人)?[这|本]个?年词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalMonthOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalLastYearOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalYearOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^我的去年词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalMonthOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalLastWeekOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalLastWeekOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^(我的)?(个人)?上个?周词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalLastWeekOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalYesterdayOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalYesterdayOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^(我的)?(个人)?昨[日|天]词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalYesterdayOrder"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string PersonalLastMonthOrder
        {
            get
            {
                var b = MainSave.ConfigMain.Object["Config"]["PersonalLastMonthOrder"]?.ToString();
                if (string.IsNullOrWhiteSpace(b))
                    return "^(我的)?(个人)?上个?月词云$";
                return b;
            }
            set { MainSave.ConfigMain.Object["Config"]["PersonalLastMonthOrder"] = value; MainSave.ConfigMain.Save(); }
        }
    }
}
