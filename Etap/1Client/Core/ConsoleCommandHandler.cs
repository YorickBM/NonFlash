using System;
using log4net;

namespace Etap.Core
{
    public class ConsoleCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger("Habbie.Core.ConsoleCommandHandler");

        public static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return;

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0].ToLower())
                {
                    default:
                        log.Error(parameters[0].ToLower() + " is an unknown or unsupported command. Type help for more information");
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                log.Error("Error in command " + inputData + ": " + e);
            }
        }
    }
}
