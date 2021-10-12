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
using static System.IO.Path;
using static System.IO.File;

namespace Gunloader
{
  public class Album
  {
    /* fallback static id for album directory & .gun file name */
    private static readonly string ID = NewGuid().ToString();

    [JsonPropertyName("video")]  public string      Video  { get; set; } = string.Empty;
    [JsonPropertyName("title")]  public string      Title  { get; set; } = string.Empty;
    [JsonPropertyName("tracks")] public List<Track> Tracks { get; set; } = new();

    [JsonIgnore]
    [XmlIgnore]
    public DirectoryInfo Target /* album title // guid on empty album */
      => new(string.IsNullOrWhiteSpace(Title) ? ID : Title);

    [JsonIgnore]
    [XmlIgnore]
    public FileInfo Storage /* target + gun extension for consistency */
      => new($"{Target}.gun");

    public FileInfo Download(YTDL YTDL)
    {
      return YTDL.Download(Video, new FileInfo(NewGuid().ToString()));
    }

    public void Encode(Toolkit toolkit)
    {
      var video = Video.Contains("https://youtu") ? Download(toolkit.YTDL) : new FileInfo(Video);

      if (!Target.Exists)
        Target.Create();

      foreach (var track in Tracks)
      {
        var encoded = track.Encode(toolkit, new FileInfo(Video));
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
      Video  = album.Video;
      Title  = album.Title;
      Tracks = album.Tracks;
    }

    public void Hydrate(FileInfo records, Metadata metadata)
    {
      var record = Record.Parse(records);

      Video = record.Video;
      Title = record.Title;

      foreach (var entry in record.Entries)
      {
        var track = new Track
        {
          Number   = entry.Number,
          Title    = entry.Title,
          Start    = entry.Start,
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
          var id   = GetFileNameWithoutExtension(Video) ?? string.Empty;
          var rule = new Regex("[a-zA-Z0-9_-]{11}");

          if (rule.IsMatch(id))
            track.Metadata.Comment = $"https://youtu.be/{id}";

          if (Video != null && Video.Contains("http"))
            track.Metadata.Comment = Video;
        }

        Tracks.Add(track);
      }

      foreach (var track in Tracks)
      {
        var next = Tracks.FindIndex(r => r.Number.Equals(track.Number));
        var end = next + 1 >= Tracks.Count
          ? string.Empty
          : Tracks[next + 1].Start;

        track.End = end;
      }
    }

    public void Compile(FileInfo records, Metadata metadata, ISerialisation serialisation)
    {
      if (!records.Extension.Contains("txt") || !records.Exists)
        throw new ArgumentException("A valid plaintext records file must exist.");

      Hydrate(records, metadata);
      Save(serialisation);
    }

    public class Record
    {
      public string      Title   { get; set; } = string.Empty;
      public string      Video   { get; set; } = string.Empty;
      public List<Entry> Entries { get; set; } = new();

      public static Record Parse(FileInfo file)
      {
        var record = new Record();
        var lines  = ReadAllLines(file.FullName);

        record.Title = lines[0];
        record.Video = lines[1];

        /* permit arbitrary number of blank lines between title+video & entries list */
        var index = 2;
        while (string.IsNullOrWhiteSpace(lines[index]))
          index++;

        /* entries parsing */
        for (var i = index; i < lines.Length; i++)
        {
          var entry = lines[i];
          var split = entry.Split(' ');

          record.Entries.Add(new Entry
          {
            Number = split[0],
            Start  = split[1],
            Title  = string.Join(' ', split.Skip(2))
          });
        }

        return record;
      }

      public class Entry
      {
        public string Number { get; set; } = string.Empty;
        public string Start  { get; set; } = string.Empty;
        public string Title  { get; set; } = string.Empty;
      }
    }
  }
}