using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using JiebaNet.Analyser;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp;
using me.cqp.luohuaming.WordCloud.Sdk.Cqp.EventArgs;
using PublicInfos;

namespace me.cqp.luohuaming.WordCloud.Code.OrderFunctions
{
    public class DrawCloud : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => "今日词云";

        public bool Judge(string destStr) => destStr.Equals(GetOrderStr());//这里判断是否能触发指令

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
            var ls = SQLHelper.GetRecordsByDate(e.FromGroup, DateTime.Now);
            StringBuilder stringBuilder = new StringBuilder();
            ls.ForEach(x => stringBuilder.AppendLine(x.Message));
            var extractor = new TfidfExtractor();
            var weight = extractor.ExtractTagsWithWeight(stringBuilder.ToString(), CloudConfig.WordNum);
            Dictionary<string, int> wordAndFrequence = new Dictionary<string, int>();
            foreach (var item in weight)
            {
                wordAndFrequence.Add(item.Word, (int)(item.Weight * 1000));
            }
            Image mask = null;
            if (!string.IsNullOrWhiteSpace(CloudConfig.MaskPath))
            {
                mask = Image.FromFile(CloudConfig.MaskPath);
            }
            var wordCloud = new WordCloudSharp.WordCloud(CloudConfig.ImageWidth, CloudConfig.ImageHeight, allowVerical: true, mask: mask, fontname: CloudConfig.Font);
            var image = wordCloud.Draw(wordAndFrequence.Select(x => x.Key).ToList(), wordAndFrequence.Select(x => x.Value).ToList());
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            image.Save(Path.Combine(MainSave.ImageDirectory, filename));

            sendText.MsgToSend.Add(CQApi.CQCode_Image(filename).ToSendString());
            result.SendObject.Add(sendText);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            throw new NotImplementedException();
        }
    }
}
