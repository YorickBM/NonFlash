using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap
{
    public class Furni
    {
        public int id; //id from database
        public string item_name, public_name; //furni name for image, name in cata + inventory + in room
        public string type; //What Type it is

        public string width, length; //width X tiles, length Y Tiles
        public string stack_height, can_stack; //Stack height for item ontop, if it can even have items ontop
        public string can_sit, is_walkable; //If it can sit on it, if user can walk over it;

        public string sprite_id; //ID in furnidata.XML (We probs SKIP and make string Path)
        public string allow_recycle, allow_trade, allow_marketplace_sell, allow_gift, allow_inventory_stack;

        public string interaction_type; //Interaction Type (For Events)
        public string interaction_modes_count; //Amount of Times you can interact before reset (If it has a use bttn)

        public string vending_ids; //Wich handitem they give So we need Vending Class
        public string height_adjustable; //Amount of height that gets added per interaction mode

        public string effect_id, wired_id; //To know wich effect to put on user, Wich wired out database wired it is.

        public string is_rare; // If its a rare item (Dont know why we need this :?)
        public string clothing_id; //If its a clothing item to get into clothing menu
        public string extra_rot; // ???
        public string behaviour_data; // ???

        private Image Image;

        public Furni()
        {
            Image = new Image(null);
            Image.Path = LoadedContent.Instance.FurniNoTexturePath;
            Image.LoadContent();
        }

        public Furni(int id, Dictionary<string, string> settings)
        {
            Image = new Image(null);
            Image.Path = LoadedContent.Instance.FurniNoTexturePath;

            this.id = id;
            settings.TryGetValue("item_name", out item_name);
            settings.TryGetValue("public_name", out public_name);
            settings.TryGetValue("type", out type);
            settings.TryGetValue("width", out width);
            settings.TryGetValue("length", out length);
            settings.TryGetValue("stack_height", out stack_height);
            settings.TryGetValue("can_stack", out can_stack);
            settings.TryGetValue("can_sit", out can_sit);
            settings.TryGetValue("is_walkable", out is_walkable);
            settings.TryGetValue("sprite_id", out sprite_id);
            settings.TryGetValue("allow_recycle", out allow_recycle);
            settings.TryGetValue("allow_trade", out allow_trade);
            settings.TryGetValue("allow_marketplace_sell", out allow_marketplace_sell);
            settings.TryGetValue("allow_gift", out allow_gift);
            settings.TryGetValue("allow_inventory_stack", out allow_inventory_stack);
            settings.TryGetValue("interaction_type", out interaction_type);
            settings.TryGetValue("interaction_modes_count", out interaction_modes_count);
            settings.TryGetValue("vending_ids", out vending_ids);
            settings.TryGetValue("height_adjustable", out height_adjustable);
            settings.TryGetValue("effect_id", out effect_id);
            settings.TryGetValue("wired_id", out wired_id);
            settings.TryGetValue("is_rare", out is_rare);
            settings.TryGetValue("clothing_id", out clothing_id);
            settings.TryGetValue("extra_rot", out extra_rot);
            settings.TryGetValue("behaviour_data", out behaviour_data);

            LoadFurni(item_name);
        }

        public void LoadFurni(string Texture)
        {
            if(!Image.Path.Equals(LoadedContent.Instance.FurniNoTexturePath)) {
                Image.Path = LoadedContent.Instance.FurniTexturePath + Texture;
                LoadContent();
            }
        }
        public void LoadContent()
        {
            Image.LoadContent();
        }
        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }

        private void ConvertToInt(string s, out int i) { i = Int32.Parse(s); }
        private void ConvertToFurniType(string s, out FurniType t) { EnumClass.FurniTypedict.TryGetValue(s, out t); }
    }
}
