using System;
using System.Text;

using Retro.Hotel.Games;
using Retro.Communication.Packets.Outgoing.GameCenter;

namespace Retro.Communication.Packets.Incoming.GameCenter
{
    class JoinPlayerQueueEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            int GameId = Packet.PopInt();

            GameData GameData = null;
            if (RetroEnvironment.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData))
            {
                string SSOTicket = Session.GetHabbo().ssoTicket;

                Session.SendMessage(new JoinQueueComposer(GameData.GameId));
                Session.SendMessage(new LoadGameComposer(Session, GameData, SSOTicket));
            }
        }

        private string GenerateSSO(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
