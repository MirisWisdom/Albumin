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
using Gunloader.Albums;

namespace Gunloader.Batches
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

    public void Hydrate(Batch batch)
    {
      foreach (var record in Record.Parse(Records))
      {
        var album = new Album
        {
          Video = record.Source,
          Title = record.Title
        };

        new Albums.Hydration(new FileInfo(record.Tracks), Metadata).Hydrate(album);

        batch.Albums.Add(album);
      }
    }
  }
}