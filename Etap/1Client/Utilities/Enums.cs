using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Utilities
{
    public enum MenuType
    {
        Navigator,
        Inventory,
        Alert,
        Info,
        Help,
        Catalogus,
        UNKNOWN
    }

    public static class EnumUtil {
        public static string MenuTypeToString(MenuType type)
        {
            string stringType = "UNKNOWN";
            switch (type)
            {
                case MenuType.Navigator:
                    stringType = "Navigator";
                    break;
                case MenuType.Inventory:
                    stringType = "Inventory";
                    break;
                case MenuType.Alert:
                    stringType = "Alert";
                    break;
                case MenuType.Info:
                    stringType = "Info";
                    break;
                case MenuType.Help:
                    stringType = "Help";
                    break;
                case MenuType.Catalogus:
                    stringType = "Catalogus";
                    break;
            }

            return stringType;
        }
        public static MenuType StringToMenuType(string type)
        {
            MenuType EnumType = MenuType.UNKNOWN;
            switch (type)
            {
                case "Navigator":
                    EnumType = MenuType.Navigator;
                    break;
                case "Inventory":
                    EnumType = MenuType.Inventory;
                    break;
                case "Alert":
                    EnumType = MenuType.Alert;
                    break;
                case "Info":
                    EnumType = MenuType.Info;
                    break;
                case "Help":
                    EnumType = MenuType.Help;
                    break;
                case "Catalogus":
                    EnumType = MenuType.Catalogus;
                    break;
            }

            return EnumType;
        }
    }
}
