using System;
using Etap.Communication.ConnectionManager;
using Etap.Communication;
using Etap.Core;
using Etap.Communication.Packets.Incoming;
using Etap.Communication.Interfaces;
using Etap.Communication.Encryption.Crypto.Prng;
using log4net;
using Etap.Utilities;
using System.Collections.Generic;
using Etap.Engine.User;
using Etap.Engine.Room;
using System.Linq;

namespace Etap.Hotel.GameClients
{
    public class GameClient
    {
        private readonly int _id;
        public string MachineId;
        private bool _disconnected;
        public ARC4 RC4Client = null;
        private GamePacketParser _packetParser;
        private ConnectionInformation _connection;
        public int PingCount { get; set; }
        internal int CurrentRoomUserId;
        internal DateTime TimePingedReceived;
        private static readonly ILog log = LogManager.GetLogger("Habbie.Hotel.Users");

        private User _user;
        private Dictionary<string, RoomData> _roomData;
        private Dictionary<string, string> _navNames;

        public void ResetRooms()
        {
            _roomData = new Dictionary<string, RoomData>();
            _navNames = new Dictionary<string, string>();
        }
        public void AddRoom(string searchCode, string cat, RoomData room)
        {
            if (!_navNames.ContainsKey(searchCode)) _navNames.Add(searchCode, cat);
            _roomData.Add(cat, room);
        }
        public RoomData GetRoomById(int id)
        {
            IEnumerable<RoomData> thing = (from room in this._roomData
                                           where room.Value.RoomId == id
                                           orderby room.Value.UsersNow descending
                                           select room.Value);
            return thing.First();
        }
        public RoomData GetRoomByCategory(int category)
        {
            IEnumerable<RoomData> thing = (from room in this._roomData
                                           where room.Value.Category == category
                                           orderby room.Value.UsersNow descending
                                           select room.Value);
            return thing.First();
        }

        public GameClient(int ClientId, ConnectionInformation pConnection)
        {
            Logger.Debug("GameClient (ClientId)-->", ClientId);
            _id = ClientId;
            _connection = pConnection;
            _packetParser = new GamePacketParser(this);

            PingCount = 0;
            ResetRooms();
        }

        private void SwitchParserRequest()
        {
            Logger.DebugWarn("Switching Parder Request Activated");
            _packetParser.SetConnection(_connection);
            _packetParser.OnNewPacket += Parser_OnNewPacket;
            byte[] data = (_connection.parser as InitialPacketParser).currentData;
            _connection.parser.Dispose();
            _connection.parser = _packetParser;
            _connection.parser.handlePacketData(data);
        }

        private void Parser_OnNewPacket(ClientPacket Message)
        {
            try
            {
                RetroEnvironment.GetGame().GetPacketManager().TryExecutePacket(this, Message);
            }
            catch (Exception e)
            {
                Logger.Error("Could not parse packet ->\r", e);
				ExceptionLogger.LogException(e);
			}
        }

        private void PolicyRequest()
        {
            _connection.SendData(RetroEnvironment.GetDefaultEncoding().GetBytes("<?xml version=\"1.0\"?>\r\n" +
                   "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                   "<cross-domain-policy>\r\n" +
                   "<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n" +
                   "</cross-domain-policy>\x0"));
        }


        public void StartConnection()
        {
            Logger.Debug("Starting Connection");

            if (_connection == null)
                return;

            PingCount = 0;

            (_connection.parser as InitialPacketParser).PolicyRequest += PolicyRequest;
            (_connection.parser as InitialPacketParser).SwitchParserRequest += SwitchParserRequest;
            _connection.startPacketProcessing();
        }

        public bool TryAuthenticate(string AuthTicket)
        {
            
			return true;
		}

		public void SendWhisper(string Message, int Colour = 0)
        {
            ///SendMessage(new WhisperComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }

        public void SendNotification(string Message)
        {
            ///SendMessage(new BroadcastMessageAlertComposer(Message));
        }

        public void LogsNotif(string Message, string Key)
        {
            ///SendMessage(new RoomNotificationComposer(Message, Key));
        }

        public void SendPacket(IServerPacket Message)
        {
            byte[] bytes = Message.GetBytes();

            if (Message == null)
                return;

            if (GetConnection() == null)
                return;

            Logger.Debug("Sending Data");
            GetConnection().SendData(bytes);
        }

        public void SendShout(string Message, int Colour = 0)
        {
            ///SendMessage(new ShoutComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }


        public int ConnectionID
        {
            get { return _id; }
        }

        public ConnectionInformation GetConnection()
        {
            return _connection;
        }

        public void Disconnect()
        {
            if (!_disconnected)
            {
                if (_connection != null)
                    _connection.Dispose();
                _disconnected = true;
            }
        }

        public void Dispose()
        {
            CurrentRoomUserId = -1;

            MachineId = string.Empty;
            _disconnected = true;
            _connection = null;
            RC4Client = null;
            _packetParser = null;
        }

        public User GetUser()
        {
            return _user;
        }
        public void SetUser(User user)
        {
            _user = user;
        }
    }
}