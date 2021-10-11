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
using static System.Diagnostics.Process;
using static System.IO.Path;

namespace Gunloader.Encoders
{
  public class Opus : Encoder
  {
    public string Program { get; set; } = "opusenc";

    public override FileInfo Encode(Track track, FileInfo source = null, FileInfo cover = null)
    {
      source ??= new FileInfo(track.Number + ".wav");
      cover  ??= new FileInfo(track.Number + ".png");
      var output = new FileInfo(GetFileNameWithoutExtension(source.FullName) + ".opus");

      /**
       * Encode the input audio into an Opus with embedded art & metadata.
       */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = "--vbr "                                                    +
                    $"--picture {cover.Name} "                                  +
                    $"--title \"{track.Title}\" "                               +
                    $"--tracknumber \"{track.Number}\" "                        +
                    $"--album \"{track.Metadata.Album}\" "                      +
                    $"--genre \"{track.Metadata.Genre}\" "                      +
                    $"--comment \"DESCRIPTION={track.Metadata.Comment}\" "      +
                    $"--artist \"{string.Join(';', track.Metadata.Artists)}\" " +
                    $"{source.Name} {output.Name} "
      })?.WaitForExit();

      return output;
    }
  }
}