# WordCloud

## 介绍
词云插件, 使用了Jieba.Net以及[WordCloudSharp词云库](https://github.com/AmmRage/WordCloudSharp)

## 使用流程
1. 下载插件
2. 解压`jieba.zip`到数据目录下(`me.cqp.luohuaming.WordCloud`)
3. 挂机一段时间，待群内有足够多的消息
4. `今[日|天]词云` => 今天的词云
5. `昨[日|天]词云` => 昨天的词云
6. `词云 yyyy-M-d` => 任意日期的词云

## 数据库配置
- Mirai-Native框架用户:
> 下载 System.Data.SQLite.dll 与 SQLite.Interop.dll 文件 放入mirai根目录的 jre\bin 中

- 其他框架用户:
> 下载 System.Data.SQLite.dll 与 SQLite.Interop.dll 文件 放在框架主体exe下即可

## 配置字段
```ini
//路径均支持相对路径 相对于数据目录
[Config]
ImageWidth=500 //词云图片的宽度
ImageHeight=500 //词云图片的高度
MaskPath=mask.png //遮罩图片的路径 必须黑白
WordNum=50 //最大词数量
Font=75W.ttf //自定义字体路径 也可以是系统内字体名称
FilterWord=http|www //过滤词，使用 | 分割
SendTmpMsg=词云合成中…… //触发功能之后的提示文本
```
