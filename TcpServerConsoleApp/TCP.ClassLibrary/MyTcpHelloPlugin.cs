using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace TCP.ClassLibrary
{
    public class MyTcpHelloPlugin : PluginBase, ITcpReceivedPlugin
    {
        public async Task OnTcpReceived(ITcpSession client, ReceivedDataEventArgs e)
        {
            Console.WriteLine("HelloPlugin");
            var msg = e.ByteBlock.Span.ToString(Encoding.UTF8);

            if (msg.Equals("Hello", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"{this.GetType()}-处理");
                //当数据被处理后，如果不想传递给下一个插件，可以中止传播链。
                //1. return 2. e.Handled = true;
                return;
            }

            await e.InvokeNext().ConfigureAwait(false);
        }
    }
}