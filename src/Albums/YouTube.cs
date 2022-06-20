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
using Gunloader.Programs;
using Gunloader.Serialisation;
using static System.Guid;
using static System.IO.File;
using static System.Text.Json.JsonSerializer;
using static System.TimeSpan;

namespace Gunloader.Albums
{
  public class YouTube : Album
  {
    private readonly string _url;
    private readonly YTDL   _ytdl;

    public YouTube()
    {
      //
    }

    public YouTube(string url, YTDL ytdl)
    {
      _url  = url;
      _ytdl = ytdl;
    }

    public override void Hydrate(Metadata metadata)
    {
      var video = new Video();
      video.Load(_url, _ytdl);

      Title  = video.Title;
      Source = video.URL;

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
          track.Metadata.Comment = Source;

        if (string.IsNullOrWhiteSpace(track.Metadata.Album))
          track.Metadata.Album = Title;

        Tracks.Add(track);
      }
    }

    public override void Save(ISerialisation serialisation)
    {
      Save(serialisation, Storage);
    }

    public override void Save(ISerialisation serialisation, FileInfo instructions)
    {
      serialisation.Marshal(instructions, this);
    }

    public override void Load(ISerialisation serialisation)
    {
      Load(serialisation, Storage);
    }

    public override void Load(ISerialisation serialisation, FileInfo instructions)
    {
      var album = serialisation.Hydrate<YouTube>(instructions);
      Source = album.Source;
      Title  = album.Title;
      Tracks = album.Tracks;
    }

    public class Video
    {
      [JsonPropertyName("id")]       public string        ID       { get; set; } = string.Empty;
      [JsonPropertyName("title")]    public string        Title    { get; set; } = string.Empty;
      [JsonPropertyName("chapters")] public List<Chapter> Chapters { get; set; } = new();
      [JsonIgnore] [XmlIgnore]       public string        URL      => $"https://youtu.be/{ID}";

      public void Load(string source, YTDL ytdl)
      {
        var metadata = ytdl.Metadata(source, new FileInfo(NewGuid().ToString()));
        var video    = Deserialize<Video>(ReadAllText(metadata.FullName));

        ID       = video?.ID;
        Title    = video?.Title;
        Chapters = video?.Chapters;

        metadata.Delete();
      }

      public class Chapter
      {
        [JsonPropertyName("title")]      public string Title { get; set; } = string.Empty;
        [JsonPropertyName("start_time")] public double Start { get; set; } = 0.0;
        [JsonPropertyName("end_time")]   public double End   { get; set; } = 0.0;
      }
    }
  }
}