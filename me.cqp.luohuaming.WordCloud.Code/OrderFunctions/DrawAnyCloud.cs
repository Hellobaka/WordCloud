using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;
using System;

namespace me.cqp.luohuaming.WordCloud.Code.OrderFunctions
{
    public class DrawAnyCloud : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => "词云 ";

        public bool Judge(string destStr) => destStr.StartsWith(GetOrderStr());//这里判断是否能触发指令

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
            if (DateTime.TryParseExact(e.Message.Text.Replace(GetOrderStr(), ""), "yyyy-M-d"
                , System.Globalization.CultureInfo.InvariantCulture
                ,System.Globalization.DateTimeStyles.None
                ,out DateTime dateTime))
            {
                if (!string.IsNullOrWhiteSpace(CloudConfig.SendTmpMsg))
                    e.FromGroup.SendGroupMessage(CloudConfig.SendTmpMsg.Replace("<@>", CQApi.CQCode_At(e.FromQQ).ToSendString()));

                sendText.MsgToSend.Add(CQApi.CQCode_Image(DrawWordCloud.Draw(e.FromGroup, dateTime).CloudFilePath).ToSendString());
            }
            else
            {
                sendText.MsgToSend.Add("请按照\"词云 2021-8-7\"的格式输入日期");
            }
            result.SendObject.Add(sendText);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            throw new NotImplementedException();
        }
    }
}
