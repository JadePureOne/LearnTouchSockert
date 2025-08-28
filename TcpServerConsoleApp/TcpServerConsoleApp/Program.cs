using TCP.ClassLibrary;
using TouchSocket.Core;
using TouchSocket.Sockets;

var tcpService = new TcpService();

await tcpService.SetupAsync(new TouchSocketConfig()//配置
    .SetListenIPHosts("127.0.0.1:7789")
    .ConfigureContainer(c =>
    {
        c.AddConsoleLogger();//添加日志服务
    })
    .ConfigurePlugins(p =>
    {
        p.Add<MyTcpPlugin>();

        #region 注册的三种方式

        //p.Add<MyTcpPlugin>();//泛型注册，可以拿到容器内的东西

        //p.Add(new MyTcpPlugin(null)
        //{
        //    Count = 5
        //}); //直接注册实例,可以传参

        ////也可以这样委托注册,可以通过方法实现
        ////1。完整写法
        //p.Add(typeof(ITcpReceivedPlugin), async (ITcpSession client, ReceivedDataEventArgs e) =>
        //{
        //    await e.InvokeNext();
        //});
        ////2。简化写法
        //p.AddTcpReceivedPlugin(async (ITcpSession client, ReceivedDataEventArgs e) =>
        //{
        //    await e.InvokeNext();
        //});

        #endregion 注册的三种方式
    })).ConfigureAwait(false);

await tcpService.StartAsync().ConfigureAwait(false);

Console.WriteLine("服务器已启动，等待客户端连接...");

while (true)
{
    var input = Console.ReadLine();
    if (input == "exit")
    {
        break;
    }
}