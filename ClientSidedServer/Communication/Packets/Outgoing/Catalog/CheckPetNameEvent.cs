using Retro.Communication.Packets.Outgoing.Catalog;
using Retro.Hotel.GameClients;


namespace Retro.Communication.Packets.Incoming.Catalog
{
    public class CheckPetNameEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string PetName = Packet.PopString();
            string word;
            if (PetName.Length < 2)
            {
                Session.SendMessage(new CheckPetNameComposer(2, "2"));
                return;
            }
            else if (PetName.Length > 15)
            {
                Session.SendMessage(new CheckPetNameComposer(1, "15"));
                return;
            }
            else if (!RetroEnvironment.IsValidAlphaNumeric(PetName))
            {
                Session.SendMessage(new CheckPetNameComposer(3, ""));
                return;
            }
            else if (RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(PetName, out word))
            {
                Session.SendMessage(new CheckPetNameComposer(4, "" + word));
                return;
            }

            Session.SendMessage(new CheckPetNameComposer(0, ""));
        }
    }
}