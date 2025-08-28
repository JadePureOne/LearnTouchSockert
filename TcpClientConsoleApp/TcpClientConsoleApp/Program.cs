using System.Text;
using TCP.ClassLibrary;
using TouchSocket.Core;
using TouchSocket.Sockets;

var tcpClient = new TcpClient();

try
{
    var config = new TouchSocketConfig();
    config.SetRemoteIPHost("127.0.0.1:7789");
    config.ConfigurePlugins(p =>
    {
        p.Add<MyTcpPlugin>();
    });
    config.ConfigureContainer(a =>
    {
        a.AddConsoleLogger();
    });

    await tcpClient.SetupAsync(config);

    Result result = await tcpClient.TryConnectAsync();//或者可以调用TryConnectAsync
    if (result.IsSuccess)
    {
        for (int i = 0; i < 10; i++)
        {
            await tcpClient.SendAsync(Encoding.UTF8.GetBytes($"这是第{i}条消息"));
            await Task.Delay(1000);
        }

        await tcpClient.CloseAsync();
        Console.ReadLine();
    }
}
catch (Exception)
{
    throw;
}