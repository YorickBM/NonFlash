using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Retro.Hotel.Users.Messenger.FriendBar;
using Retro.Communication.Packets.Outgoing.Sound;

namespace Retro.Communication.Packets.Incoming.Misc
{
    class SetFriendBarStateEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            Session.GetHabbo().FriendbarState = FriendBarStateUtility.GetEnum(Packet.PopInt());
            Session.SendMessage(new SoundSettingsComposer(Session.GetHabbo().ClientVolume, Session.GetHabbo().ChatPreference, Session.GetHabbo().AllowMessengerInvites, Session.GetHabbo().FocusPreference, FriendBarStateUtility.GetInt(Session.GetHabbo().FriendbarState)));
        }
    }
}
