using Retro.Communication.Packets.Outgoing.HabboCamera;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.GameClients;
using Retro.Hotel.Items;
using Retro.Hotel.Rooms.Camera;
using Retro.Core;

namespace Retro.Communication.Packets.Incoming.HabboCamera
{
    class PurchaseCameraPictureEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int PictureBaseId = 202030;
            var conf = ExtraSettings.CAMERA_ITEMID;
            if (!int.TryParse(conf, out PictureBaseId))
            {
                Session.SendMessage(new RoomNotificationComposer("Praat met het ontwikkelaarsteam dat uw foto niet is geïdentificeerd in de db. \n Sorry voor het ongemak!", "error"));
                return;
            }
            RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CameraPhotoCount", 1);
            var pic = HabboCameraManager.GetUserPurchasePic(Session);
            ItemData ibase = null;
            if (pic == null || !RetroEnvironment.GetGame().GetItemManager().GetItem(PictureBaseId, out ibase))
                return;

            Session.GetHabbo().GetInventoryComponent().AddNewItem(0, ibase.Id, pic.Id.ToString(), 0, true, false, 0, 0);
            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            
            Session.SendMessage(new CamereFinishPurchaseComposer());
        }
    }
}
