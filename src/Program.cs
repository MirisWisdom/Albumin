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
using static System.Console;
using static System.Environment;

namespace Gunloader
{
  /**
   * Gunloader Program.
   */
  public static partial class Program
  {
    public static bool     Interactive { get; set; }
    public static string   Source      { get; set; }
    public static FileInfo Record      { get; set; }
    public static FileInfo Batch       { get; set; }
    public static Metadata Metadata    { get; set; } = new();
    public static Toolkit  Toolkit     { get; set; } = new();

    public static void Main(string[] args)
    {
      OptionSet.Parse(args);

      if (Interactive)
        Wizard();

      Invoke();
    }

    public static void Prompt(string entity)
    {
      WriteLine(new string('-', 80));
      WriteLine($"Compiled '{entity}'.");
      WriteLine(@"Feel free to review/edit it, then press any key to continue...");
      WriteLine(new string('-', 80));
      ReadLine();
    }

    public static void Invoke()
    {
      if (Record != null)
      {
        var album = new Album {Video = Source, Title = Metadata.Album};
        album.Compile(Record, Metadata, Toolkit.Serialisation);

        Prompt(album.Storage.Name);

        album.Load(Toolkit.Serialisation);
        album.Encode(Toolkit);

        Exit(0);
      }

      if (Batch != null)
      {
        var batch = new Batch();
        batch.Compile(Batch, Metadata, Toolkit.Serialisation);

        Prompt(batch.Storage.Name);

        batch.Load(Toolkit.Serialisation);

        foreach (var album in batch.Albums)
          album.Encode(Toolkit);

        Exit(0);
      }

      WriteLine("Please provide a valid records or batch file.");
      Exit(1);
    }
  }
}