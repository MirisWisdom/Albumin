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
using static System.IO.Path;
using static System.TimeSpan;

namespace Gunloader
{
  public class Compiler
  {
    public Compiler(Toolkit toolkit)
    {
      Toolkit = toolkit;
    }

    public Toolkit Toolkit { get; }

    public Album Compile(FileInfo file, Metadata metadata, bool query = false)
    {
      var record = new Record(file, true);
      var album  = new Album();

      if (query)
        Hydrate(album, metadata, new Video(record.Source, Toolkit.YTDL, true));
      else
        Hydrate(album, metadata, record);

      album.Save(Toolkit.Serialisation);
      return album;
    }

    /**
     * Hydrate Album using data from the given Video and Metadata.
     */
    public void Hydrate(Album album, Metadata metadata, Video video)
    {
      album.Source = video.Source;
      album.Title  = video.Title;

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
          track.Metadata.Comment = album.Source;

        if (string.IsNullOrWhiteSpace(track.Metadata.Album))
          track.Metadata.Album = album.Title;

        album.Tracks.Add(track);
      }
    }

    /**
     * Hydrate Album using data from the given Record file and Metadata.
     */
    public void Hydrate(Album album, Metadata metadata, Record record)
    {
      album.Title  = record.Title;
      album.Source = record.Source;

      foreach (var entry in record.Entries)
      {
        var track = new Track
        {
          Number   = entry.Number,
          Start    = entry.Start,
          Title    = entry.Title,
          Metadata = metadata ?? new Metadata()
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
          var id   = GetFileNameWithoutExtension(album.Source) ?? string.Empty;
          var rule = new Regex("[a-zA-Z0-9_-]{11}");

          if (rule.IsMatch(id))
            track.Metadata.Comment = $"https://youtu.be/{id}";

          if (album.Source != null && album.Source.Contains("http"))
            track.Metadata.Comment = album.Source;
        }

        album.Tracks.Add(track);
      }

      /**
       * Infer each Track's ending time. Current Track's ending time = next Track's starting time.
       */

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