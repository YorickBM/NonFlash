using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Etap.Engine.Room
{
    class FloorGenerator
    {
        private static Image center;
        private static bool initialized = false, started = false;
        private static Floor floorDesign;

        private static Stopwatch stopwatch;

        public static void Initialize()
        {
            center = new Image(GameScreenManager.Instance.GetContentManager(), "Client/Floor/center", Vector2.Zero);
            stopwatch = new Stopwatch();

            initialized = true;
        }

        public static void Start()
        {
            floorDesign = new Floor();
            started = true;
            stopwatch.Start();
        }

        public static void addTile(Coordinate coordinate)
        {
            try
            {
                if (!initialized && !started)
                    return;

                floorDesign.AddTile(new Tile(coordinate, TileType.TILE, center));
            }catch(Exception ex)
            {

            }
        }

        public static void Stop(bool print = false)
        {
            started = false;
            stopwatch.Stop();

            if(print) Logger.Debug("Time elapsed: ", stopwatch.Elapsed);
        }

        public static Floor GetFloor()
        {
            if (!initialized || started)
                return null;

            return floorDesign;
        }

    }

    class Floor
    {
        private List<Tile> tiles;

        public Floor()
        {
            tiles = new List<Tile>();
        }

        public void AddTile(Tile tile) { tiles.Add(tile); }
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            foreach (Tile floorTile in tiles)
                floorTile.GetTexture().Draw(spriteBatch, floorTile.GetCoordinate().twoDPosition() + offset, 0.1f);
        }
        public void UnloadContent()
        {
            tiles.Clear();
        }
    }

    class Tile
    {
        TileType type;
        Coordinate location;
        int dir = 0;
        Image texture;

        public Tile(Coordinate loc, TileType tType, int doorDir = -1)
        {
            location = loc;
            type = tType;
            dir = doorDir;
            try
            {
                texture = new Image(GameScreenManager.Instance.GetContentManager(), "Client/Floor/center", Vector2.Zero);
            }
            catch (Exception ex)
            {
                texture = null;
            }
        }
        public Tile(Coordinate loc, TileType tType, Image img, int doorDir = -1)
        {
            location = loc;
            type = tType;
            dir = doorDir;
            texture = img;
        }

        public void MakeDoor()
        {
            type = TileType.DOOR;
        }

        public void UnloadContent()
        {
            texture.UnloadContent();
        }
        public Image GetTexture()
        {
            return texture;
        }
        public Coordinate GetCoordinate() { return location; }
        public void SetCoordinate(Coordinate cord) { location = cord; }

        public TileType GetType() { return type; }
        public int GetDir() { return dir; }
    }
}
