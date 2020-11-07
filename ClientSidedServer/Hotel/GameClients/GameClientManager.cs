using System;
using System.Collections.Generic;
using Retro.Communication.ConnectionManager;
using System.Linq;
using System.Collections.Concurrent;
using Retro.Communication.Packets.Outgoing;
using log4net;
using System.Collections;
using System.Diagnostics;
using Retro.Communication.Packets.Incoming;
using Retro.Utilities;

namespace Retro.Hotel.GameClients
{
    public class GameClientManager
    {
        public delegate void ClientDisconnected(GameClient client);
        private static readonly ILog log = LogManager.GetLogger("Habbie.Hotel.GameClients.GameClientManager");
        public ConcurrentDictionary<int, GameClient> _clients;
        private Dictionary<int, GameClient> guides;
        private Dictionary<int, GameClient> alphas;
        private ConcurrentDictionary<int, GameClient> _userIDRegister;
        private ConcurrentDictionary<string, GameClient> _usernameRegister;
        private readonly Queue timedOutConnections;
        private readonly Stopwatch clientPingStopwatch;
        public event ClientDisconnected OnClientDisconnect;

        public GameClientManager()
        {
            guides = new Dictionary<int, GameClient>();
            alphas = new Dictionary<int, GameClient>();
            _clients = new ConcurrentDictionary<int, GameClient>();
            _userIDRegister = new ConcurrentDictionary<int, GameClient>();
            _usernameRegister = new ConcurrentDictionary<string, GameClient>();

            timedOutConnections = new Queue();

            clientPingStopwatch = new Stopwatch();
            clientPingStopwatch.Start();
        }

        public void DispatchEventDisconnect(GameClient client)
        {
			OnClientDisconnect?.Invoke(client);
		}

        public void OnCycle()
        {
            TestClientConnections();
            HandleTimeouts();
            RetroEnvironment.GetGame().ClientManagerCycleEnded = true;
        }

        public GameClient GetClientByUserID(int userID)
        {
            if (_userIDRegister.ContainsKey(userID))
                return _userIDRegister[userID];
            return null;
        }

        internal Dictionary<int, GameClient> GetGuides()
        {
            return guides;
        }

        internal Dictionary<int, GameClient> GetAlphas()
        {
            return alphas;
        }

        internal void AddToAlphas(int id, GameClient client)
        {
            //visaUsers[id] = client;
            alphas[id] = client;
        }

        internal void ModifyGuide(bool online, GameClient c)
        {

        }

        public GameClient GetClientByUsername(string username)
        {
            if (_usernameRegister.ContainsKey(username.ToLower()))
                return _usernameRegister[username.ToLower()];
            return null;
        }

        public bool TryGetClient(int ClientId, out GameClient Client)
        {
            return _clients.TryGetValue(ClientId, out Client);
        }

        public bool UpdateClientUsername(GameClient Client, string OldUsername, string NewUsername)
        {
            if (Client == null || !_usernameRegister.ContainsKey(OldUsername.ToLower()))
                return false;

            _usernameRegister.TryRemove(OldUsername.ToLower(), out Client);
            _usernameRegister.TryAdd(NewUsername.ToLower(), Client);
            return true;
        }

        public void StaffAlert(ServerPacket Message, int Exclude = 0)
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer(Message, Key));
        }

        public void QuizzAlert(ServerPacket Message, int Exclude = 0) //, Item Item, Room room
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer(Message, Key));
        }

        public void ManagerAlert(ServerPacket Message, int Exclude = 0)
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer(Message, Key));

        }

        public void GroupChatAlert(ServerPacket Message, int Exclude = 0) // Group Group, 
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer(Message, Key));
        }

        public void GuideAlert(ServerPacket Message, int Exclude = 0)
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer(Message, Key), 0);
        }

        public void LogsNotif(string Message, string Key)
        {
            ///RetroEnvironment.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer(Message, Key), 0);
        }

        public void SendBubble(string Message, string Key)
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer(Message, Key));
        }

        public void ModAlert(string Message)
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer(Message, Key));
        }

        public void DoAdvertisingReport(GameClient Reporter, GameClient Target)
        {
            ///RetroEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer(Message, Key));
        }

        public void SendPacket(ServerPacket Packet, string fuse = "")
        {
            foreach (GameClient Client in this._clients.Values.ToList())
            {

                if (Client == null)
                    continue;

                if (!string.IsNullOrEmpty(fuse))
                {
                    //if (!Client.GetHabbo().GetPermissions().HasRight(fuse))
                       // continue;
                }

                Client.SendPacket(Packet);
            }
        }

        public void CreateAndStartClient(int clientID, ConnectionInformation connection, bool IsClient = false)
        {
            GameClient Client = new GameClient(clientID, connection);
            if (_clients.TryAdd(Client.ConnectionID, Client))
                Client.StartConnection();
            else
                connection.Dispose();

            if (IsClient) Client.SwitchParser();
        }

        public void DisposeConnection(int clientID)
        {
			if (!TryGetClient(clientID, out GameClient Client))
				return;

			if (Client != null)
            {
                Client.Dispose();
            }

            _clients.TryRemove(clientID, out Client);
        }

        public void LogClonesOut(int UserID)
        {
            GameClient client = GetClientByUserID(UserID);
            if (client != null)
                client.Disconnect();
        }

        public void RegisterClient(GameClient client, int userID, string username)
        {
            if (_usernameRegister.ContainsKey(username.ToLower()))
                _usernameRegister[username.ToLower()] = client;
            else
                _usernameRegister.TryAdd(username.ToLower(), client);

            if (_userIDRegister.ContainsKey(userID))
                _userIDRegister[userID] = client;
            else
                _userIDRegister.TryAdd(userID, client);
        }

        public void UnregisterClient(int userid, string username)
        {
			_userIDRegister.TryRemove(userid, out GameClient Client);
			_usernameRegister.TryRemove(username.ToLower(), out Client);
        }

        public void CloseAll()
        {
           
        }

        private void TestClientConnections()
        {
            if (clientPingStopwatch.ElapsedMilliseconds >= 30000)
            {
                clientPingStopwatch.Restart();

                try
                {
                    List<GameClient> ToPing = new List<GameClient>();

                    foreach (GameClient client in _clients.Values.ToList())
                    {
                        if (client.PingCount < 6)
                        {
                            client.PingCount++;

                            ToPing.Add(client);
                        }
                        else
                        {
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(client);
                            }
                        }
                    }

                    DateTime start = DateTime.Now;

                    foreach (GameClient Client in ToPing.ToList())
                    {
                        try
                        {
                            //Client.SendMessage(new PongComposer());
                        }
                        catch
                        {
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(Client);
                            }
                        }
                    }

                }
                catch
                {
                }
            }
        }

        private void HandleTimeouts()
        {
            if (timedOutConnections.Count > 0)
            {
                lock (timedOutConnections.SyncRoot)
                {
                    while (timedOutConnections.Count > 0)
                    {
                        GameClient client = null;

                        if (timedOutConnections.Count > 0)
                            client = (GameClient)timedOutConnections.Dequeue();

                        if (client != null)
                            client.Disconnect();
                    }
                }
            }
        }

        public int Count
        {
            get { return _clients.Count; }
        }

        public ICollection<GameClient> GetClients
        {
            get
            {
                return _clients.Values;
            }
        }
    }
}