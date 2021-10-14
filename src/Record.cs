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
using static System.IO.File;

namespace Gunloader
{
  public class Record
  {
    public Record(FileInfo file, bool load = false)
    {
      File = file;

      if (load)
        Load();
    }

    private FileInfo          File    { get; }
    public  string            Title   { get; set; } = string.Empty;
    public  string            Source  { get; set; } = string.Empty;
    public  List<RecordEntry> Entries { get; set; } = new();

    public void Load()
    {
      var lines = ReadAllLines(File.FullName)
        .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#') && !line.StartsWith(';'))
        .ToArray();

      Title  = lines[0].Trim();
      Source = lines[1].Trim();

      /* entries parsing */
      for (var i = 2; i < lines.Length; i++)
      {
        var entry = lines[i];
        var split = entry.Split(' ');

        Entries.Add(new RecordEntry
        {
          Number = split[0].Trim(),
          Start  = split[1].Trim(),
          Title  = string.Join(' ', split.Skip(2)).Trim()
        });
      }
    }

    public class RecordEntry
    {
      public string Number { get; set; } = string.Empty;
      public string Start  { get; set; } = string.Empty;
      public string Title  { get; set; } = string.Empty;
    }
  }
}