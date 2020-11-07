using System;
using Retro.Communication.ConnectionManager;
using Retro.Communication;
using Retro.Core;
using System.Linq;
using System.Collections.Generic;
using Retro.Communication.Packets.Incoming;
using Retro.Communication.Interfaces;
using Retro.Communication.Encryption.Crypto.Prng;
using Retro.Communication.Packets.Outgoing;
using System.Data;
using log4net;
using Retro.Utilities;

namespace Retro.Hotel.GameClients
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

        public GameClient(int ClientId, ConnectionInformation pConnection)
        {
            Logger.Debug(ClientId);
            _id = ClientId;
            _connection = pConnection;
            _packetParser = new GamePacketParser(this);

            PingCount = 0;
        }

        private void SwitchParserRequest()
        {
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

        public void SwitchParser()
        {
            SwitchParserRequest();
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
    }
}