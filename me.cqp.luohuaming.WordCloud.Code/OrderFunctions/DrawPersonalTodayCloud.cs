using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;

namespace me.cqp.luohuaming.WordCloud.Code.OrderFunctions
{
    public class DrawPersonalTodayCloud : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => CloudConfig.PersonalTodayOrder;

        public bool Judge(string destStr)
        {
            switch (CloudConfig.MatchMode)
            {
                case MatchMode.Regex:
                    return Regex.IsMatch(destStr, GetOrderStr());
                case MatchMode.Contain:
                    return destStr.Contains(GetOrderStr());
                case MatchMode.Full:
                    return destStr.Trim() == GetOrderStr();
                default:
                    return false;
            }
        }

        public FunctionResult Progress(CQGroupMessageEventArgs e)//群聊处理
        {
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            SendText sendText = new SendText
            {
                SendID = e.FromGroup,
            };
            if (!string.IsNullOrWhiteSpace(CloudConfig.SendTmpMsg))
                e.FromGroup.SendGroupMessage(CloudConfig.SendTmpMsg.Replace("<@>", CQApi.CQCode_At(e.FromQQ).ToSendString()));
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var drawResult = DrawWordCloud.Draw(e.FromGroup, dt, DateTime.Now, e.FromQQ);
            string statistics = $"统计时段: {dt:G}-{DateTime.Now:G}，共计: {drawResult.WordNum}个词汇";
            sendText.MsgToSend.Add(statistics);
            sendText.MsgToSend.Add(CQApi.CQCode_Image(drawResult.CloudFilePath).ToSendString());
            result.SendObject.Add(sendText);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            return new FunctionResult { Result = false, SendFlag = false };
        }
    }
}
