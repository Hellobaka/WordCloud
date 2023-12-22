using Microsoft.VisualStudio.TestTools.UnitTesting;
using me.cqp.luohuaming.WordCloud.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicInfos;
using me.cqp.luohuaming.WordCloud.Code.OrderFunctions;

namespace me.cqp.luohuaming.WordCloud.Code.Tests
{
    [TestClass()]
    public class DrawWordCloudTests
    {
        [TestMethod()]
        public void DrawTest()
        {
            MainSave.DBPath = @"data.db";
            MainSave.AppDirectory = @"E:\酷Q机器人插件开发\词云\bot-development-framework\me.cqp.luohuaming.WordCloud.CodeTests\bin\x86\Debug\";
            MainSave.ImageDirectory = @"E:\酷Q机器人插件开发\词云\bot-development-framework\me.cqp.luohuaming.WordCloud.CodeTests\bin\x86\Debug\";
            DateTime d = new DateTime(2022, 8, 25);
            //CloudConfig.ChartWidth = 800;
            //CloudConfig.ChartHeight = 600;
            //List<WordChartItem> items = new List<WordChartItem>();
            //Random r = new Random();
            //for(int i = 160; i >= 0; i--)
            //{
            //    items.Add(new WordChartItem
            //    {
            //        Count = r.Next(0, 150),
            //        Time = DateTime.Now.AddDays(i * -1)
            //    });
            //    Console.WriteLine(items.Last().Count);
            //}
            //DrawWordChart.DrawChart(items);
        }
    }
}