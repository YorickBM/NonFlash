using Retro.Communication.Packets.Outgoing.Moderation;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.Rooms;


namespace Retro.Communication.Packets.Incoming.Rooms.Action
{
    class AmbassadorWarningMessageEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int UserId = Packet.PopInt();
            int RoomId = Packet.PopInt();
            int Time = Packet.PopInt();
            string HotelName = RetroEnvironment.HotelName;

            Room Room = Session.GetHabbo().CurrentRoom;
            RoomUser Target = Room.GetRoomUserManager().GetRoomUserByHabbo(RetroEnvironment.GetUsernameById(UserId));
            if (Target == null)
                return;

            long nowTime = RetroEnvironment.CurrentTimeMillis();
            long timeBetween = nowTime - Session.GetHabbo()._lastTimeUsedHelpCommand;
            if (timeBetween < 60000)
            {
                Session.SendMessage(RoomNotificationComposer.SendBubble("Abuso", "Wacht minimaal 1 minuut om opnieuw een melding te verzenden.", ""));
                return;
            }

            else
                RetroEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("advice", "" + Session.GetHabbo().Username + " sturde zo net een alert naar " + Target.GetClient().GetHabbo().Username + ", klik hier om er heen te gaan.", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
            Target.GetClient().SendMessage(new BroadcastMessageAlertComposer("<b><font size='15px' color='#c40101'> " + HotelName + " waarschuwing!<br></font></b>Je hebt de " + HotelName + " regels overtreden. Gelieve je aan te passen of het " + HotelName + " team zal verdere sanctie's moeten ondernemen."));

            Session.GetHabbo()._lastTimeUsedHelpCommand = nowTime;
        }
    }
}
