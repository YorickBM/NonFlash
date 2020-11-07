using Retro.Communication.ConnectionManager;
using Retro.Communication.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSidedServer.Communication.ConnectionManager
{
    public abstract class ISocketManager
    {
        public delegate void ConnectionEvent(ConnectionInformation connection);
        public event ConnectionEvent connectionEvent;

        public abstract void Init(int portID, IDataParser parser, bool disableNaglesAlgorithm, int maxConnections = 0, int connectionsPerIP =  0);
        public abstract void initializeConnectionRequests();
        public abstract bool SocketConnected(Socket s);
        public abstract ConnectionInformation getConnectionInformation();

        public abstract void destroy();
        public abstract void reportDisconnect(ConnectionInformation gameConnection);
    }
}
