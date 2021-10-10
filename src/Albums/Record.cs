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

namespace Gunloader.Albums
{
  public class Record
  {
    public string Number { get; set; } = string.Empty;
    public string Start  { get; set; } = string.Empty;
    public string Title  { get; set; } = string.Empty;

    public static IEnumerable<Record> Parse(FileInfo file)
    {
      return ReadAllLines(file.FullName)
        .Select(record => record.Split(' '))
        .Select(split =>
          new Record
          {
            Number = split[0],
            Start  = split[1],
            Title  = string.Join(' ', split.Skip(2))
          })
        .ToList();
    }
  }
}