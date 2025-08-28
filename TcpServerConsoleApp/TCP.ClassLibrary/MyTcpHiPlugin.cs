using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace TCP.ClassLibrary
{
    public class MyTcpHiPlugin : PluginBase, ITcpReceivedPlugin
    {
        public async Task OnTcpReceived(ITcpSession client, ReceivedDataEventArgs e)
        {
            Console.WriteLine("HiPlugin");
            var msg = e.ByteBlock.Span.ToString(Encoding.UTF8);

            if (msg.Equals("Hi", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"{this.GetType()}-处理");
                return;
            }

            await e.InvokeNext().ConfigureAwait(false);
        }
    }
}