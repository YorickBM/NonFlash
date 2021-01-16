using Etap.Communication.Packets.Outgoing.Navigator;
using Etap.Communication.Packets.Outgoing.Rooms.Connection;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Etap.Engine.Room
{
    class RoomManager
    {
        public List<Meubi> meubis;
        public Dictionary<int, Meubi> idSortedMeubis;
        public Floor floorDesign;
        public Tile door;

        public RoomManager(ContentManager content)
        {
            meubis = new List<Meubi>();
            idSortedMeubis = new Dictionary<int, Meubi>();
            floorDesign = null;
            door = null;
        }

        public void PrintTiles()
        {
            //Console.WriteLine(floor.Count + " <");
            //IEnumerable<Coordinate> tileCoords = (from tile in floor.Values select tile.GetCoordinate());
            //tileCoords.ToList().ForEach(s => Console.WriteLine(s));
        }
        public void SetFloor()
        {
            floorDesign = FloorGenerator.GetFloor();
        }
        public void SetDoor(Coordinate cord, int doorDirection)
        {
            door = new Tile(cord, TileType.DOOR, doorDirection);
        }

        public void UpdateRoomSettings(RoomSettingType type, int value)
        {
            switch(type)
            {
                case RoomSettingType.ACCES:
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                if (isInRoom())
                {
                    Vector2 offset = new Vector2(-140, 20);
                    if (floorDesign != null)
                        floorDesign.Draw(spriteBatch, offset);
                    foreach (Meubi meubi in idSortedMeubis.Values)
                    {
                        meubi.Draw(spriteBatch, offset);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void UnloadContent()
        {
            if (meubis.Count > 0)
            {
                IEnumerable<Meubi> tileCoords = (from tile in idSortedMeubis.Values select tile);
                tileCoords.ToList().ForEach(s => s.UnloadContent());
            }
            if(floorDesign != null) floorDesign.UnloadContent();
        }

        internal void UpdateMeubi(Coordinate cord, int itemId, int spriteId, int rotation)
        {
            Meubi meubi = GetMeubiByCoord(cord);
            if (meubi == null)
                return;

            Logger.Debug("Updating furniture");

            if(!idSortedMeubis.ContainsKey(itemId))
                idSortedMeubis.Add(itemId, meubi);
            meubi.SetItemId(itemId);
            meubi.SetBaseItem(GameScreenManager.Instance.GetFurniManager().RegisterFurni(spriteId).Clone() as BaseItem);
            meubi.SetRotationState(rotation);
        }
        internal void SetMeubiInteractionState(int itemId, int interactionState)
        {
            Meubi meubi;
            if(GetMeubiByItemId(itemId, out meubi))
            {
                meubi.SetInteractionState(interactionState);
            } else
            {
                Logger.DebugWarn(itemId, " not found!");
            }
        }
        internal void SetMeubiRotationState(int itemId, int rotationState)
        {
            Meubi meubi;
            if (GetMeubiByItemId(itemId, out meubi))
            {
                meubi.SetRotationState(rotationState);
            }
            else
            {
                Logger.DebugWarn(itemId, " not found!");
            }
        }

        public bool GetMeubiByItemId(int itemId, out Meubi meubi)
        {
            return idSortedMeubis.TryGetValue(itemId, out meubi);
        }
        public Meubi GetMeubiByCoord(Coordinate cord)
        {
           foreach(Meubi meubi in meubis)
            {
                if (meubi.GetCoordinate() == cord)
                    return meubi;
            }
            return null;
        }
        public void AddMeubi(Coordinate cord)
        {
            meubis.Add(new Meubi(cord));
        }

        public bool UpdateCoordByXZ(int x, int z, Coordinate newCord)
        {
            if (newCord.Y > 0)
            {
                foreach (Meubi meubi in meubis)
                {
                    if(meubi.GetCoordinate() == new Coordinate(x, 0, z))
                    {
                        meubi.SetCoordinate(newCord);
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool inRoom = false;
        public bool isInRoom() { return inRoom; }
        public void ExitRoom() {  
            inRoom = false; 
            GameScreenManager.Instance.GetInventoryManager().Close(); 
        }
        public void EnterSucces() { inRoom = true; }
        public void EnterRoom(int roomId, int acceslevel, string passowrd) {
            if (acceslevel == 0) {
                ExitRoom();

                //foreach (KeyValuePair<Coordinate, Tile> pair in tiles)
                //    cleanUps.Add(pair.Key, pair.Value);
                meubis.Clear();

                RetroEnvironment.GetGame().GetClientManager().SendPacket(new OpenFlatConnectionEvent(roomId, ""));
            }
        }
    }

    class RoomSettings
    {

    }

    class Coordinate
    {
        public float X, Y, Z;
        public Coordinate(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector2 twoDPosition()
        {
            float x = (X * 32) + (Z * 32);
            float y = 400 + (X * 16) - (Z * 16) - (28 * Y);

            return new Vector2(x, y);
        }
        public float Depth()
        {
            return (0.2f + (Y * 0.01f) + (X * 0.01f));
        }

        public static implicit operator string(Coordinate cord) {
            return "(" + cord.X + ";" + cord.Y + ";" + cord.Z + ")";
        }
        public static bool operator ==(Coordinate cord, Coordinate cord1)
        {
            return cord.X == cord1.X && cord.Y == cord1.Y && cord.Z == cord1.Z;
        }
        public static bool operator !=(Coordinate cord, Coordinate cord1)
        {
            return cord.X != cord1.X | cord.Y != cord1.Y | cord.Z != cord1.Z;
        }
    }

    public enum RoomSettingType { ACCES, HIDE_WALLS, WALL_THICKNESS, FLOOR_THICKNESS, IS_OWNER};
    public enum TileType { TILE, DOOR };
}
