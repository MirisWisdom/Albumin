using System.IO;
using Gunloader.Encoders;
using static System.Console;
using static System.ConsoleColor;
using static System.IO.File;
using static System.IO.SearchOption;

namespace Gunloader
{
  public static partial class Program
  {
    public static void Wizard()
    {
      void Prompt(string prompt)
      {
        ForegroundColor = Red;
        Write($"{prompt}: ");
        ForegroundColor = White;
      }

      Banner();

      var file = new FileInfo("records.txt");

      if (file.Exists && file.Directory != null)
      {
        var id = file.Directory.GetFiles("records.txt*", TopDirectoryOnly).Length + 1;
        Move(file.FullName, $"{file.FullName}.backup ~ {id}");
      }

      var sample = new string("90'sアニメ主題歌セレクション RB-XYZ【奇跡の向こう側へ】 Ver.2\n"
                              + "https://youtu.be/divcisums90"
                              + "\n\n"
                              + "01 0:00:00 All You Need Is Love - 田村直美 「レイアース」OVA版主題歌\n"
                              + "02 0:05:20 HEAVEN - HIM 「YAT安心！宇宙旅行」一期OP\n"
                              + "03 0:08:48 僕であるために - FLYING KIDS 「逮捕しちゃうぞ」一期OP2\n"
                              + "04 0:12:23 LOVE SOMEBODY - 福井麻利子 「逮捕しちゃうぞ」一期OP3");

      WriteAllText(file.FullName, sample);
      WriteLine("I've created a sample records.txt file for you. Edit it, and once you're done, press any key!");
      ReadLine();

      Records.Add(file);

      var format = string.Empty;

      while (format != "flac" && format != "lame" && format != "opus" && format != "vorbis" && format != "original")
      {
        WriteLine("Please specify a format. Available formats: flac | lame | opus | vorbis | original");
        Prompt("Format");

        format = ReadLine();
      }

      switch (format)
      {
        case "lame":
          Toolkit.Encoder      = new LAME();
          Toolkit.FFmpeg.Lossy = true;
          break;
        case "flac":
          Toolkit.Encoder      = new FLAC();
          Toolkit.FFmpeg.Lossy = false;
          break;
        case "vorbis":
          Toolkit.Encoder      = new Vorbis();
          Toolkit.FFmpeg.Lossy = true;
          break;
        case "opus":
          Toolkit.Encoder      = new Opus();
          Toolkit.FFmpeg.Lossy = true;
          break;
        case "original":
          Toolkit.Encoder      = new RAW();
          Toolkit.FFmpeg.Lossy = false;
          break;
        default:
          Toolkit.Encoder      = new FLAC();
          Toolkit.FFmpeg.Lossy = false;
          break;
      }

      WriteLine("If you want, feel free to specify the album artist.");
      Prompt("Artist");
      var artist = ReadLine();

      if (artist != null)
        Metadata.Artists.Add(artist);

      WriteLine("If you want, feel free to specify the album genre.");
      Prompt("Genre");
      var genre = ReadLine();

      if (genre != null)
        Metadata.Genre = genre;

      WriteLine("If you want, feel free to specify the album comment.");
      Prompt("Comment");
      var comment = ReadLine();

      if (comment != null)
        Metadata.Comment = comment;

      WriteLine("I'll try to process all of this stuff for you now.");
      WriteLine("Before continuing, make sure that you have the necessary dependencies installed!");
      WriteLine("-   youtube-dl and ffmpeg");
      WriteLine("-   lame if you are using MP3");
      WriteLine("-   flac if you are using FLAC");
      WriteLine("-   opusenc if you are using Opus");
      WriteLine("-   oggenc if you are using Vorbis");
      WriteLine("If you are ready, press any key to continue...");
      ReadLine();

      Invoke();
    }
  }
}