/**
 * Copyright (C) 2021 Miris Wisdom
 * 
 * This file is part of Gunloader.
 * 
 * Gunloader is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; version 2.
 * 
 * Gunloader is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Gunloader.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Gunloader.Programs;
using Gunloader.Serialisation;
using static System.Guid;
using static System.IO.File;
using static System.IO.Path;
using static System.TimeSpan;

namespace Gunloader
{
  public class Album
  {
    /* fallback static id for album directory & .gun file name */
    private static readonly string ID = NewGuid().ToString();

    [JsonPropertyName("video")]  public string      Source { get; set; } = string.Empty;
    [JsonPropertyName("title")]  public string      Title  { get; set; } = string.Empty;
    [JsonPropertyName("tracks")] public List<Track> Tracks { get; set; } = new();

    [JsonIgnore]
    [XmlIgnore]
    public DirectoryInfo Target /* normalised album title // guid on empty album */
      => new(string.IsNullOrWhiteSpace(Title)
        ? ID
        : new[]
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
        }.Aggregate(Title, (current, character) =>
          current.Replace(character, "")));

    [JsonIgnore]
    [XmlIgnore]
    public FileInfo Storage /* target + gun extension for consistency */
      => new($"{Target}.gun");

    public FileInfo Download(YTDL YTDL)
    {
      return YTDL.Download(Source, new FileInfo(NewGuid().ToString()));
    }

    public void Encode(Toolkit toolkit)
    {
      var video = Source.Contains("https://youtu") ? Download(toolkit.YTDL) : new FileInfo(Source);

      if (!Target.Exists)
        Target.Create();

      foreach (var track in Tracks)
      {
        var encoded = track.Encode(toolkit, new FileInfo(Source));
        var final   = Combine(Target.FullName, $"{track.Number}. {track.Normalised}{encoded.Extension}");

        encoded.MoveTo(final);
        encoded.CreationTimeUtc   = video.CreationTimeUtc;
        encoded.LastAccessTimeUtc = video.LastAccessTimeUtc;
        encoded.LastWriteTimeUtc  = video.LastWriteTimeUtc;
      }
    }

    public void Save(ISerialisation serialisation)
    {
      serialisation.Marshal(Storage, this);
    }

    public void Load(ISerialisation serialisation)
    {
      var album = serialisation.Hydrate<Album>(Storage);
      Source = album.Source;
      Title  = album.Title;
      Tracks = album.Tracks;
    }

    public void Hydrate(FileInfo record, Metadata metadata, YTDL ytdl = null)
    {
      var records = ReadAllLines(record.FullName)
        .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#') && !line.StartsWith(';'))
        .ToArray();

      /**
       * Attempt to infer tracks from given YouTube video based on chapters metadata.
       *
       * This is only done when the first line in the Records file represents a YouTube URL Source! 
       */

      if (records[0].Contains("http") && records[0].Contains("youtu"))
      {
        Source = records[0].Trim();

        var video = new Video();
        video.Load(Source, ytdl);

        Title = video.Title;

        for (var i = 0; i < video.Chapters.Count; i++)
        {
          var chapter = video.Chapters[i];
          var start   = FromSeconds(chapter.Start);
          var end     = FromSeconds(chapter.End);
          var track = new Track
          {
            Title    = chapter.Title,
            Number   = $"{i + 1}",
            Start    = $"{start.Hours:00}:{start.Minutes:00}:{start.Seconds:00}",
            End      = $"{end.Hours:00}:{end.Minutes:00}:{end.Seconds:00}",
            Metadata = metadata
          };

          if (string.IsNullOrWhiteSpace(track.Metadata.Comment))
            track.Metadata.Comment = Source;

          if (string.IsNullOrWhiteSpace(track.Metadata.Album))
            track.Metadata.Album = Title;

          Tracks.Add(track);
        }

        if (Tracks.Any())
          return;
      }
      else
      {
        Title  = records[0].Trim();
        Source = records[1].Trim();
      }

      /**
       * Parse the Records file when the first line is NOT a YouTube video, or when no Tracks have been successfully
       * inferred from YouTube chapters.
       */

      foreach (var entry in records.Skip(2))
      {
        var split = entry.Split(' ');
        var track = new Track
        {
          Number   = split[0],
          Start    = split[1],
          Title    = string.Join(' ', split.Skip(2)).Trim(),
          Metadata = metadata ?? new Metadata()
        };

        if (string.IsNullOrWhiteSpace(track.Metadata.Album))
          track.Metadata.Album = Title;

        /**
         * Assign Source's YouTube URL to blank Comments.
         *
         * If the Source file name is heuristically determined to be a YouTube ID, then its value will be used.
         *
         * YouTube ID requirements: 11 characters; allowed characters: alphanumeric, dashes and underscores.
         */

        if (string.IsNullOrWhiteSpace(track.Metadata.Comment))
        {
          var id   = GetFileNameWithoutExtension(Source) ?? string.Empty;
          var rule = new Regex("[a-zA-Z0-9_-]{11}");

          if (rule.IsMatch(id))
            track.Metadata.Comment = $"https://youtu.be/{id}";

          if (Source != null && Source.Contains("http"))
            track.Metadata.Comment = Source;
        }

        Tracks.Add(track);
      }
      /**
       * Infer each Track's ending time. Current Track's ending time = next Track's starting time.
       */

      foreach (var track in Tracks)
      {
        var next = Tracks.FindIndex(r => r.Number.Equals(track.Number));
        var end = next + 1 >= Tracks.Count
          ? string.Empty
          : Tracks[next + 1].Start;

        track.End = end;
      }
    }

    public void Compile(FileInfo records, Metadata metadata, ISerialisation serialisation, YTDL ytdl = null)
    {
      if (!records.Extension.Contains("txt") || !records.Exists)
        throw new ArgumentException("A valid plaintext records file must exist.");

      Hydrate(records, metadata, ytdl);
      Save(serialisation);
    }
  }
}