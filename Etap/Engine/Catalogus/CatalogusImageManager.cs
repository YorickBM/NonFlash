using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Engine.Catalogus
{
    class CatalogusImageManager
    {
        ContentManager contentManager;

        Dictionary<int, Image> _icons;
        Dictionary<string, Image> _furniIcons;
        Dictionary<string, Image> _banners;
        Dictionary<string, Image> _teasers;

        public CatalogusImageManager(ContentManager content)
        {
            contentManager = content;

            _icons = new Dictionary<int, Image>();
            _furniIcons = new Dictionary<string, Image>();
            _banners = new Dictionary<string, Image>();
            _teasers = new Dictionary<string, Image>();
        }

        public void GetBanner(string id, out Image banner)
        {
            if (_banners.ContainsKey(id)) _banners.TryGetValue(id, out banner);
            else banner = RegsiterBanner(id);
        }

        public void GetFurniIcon(string furni, out Image furniIcon)
        {
            Image tmp;
            if (_furniIcons.ContainsKey(furni)) _furniIcons.TryGetValue(furni, out tmp);
            else tmp = RegisterFurniIcon(furni);

            furniIcon = tmp.Clone() as Image;
        }

        private Image RegisterFurniIcon(string furni)
        {
            try {
                Image img = new Image(contentManager, "Client/Items/" + furni + "/icon", Vector2.Zero);
                _furniIcons.Add(furni, img);
                return img.Clone() as Image;
            } catch {
                return RegisterFurniIcon(furni);
            }
        }

        private Image RegsiterBanner(string id)
        {
            try
            {
                Image img = new Image(contentManager, "catalogue/" + id, Vector2.Zero);
                _banners.Add(id, img);
                return img;
            }
            catch (Exception ex)
            {
                try
                {
                    Image img;
                    id = "catalog_frontpage_headline_shop_GENERAL";
                    if (_banners.ContainsKey(id)) _banners.TryGetValue(id, out img);
                    else
                    {
                        img = new Image(contentManager, "catalogue/" + id, Vector2.Zero);
                        _banners.Add(id, img);
                    }
                    return img;
                }catch(Exception ex1)
                {
                    return RegsiterBanner(id);
                }
            }
            //img.SetColor(new Color(37, 85, 103, 200));
        }
    }
}
