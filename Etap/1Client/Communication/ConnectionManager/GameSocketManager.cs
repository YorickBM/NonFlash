using System;
using System.Net;
using System.Net.Sockets;
using Etap.Communication.ConnectionManager.Socket_Exceptions;
using log4net;
using System.Collections.Concurrent;
using Etap.Utilities;
using ClientSidedServer.Communication.ConnectionManager;

namespace Etap.Communication.ConnectionManager
{
    public class GameSocketManager : ISocketManager
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

			this.parser = parser;
			disableNagleAlgorithm = disableNaglesAlgorithm;
			maximumConnections = maxConnections;
			portInformation = portID;
			maxIpConnectionCount = connectionsPerIP;
			prepareConnectionDetails();
			_acceptedConnections = 0;
			Logger.Info("Configration succesfull on port (" + portID + ")!");
            Logger.Info("Maximum connections per IP (" + connectionsPerIP + ")!");
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
			try
            {
                connectionListener.Bind(new IPEndPoint(IPAddress.Any, portInformation));
			}
			catch (SocketException ex)
			{
				throw new SocketInitializationException(ex.Message);
			}
		}

		/// <summary>
		///     Initializes the incoming data requests
		/// </summary>
		public override void initializeConnectionRequests()
		{
			//Out.writeLine("Starting to listen to connection requests", Out.logFlags.ImportantLogLevel);
			///connectionListener.Listen(100);
			_acceptConnections = true;

			try
			{
                Logger.Info("Establising Connection to {0}");
                connectionListener.Connect(IPAddress.Any, 30001);

                if(SocketConnected(connectionListener)) Logger.Info("Connection established");
                else Logger.Warn("Failed to establish connection");

                ///connectionListener.BeginAccept(newConnectionRequest, connectionListener);
			}
			catch
			{
                Logger.Error("Failed to establish connection");
				destroy();
			}
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
			if (connectionListener != null)
			{
				if (_acceptConnections)
				{
					try
					{
						Socket replyFromComputer = ((Socket)iAr.AsyncState).EndAccept(iAr);
						replyFromComputer.NoDelay = disableNagleAlgorithm;

						string Ip = replyFromComputer.RemoteEndPoint.ToString().Split(':')[0];

						int ConnectionCount = getAmountOfConnectionFromIp(Ip);
						if (ConnectionCount < maxIpConnectionCount)
						{
							_acceptedConnections++;
							ConnectionInformation c = new ConnectionInformation(_acceptedConnections, replyFromComputer, this, parser.Clone() as IDataParser, Ip);
							reportUserLogin(Ip);
							c.connectionChanged += c_connectionChanged;

							if (connectionEvent != null)
								connectionEvent(c);
						}
						else
						{
							Logger.Info("Connection denied from [" + replyFromComputer.RemoteEndPoint.ToString().Split(':')[0] + "]. Too many connections (" + ConnectionCount + ").");
						}
					}
					catch
					{
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
			reportUserLogout(gameConnection.getIp());
			//activeConnections.Remove(gameConnection.getConnectionID());
		}

		#endregion

		#region ip connection management

		/// <summary>
		///     reports the user with an ip as "logged in"
		/// </summary>
		/// <param name="ip">The ip of the user</param>
		private void reportUserLogin(string ip)
		{
			alterIpConnectionCount(ip, (getAmountOfConnectionFromIp(ip) + 1));
		}

		/// <summary>
		///     reports the user with an ip as "logged out"
		/// </summary>
		/// <param name="ip">The ip of the user</param>
		private void reportUserLogout(string ip)
		{
			alterIpConnectionCount(ip, (getAmountOfConnectionFromIp(ip) - 1));
		}

		/// <summary>
		///     Alters the ip connection count
		/// </summary>
		/// <param name="ip">The ip of the user</param>
		/// <param name="amount">The amount of connections</param>
		private void alterIpConnectionCount(string ip, int amount)
		{
			if (_ipConnectionsCount.ContainsKey(ip))
			{
				_ipConnectionsCount.TryRemove(ip, out int am);
			}
			_ipConnectionsCount.TryAdd(ip, amount);
		}

		/// <summary>
		///     Gets the amount of connections from 1 ip
		/// </summary>
		/// <param name="ip">The ip of the user</param>
		/// <returns>The amount of connections from the given ip address</returns>
		private int getAmountOfConnectionFromIp(string ip)
		{
			if (_ipConnectionsCount.ContainsKey(ip))
			{
				return _ipConnectionsCount[ip];
			}
			else
			{
				return 0;
			}
		}

        #endregion

        #region Not Needed For Server
        public override ConnectionInformation getConnectionInformation()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}