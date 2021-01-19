using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            floorDesign = new Floor(GameScreenManager.Instance.GetContentManager());
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
        private Image mouseImg;
        private ContentManager _content;
        private Coordinate mouseLocation;

        public Floor(ContentManager content)
        {
            tiles = new List<Tile>();
            mouseImg = new Image(content, "Client/Floor/mouse", Vector2.Zero);
            _content = content;
        }

        public void AddTile(Tile tile) { tiles.Add(tile); }
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            foreach (Tile floorTile in tiles)
                floorTile.GetTexture().Draw(spriteBatch, floorTile.GetCoordinate().twoDPosition() + offset, 0.1f);

            if(GetMouseTile(offset) != null)
                mouseImg.Draw(spriteBatch, GetMouseTile(offset).GetCoordinate().twoDPosition() + offset, 0.11f);
        }
        public void UnloadContent()
        {
            foreach (Tile tile in tiles)
                tile.UnloadContent();
            mouseImg.UnloadContent();

            tiles.Clear();
        }

        public bool InTileRange(Vector2 vec1, Vector2 vec2)
        {
            //TODO: Advanced point calculation

            double distance = Vector2.Distance(vec1, vec2);
            if (distance < 25) return true;
            return false;
        }
        public Tile GetMouseTile(Vector2 offset)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            foreach (Tile tile in tiles)
            {
                Vector2 tilePos = tile.GetCoordinate().twoDPosition();
                int width = tile.GetTexture().GetTexture().Width;
                int height = tile.GetTexture().GetTexture().Height;

                Vector2 tileCenter = new Vector2(tilePos.X + (width / 2), tilePos.Y + (height / 2));
                if (InTileRange(tileCenter, (mousePoint.ToVector2() - offset))) return tile;
            }
            return null;
        }
        public Coordinate PointToTileCoordinate(Point pnt, Vector2 offset)
        {
            foreach (Tile tile in tiles)
            {
                Vector2 tilePos = tile.GetCoordinate().twoDPosition();
                int width = tile.GetTexture().GetTexture().Width;
                int height = tile.GetTexture().GetTexture().Height;

                Vector2 tileCenter = new Vector2(tilePos.X + (width / 2), tilePos.Y + (height / 2));
                double distance = Vector2.Distance(tileCenter, (pnt.ToVector2() - offset));

                if (InTileRange(tileCenter, (pnt.ToVector2() - offset))) return tile.GetCoordinate();
            }
            return null;
        }
    }

    class Tile
    {
        TileType type;
        Coordinate location;
        int dir = 0;
        Image texture;

        public Tile(ContentManager content, Coordinate loc, TileType tType, int doorDir = -1)
        {
            location = loc;
            type = tType;
            dir = doorDir;
            try
            {
                texture = new Image(content, "Client/Floor/center", Vector2.Zero);
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
