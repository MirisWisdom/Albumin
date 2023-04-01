using static System.Console;
using static System.ConsoleColor;

namespace Gunloader
{
  public static partial class Program
  {
    public static void Banner()
    {
      ForegroundColor = Red;
      WriteLine(@"                            __                __               ");
      WriteLine(@"         ____ ___  ______  / /___  ____ _____/ /__  _____      ");
      WriteLine(@"        / __ `/ / / / __ \/ / __ \/ __ `/ __  / _ \/ ___/      ");
      WriteLine(@"       / /_/ / /_/ / / / / / /_/ / /_/ / /_/ /  __/ /          ");
      WriteLine(@"       \__, /\__,_/_/ /_/_/\____/\__,_/\__,_/\___/_/           ");
      WriteLine(@"      /____/                                                   ");
      ForegroundColor = DarkGray;
      WriteLine(@"      ---------------------------------------------------      ");
      WriteLine(@"      Author :: Miris /// GitHub :: MirisWisdom/Gunloader      ");
      WriteLine(@"      ---------------------------------------------------      ");
      ForegroundColor = White;
    }
  }
}
