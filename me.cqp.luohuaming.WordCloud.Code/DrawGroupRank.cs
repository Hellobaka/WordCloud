using JiebaNet.Analyser;
using PublicInfos;
using PublicInfos.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using static me.cqp.luohuaming.WordCloud.Code.DrawWordCloud;

namespace me.cqp.luohuaming.WordCloud.Code
{
    public class DrawGroupRank
    {
        private static PrivateFontCollection PrivateFontCollection { get; set; } = new PrivateFontCollection();

        public class GroupRankItem
        {
            public long GroupID { get; set; }
            public long QQ { get; set; }
            public List<Record> Records { get; set; }
        }

        public class GroupRankResult
        {
            public long QQ { get; set; }
            public string Nick { get; set; }
            public int WordCount { get; set; }
            public double Percent { get; set; }
            public CloudResult CloudResult { get; set; }
        }

        public static List<GroupRankItem> GetGroupRanks(long groupID, DateTime dateTimeA, DateTime dateTimeB)
        {
            var ls = SQLHelper.GetRecordsByDateRange(groupID, dateTimeA, dateTimeB);
            List<GroupRankItem> result = new List<GroupRankItem>();
            List<long> qqList = new List<long>();
            foreach (var record in ls)
            {
                if (qqList.Count > CloudConfig.GroupRankMaxCount)
                {
                    break;
                }
                if (qqList.Contains(record.QQID))
                {
                    continue;
                }
                qqList.Add(record.QQID);
            }
            foreach (var qq in qqList)
            {
                var records = ls.Where(x => x.QQID == qq).ToList();
                result.Add(new GroupRankItem
                {
                    GroupID = groupID,
                    QQ = qq,
                    Records = records
                });
            }
            return result;
        }

        public static List<GroupRankResult> GenerateRankList(List<GroupRankItem> groupRanks)
        {
            if (groupRanks.Count == 0)
            {
                return null;
            }
            try
            {
                List<GroupRankResult> result = new List<GroupRankResult>();
                var memberList = MainSave.CQApi.GetGroupMemberList(groupRanks.First().GroupID);
                int totalCount = 0;
                var extractor = new TfidfExtractor();
                foreach (var item in groupRanks)
                {
                    StringBuilder records = new StringBuilder();
                    item.Records.ForEach(x => records.AppendLine(x.Message));
                    var weight = extractor.ExtractTagsWithWeight(records.ToString(), int.MaxValue);
                    if(weight.Count() == 0)
                    {
                        continue;
                    }
                    totalCount += weight.Count();
                    result.Add(new GroupRankResult
                    {
                        QQ = item.QQ,
                        WordCount = weight.Count(),
                        Nick = memberList.FirstOrDefault(x=>x.QQ == item.QQ)?.Nick,
                        CloudResult = DrawWordCloud.Draw(item.Records, true)
                    });
                }
                result.ForEach(x => x.Percent = x.WordCount / (double)totalCount);
                extractor = null;
                GC.Collect();
                return result.OrderByDescending(x => x.WordCount).ToList();
            }
            catch (Exception ex)
            {
                MainSave.CQLog.Error("生成排行文本", $"{ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public static string GenerateRankString(List<GroupRankResult> result)
        {
            int count = Math.Min(result.Count, CloudConfig.GroupRankMaxCount);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"前{count}名发言排行：");
            for (int i = 1; i <= count; i++)
            {
                GroupRankResult item = result[i - 1];
                if (i == count)
                {
                    sb.Append($"{i}. {item.Nick}: {item.WordCount}词");
                }
                else
                {
                    sb.AppendLine($"{i}. {item.Nick}: {item.WordCount}词");
                }
            }
            return sb.ToString();
        }

        public static Bitmap DrawPieChart(List<GroupRankResult> result)
        {
            if (PrivateFontCollection.Families.Length == 0 && (CloudConfig.Font.EndsWith(".ttf") || CloudConfig.Font.EndsWith(".ttc")))
            {
                PrivateFontCollection.AddFontFile(CloudConfig.Font);
            }
            if(result.Count == 0)
            {
                return new Bitmap(400, 400);
            }
            Color[] colorArr = new Color[]
            {
                ColorTranslator.FromHtml("#66ccffAA"),
                ColorTranslator.FromHtml("#98fb98AA"),
                ColorTranslator.FromHtml("#ff0000AA"),
                ColorTranslator.FromHtml("#ffff00AA"),
                ColorTranslator.FromHtml("#00ffffAA"),
                ColorTranslator.FromHtml("#ff00ffAA"),
                ColorTranslator.FromHtml("#0000ffAA"),
                ColorTranslator.FromHtml("#00ff00AA"),
                ColorTranslator.FromHtml("#7fffd4AA"),
                ColorTranslator.FromHtml("#00bfffAA"),
            };
            Bitmap pie = new Bitmap(4000, 4000);
            int height = 400;
            int stringWidth = 0, stringHeight = 0;
            int topMargin = 10;
            List<string> legend = new List<string>();
            result.ForEach(x => legend.Add($"{x.Nick}: {x.WordCount}"));
            using (Graphics g = Graphics.FromImage(pie))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                float currentAngle = -90;
                Random rd = new Random();
                Font font = null;
                if (PrivateFontCollection.Families.Length > 0)
                {
                    font = new Font(PrivateFontCollection.Families[0], 16);
                }
                else
                {
                    font = new Font(CloudConfig.Font, 16);
                }
                for (int i = 0; i < result.Count; i++)
                {
                    var size = g.MeasureString(legend[i], font);
                    stringWidth = (int)Math.Max(stringWidth, size.Width);
                    stringHeight = (int)Math.Max(stringHeight, size.Height);
                }
                height = (result.Count) * (topMargin + stringHeight);
                for (int i = 0; i < result.Count; i++)
                {
                    var item = result[i];
                    SolidBrush brush = new SolidBrush(colorArr[(i + 1) % colorArr.Length]);
                    g.FillPie(brush, new Rectangle(0, 0, height, height), currentAngle, (float)item.Percent * 360);
                    currentAngle += (float)item.Percent * 360;
                    g.FillRectangle(brush, new Rectangle(height + 15, (topMargin) + (topMargin + stringHeight) * i + 3, 16, 16));
                    g.DrawString(legend[i], font, Brushes.Black, new Point(height + 15 + 16 + 5, (topMargin) + (topMargin + stringHeight) * i));
                    // 15方块左边距 16宽度 5方块右边距
                }
                g.DrawEllipse(Pens.Black, new Rectangle(0, 0, height, height));
            }
            Bitmap resultPic = new Bitmap(height + 15 + 16 + 5 + stringWidth + 10, height + 10);
            using (Graphics g = Graphics.FromImage(resultPic))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, resultPic.Width, resultPic.Height));
                g.DrawImage(pie, new PointF(0, 0));
            }
            pie.Dispose();
            return resultPic;
        }
    }
}
