﻿using System.Collections.Generic;
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
using static System.Guid;
using static System.IO.File;
using static System.IO.Path;

namespace Gunloader
{
  /**
   * Gunloader Program.
   */
  public static class Program
  {
    public static readonly OptionSet OptionSet = new()
    {
      {
        "tracks=|records=|timestamps=|cue=", "path to records file with track numbers, timestamps and song titles",
        s => Records = new FileInfo(s)
      },
      {
        "source=|video=|compilation=|file=", "path to an already-downloaded video file containing the compiled songs",
        s => Source = new FileInfo(s)
      },
      {
        "download=", "download video from given url to use as the source for songs",
        s => Download = s
      },
      {
        "album=", "album title to assign to the tracks' metadata; also, directory name to move tracks to",
        s => Album = s
      },
      {
        "artist=", "album artist(s) to assign to the tracks' metadata; multiple: --artist 'a' --artist 'b', etc.",
        s => Artists.Add(s)
      },
      {
        "genre=", "genre to assign to the tracks' metadata",
        s => Genre = s
      },
      {
        "comment=", "comment to assign to the tracks' metadata",
        s => Comment = s
      },
      {
        "ffmpeg=", "optional path to ffmpeg for audio & cover extraction",
        s => FFmpeg = s
      },
      {
        "lame=", "optional path to lame for mp3 encoding & tagging",
        s => LAME = s
      },
      {
        "youtube-dl=|ytdl=", "optional path to youtube-dl for video downloading",
        s => YTDL = s
      }
    };

    public static FileInfo     Records  { get; set; } = new("tracks.txt");         /* tracks numbers & titles         */
    public static FileInfo     Source   { get; set; } = new(NewGuid().ToString()); /* local video source              */
    public static string       Download { get; set; } = string.Empty;              /* youtube-dl video download       */
    public static string       Album    { get; set; } = string.Empty;              /* metadata and directory name     */
    public static List<string> Artists  { get; set; } = new() {"Various Artists"}; /* metadata in the encoded tracks  */
    public static string       Comment  { get; set; } = string.Empty;              /* metadata; default: download url */
    public static string       Genre    { get; set; } = string.Empty;              /* metadata in the encoded tracks  */
    public static string       FFmpeg   { get; set; } = "ffmpeg";                  /* audio & cover extraction        */
    public static string       LAME     { get; set; } = "lame";                    /* mp3 encoding & tagging          */
    public static string       YTDL     { get; set; } = "youtube-dl";              /* video downloading               */

    public static void Main(string[] args)
    {
      OptionSet.WriteOptionDescriptions(Out);
      OptionSet.Parse(args);

      if (!string.IsNullOrEmpty(Download))
      {
        /**
         * Set blank comment to download URL for posterity. 
         */

        if (string.IsNullOrEmpty(Comment)) 
          Comment = Download;

        /**
         * Download video if requested & it doesn't already exist on the filesystem.
         */

        if (!Source.Exists)
        {
          Start(new ProcessStartInfo
          {
            FileName = YTDL,
            Arguments = $"{Download} " +
                        $"--output {Source}"
          })?.WaitForExit();

          /**
         * Infer file extension of the downloaded file.
         */

          Source = new FileInfo(Directory.GetFiles(CurrentDirectory, $"{Source.Name}*")[0]);

          WriteLine(Source.FullName);
        }
      }

      var records     = ReadAllLines(Records.FullName); /* track records */
      var destination = new DirectoryInfo(Album);

      if (!destination.Exists)
        destination.Create();

      /**
       * TODO: Make this a wee bit more object-oriented? For the purists and masochists who seek misery here...
       */

      foreach (var record in records)
      {
        var split  = record.Split(' ');
        var number = split[0];                        /* album track number  */
        var start  = split[1];                        /* track starting time */
        var title  = string.Join(' ', split.Skip(2)); /* track title         */
        var frame = ParseExact(start, "H:mm:ss", InvariantCulture)
          .AddSeconds(30); /* (start time + 30 seconds) has correct thumbnail */

        var cover        = new FileInfo($"{number}.jpg");
        var intermediate = new FileInfo($"{number}.wav");
        var encoded      = new FileInfo($"{number}.mp3");

        /**
         * Normalise output filename for Windows & Linux systems.
         */

        var normalised = new[]
        {
          "<",  /* win */
          ">",  /* win */
          ":",  /* win */
          "\"", /* win */
          "/",  /* lin */
          "\\", /* win */
          "|",  /* win */
          "?",  /* win */
          "*"   /* win */
        }.Aggregate(title, (current, character) =>
          current.Replace(character, ""));

        var end = IndexOf(records, record) + 1 >= records.Length
          ? string.Empty
          : records[IndexOf(records, record) + 1].Split(' ')[1];

        WriteLine(new string('-', 80));
        WriteLine($"{number}. {title}");
        WriteLine($"{start} {end}");

        /**
         * Extract the cover art from the source file for the current track.
         */

        Start(new ProcessStartInfo
        {
          FileName = FFmpeg,
          Arguments = $"-ss {frame:H:mm:ss} " +
                      $"-y -i {Source.Name} " +
                      "-vframes 1 "           +
                      $"{cover.Name}"
        })?.WaitForExit();

        /**
         * Extract an intermediate WAV from the source file for the current track.
         */

        Start(new ProcessStartInfo
        {
          FileName = FFmpeg,
          Arguments = $"-ss {start} "                                        +
                      $"{(!string.IsNullOrEmpty(end) ? $"-to {end}" : "")} " +
                      $"-y -i {Source.Name} "                                +
                      $"{intermediate.Name}"
        })?.WaitForExit();

        /**
         * Encode the intermeditate WAV into an MP3 with embedded art & metadata.
         */

        Start(new ProcessStartInfo
        {
          FileName = LAME,
          Arguments = "--vbr-new "                                  +
                      $"--ti {cover.Name} "                         +
                      $"--tt \"{title}\" "                          +
                      $"--tn \"{number}\" "                         +
                      $"--tl \"{Album}\" "                          +
                      $"--tg \"{Genre}\" "                          +
                      $"--tc \"{Comment}\" "                        +
                      $"--tv \"TPE2={string.Join(';', Artists)}\" " +
                      $"{intermediate.Name} "
        })?.WaitForExit();

        /**
         * Finalise the encoded file (with normalised name, destination and time attributes), then delete temp data.
         */

        encoded.MoveTo(Combine(destination.FullName, $"{number}. {normalised}.mp3"));
        encoded.CreationTimeUtc   = Source.CreationTimeUtc;
        encoded.LastAccessTimeUtc = Source.LastAccessTimeUtc;
        encoded.LastWriteTimeUtc  = Source.LastWriteTimeUtc;

        Delete($"{number}.jpg");
        Delete($"{number}.wav");

        WriteLine(new string('=', 80));
      }

      /**
       * Set albun directory's timestamp to the source file's values. 
       */

      destination.CreationTimeUtc   = Source.CreationTimeUtc;
      destination.LastAccessTimeUtc = Source.LastAccessTimeUtc;
      destination.LastWriteTimeUtc  = Source.LastWriteTimeUtc;
    }
  }
}