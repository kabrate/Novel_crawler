c#爬取小说
====
心血来潮写的一个工具,用了HtmlAgilityPack来分析提取html代码。<br>
随便找了一个小说网站，经过分析，发现其代码都在如下位置<br>
![](image1.gif) <br> 
于是用HtmlAgilityPack得到了所有小说章节的地址,保存在List中。<br>
接下来依次访问对应的地址，保存章节题目和章节内容，整合后写入到文本中。<br>
![](image2.git) <br>