using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using System;
using System.Collections.Generic;

namespace Retro.Communication.Packets.Incoming.Sound
{
    class SoundSettingsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int count = Packet.PopInt();

            for(int i = 0; i < count; i++)
            {
                int ClientVolume = Packet.PopInt();
            }

            bool chatPreference = Packet.PopBoolean();
            bool invitesStatus = Packet.PopBoolean();
            bool focusPreference = Packet.PopBoolean();
            int friendBarState = Packet.PopInt();

            int x0 = Packet.PopInt();
            int x1 = Packet.PopInt();
        }
    }
}