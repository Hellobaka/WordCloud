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
        public static CloudResult Draw(long GroupID, DateTime dateTimeA, DateTime dateTimeB, long QQ = 0)
        {
            dateTimeA = new DateTime(dateTimeA.Year, dateTimeA.Month, dateTimeA.Day);
            dateTimeB = new DateTime(dateTimeB.Year, dateTimeB.Month, dateTimeB.Day);
            var ls = SQLHelper.GetRecordsByDateRange(GroupID, dateTimeA, dateTimeB, QQ);
            return Draw(ls);
        }
        public static CloudResult Draw(long GroupID, DateTime dateTime, long QQ = 0)
        {
            var ls = SQLHelper.GetRecordsByDate(GroupID, dateTime, QQ);
            return Draw(ls);
        }
        public static CloudResult Draw(List<PublicInfos.Model.Record> records)
        {
            StringBuilder stringBuilder = new StringBuilder();
            records.ForEach(x => stringBuilder.AppendLine(x.Message));
            var extractor = new TfidfExtractor();
            var weight = extractor.ExtractTagsWithWeight(stringBuilder.ToString(), int.MaxValue);

            CloudResult result = new CloudResult();
            Dictionary<string, int> wordAndFrequence = new Dictionary<string, int>();
            result.WordNum = weight.Count();
            int count = 0;
            foreach (var item in weight)
            {
                if (count == CloudConfig.WordNum)
                    break;
                wordAndFrequence.Add(item.Word, (int)(item.Weight));
                count++;
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
            Directory.CreateDirectory(Path.Combine(MainSave.ImageDirectory, "WordCloud"));
            image.Save(Path.Combine(MainSave.ImageDirectory, "WordCloud", filename));
            image.Dispose();
            result.CloudFilePath = "WordCloud\\" + filename;
            return result;
        }
    }
}
