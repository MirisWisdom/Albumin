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
using static System.Console;
using static System.Environment;

namespace Gunloader
{
  /**
   * Gunloader Program.
   */
  public static partial class Program
  {
    public static List<FileInfo> Records  { get; set; } = new();
    public static Metadata       Metadata { get; set; } = new();
    public static Toolkit        Toolkit  { get; set; } = new();

    public static void Main(string[] args)
    {
      OptionSet.Parse(args);
      Invoke();
    }

    public static void Invoke()
    {
      if (Records.Count == 0)
      {
        WriteLine("Please provide at least one valid album records file.");
        Exit(1);
      }

      foreach (var record in Records)
      {
        var album = new Album();
        album.Compile(record, Metadata, Toolkit.Serialisation);

        WriteLine($"Compiled '{album.Storage.Name}'.");
        WriteLine(@"Feel free to review/edit it, then press any key to continue...");
        ReadLine();

        album.Load(Toolkit.Serialisation);
        album.Encode(Toolkit);
      }

      Exit(0);
    }
  }
}