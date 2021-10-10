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

using System.Diagnostics;
using System.IO;
using Gunloader.Common;
using static System.Diagnostics.Process;

namespace Gunloader.Encoders
{
  public class FLAC : Encoder
  {
    public string Program { get; set; } = "flac";

    public override FileInfo Encode(Track track, FileInfo source = null, FileInfo cover = null)
    {
      source ??= new FileInfo(track.Number + ".wav");
      cover  ??= new FileInfo(track.Number + ".png");

      /**
       * Encode the input audio into a FLAC with embedded art & metadata.
       */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = "--best "                                                       +
                    $"--picture=\"{cover.Name}\" "                                  +
                    $"--tag=TITLE=\"{track.Title}\" "                               +
                    $"--tag=TRACKNUMBER=\"{track.Number}\" "                        +
                    $"--tag=ALBUM=\"{track.Metadata.Album}\" "                      +
                    $"--tag=GENRE=\"{track.Metadata.Genre}\" "                      +
                    $"--tag=COMMENT=\"{track.Metadata.Comment}\" "                  +
                    $"--tag=ARTIST=\"{string.Join(';', track.Metadata.Artists)}\" " +
                    $"{source.Name} "
      })?.WaitForExit();

      return new FileInfo(Path.GetFileNameWithoutExtension(source.FullName) + ".flac");
    }
  }
}