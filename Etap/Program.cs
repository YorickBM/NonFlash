using Etap.Utilities;
using System;

namespace Etap
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    /// REVERSION-yyyymmdduumm-code 
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new Main())
                    game.Run();
            }catch(Exception ex)
            {
                Console.WriteLine("CRASH");
                Console.WriteLine(ex.ToString());
            }

            
            RetroEnvironment.CompleteShutdown();
        }
    }
#endif
}
