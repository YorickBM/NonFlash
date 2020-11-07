using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro.Communication.Packets.Outgoing.HabboCamera
{
    class CameraFinishParticipateCompetitionComposer : ServerPacket
    {
        public CameraFinishParticipateCompetitionComposer()
            : base(ServerPacketHeader.CameraFinishParticipateCompetitionMessageComposer)
        {
            base.WriteBoolean(true);
            base.WriteString("Teste");
        }
    }
}
