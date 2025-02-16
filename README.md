# GithubConnect
[中文](README.md) | [English](README.en.md)
## 运作原理

此软件**仅修改host文件以及刷新DNS解析缓存**,没有用到任何*VPN软件(梯子，飞机，魔法)*<br>
在进入`https://github.com`域名时，有一个将**域名解析到ip地址**的过程，连接不上github就是因为这一步骤出的问题<br><br>

在用户点击**修复连接**按钮时，程序会在**hosts**文件新增一个`20.205.243.166 github.com`项，使`https://github.com`这个域名能解析到`20.205.243.166`。之后再调用控制台运行`ipconfig/flushdns`命令来刷新DNS解析缓存。<br><br><br>

***注意:此方法不能百分百连接成功，此方法仅针对偶尔能连接的情况。对于根本无法连接的情况请使用其他VPV软件***

## 运行库
此软件使用 **.NET Framework4.8** 框架开发，如没有安装.NET Framework框架，请按照程序指引安装.NET Framework框架<br>

## 附属文件
本程序为**单文件程序**，运行后会生成`config.ini`和`Log.log`文件。`config.ini`文件为软件的配置文件；`Log.log`文件用于存放程序运行日志，方便调试<br>

## 参考资料
[运作原理](https://blog.csdn.net/weixin_43804496/article/details/131475204)<br>