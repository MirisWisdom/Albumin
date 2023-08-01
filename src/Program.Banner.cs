using static System.Console;
using static System.ConsoleColor;

namespace Albumin
{
  public static partial class Program
  {
    public static void Banner()
    {
      ForegroundColor = Red;
      WriteLine(@"          ___    ____                    _           ");
      WriteLine(@"         /   |  / / /_  __  ______ ___  (_)___       ");
      WriteLine(@"        / /| | / / __ \/ / / / __ `__ \/ / __ \      ");
      WriteLine(@"       / ___ |/ / /_/ / /_/ / / / / / / / / / /      ");
      WriteLine(@"      /_/  |_/_/_.___/\__,_/_/ /_/ /_/_/_/ /_/       ");
      WriteLine(@"                                                     ");
      ForegroundColor = DarkGray;
      WriteLine(@"      -----------------------------------------------");
      WriteLine(@"      GITHUB PROJECT  ::  MirisWisdom/YouTube.Albumin");
      WriteLine(@"      -----------------------------------------------");
      ForegroundColor = White;
    }
  }
}
