using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.Interface;
using me.cqp.luohuaming.WordCloud.Tool.IniConfig.Linq;
using PublicInfos;
using Timer = System.Timers.Timer;

namespace me.cqp.luohuaming.WordCloud.Code
{
    public class Event_StartUp : ICQStartup
    {
        public static Timer timer = new Timer();
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            try
            {
                MainSave.AppDirectory = e.CQApi.AppDirectory;
                MainSave.CQApi = e.CQApi;
                MainSave.CQLog = e.CQLog;
                MainSave.DBPath = Path.Combine(MainSave.AppDirectory, "data.db");
                SQLHelper.CreateDB();
                MainSave.ImageDirectory = CommonHelper.GetAppImageDirectory();
                //jieba配置
                JiebaNet.Segmenter.ConfigManager.ConfigFileBaseDir = Path.Combine(MainSave.AppDirectory, "Jieba");
                //反射
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
                if (MainSave.ConfigMain.Object["Config"].ContainsKey("EnableGroup"))
                {
                    MainSave.IniChangeFlag = true;
                    MainSave.ConfigMain.Object["WhiteList"]["Switch"] = new IValue(1);
                    MainSave.ConfigMain.Object["WhiteList"]["Groups"] = MainSave.ConfigMain.Object["Config"]["EnableGroup"];
                    MainSave.ConfigMain.Object["Config"].Remove("EnableGroup");
                    MainSave.ConfigMain.Save();
                    MainSave.IniChangeFlag = false;
                }
                e.CQLog.Info("词云生效", $"{(CloudConfig.BlackListSwitch ? "黑名单模式" : "白名单模式")}, " +
                    $"共{(CloudConfig.BlackListSwitch ? "禁用" : "开启")}了 " +
                    $"{(CloudConfig.BlackListSwitch ? CloudConfig.BlackList.Count : CloudConfig.WhiteList.Count)} 个群");
                if (CloudConfig.CycleSwitch)
                {
                    timer.Interval = CloudConfig.CycleInterval;
                    timer.Elapsed += CallGenerate;
                    timer.Start();
                    e.CQLog.Info("词云时钟", $"设定成功，在{CloudConfig.CycleTime.Hour}: {CloudConfig.CycleTime.Minute}将发送{(CloudConfig.CycleMode == CycleMode.Today ? "今天" : "昨天")}的词云");
                }
            }
            catch (Exception exc)
            {
                e.CQLog.Info("载入错误", exc.Message + "\n" + exc.StackTrace);
                MainSave.Instances.Clear();
                timer.Stop();
                timer.Dispose();
            }
        }
        static bool CallFlag = false;
        private void CallGenerate(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (CallFlag)
                    return;
                int hour = DateTime.Now.Hour, minute = DateTime.Now.Minute;
                if (CloudConfig.CycleTime.Hour == hour && CloudConfig.CycleTime.Minute == minute)
                {
                    CallFlag = true;
                    Thread thread = new Thread(() => { Thread.Sleep(60 * 1000); CallFlag = false; });
                    thread.Start();
                    DateTime currentTime = DateTime.Now;
                    foreach (var item in CommonHelper.GetSendGroup())
                    {
                        DrawWordCloud.CloudResult r;
                        switch (CloudConfig.CycleMode)
                        {
                            case CycleMode.Today:
                                r = DrawWordCloud.Draw(item, currentTime);
                                break;
                            case CycleMode.Yesterday:
                                r = DrawWordCloud.Draw(item, currentTime.AddDays(-1));
                                break;
                            default:
                                r = null;
                                break;
                        }
                        if (r == null)
                        {
                            MainSave.CQLog.Info("词云生成失败", "时钟事件, result为空, 检查 CycleMode 是否为0或者为1");
                            return;
                        }
                        string sendText = CloudConfig.CycleText.Replace("\\n", "\n").Replace("<num>", r.WordNum.ToString());
                        StringBuilder sb = new StringBuilder();
                        int index = 1;
                        foreach (var word in r.HotWords)
                        {
                            sb.AppendLine($"{index}. {word}");
                            index++;
                        }
                        if (sb.Length <= 2)
                            continue;
                        sb.Replace(Environment.NewLine, "", sb.Length - 2, 1);
                        sendText = sendText.Replace("<content>", sb.ToString());
                        if (string.IsNullOrWhiteSpace(sendText) is false)
                            MainSave.CQApi.SendGroupMessage(item, sendText);
                        MainSave.CQApi.SendGroupMessage(item, CQApi.CQCode_Image(r.CloudFilePath));
                        Thread.Sleep(1000 * 60);
                    }
                }
            }
            catch (Exception exc)
            {
                MainSave.CQLog.Info("时钟错误", exc.Message + "\n" + exc.StackTrace);
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
