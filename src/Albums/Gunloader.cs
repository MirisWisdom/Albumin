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

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Gunloader.Serialisation;
using static System.IO.File;
using static System.IO.Path;

namespace Gunloader.Albums
{
  public class Gunloader : Album
  {
    private readonly FileInfo _file;

    public Gunloader()
    {
      //
    }

    public Gunloader(FileInfo file)
    {
      _file = file;
    }

    public override void Hydrate(Metadata metadata)
    {
      var records = ReadAllLines(_file.FullName)
        .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#') && !line.StartsWith(';'))
        .ToArray();

      Title  = records[0].Trim();
      Source = records[1].Trim();

      /**
       * Parse the Records file when the first line is NOT a YouTube video, or when no Tracks have been successfully
       * inferred from YouTube chapters.
       */

      for (var i = 2; i < records.Skip(2).Count(); i++)
      {
        var split = records[i].Split(' ');
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

    public override void Save(ISerialisation serialisation)
    {
      serialisation.Marshal(Storage, this);
    }

    public override void Load(ISerialisation serialisation)
    {
      var album = serialisation.Hydrate<Gunloader>(Storage);
      Source = album.Source;
      Title  = album.Title;
      Tracks = album.Tracks;
    }
  }
}