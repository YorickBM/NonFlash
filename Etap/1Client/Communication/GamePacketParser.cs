using System;
using Etap.Communication.ConnectionManager;
using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Incoming;
using Etap.Utilities;
using System.IO;
using log4net;

namespace Etap.Communication
{
    public class GamePacketParser : IDataParser
    {
        private static readonly ILog log = LogManager.GetLogger("Habbie.Communication.GamePacketParser");
        public delegate void HandlePacket(ClientPacket message);
        private readonly GameClient currentClient;
        private ConnectionInformation con;
        private bool _halfDataRecieved = false;
        private byte[] _halfData = null;
        private bool _deciphered = false;

        public GamePacketParser(GameClient me)
        {
            currentClient = me;
        }

        public void handlePacketData(byte[] Data)
        {
            try
            {
                if (currentClient.RC4Client != null && !_deciphered)
                {
                    Logger.Warn("Trying to Decrypt!!!!");
                    currentClient.RC4Client.Decrypt(ref Data);
                    _deciphered = true;
                }

                if (_halfDataRecieved)
                {
                    byte[] FullDataRcv = new byte[_halfData.Length + Data.Length];
                    Buffer.BlockCopy(_halfData, 0, FullDataRcv, 0, _halfData.Length);
                    Buffer.BlockCopy(Data, 0, FullDataRcv, _halfData.Length, Data.Length);

                    _halfDataRecieved = false; // mark done this round
                    handlePacketData(FullDataRcv); // repeat now we have the combined array

                    Logger.ColorMsg(ConsoleColor.Magenta, "Returning....", "_halfDataRecieved");
                    return;
                }

                using (BinaryReader Reader = new BinaryReader(new MemoryStream(Data)))
                {
                    if (Data.Length < 4)
                    {
                        Logger.ColorMsg(ConsoleColor.Magenta, "Returning....", "Data.Length < 4");
                        return;
                    }
                    int MsgLen = HabboEncoding.DecodeInt32(Reader.ReadBytes(4));
                    Logger.DebugWarn(MsgLen, " <- Bytes count");

                    if ((Reader.BaseStream.Length - 4) < MsgLen)
                    {
                        _halfData = Data;
                        _halfDataRecieved = true;

                        Logger.ColorMsg(ConsoleColor.Magenta, "Returning....", "_halfDataRecieved = true");
                        return;
                    }
                    else if (MsgLen < 0) //|| MsgLen > 5120
                    {
                        Logger.ColorMsg(ConsoleColor.Magenta, "Returning....", "MsgLen < 0");
                        return;
                    }

                    byte[] Packet = Reader.ReadBytes(MsgLen);
                    int H = 0;
                    using (BinaryReader R = new BinaryReader(new MemoryStream(Packet)))
                    {
                        int Header = HabboEncoding.DecodeInt16(R.ReadBytes(2));
                        H = Header;

                        byte[] Content = new byte[Packet.Length - 2];
                        Buffer.BlockCopy(Packet, 2, Content, 0, Packet.Length - 2);

                        ClientPacket Message = new ClientPacket(Header, Content);
                        OnNewPacket.Invoke(Message);

                        _deciphered = false;
                    }

                    if (Reader.BaseStream.Length - 4 > MsgLen)
                    {
                        byte[] Extra = new byte[Reader.BaseStream.Length - Reader.BaseStream.Position];
                        Buffer.BlockCopy(Data, (int)Reader.BaseStream.Position, Extra, 0, (int)(Reader.BaseStream.Length - Reader.BaseStream.Position));

                        _deciphered = true;
                        handlePacketData(Extra);
                        Logger.ColorMsg(ConsoleColor.Magenta, "Retrying to handlePacket", "Reader.BaseStream.Length - 4 > MsgLen", H);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("Packet Error!", e);
            }
        }

        public void Dispose()
        {
            OnNewPacket = null;
            GC.SuppressFinalize(this);
        }

        public object Clone()
        {
            return new GamePacketParser(currentClient);
        }

        public event HandlePacket OnNewPacket;

        public void SetConnection(ConnectionInformation con)
        {
            this.con = con;
            OnNewPacket = null;
        }
    }
}