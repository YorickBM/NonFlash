using Etap.Engine.Room;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    class NavRoomSection : ICombinable
    {
        //TODO: Soms zwart onderaan...
        //TODO: Users In Room
        //TODO: Save Collapsed voor sessie...
        //TODO: Info button
        //TODO: Twee Buttons onderaan

        private List<Image> images;
        private List<Font> fonts;
        private Vector2i size, localPosition;

        private List<Image> bodyT, headerT;
        private List<Font> bodyF, headerF;

        private Image collapsed, open, backdrop, Info;
        private List<Row> rows;

        private NavCategory category;
        private Timer delay;
        private int rowNum = 0;
        public int BodySize;

        public NavRoomSection(ContentManager content, NavCategory category, Vector2i position, Vector2i size, out int bodySize)
        {
            images = new List<Image>();
            fonts = new List<Font>();
            bodyT = new List<Image>();
            headerT = new List<Image>();
            bodyF = new List<Font>();
            headerF = new List<Font>();
            rows = new List<Row>();

            this.size = size;
            this.localPosition = position;
            this.category = category;
            delay = new Timer(200);
            delay.Elapsed += Delay_Elapsed;

            #region Section Body

            rowNum = 0;
            Logger.Debug("Generating Body for:", category.Name);
            foreach(RoomData room in category.Rooms)
            {
                try
                {
                    Row row = new Row(content, rowNum++, position, size, room);
                    row.hidden = category.isCollapsed;
                    bodyF.AddRange(row.GetFonts());
                    bodyT.AddRange(row.GetImages());
                    rows.Add(row);
                }
                catch (Exception ex)
                {
                    Logger.Error("Following room could not be displayed:", room.Name, "(" + room.RoomId + ")");
                    Logger.Error(ex);
                }
            }

            #endregion

            #region Section Header
            Logger.Debug("Generating header for:", category.Name);

            Font title = new Font(content, "Fonts/Navigator/SectionTitle", category.Name, new Color(15, 85, 123)); //125, 216, 246
            title.SetPosition(position + new Vector2i(23, size.Y / 2 - title.measureString().Y / 2));
            headerF.Add(title);

            backdrop = new Image(content, "Menu/navigator/Section/title", new Vector2i(size.X, 29));
            backdrop.SetPosition(position);
            if (!category.isCollapsed) backdrop.resize((int)backdrop.Size.X, 29 + rowNum * 20);
            headerT.Add(backdrop);

            collapsed = new Image(content, "Menu/navigator/Section/openClose", new Vector2i(10, 9));
            collapsed.SourceRect = new Rectangle((int)0 + (int)collapsed.Size.X, (int)0, (int)collapsed.Size.X, (int)collapsed.Size.Y);
            collapsed.SetPosition(position + new Vector2i(6, size.Y / 2 - collapsed.dimensions.Y / 2));

            open = new Image(content, "Menu/navigator/Section/openClose", new Vector2i(10, 9));
            open.SetPosition(position + new Vector2i(6, size.Y / 2 - open.dimensions.Y / 2));

            if (!category.isCollapsed) headerT.Add(open);
            else headerT.Add(collapsed);

            #endregion

            UploadTextures();
            bodySize = rowNum * 20;
            BodySize = bodySize;
        }

        internal void UpdatePosition(Vector2i position, out int bodySize)
        {
            this.localPosition = position;
            bodySize = 0;
            backdrop.SetPosition(position);
            collapsed.SetPosition(position + new Vector2i(6, size.Y / 2 - collapsed.dimensions.Y / 2));
            Font title = headerF[0];
            title.SetPosition(position + new Vector2i(23, size.Y / 2 - title.measureString().Y / 2));
            open.SetPosition(position + new Vector2i(6, size.Y / 2 - open.dimensions.Y / 2));

            int rowNum = 0;
            UpdateRows(rowNum, position, size);

            if (!category.isCollapsed) bodySize = BodySize;
            UploadTextures();
        }

        private void UpdateRows(int rowNum, Vector2i position, Vector2i size)
        {
            BodyCleanUp();
            foreach (Row row in rows)
            {
                row.CleanUp();
                row.Update(rowNum++, position, size);
                if(!category.isCollapsed) bodyF.AddRange(row.GetFonts());
                if (!category.isCollapsed) bodyT.AddRange(row.GetImages());

                if (category.isCollapsed) row.hidden = true;
                else row.hidden = false;
            }
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }

        public void BodyCleanUp()
        {
            //foreach (Image img in bodyT) img.UnloadContent();
            //foreach (Font fnt in bodyF) fnt.UnloadContent();
            bodyT.Clear();
            bodyF.Clear();
        }
        public void CleanUp()
        {
            //foreach (Image img in images) img.UnloadContent();
            //foreach (Font fnt in fonts) fnt.UnloadContent();
            images.Clear();
            fonts.Clear();
        }

        internal void UploadTextures()
        {
            CleanUp();

            images.AddRange(headerT);
            if (!category.isCollapsed) images.AddRange(bodyT);
            fonts.AddRange(headerF);
            if (!category.isCollapsed) fonts.AddRange(bodyF);
        }

        public void UnloadContent()
        {
            foreach(SectionButton btn in images)
                btn.UnloadContent();
        }

        private bool isClicked = false;
        public void Update(GameTime gameTime, Vector2i offset, ref ScrollView view)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle(localPosition.X + offset.X, localPosition.Y + offset.Y, size.X, 29);
            var rectangleV = new Rectangle(view.GetPosition().X, view.GetPosition().Y, view.GetViewSize().X, view.GetViewSize().Y);

            if (rectangle.Contains(mousePoint) && rectangleV.Contains(mousePoint))
            {
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                isClicked = false;
            }

            if (isClicked)
            {
                if (!delay.Enabled)
                {
                    category.isCollapsed = !category.isCollapsed;
                    delay.Enabled = true;
                }
            }

            foreach (Row row in rows) row.LogisticUpdate(gameTime, offset, localPosition, size, ref view);

            #region Image Updator on Category Settings
            if (!category.isCollapsed)
            {
                if (headerT.Contains(collapsed)) headerT.Remove(collapsed);
                if (!headerT.Contains(open)) view.texturesChanged = true;
                if (!headerT.Contains(open))
                {
                    headerT.Remove(backdrop);
                    backdrop.resize((int)backdrop.Size.X, 29 + rowNum * 20);
                    headerT.Add(backdrop);

                    view.texturesChanged = true;
                    headerT.Add(open);
                    UploadTextures();
                }
            }
            else
            {
                if (headerT.Contains(open)) headerT.Remove(open);
                if (!headerT.Contains(collapsed)) {
                    headerT.Remove(backdrop);
                    backdrop.resize((int)backdrop.Size.X, 29);
                    headerT.Add(backdrop);

                    view.texturesChanged = true;
                    headerT.Add(collapsed);
                    UploadTextures();
                }
            }
            #endregion
        }

        public Image[] GetImages()
        {
            return images.ToArray();
        }
        public Font[] GetFonts()
        {
            return fonts.ToArray();
        }
        public Vector2i GetSize()
        {
            return size;
        }
    }

    class Row : ICombinable
    {
        List<Image> images;
        List<Font> fonts;

        private Image edgeL, edgeR, row, UsersInRoom, Info, Group, Lock, usersIcon;
        private Font roomName, users;
        private RoomData room;

        private Timer timer;
        private int rowN;
        public bool hidden;

        public Row(ContentManager content, int rowNum, Vector2i position, Vector2i size, RoomData room)
        {
            images = new List<Image>();
            fonts = new List<Font>();
            this.room = room;
            timer = new Timer(200);
            timer.Elapsed += Timer_Elapsed;
            rowN = rowNum;
            hidden = true;

            edgeL = new Image(content, "Menu/navigator/Section/rowEdge", new Vector2i(3, 20));
            edgeR = new Image(content, "Menu/navigator/Section/rowEdge", new Vector2i(3, 20), 1, 0, SpriteEffects.FlipHorizontally);
            row = new Image(content, "Menu/navigator/Section/row", new Vector2i(-5 + size.X - edgeL.Size.X - edgeR.Size.X, 20));
            UsersInRoom = new Image(content, "Menu/navigator/Section/inRoomCount", new Vector2i(40, 18));
            Info = new Image(content, "Menu/navigator/Section/InfoButton", new Vector2i(18, 18));
            Group = new Image(content, "Menu/navigator/Section/hasGroup", new Vector2i(13, 11));
            Lock = new Image(content, "Menu/navigator/Section/lock", new Vector2i(13, 15));
            usersIcon = new Image(content, "Menu/navigator/Section/userCount", new Vector2i(7, 8));
            roomName = new Font(content, "Fonts/Navigator/RoomnameTable", room.Name, Color.Black);
            users = new Font(content, "Fonts/Navigator/UsersNow", room.UsersNow + "", Color.White);

            int offsetPosX = 4;
            int offsetPosY = (20 * rowNum) + (int)29;

            int offsetX = 0;
            int offsetY = 0;

            bool isOdd = rowNum % 2 != 0;
            if (isOdd) offsetY += 20;

            edgeL.SetPosition(position + new Vector2i(offsetPosX, offsetPosY));
            edgeL.SourceRect = new Rectangle(0 + offsetX, 0 + offsetY, (int)edgeL.Size.X, (int)edgeL.Size.Y);
            images.Add(edgeL);

            edgeR.SetPosition(position + new Vector2i(size.X - edgeR.Size.X - 1, offsetPosY));
            edgeR.SourceRect = new Rectangle(0 + offsetX, 0 + offsetY, (int)edgeR.Size.X, (int)edgeR.Size.Y);
            images.Add(edgeR);

            row.SetPosition(position + new Vector2i(offsetPosX + edgeL.dimensions.X, offsetPosY));
            row.SourceRect = new Rectangle(0 + offsetX, 0 + offsetY, (int)row.Size.X, (int)row.Size.Y);
            images.Add(row);

            roomName.SetPosition(position + new Vector2i(offsetPosX + 42, offsetPosY + row.Size.Y / 2 - roomName.measureString().Y / 2));
            fonts.Add(roomName);

            float textIconWidth = usersIcon.Size.X + 7 + users.measureString().X;
            int offsetRightIcon = (int)Math.Ceiling((UsersInRoom.Size.X - textIconWidth) / 2);
            int percentageInRoom = (int)Math.Ceiling((double)(room.UsersNow / room.UsersMax) * 100);
            int multiplicationFromPercentage = 0;
            if (room.UsersNow > 0) multiplicationFromPercentage = 1;
            if (percentageInRoom > 40) multiplicationFromPercentage = 2;
            if (percentageInRoom > 90) multiplicationFromPercentage = 3;

            UsersInRoom.SetPosition(position + new Vector2i(offsetPosX, offsetPosY + 1));
            UsersInRoom.SourceRect = new Rectangle((int)(UsersInRoom.Size.X * multiplicationFromPercentage), 0, (int)UsersInRoom.Size.X, (int)UsersInRoom.Size.Y);
            images.Add(UsersInRoom);

            users.SetPosition(position + new Vector2i(offsetRightIcon + offsetPosX + 6 + usersIcon.Size.X, offsetPosY - (usersIcon.Size.Y / 2) + (users.measureString().Y / 2)));
            fonts.Add(users);

            usersIcon.SetPosition(position + new Vector2i(offsetRightIcon + offsetPosX, offsetPosY - (usersIcon.Size.Y / 2) + (UsersInRoom.Size.Y / 2)));
            images.Add(usersIcon);

            Info.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - edgeR.Size.X, offsetPosY + 1));
            images.Add(Info);

            Group.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - edgeR.Size.X, offsetPosY + 5));
            if (room.Group != null) images.Add(Group);

            if (room.Acces == 1)
            {
                Lock.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - 4 - Lock.Size.X - edgeR.Size.X, offsetPosY + 2));
                Lock.SourceRect = new Rectangle((int)(0 * Lock.Size.X), (int)position.Y, (int)Lock.Size.X, (int)Lock.Size.Y);
                images.Add(Lock);
            }
            else if (room.Acces == 2)
            {
                Lock.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - 4 - Lock.Size.X - edgeR.Size.X, offsetPosY + 2));
                Lock.SourceRect = new Rectangle((int)(1 * Lock.Size.X), (int)position.Y, (int)Lock.Size.X, (int)Lock.Size.Y);
                images.Add(Lock);
            }
            else if (room.Acces == 3)
            {
                Lock.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - 4 - Lock.Size.X - edgeR.Size.X, offsetPosY + 2));
                Lock.SourceRect = new Rectangle((int)(2 * Lock.Size.X), (int)position.Y, (int)Lock.Size.X, (int)Lock.Size.Y);
                images.Add(Lock);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
        }

        public void Update(int rowNum, Vector2i position, Vector2i size)
        {
            rowN = rowNum;
            int offsetPosX = 4;
            int offsetPosY = (20 * rowNum) + (int)29;

            int offsetX = 0;
            int offsetY = 0;

            bool isOdd = rowNum % 2 != 0;
            if (isOdd) offsetY += 20;

            edgeL.SetPosition(position + new Vector2i(offsetPosX, offsetPosY));
            edgeL.SourceRect = new Rectangle((int)0 + offsetX, 0 + offsetY, (int)edgeL.Size.X, (int)edgeL.Size.Y);
            images.Add(edgeL);

            edgeR.SetPosition(position + new Vector2i(size.X - edgeR.Size.X - 1, offsetPosY));
            edgeR.SourceRect = new Rectangle((int)0 + offsetX, 0 + offsetY, (int)edgeR.Size.X, (int)edgeR.Size.Y);
            images.Add(edgeR);

            row.SetPosition(position + new Vector2i(offsetPosX + edgeL.dimensions.X, offsetPosY));
            row.SourceRect = new Rectangle((int)0 + offsetX, 0 + offsetY, (int)row.Size.X, (int)row.Size.Y);
            images.Add(row);

            roomName.SetPosition(position + new Vector2i(offsetPosX + 42, offsetPosY + row.Size.Y / 2 - roomName.measureString().Y / 2));
            fonts.Add(roomName);

            users.SetText(room.UsersNow + "");
            float textIconWidth = usersIcon.Size.X + 7 + users.measureString().X;
            int offsetRightIcon = (int)Math.Ceiling((UsersInRoom.Size.X - textIconWidth) / 2);
            int percentageInRoom = (int)Math.Ceiling((double)(room.UsersNow / room.UsersMax) * 100);
            int multiplicationFromPercentage = 0;
            if (room.UsersNow > 0) multiplicationFromPercentage = 1;
            if (percentageInRoom > 40) multiplicationFromPercentage = 2;
            if (percentageInRoom > 90) multiplicationFromPercentage = 3;

            UsersInRoom.SetPosition(position + new Vector2i(offsetPosX, offsetPosY + 1));
            UsersInRoom.SourceRect = new Rectangle((int)(UsersInRoom.Size.X * multiplicationFromPercentage), 0, (int)UsersInRoom.Size.X, (int)UsersInRoom.Size.Y);
            images.Add(UsersInRoom);

            users.SetPosition(position + new Vector2i(offsetRightIcon + offsetPosX + 6 + usersIcon.Size.X, offsetPosY - (usersIcon.Size.Y / 2) + (users.measureString().Y / 2)));
            fonts.Add(users);

            usersIcon.SetPosition(position + new Vector2i(offsetRightIcon + offsetPosX, offsetPosY - (usersIcon.Size.Y / 2) + (UsersInRoom.Size.Y / 2)));
            images.Add(usersIcon);

            Info.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - edgeR.Size.X, offsetPosY + 1));
            images.Add(Info);

            Group.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - edgeR.Size.X, offsetPosY + 5));
            if (room.Group != null) images.Add(Group);

            if (room.Acces == 1)
            {
                Lock.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - 4 - Lock.Size.X - edgeR.Size.X, offsetPosY + 2));
                Lock.SourceRect = new Rectangle((int)(0 * Lock.Size.X), 0, (int)Lock.Size.X, (int)Lock.Size.Y);
                images.Add(Lock);
            }
            else if (room.Acces == 2)
            {
                Lock.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - 4 - Lock.Size.X - edgeR.Size.X, offsetPosY + 2));
                Lock.SourceRect = new Rectangle((int)(1 * Lock.Size.X), 0, (int)Lock.Size.X, (int)Lock.Size.Y);
                images.Add(Lock);
            }
            else if (room.Acces == 3)
            {
                Lock.SetPosition(position + new Vector2i(size.X - Info.Size.X - 4 - 4 - Group.Size.X - 4 - Lock.Size.X - edgeR.Size.X, offsetPosY + 2));
                Lock.SourceRect = new Rectangle((int)(2 * Lock.Size.X), 0, (int)Lock.Size.X, (int)Lock.Size.Y);
                images.Add(Lock);
            }
        }

        private bool isClicked = false;
        public void LogisticUpdate(GameTime gameTime, Vector2i offset, Vector2i position, Vector2i size, ref ScrollView view)
        {
            int offsetPosX = 4;
            int offsetPosY = (20 * rowN) + (int)29;

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle(position.X + offset.X + offsetPosX, position.Y + offset.Y + offsetPosY, size.X, (int)row.Size.Y);
            var rectangleV = new Rectangle(view.GetPosition().X, view.GetPosition().Y, view.GetViewSize().X, view.GetViewSize().Y);

            if (rectangle.Contains(mousePoint) && !hidden && rectangleV.Contains(mousePoint))
            {
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                isClicked = false;
            }

            if(isClicked)
            {
                if(!timer.Enabled)
                {
                    timer.Start();

                    Logger.Debug("Entering room: " + room.Name + " (" + room.RoomId + ")");
                    GameScreenManager.Instance.GetNavigatorManager().GoToRoom(room.RoomId, room.Acces);
                }
            }
        }

        public void CleanUp()
        {
            //foreach (Image img in images) img.UnloadContent();
            //foreach (Font fnt in fonts) fnt.UnloadContent();
            images.Clear();
            fonts.Clear();
        }

        public Image[] GetImages()
        {
            return images.ToArray();
        }
        public Font[] GetFonts()
        {
            return fonts.ToArray();
        }
    }
}
