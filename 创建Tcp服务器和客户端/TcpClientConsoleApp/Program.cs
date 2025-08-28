using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

var tcpClient = new TcpClient();

tcpClient.Connecting = async (client, e) =>
{
    Console.WriteLine($"服务端{client.IP}连接中...");

    await Task.CompletedTask;
};

tcpClient.Connected = async (client, e) =>
{
    //客户端连接成功
    Console.WriteLine($"服务端{client.IP}连接成功");
    await Task.CompletedTask;
};

//有客户端正在断开连接，只有当主动断开时才有效。
tcpClient.Closing = (client, e) =>
{
    Console.WriteLine($"服务端{client.IP}断开中...");
    return EasyTask.CompletedTask;
};
tcpClient.Closed = (client, e) =>
{
    Console.WriteLine($"服务端{client.IP}断开连接");
    return EasyTask.CompletedTask;
};

tcpClient.Received = async (client, e) =>
{
    //从客户端收到信息
    var mes = e.ByteBlock.Span.ToString(Encoding.UTF8);

    Console.WriteLine($"已从{client.IP}接收到信息：{mes}");
    await Task.CompletedTask;
};

try
{
    await tcpClient.ConnectAsync("127.0.0.1:7789");//调用连接，当连接不成功时，会抛出异常。

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