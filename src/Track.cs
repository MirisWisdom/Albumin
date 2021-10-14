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
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Gunloader.Encoders;

namespace Gunloader
{
  public class Track
  {
    [JsonPropertyName("title")]    public string   Title    { get; set; } = string.Empty;
    [JsonPropertyName("number")]   public string   Number   { get; set; } = string.Empty;
    [JsonPropertyName("metadata")] public Metadata Metadata { get; set; } = new();
    [JsonPropertyName("start")]    public string   Start    { get; set; } = string.Empty;
    [JsonPropertyName("end")]      public string   End      { get; set; } = string.Empty;

    /**
     * Normalise output filename for Windows & Linux systems.
     */

    [JsonIgnore]
    [XmlIgnore]
    public string Normalised => new[]
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
      current.Replace(character, ""));

    public FileInfo Encode(Toolkit toolkit, FileInfo video)
    {
      var      cover = toolkit.FFmpeg.ExtractCover(video, this);
      FileInfo encoded;

      if (toolkit.Encoder is RAW)
      {
        encoded = toolkit.Encoder.Encode(this, video, cover);
      }
      else
      {
        var audio = toolkit.FFmpeg.ExtractAudio(video, this);
        encoded = toolkit.Encoder.Encode(this, audio, cover);

        if (audio.Exists)
          audio.Delete();
      }

      if (cover.Exists)
        cover.Delete();

      return encoded;
    }
  }
}