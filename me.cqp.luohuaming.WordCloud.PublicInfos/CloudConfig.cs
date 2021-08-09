using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicInfos
{
    public static class CloudConfig
    {
        public static int ImageWidth
        {
            get { return (int)(MainSave.ConfigMain.Object["Config"]["ImageWidth"]?.ToInt32()); }
            set { MainSave.ConfigMain.Object["Config"]["ImageWidth"] = value; MainSave.ConfigMain.Save(); }
        }
        public static int ImageHeight
        {
            get { return (int)MainSave.ConfigMain.Object["Config"]["ImageHeight"]?.ToInt32(); }
            set { MainSave.ConfigMain.Object["Config"]["ImageHeight"] = value; MainSave.ConfigMain.Save(); }
        }
        public static int WordNum
        {
            get { return (int)(MainSave.ConfigMain.Object["Config"]["WordNum"]?.ToInt32()); }
            set { MainSave.ConfigMain.Object["Config"]["WordNum"] = value; MainSave.ConfigMain.Save(); }
        }
        public static string MaskPath
        {
            get 
            {
                string c = MainSave.ConfigMain.Object["Config"]["MaskPath"]?.ToString();
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

        public static string SendTmpMsg
        {
            get 
            { 
                return MainSave.ConfigMain.Object["Config"]["SendTmpMsg"]?.ToString();
            }
            set { MainSave.ConfigMain.Object["Config"]["SendTmpMsg"] = value; MainSave.ConfigMain.Save(); }
        }

    }
}
