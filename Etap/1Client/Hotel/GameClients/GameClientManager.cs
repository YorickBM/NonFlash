using Etap.Communication.ConnectionManager;
using Etap.Communication.Packets.Incoming;
using Etap.Communication.Packets.Outgoing;
using Etap.Engine.Room;
using Etap.Hotel.GameClients;
using Etap.ImagesCode;
using Etap.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Hotel.GameClients
{
    public class GameClientManager
    {
        public delegate void ClientDisconnected(GameClient client);
        public ConcurrentDictionary<int, GameClient> _clients;
        private Dictionary<int, GameClient> guides;
        private Dictionary<int, GameClient> alphas;
        private ConcurrentDictionary<int, GameClient> _userIDRegister;
        private ConcurrentDictionary<string, GameClient> _usernameRegister;
        public event ClientDisconnected OnClientDisconnect;

        public GameClientManager()
        {
            guides = new Dictionary<int, GameClient>();
            alphas = new Dictionary<int, GameClient>();
            _clients = new ConcurrentDictionary<int, GameClient>();
            _userIDRegister = new ConcurrentDictionary<int, GameClient>();
            _usernameRegister = new ConcurrentDictionary<string, GameClient>();
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
            //if (c == null || c.GetHabbo() == null) return;
            //if (online)
            //    guides[c.GetHabbo().Id] = c;
            //else
            //    guides.Remove(c.GetHabbo().Id);
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

        public void SendPacket(ServerPacket Packet, string fuse = "")
        {
            foreach (GameClient Client in this._clients.Values.ToList())
            {
                if (Client == null)
                    continue;

                if (!string.IsNullOrEmpty(fuse))
                {
                    //if (!Client.GetHabbo().GetPermissions().HasRight(fuse))
                        //continue;
                }

                Logger.Debug("Sending Packet");
                Client.SendPacket(Packet);
            }
        }

        public void SendMessage(ServerPacket Packet, string fuse = "")
        {
            foreach (GameClient Client in _clients.Values.ToList())
            {
                if (Client == null || Client.GetUser() == null)
                    continue;

                if (!string.IsNullOrEmpty(fuse))
                {
                    Logger.Debug("Checking User Rights");
                    //if (!Client.GetHabbo().GetPermissions().HasRight(fuse))
                        //continue;
                }

                Client.SendPacket(Packet);
            }
        }

        public void CreateAndStartClient(int clientID, ConnectionInformation connection)
        {
            Logger.Debug("Creating And Starting Client");
            GameClient Client = new GameClient(clientID, connection);
            if (_clients.TryAdd(Client.ConnectionID, Client))
                Client.StartConnection();
            else
                connection.Dispose();
        }

        public void DisposeConnection(int clientID)
        {
            if (!TryGetClient(clientID, out GameClient Client))
                return;

            if (Client != null)
            {

                if (OnClientDisconnect != null && Client.GetUser() != null)
                    OnClientDisconnect(Client);

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

            GameScreenManager.Instance.ClientID = userID;
            Console.WriteLine(GameScreenManager.Instance.ClientID + ", " + userID);
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
           
        }

        private void HandleTimeouts()
        {
           
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
