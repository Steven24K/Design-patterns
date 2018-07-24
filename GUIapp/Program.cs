using System;
using System.IO;

namespace GUIapp
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //The main entry point for the game, sets the game to run
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
