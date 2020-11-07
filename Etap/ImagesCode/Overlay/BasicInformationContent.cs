using Etap;
using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Image = Util.Image;
using Font = Util.Font;
using Util.ButtonTypes;
using Etap.Engine.User;

namespace Overlay
{
    class BasicInformationContent
    {
        /* Icons */
        public Image DiamondIcon { get; set; }
        public Image CreditIcon { get; set; }
        public Image DucketIcon { get; set; }
        public Image HCIcon { get; set; }

        /* Buttons */
        public HoverButton HelpButton { get; set; }
        public HoverButton ExitButton { get; set; }
        public HoverButton SettingsButton { get; set; }

        /* Texts */
        public Font txtDiamond { get; set; }
        public Font txtCredit { get; set; }
        public Font txtDucket { get; set; }
        public Font txtHC { get; set; }

        /* Background */
        public TransparentBlackBox background { get; set; }

        /* Details */
        public Image imgLine { get; set; }
        public Image imgHCBackdrop { get; set; }

        /* Configuration */
        Vector2 offset;
        Vector2 offsetIcon;
        Vector2 offsetBorder;

        public BasicInformationContent(ContentManager content, int offsetX = 0, int offsetY = 0, String Folder = "")
        {
            //load icons
            DiamondIcon = new Image(content, Folder + "Menu/Icons/DiamondSmall", Vector2.Zero);
            CreditIcon = new Image(content, Folder + "Menu/Icons/CreditSmall", Vector2.Zero);
            DucketIcon = new Image(content, Folder + "Menu/Icons/DucketSmall", Vector2.Zero);
            HCIcon = new Image(content, Folder + "Menu/Icons/HCSmall", Vector2.Zero);

            //load buttons
            HelpButton = new HoverButton(content, Folder + "Menu/Buttons/HelpBtn", new Vector2i(1, 3), () => { Console.WriteLine("Help"); }, Color.White);
            ExitButton = new HoverButton(content, Folder + "Menu/Buttons/ExitBtn", new Vector2i(1, 3), () => { GameScreenManager.Instance.Quit = true; }, Color.White);
            SettingsButton = new HoverButton(content, Folder + "Menu/Buttons/SettingsBtn", new Vector2i(1, 3), () => { Console.WriteLine("Settings"); }, Color.White);

            //load Details
            imgLine = new Image(content, Folder + "Menu/HC/Divider", new Vector2(1, 55));
            imgHCBackdrop = new Image(content, Folder + "Menu/HC/BackdropHC", Vector2.Zero);

            //load fonts 
            txtDiamond = new Font(content, "Fonts/UbuntuRegular", "0", new Color(54, 177, 211));
            txtCredit = new Font(content, "Fonts/UbuntuRegular", "0", new Color(205, 167, 34));
            txtDucket = new Font(content, "Fonts/UbuntuRegular", "0", new Color(214, 134, 214));

            //Load Background
            background = new TransparentBlackBox(content);
            background.LoadContent(new Vector2(192, 71), false, true, true, true);

            //Configurate
            offset = new Vector2(offsetX, offsetY);
            offsetIcon = new Vector2(40, 4);
            offsetBorder = new Vector2(3, 0);
        }

        public void UnloadContent()
        {
            background.UnloadContent();
            DiamondIcon.UnloadContent();
            CreditIcon.UnloadContent();
            DucketIcon.UnloadContent();
            imgLine.UnloadContent();
            imgHCBackdrop.UnloadContent();
            HCIcon.UnloadContent();

            txtDiamond.UnloadContent();
            txtCredit.UnloadContent();
            txtDucket.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 offsets = offset + position;
            Vector2 offsetIcons = offsets + offsetIcon;

            background.Draw(spriteBatch, offsets, 0.8f);

            imgLine.Draw(spriteBatch, new Vector2(114, 6) + offsets, 0.81f);
            imgHCBackdrop.Draw(spriteBatch, new Vector2(61, 2) + offsets, 0.81f);

            DiamondIcon.Draw(spriteBatch, new Vector2(0, 0) + offsetIcons, 0.81f);
            CreditIcon.Draw(spriteBatch, new Vector2(0, 19) + offsetIcons, 0.81f);
            DucketIcon.Draw(spriteBatch, new Vector2(0, 38) + offsetIcons, 0.81f);
            HCIcon.Draw(spriteBatch, new Vector2(78, 13) + offsets, 0.81f);

            HelpButton.Draw(spriteBatch, new Vector2(120, 3) + offsets, 0.81f);
            ExitButton.Draw(spriteBatch, new Vector2(120, 24) + offsets, 0.81f);
            SettingsButton.Draw(spriteBatch, new Vector2(120, 45) + offsets, 0.81f);

            txtDiamond.Draw(spriteBatch, offsets + new Vector2(38 - txtDiamond.measureString().X, 6));
            txtCredit.Draw(spriteBatch, offsets + new Vector2(38 - txtCredit.measureString().X, 24));
            txtDucket.Draw(spriteBatch, offsets + new Vector2(38 - txtDucket.measureString().X, 44));
            //100 = HC Text
        }

        public void Update(GameTime gameTime)
        {
            background.Update(gameTime);
            HelpButton.Update(gameTime);
            ExitButton.Update(gameTime);
            SettingsButton.Update(gameTime);

            if (GameScreenManager.Instance.ClientID != -1)
            {
                User usr = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(GameScreenManager.Instance.ClientID).GetUser();
                if (usr != null)
                {
                    txtDiamond.SetText(usr.GetDiamonds());
                    txtCredit.SetText(usr.GetCredits());
                    txtDucket.SetText(usr.GetDuckets());
                }
            }
        }
    }
}
