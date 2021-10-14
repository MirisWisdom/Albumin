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
using Gunloader.Programs;
using static System.Guid;
using static System.IO.File;
using static System.Text.Json.JsonSerializer;

namespace Gunloader
{
  public class Video
  {
    [JsonPropertyName("id")]       public string        ID       { get; set; } = string.Empty;
    [JsonPropertyName("url")]      public string        URL      { get; set; } = string.Empty;
    [JsonPropertyName("title")]    public string        Title    { get; set; } = string.Empty;
    [JsonPropertyName("chapters")] public List<Chapter> Chapters { get; set; } = new();

    public void Hydrate(FileInfo file)
    {
      Hydrate(ReadAllText(file.FullName));
    }

    public void Hydrate(YTDL ytdl, string source)
    {
      var metadata = ytdl.Metadata(source, new FileInfo(NewGuid().ToString()));
      Hydrate(metadata);
      metadata.Delete();
    }

    public void Hydrate(string json)
    {
      var video = Deserialize<Video>(json);

      ID       = video?.ID;
      Title    = video?.Title;
      Chapters = video?.Chapters;
      URL      = $"https://youtu.be/{video?.ID}";
    }

    public class Chapter
    {
      [JsonPropertyName("title")]      public string Title { get; set; } = string.Empty;
      [JsonPropertyName("start_time")] public double Start { get; set; } = 0.0;
      [JsonPropertyName("end_time")]   public double End   { get; set; } = 0.0;
    }
  }
}