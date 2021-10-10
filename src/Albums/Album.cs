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

using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Gunloader.Common;
using Gunloader.Persistence;
using Gunloader.Programs;
using static System.Guid;
using static System.IO.Path;

namespace Gunloader.Albums
{
  public class Album : Persistent
  {
    [JsonPropertyName("video")]  public string      Video  { get; set; } = string.Empty;
    [JsonPropertyName("title")]  public string      Title  { get; set; } = string.Empty;
    [JsonPropertyName("tracks")] public List<Track> Tracks { get; set; } = new();

    [JsonIgnore]
    [XmlIgnore]
    public DirectoryInfo Target => new(string.IsNullOrWhiteSpace(Title) ? NewGuid().ToString() : Title);

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

    public override void Save(ISerialisation serialisation)
    {
      serialisation.Marshal(Storage, this);
    }

    public override void Load(ISerialisation serialisation)
    {
      var album = serialisation.Hydrate<Album>(Storage);
      Video  = album.Video;
      Title  = album.Title;
      Tracks = album.Tracks;
    }
  }
}