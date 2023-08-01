/**
 * Copyright (C) 2021 Miris Wisdom
 * 
 * This file is part of Albumin.
 * 
 * Albumin is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; version 2.
 * 
 * Albumin is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Albumin.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Albumin.Encoders;
using static System.IO.File;

namespace Albumin
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
      if (toolkit.Encoder is RAW)
        return toolkit.Encoder.Encode(this, video);

      FileInfo cover;
      bool     extracted;

      if (!Exists(Metadata.Cover))
      {
        cover     = toolkit.FFmpeg.ExtractCover(video, this);
        extracted = true;
      }
      else
      {
        cover     = new FileInfo(Metadata.Cover);
        extracted = false;
      }

      var audio   = toolkit.FFmpeg.ExtractAudio(video, this);
      var encoded = toolkit.Encoder.Encode(this, audio, cover);

      if (audio.Exists)
        audio.Delete();

      if (cover.Exists && extracted)
        cover.Delete();

      return encoded;
    }
  }
}