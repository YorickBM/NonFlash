#region

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

#endregion

namespace Etap.Core
{
    class HabbieSteam
    {
        public static string LICENSE = "";
        private static readonly ILog log = LogManager.GetLogger("Habbie.HabbieEnvironment");

        public static bool RunLicenseKey()
        {
            if (!File.Exists("HabbieConfig/license.ini"))
                return false;
            foreach (var @params in from line in File.ReadAllLines("HabbieConfig/license.ini", Encoding.Default) where !String.IsNullOrWhiteSpace(line) && line.Contains("=") select line.Split('='))
            {
                switch (@params[0])
                {
                    case "license":
                        LICENSE = @params[1];
                        break;
                }
            }
            return true;
        }
    }
}