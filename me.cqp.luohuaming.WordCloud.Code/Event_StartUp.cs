using System;
using System.IO;
using System.Reflection;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.Interface;
using PublicInfos;

namespace me.cqp.luohuaming.WordCloud.Code
{
    public class Event_StartUp : ICQStartup
    {
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            MainSave.AppDirectory = e.CQApi.AppDirectory;
            MainSave.CQApi = e.CQApi;
            MainSave.CQLog = e.CQLog;
            MainSave.DBPath = Path.Combine(MainSave.AppDirectory, "data.db");
            SQLHelper.CreateDB();
            MainSave.ImageDirectory = CommonHelper.GetAppImageDirectory();
            JiebaNet.Segmenter.ConfigManager.ConfigFileBaseDir = Path.Combine(MainSave.AppDirectory, "Jieba");
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
            //这里写处理逻辑
            //MainSave.Instances.Add(new ExampleFunction());//这里需要将指令实例化填在这里
        }
    }
}
