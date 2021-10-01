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
using Mono.Options;

namespace Gunloader
{
  /**
   * Gunloader Program Arguments.
   */
  public static partial class Program
  {
    public static readonly OptionSet OptionSet = new()
    {
      {
        "help|about|version", "show program information, instructions, etc.",
        s =>
        {
          Console.WriteLine(@"                            __                __               ");
          Console.WriteLine(@"         ____ ___  ______  / /___  ____ _____/ /__  _____      ");
          Console.WriteLine(@"        / __ `/ / / / __ \/ / __ \/ __ `/ __  / _ \/ ___/      ");
          Console.WriteLine(@"       / /_/ / /_/ / / / / / /_/ / /_/ / /_/ /  __/ /          ");
          Console.WriteLine(@"       \__, /\__,_/_/ /_/_/\____/\__,_/\__,_/\___/_/           ");
          Console.WriteLine(@"      /____/                                                   ");
          Console.WriteLine(@"      ---------------------------------------------------      ");
          Console.WriteLine(@"      author miris ~ github yumiris/youtube.album.extract      ");
          Console.WriteLine(@"      ---------------------------------------------------      ");
          OptionSet.WriteOptionDescriptions(Console.Out);
          Environment.Exit(0);
        }
      },
      {
        "tracks=|records=|timestamps=|cue=", "path to records file with track numbers, timestamps and song titles",
        s => Records = new FileInfo(s)
      },
      {
        "source=|video=|compilation=|file=", "path to an already-downloaded video file containing the compiled songs",
        s => Source = new FileInfo(s)
      },
      {
        "download=", "download video from given url to use as the source for songs",
        s => Download = s
      },
      {
        "batch=", "download video from given url to use as the source for songs",
        s => Batch = new FileInfo(s)
      },
      {
        "album=", "album title to assign to the tracks' metadata; also, directory name to move tracks to",
        s => Album = s
      },
      {
        "artist=", "album artist(s) to assign to the tracks' metadata; multiple: --artist 'a' --artist 'b', etc.",
        s =>
        {
          Artists ??= new List<string>();
          Artists.Add(s);
        }
      },
      {
        "genre=", "genre to assign to the tracks' metadata",
        s => Genre = s
      },
      {
        "cover=", "path to album art image for assigning to songs",
        s => Cover = new FileInfo(s)
      },
      {
        "comment=", "comment to assign to the tracks' metadata",
        s => Comment = s
      },
      {
        "ffmpeg=", "optional path to ffmpeg for audio & cover extraction",
        s => FFmpeg = s
      },
      {
        "lame=", "optional path to lame for mp3 encoding & tagging",
        s => LAME = s
      },
      {
        "youtube-dl=|ytdl=", "optional path to youtube-dl for video downloading",
        s => YTDL = s
      }
    };
  }
}