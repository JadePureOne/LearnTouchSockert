using TcpServerConsoleApp;
using TouchSocket.Core;
using TouchSocket.Sockets;

var tcpService = new TcpService();

//tcpService.Connecting = async (client, e) =>
//{
//    //可进行连接验证，验证不通过可取消连接
//    if (client.IP == "127.0.0.1")
//    {
//        //拒绝本地连接
//        e.IsPermitOperation = false;
//        e.Message = "拒绝本地连接";
//        Console.WriteLine($"拒绝本地连接");
//    }
//    await Task.CompletedTask;
//};

tcpService.Connected = async (client, e) =>
{
    //客户端连接成功
    Console.WriteLine($"客户端{client.IP}连接成功");
    await Task.CompletedTask;
};

//有客户端正在断开连接，只有当主动断开时才有效。
tcpService.Closing = (client, e) =>
{
    Console.WriteLine($"客户端{client.IP}断开中...");
    return EasyTask.CompletedTask;
};
tcpService.Closed = (client, e) =>
{
    Console.WriteLine($"客户端{client.IP}断开连接");
    return EasyTask.CompletedTask;
};

await tcpService.SetupAsync(new TouchSocketConfig()//配置
    .SetListenIPHosts("127.0.0.1:7789")
    .ConfigureContainer(c =>
    {
        c.AddConsoleLogger();//添加日志服务
    })
    .ConfigurePlugins(p =>
    {
        p.Add<MyTcpPlugin>();//泛型注册，可以拿到容器内的东西
        p.Add(new MyTcpPlugin(null)
        {
            Count = 5
        }); //直接注册实例,可以传参

        //也可以这样委托注册
        //1。完整写法
        p.Add(typeof(ITcpReceivedPlugin), async (ITcpSession client, ReceivedDataEventArgs e) =>
        {
            await e.InvokeNext();
        });
        //2。简化写法
        p.AddTcpReceivedPlugin(async (ITcpSession client, ReceivedDataEventArgs e) =>
        {
            await e.InvokeNext();
        });
    }));

await tcpService.StartAsync();

//await tcpService.StartAsync(7789);//启动

Console.WriteLine("服务器已启动，等待客户端连接...");

while (true)
{
    var input = Console.ReadLine();
    if (input == "exit")
    {
        break;
    }
}