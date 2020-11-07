using System;
using ClientSidedServer.Communication.ConnectionManager;
using Etap.Core;

namespace Etap.Communication.ConnectionManager
{
    public class ConnectionHandling
	{
        private readonly ISocketManager manager;

        public ConnectionHandling(int port, int maxConnections, int connectionsPerIP, bool enabeNagles)
		{
			manager = new GameSocketManager();
			manager.Init(port, new InitialPacketParser(), !enabeNagles, maxConnections, connectionsPerIP);
		}
        public ConnectionHandling(int port, bool enabeNagles)
        {
            manager = new ClientSocketManager();
            manager.Init(port, new InitialPacketParser(), !enabeNagles);
        }

        public void Init()
		{
            manager.connectionEvent += manager_connectionEvent;
			manager.initializeConnectionRequests();
        }

        public void CreateClient()
        {
            ConnectionInformation connection = manager.getConnectionInformation();

            connection.connectionChanged += connectionChanged;
            RetroEnvironment.GetGame().GetClientManager().CreateAndStartClient(Convert.ToInt32(connection.getConnectionID()), connection);

        }

        private void manager_connectionEvent(ConnectionInformation connection)
		{
			connection.connectionChanged += connectionChanged;
            RetroEnvironment.GetGame().GetClientManager().CreateAndStartClient(Convert.ToInt32(connection.getConnectionID()), connection);
		}

		private void connectionChanged(ConnectionInformation information, ConnectionState state)
		{
			if (state == ConnectionState.CLOSED)
			{
				CloseConnection(information);
			}
		}

		private void CloseConnection(ConnectionInformation Connection)
		{
			try
			{
				Connection.Dispose();
                RetroEnvironment.GetGame().GetClientManager().DisposeConnection(Convert.ToInt32(Connection.getConnectionID()));
			}
			catch (Exception e)
			{
				ExceptionLogger.LogException(e);
			}
		}

		public void Destroy()
		{
			manager.destroy();
		}
	}
}