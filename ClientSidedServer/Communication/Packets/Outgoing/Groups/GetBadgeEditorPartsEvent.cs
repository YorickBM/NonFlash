using Retro.Communication.Packets.Outgoing.Groups;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class GetBadgeEditorPartsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgeEditorPartsComposer(
                RetroEnvironment.GetGame().GetGroupManager().BadgeBases,
                RetroEnvironment.GetGame().GetGroupManager().BadgeSymbols,
                RetroEnvironment.GetGame().GetGroupManager().BadgeBaseColours,
                RetroEnvironment.GetGame().GetGroupManager().BadgeSymbolColours,
                RetroEnvironment.GetGame().GetGroupManager().BadgeBackColours));
        }
    }
}
