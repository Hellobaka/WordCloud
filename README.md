# WordCloud

## 介绍
词云插件, 使用了Jieba.Net以及[WordCloudSharp词云库](https://github.com/AmmRage/WordCloudSharp)

## 使用流程
以下提及的所有文件均在[1.0.0](https://github.com/Hellobaka/WordCloud/releases/tag/1.0.0)
1. 下载插件
2. 解压`jieba.zip`到数据目录下(`me.cqp.luohuaming.WordCloud`)
3. 挂机一段时间，待群内有足够多的消息
4. `今[日|天]词云` => 今天的词云
5. `昨[日|天]词云` => 昨天的词云
6. 更多指令见配置文件
6. `词云 yyyy-M-d` => 任意日期的词云

## 数据库配置
- Mirai-Native框架用户:
> 下载 System.Data.SQLite.dll 与 SQLite.Interop.dll 文件 放入mirai根目录的 jre\bin 中

- 其他框架用户:
> 下载 System.Data.SQLite.dll 与 SQLite.Interop.dll 文件 放在框架主体exe下即可

## 交流群
671467200

## 配置字段
```ini
;请不要直接把这段文本直接粘贴到文件内，需要什么字段就复制什么字段，警 惕 空 格
;路径均支持相对路径 相对于数据目录
;除了 CycleSwitch 以及 Interval 之外, 所有配置更改后立即生效
[Config]
ImageWidth=500 ;词云图片的宽度
ImageHeight=500 ;词云图片的高度
MaskPath=mask.png ;遮罩图片的路径 必须黑白
WordNum=50 ;最大词数量
Font=75W.ttf ;自定义字体路径 也可以是系统内字体名称
FilterWord=http|www ;过滤词，使用 | 分割
SendTmpMsg=词云合成中…… ;触发功能之后的提示文本，需要at用户请加上 <@>
MatchMode=0 ;指令的触发模式, 但只针对于下面两个可自定义的指令生效, 默认为正则模式. 0 => 正则, 指令必须符合正则的语法. 1 => 模糊匹配, 消息中包含指令将会触发. 2 => 完全匹配
TodayCloudOrder=^今[日|天]词云$ ;这是一个正则的例子, 在你不配置这个字段时, 默认也是这个
YesterdayCloudOrder=^昨[日|天]词云$
LastWeekCloudOrder=^上个?周词云$
WeekCloudOrder=^[这|本]个?周词云$
LastMonthCloudOrder=^上个?月词云$
MonthCloudOrder=^[这|本]个?月词云$
YearCloudOrder=^[这|本]个?年词云$
PersonalWeekOrder=^(我的)?(个人)?本周词云$
PersonalTodayOrder=^(我的)?(个人)?今[日|天]词云$
PersonalMonthOrder=^(我的)?(个人)?本月词云$
PersonalLastWeekOrder=^(我的)?(个人)?上个?周词云$
PersonalYesterdayOrder=^(我的)?(个人)?昨[日|天]词云$
PersonalLastMonthOrder=^(我的)?(个人)?上个?月词云$


[Cycle]
CycleSwitch=1 ;定时发送开关, 1 => 开启. 0 => 不开启. 不写默认为不开启
CycleText=晚安！今日共收到<num>个词汇，前三的词汇为:\n<content> ;词云图片发送前的前导文本, 不写不会发送, 使用<num>来表示记录了多少个词汇, 使用<content>来表示这里罗列权重最高的前三个词汇
CycleMode=1 ;定时发送时发送的词云类型. 1 => 今日词云. 0 => 昨日词云, 不写默认为昨日词云
CycleTime=1970-01-01T23:58:00 ;定时生效的时间, 仅小时与分钟生效, 且必须是两位, 比如07:58:00. 其余位置改了也没用, 但是这个格式必须保留. 不写默认12点触发
Interval=20000 ;时间判断周期, 单位ms, 请设定在10000-59000之间, 设定太小了会频繁打开此文件读取触发时间

; 白名单优先级高于黑名单, 也就是说, 在白名单与黑名单均启用的情况下, 最终生效的是白名单
; 均不开启的时候, 默认也是生效白名单
; 白名单, 在开启白名单模式下, 只有白名单内的群才会启用功能
[WhiteList]
Groups=89****846|644****97 ;开启功能的群号, 使用 | 分割, 未加入的群将不会记录消息以及触发指令
Switch=0

; 黑名单, 在开启黑名单模式下, 会跳过字段内的群号
; 广播词云时, 会获取群列表, 之后跳过黑名单的群进行广播
[BlackList]
Groups=89****846|644****97
Switch=1

; 使用|分割每个QQ号
; 拦截的QQ号不能触发功能、记录发言
[BlockList]
QQID=114514|1919810
```
