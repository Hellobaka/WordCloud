using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;

namespace me.cqp.luohuaming.WordCloud.Code
{
    public class Event_GroupMessage
    {
        public static FunctionResult GroupMessage(CQGroupMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult()
            {
                SendFlag = false
            };
            try
            {
                if (!GetCanCall(e.FromGroup, e.FromQQ))
                    return result;
                foreach (var item in MainSave.Instances.Where(item => item.Judge(e.Message.Text)))
                {
                    return item.Progress(e);
                }
                return result;
            }
            catch (Exception exc)
            {
                MainSave.CQLog.Info("异常抛出", exc.Message + exc.StackTrace);
                return result;
            }
        }
        private static bool GetCanCall(long group, long QQ)
        {
            if (CloudConfig.BlockList.Any(x => x == QQ)) return false;
            if (CloudConfig.WhiteListSwitch)
                return CloudConfig.WhiteList.Any(x => x == group);
            else if (CloudConfig.BlackListSwitch)
                return !CloudConfig.BlackList.Any(x => x == group);
            else
                return CloudConfig.WhiteList.Any(x => x == group);
        }
    }
}
