using static System.Console;
using static System.ConsoleColor;
using static System.Diagnostics.Process;
using static System.Environment;
using static System.Runtime.InteropServices.OSPlatform;
using static System.Runtime.InteropServices.RuntimeInformation;

namespace Albumin
{
  public static partial class Program
  {
    public static void Help(bool verbose, bool exit = true)
    {
      Banner();
      
      if (verbose)
      {
        void Step(string step, string instruction)
        {
          ForegroundColor = Red;
          Write($"{step} STEP: ");
          ForegroundColor = White;
          WriteLine(instruction);
        }

        void Example(string example)
        {
          ForegroundColor = DarkGray;
          WriteLine();
          WriteLine(example);
          WriteLine();
          ForegroundColor = White;
        }

        Step("1st", @"Create a text file with the album details, as instructed in the README. Example:");

        Example("  90'sアニメ主題歌セレクション RB-XYZ【奇跡の向こう側へ】 Ver.2\n"
                + "  https://youtu.be/divcisums90"
                + "  \n\n"
                + "  01 0:00:00 All You Need Is Love - 田村直美 「レイアース」OVA版主題歌\n"
                + "  02 0:05:20 HEAVEN - HIM 「YAT安心！宇宙旅行」一期OP\n"
                + "  03 0:08:48 僕であるために - FLYING KIDS 「逮捕しちゃうぞ」一期OP2\n"
                + "  04 0:12:23 LOVE SOMEBODY - 福井麻利子 「逮捕しちゃうぞ」一期OP3");

        var process = GetCurrentProcess().ProcessName;

        if (IsOSPlatform(Windows))
        {
          Step("2nd", @"Open a CMD window and run Albumin, as instructed in the README. Example:");
          Example($@"  .\{process}.exe --album 'C:\Path\To\Records-File.txt' --format flac --genre OST");
        }
        else
        {
          Step("2nd", @"Open a Terminal and run Albumin as instructed in the README. Example:");
          Example($@"  ./{process} --album 'C:\Path\To\Records-File.txt' --format flac --genre OST");
        }

        Step("3rd", "Read the README, as instructed in the README. Press any key to continue...");
        ReadLine();
      }

      OptionSet.WriteOptionDescriptions(Out);
        
      if (exit)
        Exit(0);
    }
  }
}