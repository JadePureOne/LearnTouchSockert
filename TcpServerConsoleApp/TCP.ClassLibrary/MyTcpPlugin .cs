using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace TCP.ClassLibrary
{
    //可以不继承IPlugin，而是继承PluginBase，这样可以省去很多不必要的方法实现。
    public class MyTcpPlugin : PluginBase, ITcpConnectingPlugin, ITcpConnectedPlugin, ITcpClosingPlugin, ITcpClosedPlugin, ITcpReceivedPlugin
    {
        private readonly ILog _logger;
        public int Count { get; set; }

        public MyTcpPlugin(ILog logger)
        {
            this._logger = logger;
        }

        public async Task OnTcpConnecting(ITcpSession client, ConnectingEventArgs e)
        {
            //if (client.IP == "127.0.0.1")
            //{
            //    e.IsPermitOperation = false;
            //    e.Message = "拒绝本地连接";
            //    Console.WriteLine($"拒绝本地连接");
            //}
            await e.InvokeNext().ConfigureAwait(false);
        }

        public async Task OnTcpConnected(ITcpSession client, ConnectedEventArgs e)
        {
            Console.WriteLine($"客户端{client.IP}连接成功");
            await e.InvokeNext().ConfigureAwait(false);
        }

        //有客户端正在断开连接，只有当主动断开时才有效。
        public async Task OnTcpClosing(ITcpSession client, ClosingEventArgs e)
        {
            Console.WriteLine($"客户端{client.IP}断开中...");
            await e.InvokeNext().ConfigureAwait(false);
        }

        public async Task OnTcpClosed(ITcpSession client, ClosedEventArgs e)
        {
            Console.WriteLine($"客户端{client.IP}断开连接");
            await e.InvokeNext().ConfigureAwait(false);
        }

        //这里处理数据接收
        public async Task OnTcpReceived(ITcpSession client, ReceivedDataEventArgs e)
        {
            ByteBlock byteBlock = e.ByteBlock;
            var text = byteBlock.Span.ToString(Encoding.UTF8);

            IRequestInfo requestInfo = e.RequestInfo;

            //如果是server
            if (client is ITcpSessionClient sessionClient)
            {
                //从客户端收到消息
                _logger.Info($"已从客户端{client.IP}接收到信息：{text}");

                //回发
                await sessionClient.SendAsync(Encoding.UTF8.GetBytes($"服务器已收到你的消息：{text}"));
            }
            //如果是client
            else if (client is ITcpClient tcpClient)
            {
                //从服务端收到消息
                _logger.Info($"已从服务器{client.IP}接收到信息：{text}");
            }
            await e.InvokeNext().ConfigureAwait(false);
        }
    }
}