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

      void Info(string message)
      {
        ForegroundColor = DarkGray;
        WriteLine(new string('-', 80));
        ForegroundColor = White;
        WriteLine(message);
      }

      Banner();

      var file = new FileInfo("records.txt");

      if (file.Exists && file.Directory != null)
      {
        var id     = file.Directory.GetFiles("records.txt.*", TopDirectoryOnly).Length + 1;
        var backup = new FileInfo($"{file.FullName}.{id:0000}");

        Move(file.FullName, backup.FullName);

        Info($"I've backed up your existing records file!\n{file.Name} => {backup.Name}");
      }

      var sample = new string("90'sアニメ主題歌セレクション RB-XYZ【奇跡の向こう側へ】 Ver.2\n"
                              + "https://youtu.be/divcisums90"
                              + "\n\n"
                              + "01 0:00:00 All You Need Is Love - 田村直美 「レイアース」OVA版主題歌\n"
                              + "02 0:05:20 HEAVEN - HIM 「YAT安心！宇宙旅行」一期OP\n"
                              + "03 0:08:48 僕であるために - FLYING KIDS 「逮捕しちゃうぞ」一期OP2\n"
                              + "04 0:12:23 LOVE SOMEBODY - 福井麻利子 「逮捕しちゃうぞ」一期OP3");

      WriteAllText(file.FullName, sample);

      Info("I've created a new sample records.txt file for you to edit.");
      WriteLine("Edit it using a text editor with the relevant information, then press any key!");
      ReadLine();

      Records.Add(file);

      var format = string.Empty;

      while (format != "flac" && format != "lame" && format != "opus" && format != "vorbis" && format != "original")
      {
        Info("Please specify a format.\nAvailable formats: flac | lame | opus | vorbis | original");
        Prompt("Format");

        format = ReadLine()?.ToLower();
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

      Info("If you want, feel free to specify the album artist(s).");
      WriteLine("Use ; to specify multiple artists, e.g. Yoko Takahashi; Shinichi Ishihara");
      Prompt("Artist(s)");
      var artists = ReadLine();

      if (artists != null)
        foreach (var artist in artists.Split(';'))
          Metadata.Artists.Add(artist.Trim());

      Info("If you want, feel free to specify the album genre.");
      Prompt("Genre");
      var genre = ReadLine();

      if (genre != null)
        Metadata.Genre = genre;

      Info("If you want, feel free to specify the album comment.");
      Prompt("Comment");
      var comment = ReadLine();

      if (comment != null)
        Metadata.Comment = comment;

      Info("I'll try to process all of this stuff for you now.\n");
      WriteLine("Before continuing, make sure that you have the necessary dependencies installed!\n");
      WriteLine("-   youtube-dl and ffmpeg");
      WriteLine("-   lame if you are using MP3");
      WriteLine("-   flac if you are using FLAC");
      WriteLine("-   opusenc if you are using Opus");
      WriteLine("-   oggenc if you are using Vorbis");
      Info("If you are ready, press any key to continue...");
      ReadLine();

      Invoke();
    }
  }
}