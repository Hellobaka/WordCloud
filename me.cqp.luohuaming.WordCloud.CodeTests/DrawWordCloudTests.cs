﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var r = DrawWordCloud.Draw(644933097, dt, dt.AddDays(31), 863450594);
            Console.WriteLine($"WordNum: {r.WordNum}, pic: {r.CloudFilePath}");
        }
    }
}