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
    public class WordChartItem
    {
        public int Count { get; set; }

        public DateTime Time { get; set; }
    }

    public class DrawWordChart
    {
        public static string DrawChart(List<WordChartItem> items)
        {
            int padding = 40;
            int spotRadius = 5;
            int fontSize = 12;
            int width = CloudConfig.ChartWidth;
            int height = CloudConfig.ChartHeight;
            using Bitmap bitmap = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(Color.White);
            int max = items.Max(x => x.Count);
            int min = items.Min(x => x.Count);
            DateTime maxTime = items.Max(x => x.Time);
            DateTime minTime = items.Min(x => x.Time);
            float perHeight = (height - padding * 2) / ((float)max);
            float perWidth = (float)((width - padding * 2) / ((maxTime - minTime).TotalDays + 2));
            List<PointF> pointList = new();
            for (int i = 0; i < items.Count; i++)
            {
                float x = padding + (i + 1) * perWidth - spotRadius / 2;
                float y = height - padding - items[i].Count * perHeight - spotRadius / 2;
                pointList.Add(new PointF(x, y));
            }
            // 轴
            Font font = new(CloudConfig.Font, fontSize);
            int yStep = (max - min) / 5;
            int xStep = 0;
            for (int i = 0; i * yStep <= max + yStep * 1.5; i++)
            {
                int num = i * yStep;
                float y = height - padding - (num * perHeight);
                var size = g.MeasureString(num.ToString(), font);
                g.DrawString(num.ToString(), font, Brushes.Black, new PointF(padding - size.Width - 3, y - (font.Size / 1.5f)));
                Pen dashPen = new(Brushes.LightGray, 1);
                dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                dashPen.DashOffset = 5;
                g.DrawLine(dashPen, new PointF(padding, y), new PointF(width - padding, y));
            }
            float maxX = 0;
            for (DateTime j = minTime; j <= maxTime; j = j.AddDays(1))
            {
                maxX = Math.Max(g.MeasureString(j.ToString("D"), font).Width, maxX);
            }
            for (int i = 1; i < pointList.Count; i++)
            {
                xStep = i;
                if (maxX < (width - padding * 2) / (pointList.Count / i + 1))
                {
                    break;
                }
            }
            int lastX = -65535;
            for (int i = 0; i < pointList.Count; i++)
            {
                string str = items[i].Time.ToString("D");
                var size = g.MeasureString(str, font);
                int x = (int)(pointList[i].X - size.Width / 2);
                if (x < padding)
                {
                    continue;
                }
                if (x > width - padding - size.Width)
                {
                    x = (int)(width - padding - size.Width);
                }
                if (x + size.Width > width - padding - size.Width - 5 && i != pointList.Count - 1)
                {
                    continue;
                }
                if (i > 0 && x < lastX + size.Width + 5)
                {
                    continue;
                }
                lastX = x;
                g.DrawString(str, font, Brushes.Black, new PointF(x, height - padding + 5));
                Pen dashPen = new(Brushes.LightGray, 1);
                dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                dashPen.DashOffset = 5;
                g.DrawLine(dashPen, new PointF(pointList[i].X + spotRadius / 2, padding), new PointF(pointList[i].X + spotRadius / 2, height - padding));
            }

            g.DrawLine(Pens.Black, new Point(padding, padding), new Point(padding, height - padding));
            g.DrawLine(Pens.Black, new Point(padding, height - padding), new Point(width - padding, height - padding));

            for (int i = 1; i < pointList.Count; i++)
            {
                PointF p = pointList[i];
                PointF lastP = pointList[i - 1];
                g.DrawLine(Pens.DimGray, new PointF(p.X + spotRadius / 2, p.Y + spotRadius / 2), new PointF(lastP.X + spotRadius / 2, lastP.Y + spotRadius / 2));
            }
            foreach (var item in pointList)
            {
                g.FillEllipse(Brushes.LightSkyBlue, new RectangleF(item, new Size(spotRadius, spotRadius)));
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            Directory.CreateDirectory(Path.Combine(MainSave.ImageDirectory, "WordCloud"));
            bitmap.Save(Path.Combine(MainSave.ImageDirectory, "WordCloud", filename));
            return filename;
        }
    }
}