using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;
using System;
using System.Text.RegularExpressions;

namespace me.cqp.luohuaming.WordCloud.Code.OrderFunctions
{
    public class DrawYesterdayCloud : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => "昨[日|天]词云";

        public bool Judge(string destStr) => Regex.IsMatch(destStr, GetOrderStr());//这里判断是否能触发指令

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

            sendText.MsgToSend.Add(CQApi.CQCode_Image(DrawWordCloud.Draw(e.FromGroup, DateTime.Now.AddDays(-1))).ToSendString());
            result.SendObject.Add(sendText);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            throw new NotImplementedException();
        }
    }
}

