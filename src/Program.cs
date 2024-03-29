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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Albumin.Albums;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.IO.File;

namespace Albumin
{
  /**
   * Albumin Program.
   */
  public static partial class Program
  {
    public static bool           Prompt       { get; set; } = true;
    public static List<FileInfo> Records      { get; set; } = new();
    public static Metadata       Metadata     { get; set; } = new();
    public static Toolkit        Toolkit      { get; set; } = new();
    public static FileInfo       Instructions { get; set; }

    public static void Main(string[] args)
    {
      if (args.Length > 0)
        OptionSet.Parse(args);
      else
        Wizard();

      try
      {
        Invoke();
      }
      catch (Exception e)
      {
        ForegroundColor = Red;
        WriteLine(e);
        Exit(255);
      }
    }

    public static void Invoke()
    {
      if (Instructions != null)
      {
        var album = new Albums.Albumin();
        album.Load(Toolkit.Serialisation, Instructions);
        album.Encode(Toolkit);
        Exit(0);
      }
      
      if (Records.Count == 0)
      {
        WriteLine("Please provide at least one valid album records file.");
        Exit(1);
      }

      foreach (var record in Records)
      {
        var first = ReadLines(record.FullName).First();

        Album album = first.Contains("https") && first.Contains("youtu")
          ? new YouTube(first, Toolkit.YTDL)
          : new Albums.Albumin(record);

        album.Compile(Metadata, Toolkit.Serialisation);

        if (Prompt)
        {
          WriteLine($"Compiled '{album.Storage.Name}'.");
          WriteLine(@"Feel free to review/edit it, then press any key to continue...");
          ReadLine();
        }

        album.Load(Toolkit.Serialisation);
        album.Encode(Toolkit);
      }

      Exit(0);
    }
  }
}