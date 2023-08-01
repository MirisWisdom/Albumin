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

using System.Diagnostics;
using System.IO;
using static System.Diagnostics.Process;

namespace Albumin.Encoders
{
  public class FLAC : Encoder
  {
    public string Program { get; set; } = "flac";

    public override FileInfo Encode(Track track, FileInfo source = null, FileInfo cover = null)
    {
      source ??= new FileInfo(track.Number + ".wav");
      cover  ??= new FileInfo(track.Number + ".png");

      if (!source.Exists)
        throw new FileNotFoundException("Could not encode given track to FLAC. Source file not found.");

      /**
       * Encode the input audio into a FLAC with embedded art & metadata.
       */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = "--best "                                      +
                    $"--picture=\"{cover.Name}\" "                 +
                    $"--tag=TITLE=\"{track.Title}\" "              +
                    $"--tag=TRACKNUMBER=\"{track.Number}\" "       +
                    $"--tag=ALBUM=\"{track.Metadata.Album}\" "     +
                    $"--tag=GENRE=\"{track.Metadata.Genre}\" "     +
                    $"--tag=COMMENT=\"{track.Metadata.Comment}\" " +
                    (track.Metadata.Artists is {Count: > 0}
                      ? $"--tag=ARTIST=\"{string.Join(';', track.Metadata.Artists)}\" "
                      : string.Empty) +
                    $"{source.Name} "
      })?.WaitForExit();

      return new FileInfo(Path.GetFileNameWithoutExtension(source.FullName) + ".flac");
    }
  }
}