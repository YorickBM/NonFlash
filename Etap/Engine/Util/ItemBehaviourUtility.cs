using Etap.Communication.Packets.Incoming;
using Etap.ImagesCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Engine.Util
{
    static class ItemBehaviourUtility
    {
        public static object[] PopExtradata(ClientPacket packet, InteractionType type, int itemId)
        {
            object[] arr = new object[1];
            switch (type)
            {
                #region wired_score_board
                case InteractionType.wired_score_board:
                    int wired_score_board = packet.PopInt(); //0
                    int wired_score_board1 = packet.PopInt(); //6
                    string wired_score_board2 = packet.PopString(); //1
                    int inRoom = packet.PopInt();

                    int scoreType = packet.PopInt();
                    int count = packet.PopInt();
                    for(int i = 0; i < count; i++)
                    {
                        int score = packet.PopInt();
                        int wired_score_board3 = packet.PopInt(); //1
                        string userName = packet.PopString();
                    }

                    break;
                #endregion
                #region CAMERA_PICTURE
                case InteractionType.CAMERA_PICTURE:
                    
                    int CAMERA_PICTURE = packet.PopInt(); //0
                    int CAMERA_PICTURE1 = packet.PopInt(); //0
                    string jsonData = packet.PopString();
                    break;
                #endregion
                #region MUSIC_DISC
                case InteractionType.MUSIC_DISC:
                    int musicdata = packet.PopInt();
                    int MUSIC_DISC1 = packet.PopInt(); //1
                    string extraDataMusic = packet.PopString();
                    break;
                #endregion
                #region GUILD_STUFF
                case InteractionType.GUILD_ITEM:
                case InteractionType.GUILD_GATE:
                case InteractionType.GUILD_FORUM_CHAT:
                case InteractionType.GUILD_FORUM:
                    int GUILD_STUFF0 = packet.PopInt(); // 1 ? 0
                    int foundGroup = packet.PopInt(); // 0 ? 2
                    Console.Write(GUILD_STUFF0 + " | " + foundGroup);
                    if (foundGroup > 0)
                    {
                        int GUILD_STUFF1 = packet.PopInt(); //5
                        string extraDataGuild = packet.PopString();
                        string groupId = packet.PopString();
                        string badge = packet.PopString();
                        string colorCodeOne = packet.PopString();
                        string colorCodeTwo = packet.PopString();
                        Console.Write(GUILD_STUFF1 + " | " + groupId + " | " + colorCodeOne + " | " + colorCodeTwo);
                    } else
                    {
                        string extraDataGuild = packet.PopString();
                    }
                    Console.WriteLine("");
                    break;
                #endregion
                #region TERMINAL
                case InteractionType.BACKGROUND:
                   /* int BACKGROUND0 = packet.PopInt(); //0
                    int BACKGROUND1 = packet.PopInt(); //1
                    int BACKGROUND2 = packet.PopInt();
                    break;*/
                case InteractionType.TERMINAL:
                    int TERMINAL0 = packet.PopInt(); //0
                    int hasExtraData = packet.PopInt(); //1
                    int strings = packet.PopInt();
                    
                    for(int i = 0; i < (strings * 2); i++)
                    {
                        string extraDataString = packet.PopString();
                    }
                    break;
                #endregion
                #region GIFT
                case InteractionType.GIFT:
                    break;
                #endregion
                #region MANNEQUIN
                case InteractionType.MANNEQUIN:
                    int mannequin0 = packet.PopInt(); //0
                    int mannequin1 = packet.PopInt(); //1
                    int mannequin2 = packet.PopInt(); //3

                    string titleGender = packet.PopString();
                    string gender = packet.PopString();
                    string titleFigure = packet.PopString();
                    string figure = packet.PopString();
                    string titleName= packet.PopString();
                    string name = packet.PopString();

                    break;
                #endregion
                #region TONER
                case InteractionType.TONER:
                    int TONER0 = packet.PopInt(); // 0 ? 0
                    int TONER1 = packet.PopInt(); // 5 ? 0

                    if(TONER1 > 0)
                    {
                        int TONER2 = packet.PopInt(); //4

                        int enabled = packet.PopInt();
                        int hue = packet.PopInt();
                        int saturation = packet.PopInt();
                        int brightness = packet.PopInt();
                    } else
                    {
                        string TONER2 = packet.PopString(); //EMPTY
                    }

                    break;
                #endregion
                #region BADGE_DISPLAY
                case InteractionType.BADGE_DISPLAY:
                    int BADGE_DISPLAY0 = packet.PopInt(); //0
                    int BADGE_DISPLAY1 = packet.PopInt(); //2
                    int BADGE_DISPLAY2 = packet.PopInt(); //4

                    string BADGE_DISPLAY3 = packet.PopString(); //0
                    string badgeName = packet.PopString();
                    string owner = packet.PopString();
                    string date = packet.PopString();
                    break;
                #endregion
                #region TV
                case InteractionType.TELEVISION:
                    int TELEVISION0 = packet.PopInt(); //0
                    int TELEVISION1 = packet.PopInt(); //1
                    int TELEVISION2 = packet.PopInt(); //1

                    string TELEVISION3 = packet.PopString(); //THUMBNAIL_URL
                    string url = packet.PopString();
                    break;
                #endregion

                #region TRAX
                case InteractionType.TRAX:
                    break;
                #endregion

                #region LoveLock
                case InteractionType.LOVELOCK:
                    int LOVELOCK0 = packet.PopInt(); //0 ? 0
                    int LOVELOCK1 = packet.PopInt(); //2 ? 0


                    int dataCount = packet.PopInt();
                    for(int i = 0; i < dataCount; i++)
                    {
                        string dataS = packet.PopString();
                    }

                    break;
                #endregion
                #region Monster Seed
                case InteractionType.MONSTERPLANT_SEED:
                    int MONSTERPLANT_SEED0 = packet.PopInt(); //0
                    int MONSTERPLANT_SEED1 = packet.PopInt(); //1
                    int MONSTERPLANT_SEED2 = packet.PopInt(); //1

                    string monsterType = packet.PopString(); //rarity
                    string level = packet.PopString(); //1
                    break;
                #endregion

                default:
                    arr = new object[3];
                    arr[0] = packet.PopInt(); //1
                    arr[1] = packet.PopInt(); //0
                    arr[2] = packet.PopString();
                    return arr;
            }
            arr[0] = null;

            return arr;
        }
    }
}
