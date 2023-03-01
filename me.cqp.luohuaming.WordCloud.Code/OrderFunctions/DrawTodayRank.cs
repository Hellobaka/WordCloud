using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace me.cqp.luohuaming.WordCloud.Code.OrderFunctions
{
    public class DrawTodayRank : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => CloudConfig.TodayRank;

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
            result.SendObject.Add(sendText);

            if (!string.IsNullOrWhiteSpace(CloudConfig.SendTmpMsg))
            {
                e.FromGroup.SendGroupMessage(CloudConfig.SendTmpMsg.Replace("<@>", CQApi.CQCode_At(e.FromQQ).ToSendString()));
            }

            var groupRanks = DrawGroupRank.GetGroupRanks(e.FromGroup, DateTime.Now.AddDays(-1), DateTime.Now);
            if(groupRanks == null)
            {
                sendText.MsgToSend.Add("生成失败");
                return result;
            }
            var rankResult = DrawGroupRank.GenerateRankList(groupRanks);
            var pic = DrawGroupRank.DrawPieChart(rankResult);
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            Directory.CreateDirectory(Path.Combine(MainSave.ImageDirectory, "WordCloud"));
            pic.Save(Path.Combine(MainSave.ImageDirectory, "WordCloud", filename));
            pic.Dispose();
            sendText.MsgToSend.Add(CQApi.CQCode_Image($"WordCloud\\{filename}").ToSendString());
            sendText.MsgToSend.Add(DrawGroupRank.GenerateRankString(rankResult));
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            return new FunctionResult { Result = false, SendFlag = false };
        }
    }
}
