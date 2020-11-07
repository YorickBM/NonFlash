#region

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using Retro.Utilities;

#endregion

namespace Retro.Core
{
    class NotificationSettings
    {
        public static string NOTIFICATION_EVENT_IMG = "";
        public static string NOTIFICATION_ABOUT_IMG = "";
        public static string NOTIFICATION_FILTER_IMG = "";
        public static string NOTIFICATION_OLE_IMG = "";
        public static string NOTIFICATION_PUBLI_IMG = "";
        public static readonly ILog log = LogManager.GetLogger("Habbie.Core");

        public static bool RunNotiSettings()
        {
            if (!File.Exists("HabbieConfingThiago/Notifications/Config.ini"))
                return false;
            foreach (var @params in from line in File.ReadAllLines("HabbieConfingThiago/Notifications/Config.ini", Encoding.Default) where !String.IsNullOrWhiteSpace(line) && line.Contains("=") select line.Split('='))
            {
                switch (@params[0])
                {
                    case "notification.event.img":
                        NOTIFICATION_EVENT_IMG = @params[1];
                        break;
                    case "notification.about.img":
                        NOTIFICATION_ABOUT_IMG = @params[1];
                        break;
                    case "notification.filter.img":
                        NOTIFICATION_FILTER_IMG = @params[1];
                        break;
                    case "notification.da2.img":
                        NOTIFICATION_OLE_IMG = @params[1];
                        break;
                    case "notification.publi.img":
                        NOTIFICATION_PUBLI_IMG = @params[1];
                        break;
                }

            }
            Logger.Info("Notification Manager -> LOADED!");
            return true;
        }
    }
}