# SYSU MSClub Puzzle Server

## 简介

MSC Puzzle 是用于社团招新的趣味性解谜游戏，内容除古典密码、数字谜题等经典谜题外，还包含部分需要基础计算机知识、基础CTF知识以及部分基础算法知识解决的谜题。

游戏中的题目答案均为50字符以下的字符串，满足正则表达式`^msc{[a-zA-Z0-9_-]+}$`，示例：`msc{hello_world}`，答案区分大小写，且均为可读、有意义的英语单词组成，但有时可能会存在**不影响阅读的字符混淆**，如`hello`可写为`he1l0`，同时单词之间使用`_`分隔。

部分题目的结果可能仅为英语单词，请将其用`_`拼接后，包裹`msc{}`后提交。

## 注意事项

- 我们支持**交流**，有不会的题目欢迎在招新群、吹水群等交流平台互相讨论、交流，但**严禁直接发送题目答案**。
- 题目如有多解等情况，一遍会在题设中给出，取可读的结果为答案，如有其他问题，可咨询俱乐部管理层。
- 通关者会得到俱乐部招新**免除一次面试**的机会，并且可获得ACM、CTF等竞赛的着重培养机会。

俱乐部管理层对全部谜题保留最终解释权，如有问题欢迎咨询。
