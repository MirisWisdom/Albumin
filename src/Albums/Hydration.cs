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
using System.Text.RegularExpressions;

namespace Gunloader.Albums
{
  public class Hydration
  {
    public Hydration(FileInfo records, Metadata metadata)
    {
      Records  = records;
      Metadata = metadata;
    }

    public FileInfo Records  { get; set; }
    public Metadata Metadata { get; set; }

    public void Hydrate(Album album)
    {
      foreach (var record in Record.Parse(Records))
      {
        var track = new Track
        {
          Number   = record.Number,
          Title    = record.Title,
          Start    = record.Start,
          Metadata = Metadata ?? new Metadata()
        };

        if (string.IsNullOrWhiteSpace(track.Metadata.Album))
          track.Metadata.Album = album.Title;

        /**
         * Assign Source's YouTube URL to blank Comments.
         *
         * If the Source file name is heuristically determined to be a YouTube ID, then its value will be used.
         *
         * YouTube ID requirements: 11 characters; allowed characters: alphanumeric, dashes and underscores.
         */

        if (string.IsNullOrWhiteSpace(track.Metadata.Comment))
        {
          var id   = Path.GetFileNameWithoutExtension(album.Video) ?? string.Empty;
          var rule = new Regex("[a-zA-Z0-9_-]{11}");

          if (rule.IsMatch(id))
            track.Metadata.Comment = $"https://youtu.be/{id}";

          if (album.Video != null && album.Video.Contains("http"))
            track.Metadata.Comment = album.Video;
        }

        album.Tracks.Add(track);
      }

      foreach (var track in album.Tracks)
      {
        var next = album.Tracks.FindIndex(r => r.Number.Equals(track.Number));
        var end = next + 1 >= album.Tracks.Count
          ? string.Empty
          : album.Tracks[next + 1].Start;

        track.End = end;
      }
    }
  }
}