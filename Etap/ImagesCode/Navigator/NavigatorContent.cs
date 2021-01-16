using Etap;
using Etap.Communication.Packets.Outgoing.Navigator;
using Etap.Hotel.GameClients;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Util;
using Util.ButtonTypes;
using Util.Combiner;

namespace Engine.Navigator
{
    class NavigatorContent : CloseAndInfoUI
    {
        private Image hiddenLine, backdrop;
        private List<InMenuButton> menuButtons;
        private List<NavRoomSection> sections;
        private DropdownButton dropdown;
        private ScrollView view;
        private NavRoomSection[] navSections;
        private ContentManager updateContentVar;

        private Timer delay;

        //TODO: Reset view wnr tab wisselt
        //TODO: Reset tab wnr je menu opent
        //TODO: UI een extra 5px buffer onderaan?????
        //TODO: Als nav dicht is niet nog nav knoppen laten werken
        //TODO: Wnr dropdown open is knoppen onder niet laten werken
        //TODO: IsActive() Variable in gebruik stellen.

        public NavigatorContent(ContentManager content, Vector2i position, Vector2i size, int offsetX = 0, int offsetY = 0) : base(content, position, size, "Navigator", offsetX, offsetY)
        {
            hiddenLine = new Image(content, "Menu/Navigator/hiddenLine", Vector2.Zero);
            backdrop = new Image(content, "Menu/Navigator/backdrop", Vector2.Zero);

            int width = size.X - 2 * 8;
            width = width / 4;
            this.updateContentVar = content;

            delay = new Timer(400);
            delay.Elapsed += Delay_Elapsed;

            menuButtons = new List<InMenuButton>();
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect(); RetroEnvironment.GetGame().GetClientManager().SendPacket(new NavigatorSearchEvent("official_view", "")); }, "navigator.toplevelview.official_view"));
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect(); RetroEnvironment.GetGame().GetClientManager().SendPacket(new NavigatorSearchEvent("hotel_view", "")); }, "navigator.toplevelview.hotel_view"));
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect(); RetroEnvironment.GetGame().GetClientManager().SendPacket(new NavigatorSearchEvent("roomads_view", "")); }, "navigator.toplevelview.roomads_view"));
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect(); RetroEnvironment.GetGame().GetClientManager().SendPacket(new NavigatorSearchEvent("myworld_view", "")); }, "navigator.toplevelview.myworld_view"));
            menuButtons.First().SetActive();

            dropdown = new DropdownButton(content, 116, position, 
                new DropdownItem("navigator.filter.anything", ()=> { }),
                new DropdownItem("navigator.filter.room.name", () => { }),
                new DropdownItem("navigator.filter.owner", () => { }),
                new DropdownItem("navigator.filter.tag", () => { }),
                new DropdownItem("navigator.filter.group", () => { })
                );

            view = new ScrollView(content, new Vector2i(0, 0), new Vector2i(388, 356), "menu/Navigator/backdrop", null);
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }

        private bool isInitialized = false;
        private NavCategory[] identifyers = new NavCategory[0];
        public void Initialize(ContentManager content, string searchCode)
        {
            SectionCombiner combiner = new SectionCombiner();
            GameClient session = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(GameScreenManager.Instance.ClientID);
            identifyers = session.GetCategorysByIdentifyer(searchCode).ToArray();

            navSections = new NavRoomSection[identifyers.Length];
            this.sections = navSections.ToList();

            Vector2i offset = new Vector2i();
            for (int i = 0; i < identifyers.Length; i++)
            {
                int bodySize = 0;
                navSections[i] = new NavRoomSection(content, identifyers[i], offset, new Vector2i(view.GetViewSize().X, 29), out bodySize);
                offset += new Vector2i(0, navSections[i].GetSize().Y + 5);
                if (!identifyers[i].isCollapsed) offset += new Vector2i(0, bodySize);
            }

            view.SetContent(combiner.AddSections(navSections).GetImages());
            view.SetText(combiner.AddSections(navSections).GetFonts());
            isInitialized = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            hiddenLine.UnloadContent();
            backdrop.UnloadContent();
            foreach(InMenuButton menuButton in menuButtons)
                menuButton.UnloadContent();
            dropdown.UnloadContent();
            if(isInitialized) view.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (isInitialized && isOpen())
            {
                base.Update(gameTime);
                backdrop.resize(size.X - 6, size.Y - headerHeight - 33 - edgeBottom.dimensions.Y);
                backdrop.Update(gameTime);

                hiddenLine.resize(size.X - 6, hiddenLine.dimensions.Y);
                hiddenLine.Update(gameTime);

                foreach (InMenuButton menuButton in menuButtons)
                    menuButton.Update(gameTime);

                dropdown.SetPosition(position + offset + new Vector2i(14, 43 + headerHeight));
                dropdown.Update(gameTime);

                view.Update(gameTime, position + new Vector2i(14, headerHeight + 75));

                try
                {
                    foreach (NavRoomSection section in navSections)
                        section.Update(gameTime, view.GetPosition() - view.GetScrolled(), ref view);
                }catch
                {
                    Logger.Warn("Can not render a Navigator Body!! Soo loading???!");
                }

                if (view.texturesChanged)
                {
                    Vector2i offset = new Vector2i();
                    for ( int i = 0; i < navSections.Length; i++)
                    {
                        int bodySize = 0;
                        NavRoomSection section = navSections[i];
                        section.UpdatePosition(offset, out bodySize);
                        offset += new Vector2i(0, navSections[i].GetSize().Y + 5);
                        if (!identifyers[i].isCollapsed) offset += new Vector2i(0, bodySize);
                    }
                    SectionCombiner combiner = new SectionCombiner();
                    view.SetContent(combiner.AddSections(navSections).GetImages());
                    view.SetText(combiner.AddSections(navSections).GetFonts());
                    view.texturesChanged = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isOpen() && isInitialized)
            {
                backdrop.Draw(spriteBatch, (position + offset + new Vector2i(3, headerHeight + 33 + 1)), 0.9f);
                hiddenLine.Draw(spriteBatch, (position + offset + new Vector2i(3, headerHeight + 33)), 0.9f);
                base.Draw(spriteBatch);

                Vector2i specialOffset = new Vector2i(0, 0);
                foreach (InMenuButton menuButton in menuButtons)
                {
                    menuButton.Draw(spriteBatch, (specialOffset + position + offset + new Vector2i(1 + 8, headerHeight + 4)), 0.95f);
                    specialOffset = new Vector2i(specialOffset.X + menuButton.GetSize().X, specialOffset.Y);
                }

                dropdown.Draw(spriteBatch, 0.97f);
                view.Draw(spriteBatch, 0.96f);
            }
        }
    }
}
