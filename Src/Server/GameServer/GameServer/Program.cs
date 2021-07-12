using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using ProtoBuf;
using System.IO;
using Common;
using System.Threading;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建文件对象 导入 log for net 配置
            FileInfo fi = new System.IO.FileInfo("log4net.xml");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(fi);
            Log.Init("GameServer");
            Log.Info("Game Server Init end");

            //实例化游戏服务器对象
            GameServer server = new GameServer();
            server.Init(); //执行初始化
            server.Start(); //服务器启动
            Console.WriteLine("Game Server is Running......");
            //组件帮助类运行
            CommandHelper.Run();
            Log.Info("Game Server Exiting...");
            server.Stop(); //服务器停止
            Log.Info("Game Server Exited");
        }
    }
}
