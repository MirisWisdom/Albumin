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
using Gunloader.Encoders;
using Gunloader.Serialisation;
using Mono.Options;
using static System.Console;
using static System.Environment;

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
        _ => { Help(false); }
      },
      {
        "format=|encoding=",
        "audio encoding format; supported values: mp3, flac, vorbis, opus",
        s =>
        {
          if (s == null)
            return;

          switch (s)
          {
            case "mp3":
            case "lame":
              Toolkit.Encoder      = new LAME();
              Toolkit.FFmpeg.Lossy = true;
              break;
            case "flac":
              Toolkit.Encoder      = new FLAC();
              Toolkit.FFmpeg.Lossy = false;
              break;
            case "vorbis":
            case "oggenc":
              Toolkit.Encoder      = new Vorbis();
              Toolkit.FFmpeg.Lossy = true;
              break;
            case "opus":
            case "opusenc":
              Toolkit.Encoder      = new Opus();
              Toolkit.FFmpeg.Lossy = true;
              break;
            case "raw":
            case "original":
            case "no-encode":
              Toolkit.Encoder      = new RAW();
              Toolkit.FFmpeg.Lossy = false;
              break;
            default:
              WriteLine("Unknown format provided!");
              Exit(2);
              break;
          }
        }
      },
      {
        "album=|tracks=|records=|timestamps=|cue=",
        "path to album records file(s); multiple: --album 'abc.txt' --album 'xyz.txt'",
        s =>
        {
          var record = new FileInfo(s);

          if (record.Exists && record.Extension.Contains("txt"))
          {
            Records.Add(record);
            return;
          }

          WriteLine("Please provide a valid records file that exists on your system!");
          Exit(1);
        }
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
        "no-prompt",
        "don't prompt to review the generated .gun file",
        s =>
        {
          if (s != null)
            Prompt = false;
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
        s => Toolkit.Encoder = new LAME { Program = s }
      },
      {
        "flac=",
        "optional path to flac for flac encoding & tagging",
        s => Toolkit.Encoder = new FLAC { Program = s }
      },
      {
        "youtube-dl=|ytdl=",
        "optional path to youtube-dl for video downloading",
        s => Toolkit.YTDL.Program = s
      }
    };
  }
}