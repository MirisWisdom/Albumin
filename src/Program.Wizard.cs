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
using System.Linq;
using Gunloader.Encoders;
using static System.Console;

namespace Gunloader
{
  /**
   * Gunloader Program.
   */
  public static partial class Program
  {
    private static void Wizard()
    {
      WriteLine("Welcome to Gunloader! This wizard will help you set up everything you need.");

      WriteLine("First, please specify your preferred format, otherwise press enter to use MP3");
      Write("[format] :: ");

      switch (ReadLine())
      {
        case "mp3":
        case "lame":
          Toolkit.Encoder      = new LAME();
          Toolkit.FFmpeg.Lossy = true;
          WriteLine();
          WriteLine("Will be using lame to encode your MP3 files! Please make sure it's installed and accessible.");
          break;
        case "flac":
          Toolkit.Encoder      = new FLAC();
          Toolkit.FFmpeg.Lossy = false;
          WriteLine("Will be using flac to encode your FLAC files! Please make sure it's installed and accessible.");
          break;
        case "vorbis":
        case "oggenc":
          Toolkit.Encoder      = new Vorbis();
          Toolkit.FFmpeg.Lossy = true;
          WriteLine("Will be using oggenc to encode your FLAC files! Please make sure it's installed and accessible.");
          break;
        case "opus":
        case "opusenc":
          Toolkit.Encoder      = new Opus();
          Toolkit.FFmpeg.Lossy = false;
          WriteLine("Will be using opusenc to encode your FLAC files! Please make sure it's installed and accessible.");
          break;
        default:
          Toolkit.Encoder      = new LAME();
          Toolkit.FFmpeg.Lossy = true;
          WriteLine("Will be using lame to encode your MP3 files! Please make sure it's installed and accessible.");
          break;
      }

      while (string.IsNullOrWhiteSpace(Source) || !Source.Contains("http") && !new FileInfo(Source).Exists)
      {
        WriteLine("Please provide a valid source (either a YouTube URL or local path to an existing file:");
        Write("[source] :: ");
        Source = ReadLine();
        WriteLine();
      }

      WriteLine("Now, we will start with the songs:");

      Write("[album] :: ");
      Metadata.Album = ReadLine();
      WriteLine();

      Write("[genre] :: ");
      Metadata.Genre = ReadLine();
      WriteLine();

      Write("[cover] :: ");
      Metadata.Cover = ReadLine();
      WriteLine();

      Write("[comment] :: ");
      Metadata.Comment = ReadLine();
      WriteLine();

      Write("[artists] (use ; for separating multiple) :: ");
      Metadata.Artists = $"{ReadLine()};".Split(';').ToList();
      WriteLine();
    }
  }
}