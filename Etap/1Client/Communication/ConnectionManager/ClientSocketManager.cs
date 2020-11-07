using System;
using System.Net;
using System.Net.Sockets;
using Etap.Communication.ConnectionManager.Socket_Exceptions;
using log4net;
using System.Collections.Concurrent;
using System.Text;
using Etap.Utilities;
using ClientSidedServer.Communication.ConnectionManager;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming.Handshake;
using Etap.Communication.Interfaces;
using Etap.Communication.Packets.Outgoing.Handshake;
using Etap.Communication.Packets.Incoming;
using ClientSidedServer.Communication;
using Etap.Communication.Packets.Incoming.Misc;
using Etap.Communication.Packets.Outgoing;
using Etap.Communication.Packets.Incoming.Moderation;
using System.Threading;

namespace Etap.Communication.ConnectionManager
{
	public class ClientSocketManager : ISocketManager
	{
		private static readonly ILog log = LogManager.GetLogger("Habbie.Communication.ConnectionManager");

		#region declares
		/// <summary>
		///     Indicates if connections should be accepted or not
		/// </summary>
		private bool _acceptConnections;

		/// <summary>
		///     Indicates the amount of accepted connections.
		/// </summary>
		private int _acceptedConnections;

		/// <summary>
		///     The Socket used for incoming data requests.
		/// </summary>
		private Socket connectionListener;

        private bool disableNagleAlgorithm;

		/// <summary>
		///     Contains the max conenctions per ip count
		/// </summary>
		private int maxIpConnectionCount;

		/// <summary>
		///     The maximum amount of connections the server should be allowed to have
		/// </summary>
		private int maximumConnections;

        /// <summary>
		///     Occurs when a new connection was established
		/// </summary>
		public event ConnectionEvent connectionEvent;

        private IDataParser parser;

		/// <summary>
		///     The port information, contains the nummeric value the socket should listen on.
		/// </summary>
		private int portInformation;

		/// <summary>
		/// Contains the ip's and their connection counts
		/// </summary>
		//private Dictionary<string, int> ipConnectionCount;
		private ConcurrentDictionary<string, int> _ipConnectionsCount;

        /// <summary>
        /// Contains your computer IpAddress
        /// </summary>
        private IPAddress ipAddr;
        #endregion

        #region initializer

        /// <summary>
        ///     Initializes the connection instance
        /// </summary>
        /// <param name="portID">The ID of the port this item should listen on</param>
        /// <param name="maxConnections">The maximum amount of connections</param>
        public override void Init(int portID, IDataParser parser, bool disableNaglesAlgorithm, int maxConnections = 0, int connectionsPerIP = 0)
		{
			this._ipConnectionsCount = new ConcurrentDictionary<string, int>();

			Logger.Debug(RetroEnvironment.GetConfig().data["game.tcp.bindip"].Trim());
            IPHostEntry ipHost = Dns.GetHostEntry(RetroEnvironment.GetConfig().data["game.tcp.bindip"].Trim()); //HARDCODED
            ipAddr = ipHost.AddressList[0];

            this.parser = parser;
			disableNagleAlgorithm = disableNaglesAlgorithm;
			portInformation = portID;
			prepareConnectionDetails();
        }

		/// <summary>
		///     Prepares the socket for connections
		/// </summary>
		private void prepareConnectionDetails()
		{
			connectionListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				NoDelay = disableNagleAlgorithm
			};
        }

        /// <summary>
        ///     Initializes the incoming data requests
        /// </summary>
        /// 
        private ConnectionInformation ci;
        public override void initializeConnectionRequests()
		{
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, portInformation);
            //Out.writeLine("Starting to listen to connection requests", Out.logFlags.ImportantLogLevel);
            try
			{
                Logger.Info("Establising Connection to", localEndPoint.ToString());
                connectionListener.Connect(localEndPoint);
            }
			catch
			{
                Logger.Error("Failed to establish connection");
				destroy();
			}
            ConnectionInformation c = new ConnectionInformation(1, connectionListener, this, parser.Clone() as IDataParser, ipAddr.ToString());
            ci = c;
        }
        public override ConnectionInformation getConnectionInformation()
        {
            if(ci == null)
            {
                Logger.Error("No ConnectionInformation Found!?");
            }
            return ci;
        }

        public override bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        #endregion

        #region destructor

        /// <summary>
        ///     Destroys the current connection manager and disconnects all users
        /// </summary>
        public override void destroy()
		{
			_acceptConnections = false;
			try { connectionListener.Close(); }
			catch { }
			connectionListener = null;
		}

		#endregion

		#region connection request

		/// <summary>
		///     Handels a new incoming data request from some computer from arround the world
		/// </summary>
		/// <param name="iAr">the IAsyncResult of the connection</param>
		private void newConnectionRequest(IAsyncResult iAr)
		{
            Logger.Debug("newConnectionRequest");
			if (connectionListener != null)
			{
				if (_acceptConnections)
				{
					try
					{
                        Logger.Debug("Getting Connection Socket");
                        Socket replyFromComputer = ((Socket)iAr.AsyncState).EndAccept(iAr);
						replyFromComputer.NoDelay = disableNagleAlgorithm;

						string Ip = replyFromComputer.RemoteEndPoint.ToString().Split(':')[0];

						ConnectionInformation c = new ConnectionInformation(1, replyFromComputer, this, parser.Clone() as IDataParser, Ip);
						c.connectionChanged += c_connectionChanged;

						if (connectionEvent != null)
							connectionEvent(c);
                    }
					catch
					{
                        Logger.Error("Could not setup recieving socket");
					}
					finally
					{
						connectionListener.BeginAccept(newConnectionRequest, connectionListener);
					}
				}
				else
				{
				}
			}
		}

		private void c_connectionChanged(ConnectionInformation information, ConnectionState state)
		{
			if (state == ConnectionState.CLOSED)
			{
				reportDisconnect(information);
			}
		}

		#endregion

		#region connection disconnected

		/// <summary>
		///     Reports a gameconnection as disconnected
		/// </summary>
		/// <param name="gameConnection">The connection which is logging out</param>
		public override void reportDisconnect(ConnectionInformation gameConnection)
		{
			gameConnection.connectionChanged -= c_connectionChanged;
			//activeConnections.Remove(gameConnection.getConnectionID());
		}

        #endregion

    }
}