using Retro.Hotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.Rooms.Camera;
using Retro.Communication.Packets.Outgoing.HabboCamera;

namespace Retro.Communication.Packets.Incoming.HabboCamera
{
    public class HabboCameraPictureDataEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var len = Packet.PopInt();
            var bytes = Packet.ReadBytes(len);//Not in use when MOD camera
            
            HabboCameraManager.GetUserPurchasePic(Session, true);
            HabboCameraManager.AddNewPicture(Session);
        }
    }
}
