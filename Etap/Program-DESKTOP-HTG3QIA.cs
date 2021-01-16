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
            int trys = 0;

            try
            {
                using (var game = new Main())
                {
                    try
                    {
                        game.Run();
                    }catch(InvalidOperationException ex)
                    {
                        if (trys++ < 5)
                            game.Run();
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine("CRASH");
                Console.WriteLine(ex.ToString());

                Console.ReadLine();
            }

            
            RetroEnvironment.CompleteShutdown();
        }
    }
#endif
}
