using JiebaNet.Analyser;
using PublicInfos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.luohuaming.WordCloud.Code
{
    public static class DrawWordCloud
    {
        public static string Draw(long GroupID, DateTime dateTime, long QQ = 0)
        {
            var ls = SQLHelper.GetRecordsByDate(GroupID, dateTime);
            StringBuilder stringBuilder = new StringBuilder();
            ls.ForEach(x => stringBuilder.AppendLine(x.Message));
            var extractor = new TfidfExtractor();
            var weight = extractor.ExtractTagsWithWeight(stringBuilder.ToString(), CloudConfig.WordNum);
            Dictionary<string, int> wordAndFrequence = new Dictionary<string, int>();
            foreach (var item in weight)
            {
                wordAndFrequence.Add(item.Word, (int)(item.Weight * 1000));
            }
            Image mask = null;
            if (!string.IsNullOrWhiteSpace(CloudConfig.MaskPath) && File.Exists(CloudConfig.MaskPath))
            {
                mask = Image.FromFile(CloudConfig.MaskPath);
            }
            var wordCloud = new WordCloudSharp.WordCloud(CloudConfig.ImageWidth, CloudConfig.ImageHeight, allowVerical: true, mask: mask, fontname: CloudConfig.Font);
            var image = wordCloud.Draw(wordAndFrequence.Select(x => x.Key).ToList(), wordAndFrequence.Select(x => x.Value).ToList());
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            image.Save(Path.Combine(MainSave.ImageDirectory, filename));
            return filename;
        }
    }
}
