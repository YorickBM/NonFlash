using System.Linq;
using System.Collections.Generic;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Moderation
{
    class CfhTopicsInitComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int userActionCatagoryPresets = Packet.PopInt();

            for(int catPreset = 0; catPreset < userActionCatagoryPresets; catPreset++)
            {
                string catagory = Packet.PopString();
                int userActionPresets = Packet.PopInt();

                for (int preset = 0; preset < userActionPresets; preset++)
                {
                    string caption = Packet.PopString();
                    int id = Packet.PopInt();
                    string type = Packet.PopString();
                }
            }
        }
    }
}
