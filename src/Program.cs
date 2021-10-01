using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Console;
using static System.DateTime;
using static System.Diagnostics.Process;
using static System.Environment;
using static System.Globalization.CultureInfo;
using static System.Guid;
using static System.IO.File;
using static System.IO.Path;
using static System.Text.Json.JsonSerializer;

namespace Gunloader
{
  /**
   * Gunloader Program.
   */
  public static partial class Program
  {
    public static FileInfo     Records  { get; set; }                              /* tracks numbers & titles         */
    public static FileInfo     Source   { get; set; } = new(NewGuid().ToString()); /* local video source              */
    public static string       Download { get; set; }                              /* youtube-dl video download       */
    public static FileInfo     Batch    { get; set; }                              /* batch self-invocation           */
    public static string       Album    { get; set; }                              /* metadata and directory name     */
    public static List<string> Artists  { get; set; }                              /* metadata in the encoded tracks  */
    public static string       Comment  { get; set; }                              /* metadata; default: download url */
    public static string       Genre    { get; set; }                              /* metadata in the encoded tracks  */
    public static FileInfo     Cover    { get; set; }                              /* custom album art cover          */
    public static string       FFmpeg   { get; set; } = "ffmpeg";                  /* audio & cover extraction        */
    public static string       LAME     { get; set; } = "lame";                    /* mp3 encoding & tagging          */
    public static string       YTDL     { get; set; } = "youtube-dl";              /* video downloading               */

    public static void Main(string[] args)
    {
      OptionSet.Parse(args);
      Invoke();
    }

    public static void Invoke()
    {
      if (Records is not {Exists: true} && Batch is not {Exists: true})
      {
        WriteLine("Please provide a valid records or batch file.");
        Exit(1);
      }

      /**
       * Convert legacy Records text file to JSON.
       */

      if (Records is {Exists: true} && Records.Extension.Contains("txt"))
      {
        var path = GetFileNameWithoutExtension(Records.FullName) + ".json";
        var json = Serialize(ReadAllLines(Records.FullName)
          .Select(record => record.Split(' '))
          .Select(split => new Track
          {
            Number  = split[0], /* album track number  */
            Start   = split[1], /* track starting time */
            Title   = string.Join(' ', split.Skip(2)) /* track title         */,
            Genre   = Genre,
            Album   = Album,
            Comment = Comment,
            Artists = Artists,
            Cover   = Cover.FullName
          }).ToList(), new JsonSerializerOptions {WriteIndented = true});

        WriteAllText(path, json);
        WriteLine($"Serialised '{GetFileName(path)}'. Feel free to review/edit it, then press any key to continue...");
        ReadLine();
      }

      /**
       * Convert legacy Batch text file to JSON.
       */

      if (Batch is {Exists: true} && Batch.Extension.Contains("txt"))
      {
        var path = GetFileNameWithoutExtension(Batch.FullName) + ".json";
        var json = Serialize(ReadAllLines(Batch.FullName)
          .Select(album => album.Split(' '))
          .Select(split => new Album
          {
            Source  = split[0], /* album source video */
            Records = split[1], /* album records path */
            Title   = string.Join(' ', split.Skip(2)) /* album title        */,
            Genre   = Genre,
            Comment = Comment,
            Artists = Artists,
            Cover   = Cover.FullName
          }).ToList(), new JsonSerializerOptions {WriteIndented = true});

        WriteAllText(path, json);
        WriteLine($"Serialised '{GetFileName(path)}'. Feel free to review/edit it, then press any key to continue...");
        ReadLine();
      }

      /**
       * Rudmientary batch porocessing. This is most definitely Loveraftian in nature.
       */

      if (Batch != null)
      {
        var path   = Batch.FullName;
        var albums = Deserialize<List<Album>>(ReadAllText(path));

        Batch = null; /* prevent infinite loop */

        if (albums == null)
          Exit(1);

        foreach (var album in albums)
        {
          Album   = album.Title;
          Artists = album.Artists ?? Artists;
          Records = new FileInfo(album.Records);
          Comment = !string.IsNullOrWhiteSpace(album.Comment) ? album.Comment : Comment;
          Genre   = !string.IsNullOrWhiteSpace(album.Genre) ? album.Genre : Genre;
          Cover   = !string.IsNullOrWhiteSpace(album.Cover) ? new FileInfo(album.Cover) : Cover;

          if (album.Source.Contains("https://youtu"))
            Download = album.Source;
          else
            Source = new FileInfo(album.Source);

          Invoke();
        }

        Exit(0);
      }

      /**
       * Download the video representing the album in question.
       */

      if (!string.IsNullOrWhiteSpace(Download) && !Source.Exists)
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

      /**
       * Assign Source's YouTube URL to blank Comments.
       *
       * -   If a Download URL is provided, its value will be used for the Comment; otherwise...
       *
       * -   If the Source file name is heuristically determined to be a YouTube ID, then its value will be used.
       *     YouTube ID requirements: 11 characters; allowed characters: alphanumeric, dashes and underscores.
       */

      if (string.IsNullOrWhiteSpace(Comment))
      {
        var id   = GetFileNameWithoutExtension(Source.Name);
        var rule = new Regex("[a-zA-Z0-9_-]{11}");

        if (rule.IsMatch(id))
          Comment = $"https://youtu.be/{id}";

        if (!string.IsNullOrWhiteSpace(Download))
          Comment = Download;
      }

      /**
       * Begin the extraction & encoding procedures...
       */

      var records     = Deserialize<List<Track>>(ReadAllText(Records.FullName)); /* track records */
      var destination = new DirectoryInfo(Album);

      if (!destination.Exists)
        destination.Create();

      /**
       * TODO: Make this a wee bit more object-oriented? For the purists and masochists who seek misery here...
       */

      if (records == null)
        Exit(1);

      foreach (var record in records)
      {
        var number       = record.Number; /* album track number  */
        var start        = record.Start;  /* track starting time */
        var title        = record.Title;  /* track title         */
        var artists      = record.Artists ?? Artists;
        var album        = !string.IsNullOrWhiteSpace(record.Album) ? record.Album : Album;
        var genre        = !string.IsNullOrWhiteSpace(record.Genre) ? record.Genre : Genre;
        var comment      = !string.IsNullOrWhiteSpace(record.Comment) ? record.Comment : Comment;
        var cover        = !string.IsNullOrWhiteSpace(record.Cover) ? new FileInfo(record.Cover) : Cover;
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

        var next = records.FindIndex(r => r.Number.Equals(record.Number));
        var end = next + 1 >= records.Count
          ? string.Empty
          : records[next + 1].Start;

        WriteLine(new string('-', 80));
        WriteLine($"{number}. {title}");
        WriteLine($"{start} {end}");

        /**
         * Extract the cover art from the source file for the current track as a fallback.
         */

        if (cover is not {Exists: true})
        {
          cover = new FileInfo($"{number}.jpg");

          var frame = ParseExact(start, "H:mm:ss", InvariantCulture)
            .AddSeconds(30); /* (start time + 30 seconds) has correct thumbnail */

          Start(new ProcessStartInfo
          {
            FileName = FFmpeg,
            Arguments = $"-ss {frame:H:mm:ss} " +
                        $"-y -i {Source.Name} " +
                        "-vframes 1 "           +
                        $"{cover.Name}"
          })?.WaitForExit();
        }

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
                      $"--tl \"{album}\" "                          +
                      $"--tg \"{genre}\" "                          +
                      $"--tc \"{comment}\" "                        +
                      $"--tv \"TPE2={string.Join(';', artists)}\" " +
                      $"{intermediate.Name} "
        })?.WaitForExit();

        /**
         * Finalise the encoded file (with normalised name, destination and time attributes), then delete temp data.
         */

        encoded.MoveTo(Combine(destination.FullName, $"{number}. {normalised}.mp3"));
        encoded.CreationTimeUtc   = Source.CreationTimeUtc;
        encoded.LastAccessTimeUtc = Source.LastAccessTimeUtc;
        encoded.LastWriteTimeUtc  = Source.LastWriteTimeUtc;

        if (cover.Exists)
          cover.Delete();

        if (intermediate.Exists)
          intermediate.Delete();

        WriteLine(new string('=', 80));
      }

      /**
       * Set album directory's timestamp to the source file's values. 
       */

      destination.CreationTimeUtc   = Source.CreationTimeUtc;
      destination.LastAccessTimeUtc = Source.LastAccessTimeUtc;
      destination.LastWriteTimeUtc  = Source.LastWriteTimeUtc;
    }
  }
}