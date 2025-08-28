using System.Text;
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

tcpService.Received = async (client, e) =>
{
    var byteBlock = e.ByteBlock; //收到的数据是字节块
    var bytes = byteBlock.ToArray(); //转换为字节数组
    var text = Encoding.UTF8.GetString(bytes); //转换为字符串

    //从客户端收到信息
    var mes = e.ByteBlock.Span.ToString(Encoding.UTF8);

    Console.WriteLine($"已从{client.IP}接收到信息：{mes}");

    //回复客户端
    await client.SendAsync($"服务器已收到：{byteBlock.Memory}");

    await Task.CompletedTask;
};

await tcpService.StartAsync(7789);//启动

Console.WriteLine("服务器已启动，等待客户端连接...");

while (true)
{
    var input = Console.ReadLine();
    if (input == "exit")
    {
        break;
    }
}