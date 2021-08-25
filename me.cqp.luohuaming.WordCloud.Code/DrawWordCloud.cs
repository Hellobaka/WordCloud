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
        public class CloudResult
        {
            public string CloudFilePath { get; set; }
            public int WordNum { get; set; }
            public List<string> HotWords { get; set; }
        }
        public static CloudResult Draw(long GroupID, DateTime dateTime, long QQ = 0)
        {
            var ls = SQLHelper.GetRecordsByDate(GroupID, dateTime);
            StringBuilder stringBuilder = new StringBuilder();
            ls.ForEach(x => stringBuilder.AppendLine(x.Message));
            var extractor = new TfidfExtractor();
            var weight = extractor.ExtractTagsWithWeight(stringBuilder.ToString(), CloudConfig.WordNum);

            CloudResult result = new CloudResult();
            Dictionary<string, int> wordAndFrequence = new Dictionary<string, int>();
            result.WordNum = weight.Count();
            foreach (var item in weight)
            {
                wordAndFrequence.Add(item.Word, (int)(item.Weight * 1000));
            }

            weight = weight.OrderByDescending(x => x.Weight);
            result.HotWords = new List<string>();
            int index = 0;
            foreach(var item in weight)
            {
                if (index == 3)
                    break;
                result.HotWords.Add(item.Word);
                index++;
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
            result.CloudFilePath = filename;
            return result;
        }
    }
}
