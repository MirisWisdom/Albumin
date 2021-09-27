using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Options;
using static System.Array;
using static System.Console;
using static System.DateTime;
using static System.Diagnostics.Process;
using static System.Environment;
using static System.Globalization.CultureInfo;
using static System.IO.File;
using static System.IO.Path;

namespace Gunloader
{
  internal class Program
  {
    public static readonly OptionSet OptionSet = new()
    {
      {
        "url=", "url of the gundober playlist",
        s => URL = s
      },
      {
        "records=", "path to records file with times and titles",
        s => Records = new FileInfo(s)
      },
      {
        "download=|file=", "path to a downloaded gundober playlist file",
        s => Download = new FileInfo(s)
      }
    };

    public static string   URL      { get; set; } = string.Empty;
    public static FileInfo Records  { get; set; } = new(Combine(CurrentDirectory, "tracks.txt"));
    public static FileInfo Download { get; set; } = new(Combine(CurrentDirectory, Guid.NewGuid().ToString()));

    private static void Main(string[] args)
    {
      OptionSet.WriteOptionDescriptions(Out);
      OptionSet.Parse(args);

      if (!string.IsNullOrWhiteSpace(URL) && !Download.Exists)
        Start(new ProcessStartInfo
        {
          FileName = "youtube-dl",
          Arguments = $"-k {URL} " +
                      $"-o {Download.FullName}"
        });

      var download = Download.FullName;
      var records  = ReadAllLines(Records.FullName);

      foreach (var record in records)
      {
        var split  = record.Split(' ');
        var number = split[0];
        var start  = split[1];
        var title  = string.Join(' ', split.Skip(2));
        var cover  = ParseExact(start, "H:mm:ss", InvariantCulture).AddSeconds(30);

        var normalised = new[]
        {
          "<",
          ">",
          ":",
          "\"",
          "/",
          "\\",
          "|",
          "?",
          "*"
        }.Aggregate(title, (current, character) =>
          current.Replace(character, ""));

        var end = IndexOf(records, record) + 1 >= records.Length
          ? string.Empty
          : records[IndexOf(records, record) + 1].Split(' ')[1];

        WriteLine($"{start} -- {cover:H:mm:ss}");

        Start(new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"-i {download} "                                      +
                      $"-ss {start} "                                        +
                      $"{(!string.IsNullOrEmpty(end) ? $"-to {end}" : "")} " +
                      $"{number}.mp3"
        })?.WaitForExit();

        Start(new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"-i {download} -ss {cover:H:mm:ss} " +
                      "-vframes 1 "                         +
                      $"{number}.png"
        })?.WaitForExit();

        Start(new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"-i {number}.mp3 "                        +
                      $"-i {number}.png "                        +
                      "-map 0:0 "                                +
                      "-map 1:0 "                                +
                      "-c copy "                                 +
                      "-id3v2_version 3 "                        +
                      "-metadata:s:v title=\"Album cover\" "     +
                      "-metadata:s:v comment=\"Cover (front)\" " +
                      $"-metadata title=\"{title}\" "            +
                      $"{number}~1.mp3"
        })?.WaitForExit();

        Move($"{number}~1.mp3", $"{number}. {normalised}.mp3");

        Delete($"{number}.png");
        Delete($"{number}.mp3");
      }
    }
  }
}