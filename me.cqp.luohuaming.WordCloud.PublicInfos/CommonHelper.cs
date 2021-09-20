using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.Model;
using me.cqp.luohuaming.WordCloud.Tool.IniConfig;

namespace PublicInfos
{
    public static class CommonHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        /// <summary>
        /// 获取CQ码中的图片网址
        /// </summary>
        /// <param name="imageCQCode">需要解析的图片CQ码</param>
        /// <returns></returns>
        public static string GetImageURL(string imageCQCode)
        {
            string path = MainSave.ImageDirectory + CQCode.Parse(imageCQCode)[0].Items["file"] + ".cqimg";
            IniConfig image = new IniConfig(path);
            image.Load();
            return image.Object["image"]["url"].ToString();
        }
        public static string GetAppImageDirectory()
        {
            var ImageDirectory = Path.Combine(Environment.CurrentDirectory, "data", "image\\");
            return ImageDirectory;
        }
        public static List<long> GetSendGroup()
        {
            if (CloudConfig.WhiteListSwitch)
                return CloudConfig.WhiteList;
            else if (CloudConfig.BlackListSwitch)
            {
                var l = MainSave.CQApi.GetGroupList();
                List<long> r = new List<long>();
                foreach (var item in l.Select(x => x.Group.Id))
                {
                    if (CloudConfig.BlackList.Any(x => x == item))
                        continue;
                    r.Add(item);
                }
                return r;
            }
            else
                return CloudConfig.WhiteList;
        }
    }
}
