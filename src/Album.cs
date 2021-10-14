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
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Gunloader.Programs;
using Gunloader.Serialisation;
using static System.Guid;
using static System.IO.Path;

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
      var video = Source.Contains("http")
        ? Download(toolkit.YTDL)
        : new FileInfo(Source);

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
  }
}