using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace TcpServerConsoleApp
{
    //可以不继承IPlugin，而是继承PluginBase，这样可以省去很多不必要的方法实现。
    internal class MyTcpPlugin : PluginBase, ITcpReceivedPlugin
    {
        private readonly ILog _logger;

        public MyTcpPlugin(ILog logger)
        {
            this._logger = logger;
        }

        public int Count { get; set; }

        //这里处理数据接收
        public async Task OnTcpReceived(ITcpSession client, ReceivedDataEventArgs e)
        {
            //根据适配器类型，e.ByteBlock与e.RequestInfo会呈现不同的值，具体看文档=》适配器部分。
            ByteBlock byteBlock = e.ByteBlock;
            IRequestInfo requestInfo = e.RequestInfo;

            var text = byteBlock.Span.ToString(Encoding.UTF8);

            _logger.Info($"已从{client.IP}接收到信息：{text}");

            await e.InvokeNext();//如果本插件无法处理当前数据，请将数据转至下一个插件。
        }
    }
}