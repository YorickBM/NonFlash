using Retro.Communication.Packets.Outgoing.HabboCamera;
using Retro.Hotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro.Communication.Packets.Incoming.HabboCamera
{
    class ParticipatePictureCameraCompetitionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new CameraFinishParticipateCompetitionComposer());
        }
    }
}
