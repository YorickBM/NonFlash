using Retro.Communication.Packets.Outgoing.Rooms.Avatar;
using Retro.Hotel.GameClients;
using Retro.Hotel.Quests;
using Retro.Hotel.Rooms;


namespace Retro.Communication.Packets.Incoming.Rooms.Avatar
{
    public class ActionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            int Action = Packet.PopInt();

            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (User.DanceId > 0)
                User.DanceId = 0;

            if (Session.GetHabbo().Effects().CurrentEffect > 0)
                Room.SendMessage(new AvatarEffectComposer(User.VirtualId, 0));

            User.UnIdle();
            Room.SendMessage(new ActionComposer(User.VirtualId, Action));

            if (Action == 5) // idle
            {
                User.IsAsleep = true;
                Room.SendMessage(new SleepComposer(User, true));
            }

            RetroEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_WAVE);
        }
    }
}