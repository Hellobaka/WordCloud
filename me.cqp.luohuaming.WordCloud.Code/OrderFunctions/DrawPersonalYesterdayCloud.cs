﻿using System;
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
    public class DrawPersonalYesterdayCloud : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => CloudConfig.PersonalYesterdayOrder;

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
            DateTime dt = DateTime.Now.AddDays(-1);
            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            var drawResult = DrawWordCloud.Draw(e.FromGroup, dt, dt.AddDays(1), e.FromQQ);
            sendText.MsgToSend.Add(e.FromQQ.CQCode_At().ToSendString());
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
