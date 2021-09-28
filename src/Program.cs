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
        "records=|timestamps=|cue=", "path to records file with times and titles",
        s => Records = new FileInfo(s)
      },
      {
        "compilation=|source=|file=", "path to a downloaded gundober compilation file",
        s => Source = new FileInfo(s)
      },
      {
        "album=", "album name to move tracks to, and to assign to the tracks' metadata",
        s => Album = s
      },
      {
        "artist=", "album artist to assign to the tracks' metadata",
        s => Artist = s
      },
      {
        "genre=", "genre to assign to the tracks' metadata",
        s => Genre = s
      }
    };


    public static FileInfo Records { get; set; } = new(Combine(CurrentDirectory, "tracks.txt"));
    public static FileInfo Source  { get; set; } = new(Combine(CurrentDirectory, NewGuid().ToString()));
    public static string   Album   { get; set; } = string.Empty; /* for the metadata and output directory name */
    public static string   Artist  { get; set; } = string.Empty; /* for the metadata in the output mp3 tracks  */
    public static string   Genre   { get; set; } = string.Empty; /* for the metadata in the output mp3 tracks  */

    public static void Main(string[] args)
    {
      OptionSet.WriteOptionDescriptions(Out);
      OptionSet.Parse(args);

      var compilation = Source.FullName;                /* source path   */
      var records     = ReadAllLines(Records.FullName); /* track records */

      if (!string.IsNullOrWhiteSpace(Album))
        Directory.CreateDirectory(Album);

      /**
       * TODO: Parallel implementation for contributing towards a more sustainable future for our planet.
       * TODO: Make this a wee bit more object-oriented? For the purists and masochists who seek misery here...
       */

      foreach (var record in records)
      {
        var split  = record.Split(' ');
        var number = split[0];                        /* album track number  */
        var start  = split[1];                        /* track starting time */
        var title  = string.Join(' ', split.Skip(2)); /* track title         */
        var cover = ParseExact(start, "H:mm:ss", InvariantCulture)
          .AddSeconds(30); /* (start time + 30 seconds) has correct thumbnail */

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
          FileName = "ffmpeg",
          Arguments = $"-ss {cover:H:mm:ss} " +
                      $"-y -i {compilation} " +
                      "-vframes 1 "           +
                      $"{number}.jpg"
        })?.WaitForExit();

        /**
         * Extract the MP3 from the source file for the current track.
         */

        Start(new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"-ss {start} "                                        +
                      $"{(!string.IsNullOrEmpty(end) ? $"-to {end}" : "")} " +
                      $"-y -i {compilation} "                                +
                      "-b:a 192K -vn "                                       +
                      $"{number}.mp3"
        })?.WaitForExit();

        /**
         * Embed cover art & album title metadata within the track.
         *
         * TODO: Figure out a way of extracting the audio *and* adding the data in one pass.
         */

        Start(new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"-y -i {number}.mp3 "                     +
                      $"-i {number}.jpg "                        +
                      "-map 0:0 "                                +
                      "-map 1:0 "                                +
                      "-c copy "                                 +
                      "-id3v2_version 3 "                        +
                      "-metadata:s:v title=\"Album cover\" "     +
                      "-metadata:s:v comment=\"Cover (front)\" " +
                      $"-metadata title=\"{title}\" "            +
                      $"-metadata album=\"{Album}\" "            +
                      $"-metadata album_artist=\"{Artist}\" "    +
                      $"-metadata genre=\"{Genre}\" "            +
                      $"{number}~1.mp3"
        })?.WaitForExit();

        /**
         * Move file to the normalised name of the track (within album directory if exists), and remove temporary files.
         */

        Move($"{number}~1.mp3", Combine(Album ?? string.Empty, $"{number}. {normalised}.mp3"));

        Delete($"{number}.jpg");
        Delete($"{number}.mp3");

        WriteLine(new string('=', 80));
      }
    }
  }
}