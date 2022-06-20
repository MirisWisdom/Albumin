using System;
using System.IO;
using System.Net.Http;
using Gunloader.Encoders;
using static System.Console;
using static System.ConsoleColor;
using static System.IO.File;
using static System.IO.Path;

namespace Gunloader
{
  public static partial class Program
  {
    public static void Wizard()
    {
      const string URL = "https://gunloader.miris.design";
      const string API = "https://gunloader.miris.design/api/instructions";

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

      Info($"Please go to {URL} and follow the instructions there, then type the ID in here!");
      Prompt("Gunloader ID");

      var path = new FileInfo(GetTempFileName());

      Download($"{API}/{ReadLine()}", path.FullName);

      Instructions = path;

      var format = string.Empty;

      while (format != "flac"   &&
             format != "lame"   &&
             format != "opus"   &&
             format != "vorbis" &&
             format != "aac"    &&
             format != "original")
      {
        Info("Please specify a format.\nAvailable formats: flac | lame | opus | vorbis | aac | original");
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
        case "aac":
          Toolkit.Encoder      = new AAC();
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

      Invoke();
    }

    private static async void Download(string uri, string destination)
    {
      if (!Uri.TryCreate(uri, UriKind.Absolute, out _))
        throw new InvalidOperationException("URI is invalid.");

      if (!Exists(destination))
        throw new FileNotFoundException("File not found."
          , nameof(destination));

      var fileBytes = await new HttpClient().GetByteArrayAsync(uri);
      await WriteAllBytesAsync(destination, fileBytes);
    }
  }
}