using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Yorick.Core.Communication
{
    public interface IConnectionStateListener
    {
        void connectionInit(string host, int port);
        void messageReceived(string arg1);
        void messageSent(string arg1);
        void messageParseError(IMessageDataWrapper arg1);
    }
}
