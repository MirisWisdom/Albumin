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
using Gunloader.Encoders;
using Gunloader.Serialisation;
using static System.Guid;
using static System.IO.File;
using static System.IO.Path;

namespace Gunloader.Albums
{
  public abstract class Album
  {
    /* fallback static id for album directory & .gun file name */
    protected static readonly string UUID = NewGuid().ToString();

    [JsonPropertyName("video")]  public string      Source { get; set; } = string.Empty;
    [JsonPropertyName("title")]  public string      Title  { get; set; } = string.Empty;
    [JsonPropertyName("tracks")] public List<Track> Tracks { get; set; } = new();

    [JsonIgnore]
    [XmlIgnore]
    public DirectoryInfo Target /* normalised album title // guid on empty album */
      => new(string.IsNullOrWhiteSpace(Title)
        ? UUID
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

    public void Encode(Toolkit toolkit)
    {
      FileInfo source;

      if (Source.Contains("http"))
        source = toolkit.Encoder is RAW || Tracks.All(track => Exists(track.Metadata.Cover))
          ? toolkit.YTDL.GetAudio(Source, new FileInfo($"AUDIO~{NewGuid().ToString()}"))
          : toolkit.YTDL.GetVideo(Source, new FileInfo($"VIDEO~{NewGuid().ToString()}"));
      else
        source = new FileInfo(Source);

      if (!Target.Exists)
        Target.Create();

      foreach (var track in Tracks)
      {
        var encoded = track.Encode(toolkit, source);
        var final   = Combine(Target.FullName, $"{track.Number}. {track.Normalised}{encoded.Extension}");

        encoded.MoveTo(final);
        encoded.CreationTimeUtc   = source.CreationTimeUtc;
        encoded.LastAccessTimeUtc = source.LastAccessTimeUtc;
        encoded.LastWriteTimeUtc  = source.LastWriteTimeUtc;
      }

      Target.CreationTimeUtc   = source.CreationTimeUtc;
      Target.LastAccessTimeUtc = source.LastAccessTimeUtc;
      Target.LastWriteTimeUtc  = source.LastWriteTimeUtc;
    }

    public abstract void Save(ISerialisation serialisation);

    public abstract void Load(ISerialisation serialisation);

    public abstract void Hydrate(Metadata metadata);

    public void Compile(Metadata metadata, ISerialisation serialisation)
    {
      Hydrate(metadata);
      Save(serialisation);
    }
  }
}