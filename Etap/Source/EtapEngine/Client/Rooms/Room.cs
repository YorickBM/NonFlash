using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etap
{
    public class Room : GameScreen
    {
        public int ID;

        public RoomSettings RoomSettings;
        public FloorPlan FloorPlan;

        public List<Furni> RoomFurnis;

        public Room(RoomSettings roomSettings, FloorPlan floorPlan, List<Furni> list) {
            RoomFurnis = list;
            RoomSettings = roomSettings;
            FloorPlan = floorPlan;
        }

        internal void CreateFloor()
        {
            string HeightMap = FloorPlan.heightmap;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
