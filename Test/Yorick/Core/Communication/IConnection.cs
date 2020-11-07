using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Yorick.Core.Communication
{
    public interface IConnection
    {
        void dispose();
        bool disposed();
        bool init(String arg1, uint arg2 = 0);
        void timeout(int arg1);
        bool send(IMessageComposer arg1);
        bool sendUnencrypted(IMessageComposer arg1);
        void setEncryption(IEncryption arg1, IEncryption _arg_2);
        IEncryption getServerToClientEncryption();
        void registerMessageClasses(IMessageConfiguration arg1);
        void addMessageEvent(IMessageEvent arg1);
        void removeMessageEvent(IMessageEvent arg1);
        void processReceivedData();
        bool connected();
        void close();
        void isAuthenticated();
        void isConfigured();
        void createSocket();
    }
}
