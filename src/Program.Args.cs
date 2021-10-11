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
using System.IO;
using Gunloader.Encoders;
using Gunloader.Serialisation;
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
        "help|about|version",
        "show program information, instructions, etc.",
        _ =>
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
        "lossy",
        "use lossy mp3 encoding instead of flac (and also jpeg instead of png for cover art)",
        s =>
        {
          if (s == null)
            return;

          Lossy                = true;
          Toolkit.Encoder      = new LAME();
          Toolkit.FFmpeg.Lossy = true;
        }
      },
      {
        "tracks=|records=|timestamps=|cue=",
        "path to records file with track numbers, timestamps and song titles",
        s => Record = new FileInfo(s)
      },
      {
        "source=|video=|compilation=|file=",
        "path to the video containing the compiled songs (can be a youtube video or local file)",
        s => Source = s
      },
      {
        "batch=",
        "download video from given url to use as the source for songs",
        s => Batch = new FileInfo(s)
      },
      {
        "album=",
        "album title to assign to the tracks' metadata; also, directory name to move tracks to",
        s => Metadata.Album = s
      },
      {
        "artist=",
        "album artist(s) to assign to the tracks' metadata; multiple: --artist 'a' --artist 'b', etc.",
        s => Metadata.Artists.Add(s)
      },
      {
        "genre=",
        "genre to assign to the tracks' metadata",
        s => Metadata.Genre = s
      },
      {
        "cover=",
        "path to album art image for assigning to songs",
        s => Metadata.Cover = s
      },
      {
        "comment=",
        "comment to assign to the tracks' metadata",
        s => Metadata.Comment = s
      },
      {
        "xml",
        "use xml format instead of json",
        s =>
        {
          if (s != null)
            Toolkit.Serialisation = new XML();
        }
      },
      {
        "ffmpeg=",
        "optional path to ffmpeg for audio & cover extraction",
        s => Toolkit.FFmpeg.Program = s
      },
      {
        "lame=",
        "optional path to lame for mp3 encoding & tagging",
        s => Toolkit.Encoder = new LAME {Program = s}
      },
      {
        "flac=",
        "optional path to flac for flac encoding & tagging",
        s => Toolkit.Encoder = new FLAC {Program = s}
      },
      {
        "youtube-dl=|ytdl=",
        "optional path to youtube-dl for video downloading",
        s => Toolkit.YTDL.Program = s
      }
    };
  }
}