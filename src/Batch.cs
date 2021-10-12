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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Gunloader.Serialisation;
using static System.DateTime;
using static System.Guid;
using static System.IO.File;

namespace Gunloader
{
  public class Batch
  {
    /* fallback static id for batch .gun file name */
    private static readonly string ID = NewGuid().ToString();

    [JsonPropertyName("albums")] public List<Album> Albums { get; set; } = new();

    [JsonIgnore]
    [XmlIgnore]
    public FileInfo Storage => /* date & album count // guid on empty album */
      new($"{(Albums.Count == 0 ? ID : $"{Now:yyyy-MM-dd} - {Albums.Count} Albums")}.gun");

    public virtual void Save(ISerialisation serialisation)
    {
      serialisation.Marshal(Storage, this);
    }

    public virtual void Load(ISerialisation serialisation)
    {
      var batch = serialisation.Hydrate<Batch>(Storage);
      Albums = batch.Albums;
    }

    public void Hydrate(FileInfo records, Metadata metadata)
    {
      var record = Record.Parse(records);

      foreach (var entry in record.Records)
      {
        var album = new Album();
        album.Hydrate(new FileInfo(entry), metadata);

        Albums.Add(album);
      }
    }

    public void Compile(FileInfo records, Metadata metadata, ISerialisation serialisation)
    {
      if (!records.Extension.Contains("txt") || !records.Exists)
        throw new ArgumentException("A valid plaintext records file must exist.");

      Hydrate(records, metadata);
      Save(serialisation);
    }

    public class Record
    {
      public List<string> Records { get; set; } = new();

      public static Record Parse(FileInfo file)
      {
        var record = new Record();

        foreach (var entry in ReadAllLines(file.FullName))
          record.Records.Add(entry);

        return record;
      }
    }
  }
}