using Microsoft.VisualStudio.TestTools.UnitTesting;
using me.cqp.luohuaming.WordCloud.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicInfos;

namespace me.cqp.luohuaming.WordCloud.Code.Tests
{
    [TestClass()]
    public class DrawWordCloudTests
    {
        [TestMethod()]
        public void DrawTest()
        {
            MainSave.DBPath = @"data.db";
            MainSave.AppDirectory = @"D:\Code\WordCloud\me.cqp.luohuaming.WordCloud.CodeTests\bin\x86\Debug\";
            MainSave.ImageDirectory = @"D:\Code\WordCloud\me.cqp.luohuaming.WordCloud.CodeTests\bin\x86\Debug\";
            DateTime d = new DateTime(2022, 8, 25);

            var groupRanks = DrawGroupRank.GetGroupRanks(644933097, d, d.AddDays(1));
            if(groupRanks == null)
            {
                Assert.Fail();
            }
            var rankResult = DrawGroupRank.GenerateRankList(groupRanks);
            var pic = DrawGroupRank.DrawPieChart(rankResult);
            pic.Save("1.png");
            pic.Dispose();
            Console.WriteLine(DrawGroupRank.GenerateRankString(rankResult));
        }
    }
}