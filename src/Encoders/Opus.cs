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
using static System.IO.Path;

namespace Albumin.Encoders
{
  public class Opus : Encoder
  {
    public string Program { get; set; } = "opusenc";

    public override FileInfo Encode(Track track, FileInfo source = null, FileInfo cover = null)
    {
      source ??= new FileInfo(track.Number + ".wav");
      cover  ??= new FileInfo(track.Number + ".png");
      var output = new FileInfo(GetFileNameWithoutExtension(source.FullName) + ".opus");

      if (!source.Exists)
        throw new FileNotFoundException("Could not encode given track to Opus. Source file not found.");

      /**
       * Encode the input audio into an Opus with embedded art & metadata.
       */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = "--vbr "                                               +
                    $"--picture {cover.Name} "                             +
                    $"--title \"{track.Title}\" "                          +
                    $"--tracknumber \"{track.Number}\" "                   +
                    $"--album \"{track.Metadata.Album}\" "                 +
                    $"--genre \"{track.Metadata.Genre}\" "                 +
                    $"--comment \"DESCRIPTION={track.Metadata.Comment}\" " +
                    (track.Metadata.Artists is {Count: > 0}
                      ? $"--artist \"{string.Join(';', track.Metadata.Artists)}\" "
                      : string.Empty) +
                    $"{source.Name} {output.Name} "
      })?.WaitForExit();

      return output;
    }
  }
}