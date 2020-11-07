using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Utilities
{
    public static class Logger
    {
        public static void Debug(params object[] messageList)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                string message = "";
                for (int i = 0; i < messageList.Length; i++)
                {
                    message += " " + messageList[i].ToString();
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - [DEBUG] " + message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        
        public static void DebugWarn(params object[] messageList)
        {
            if (System.Diagnostics.Debugger.IsAttached && RetroEnvironment.ShowDebugWarn)
            {
                string message = "";
                for (int i = 0; i < messageList.Length; i++)
                {
                    message += " " + messageList[i].ToString();
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - [WARN] " + message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void Warn(params object[] messageList)
        {
            string message = "";
            for (int i = 0; i < messageList.Length; i++)
            {
                message += " " + messageList[i].ToString();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - [WARN] " + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Info(params object[] messageList)
        {
            string message = "";
            for (int i = 0; i < messageList.Length; i++)
            {
                message += " " + messageList[i].ToString();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - [INFO] " + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Error(params object[] messageList)
        {
            string message = "";
            for (int i = 0; i < messageList.Length; i++)
            {
                message += " " + messageList[i].ToString();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - [ERROR] " + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ColorMsg(ConsoleColor color, params object[] messageList)
        {
            string message = "";
            for (int i = 0; i < messageList.Length; i++)
            {
                message += " " + messageList[i].ToString();
            }
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - [ERROR] " + message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
