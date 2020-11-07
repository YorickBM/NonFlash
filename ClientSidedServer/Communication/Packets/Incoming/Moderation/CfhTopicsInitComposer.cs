using System.Linq;
using System.Collections.Generic;
using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Moderation
{
    class CfhTopicsInitComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int userActionCatagoryPresets = Packet.PopInt();
            Logger.DebugWarn("Cfh Topics Init Composer (Cats)->", userActionCatagoryPresets);

            for(int catPreset = 0; catPreset < userActionCatagoryPresets; catPreset++)
            {
                string catagory = Packet.PopString();
                int userActionPresets = Packet.PopInt();
                Logger.DebugWarn("Cfh Topics Init Composer (presets)->", catagory, userActionPresets);

                for (int preset = 0; preset < userActionPresets; preset++)
                {
                    string caption = Packet.PopString();
                    int id = Packet.PopInt();
                    string type = Packet.PopString();
                    Logger.DebugWarn("Cfh Topics Init Composer -->", id, caption, type);
                }
            }
        }
    }
}
