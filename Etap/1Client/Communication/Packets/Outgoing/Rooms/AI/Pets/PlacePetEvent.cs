using System;
using System.Collections.Generic;
using Retro.Hotel.Rooms.AI;
using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Inventory.Pets;
using Retro.Hotel.Rooms.AI.Speech;
using log4net;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Retro.Communication.Packets.Incoming.Rooms.AI.Pets
{
    class PlacePetEvent : IPacketEvent
    {
        private static readonly ILog log = LogManager.GetLogger("Habbie.Communication.Packets.Incoming.Rooms.AI.Pets.PlacePetEvent");

        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if ((Room.AllowPets == 0 && !Room.CheckRights(Session, true)) || !Room.CheckRights(Session, true))
            {
                Session.SendMessage(new RoomErrorNotifComposer(1));
                return;
            }

            if (Room.GetRoomUserManager().PetCount > Convert.ToInt32(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("room.pets.placement_limit")))
            {
                Session.SendMessage(new RoomErrorNotifComposer(2));//5 = I have too many.
                return;
            }

            Pet Pet = null;
            if (!Session.GetHabbo().GetInventoryComponent().TryGetPet(Packet.PopInt(), out Pet))
                return;

            if (Pet == null)
                return;

            if (Pet.PlacedInRoom)
            {
                Session.SendNotification("This pet is already in the room!");
                return;
            }

            int X = Packet.PopInt();
            int Y = Packet.PopInt();

            if (!Room.GetGameMap().CanWalk(X, Y, false))
            {
                Session.SendMessage(new RoomErrorNotifComposer(4));
                return;
            }

            RoomUser OldPet = null;
            if (Room.GetRoomUserManager().TryGetPet(Pet.PetId, out OldPet))
            {
                Room.GetRoomUserManager().RemoveBot(OldPet.VirtualId, false);
            }

            Pet.X = X;
            Pet.Y = Y;

            Pet.PlacedInRoom = true;
            Pet.RoomId = Room.RoomId;

            List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
            RoomBot RoomBot = new RoomBot(Pet.PetId, Pet.RoomId, "pet", "freeroam", Pet.Name, "", Pet.Look, X, Y, 0, 0, 0, 0, 0, 0, ref RndSpeechList, "", 0, Pet.OwnerId, false, 0, false, 0);
            if (RoomBot == null)
                return;
            log.Info("1");

            Room.GetRoomUserManager().DeployBot(RoomBot, Pet);

            Pet.DBState = DatabaseUpdateState.NeedsUpdate;
            Room.GetRoomUserManager().UpdatePets();

            Pet ToRemove = null;
            if (!Session.GetHabbo().GetInventoryComponent().TryRemovePet(Pet.PetId, out ToRemove))
            {
                log.Error("Error whilst removing pet: " + ToRemove.PetId);
                return;
            }

            Session.SendMessage(new PetInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetPets()));
        }
    }
}
