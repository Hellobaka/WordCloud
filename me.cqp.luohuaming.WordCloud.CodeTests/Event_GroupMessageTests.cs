using Microsoft.VisualStudio.TestTools.UnitTesting;
using me.cqp.luohuaming.WordCloud.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using System.Threading;
using System.Diagnostics;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.Expand;
using System.Windows;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.Model;
using System.Reflection;

namespace me.cqp.luohuaming.WordCloud.Code.Tests
{
    [TestClass()]
    public class Event_GroupMessageTests
    {
        public void Pre()
        {
            Event_StartUp event_ = new Event_StartUp();
            CQApi api = new CQApi(new Sdk.Cqp.Model.AppInfo("",1,1,"","1.0.0",1,"","",0));
            CQLog log = new CQLog(0);
            CQStartupEventArgs e = new CQStartupEventArgs(api,log,0,0,"","",0);
            event_.CQStartup(null, e);
        }
        [TestMethod()]
        public void GroupMessageStressTest()
        {
            Pre();
            List<long> rdG = new List<long>();
            Random rd = new Random();
            //for(int i=0;i<10;i++)
            //    rdG.Add(rd.Next(1000000,10000000));
            CloudConfig.WhiteList.ForEach(x => rdG.Add(x));
            Debug.WriteLine("随机群号：");
            rdG.ForEach(x => Debug.WriteLine(x));

            var msg = SQLHelper.GetAllMsg();
            msg.Add("今日词云");
            msg.Add("今日词云");
            msg.Add("今日词云");
            msg.Add("今日词云");
            msg.Add("今日词云");
            msg.Add("今日词云");
            msg.Add("今日词云");
            msg.Add("今日词云");
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"第 {i} 轮压测，共 1000 次");
                for (int j = 0; j < 1000; j++)
                {
                    Thread thread = new Thread(() =>
                    {
                        long Group = rdG[rd.Next(rdG.Count)];
                        long QQ = rd.Next(1000000, 10000000);
                        string Msg = msg[rd.Next(msg.Count)];
                        CQGroupMessageEventArgs e = new CQGroupMessageEventArgs(MainSave.CQApi, MainSave.CQLog, 0, 0, "", "", 0, 0, 1, Group, QQ, "", Msg, false);
                        var r = Event_GroupMessage.GroupMessage(e);
                        Debug.WriteLine($"Group: {Group} QQ: {QQ} Msg: {Msg}");
                        Debug.WriteLine($"Result：{r.Result} {(r.Result ? r.SendObject[0].MsgToSend[0] : "")}");
                    });
                    thread.Start();
                    Thread.Sleep(50);
                }
            }
        }
        [TestMethod()]
        public void GroupMessageTest()
        {
            foreach (var item in Assembly.GetAssembly(typeof(Event_GroupMessage)).GetTypes())
            {
                if (item.IsInterface)
                    continue;
                foreach (var instance in item.GetInterfaces())
                {
                    if (instance == typeof(IOrderModel))
                    {
                        IOrderModel obj = (IOrderModel)Activator.CreateInstance(item);
                        if (obj.ImplementFlag == false)
                            break;
                        MainSave.Instances.Add(obj);
                    }
                }
            }

            MainSave.DBPath = @"data.db";
            MainSave.AppDirectory = @"D:\Code\WordCloud\me.cqp.luohuaming.WordCloud.CodeTests\bin\x86\Debug\";
            MainSave.ImageDirectory = @"D:\Code\WordCloud\me.cqp.luohuaming.WordCloud.CodeTests\bin\x86\Debug\";
            string msg = "个人上周词云";
            CQGroupMessageEventArgs e = new CQGroupMessageEventArgs(MainSave.CQApi, MainSave.CQLog, 0, 0, "", "", 0, 0, 1, 644933097, 863450594, "", msg, false);
            var r = Event_GroupMessage.GroupMessage(e);
            Console.WriteLine($"Result：{r.Result}");
            if(r.SendFlag)
            {
                foreach(var item in r.SendObject[0].MsgToSend)
                {
                    Console.WriteLine(item);
                }
            }
        }
        [TestMethod()]
        public void ConfigTest()
        {
            Pre();
            Clipboard.SetText(BinaryReaderExpand.ImageToBase64(@"E:\图\2071.png"));
            //DrawWordCloud.Draw(644933097, DateTime.Now);
        }
    }
}