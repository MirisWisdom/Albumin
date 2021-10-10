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
using Gunloader.Batches;
using static System.Console;
using static System.Environment;
using Compiler = Gunloader.Albums.Compiler;
using Hydration = Gunloader.Albums.Hydration;

namespace Gunloader
{
  /**
   * Gunloader Program.
   */
  public static partial class Program
  {
    public static bool     Lossy    { get; set; } /* use mp3 instead of flac   */
    public static string   Source   { get; set; }
    public static FileInfo Record   { get; set; }
    public static FileInfo Batch    { get; set; }
    public static Metadata Metadata { get; set; } = new();
    public static Toolkit  Toolkit  { get; set; } = new();

    public static void Main(string[] args)
    {
      OptionSet.Parse(args);
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
      if (Record is {Exists: true} && Record.Extension.Contains("txt"))
      {
        var album = new Album {Video = Source, Title = Metadata.Album};
        new Compiler(Toolkit.Serialisation).Compile(new Hydration(Record, Metadata), album);

        Prompt(album.Storage.Name);

        album.Load(Toolkit.Serialisation);
        album.Encode(Toolkit);

        Exit(0);
      }

      if (Batch is {Exists: true} && Batch.Extension.Contains("txt"))
      {
        var batch = new Batch();
        new Batches.Compiler(Toolkit.Serialisation).Compile(new Batches.Hydration(Batch, Metadata), batch);

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