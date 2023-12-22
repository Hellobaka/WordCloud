using System;
using System.Text.RegularExpressions;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;

namespace me.cqp.luohuaming.WordCloud.Code.OrderFunctions
{
    public class DrawYearCloud : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => CloudConfig.YearCloudOrder;

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
            DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
            var drawResult = DrawWordCloud.Draw(e.FromGroup, dt, DateTime.Now);
            string statistics = $"统计时段: {dt.ToString("G").Replace(" 0:00:00", "")}-{DateTime.Now.ToString("G").Replace(" 0:00:00", "")}，共计: {drawResult.WordNum}个词汇";
            int count = CloudConfig.YearShowWordCount;
            if (count > 0)
            {
                statistics += $"\r\n前{count}的词汇为: \r\n";
                for (int i = 0; i < Math.Min(count, drawResult.Words.Count); i++)
                {
                    statistics += $"{i + 1}. {drawResult.Words[i]}\r\n";
                }
            }
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
