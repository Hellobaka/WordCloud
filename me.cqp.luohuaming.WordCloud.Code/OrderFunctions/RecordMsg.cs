using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;
using System;

namespace me.cqp.luohuaming.WordCloud.Code.OrderFunctions
{
    class RecordMsg : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => "";

        public bool Judge(string destStr) => true;

        public FunctionResult Progress(CQGroupMessageEventArgs e)//群聊处理
        {
            FunctionResult result = new FunctionResult
            {
                Result = false,
                SendFlag = false,
            };
            SQLHelper.AddRecord(e.FromGroup, e.FromQQ, e.Message);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            return new FunctionResult { Result = false, SendFlag = false };
        }
    }
}
