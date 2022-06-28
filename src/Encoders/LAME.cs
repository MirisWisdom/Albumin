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

namespace Gunloader.Encoders
{
  public class LAME : Encoder
  {
    public string Program { get; set; } = "lame";

    public override FileInfo Encode(Track track, FileInfo source = null, FileInfo cover = null)
    {
      source ??= new FileInfo(track.Number + ".wav");
      cover  ??= new FileInfo(track.Number + ".png");

      if (!source.Exists)
        throw new FileNotFoundException("Could not encode given track to MP3. Source file not found.");

      /**
       * Encode the input audio into an MP3 with embedded art & metadata.
       */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = "--vbr-new "                          +
                    $"--ti {cover.Name} "                 +
                    $"--tt \"{track.Title}\" "            +
                    $"--tn \"{track.Number}\" "           +
                    $"--tl \"{track.Metadata.Album}\" "   +
                    $"--tg \"{track.Metadata.Genre}\" "   +
                    $"--tc \"{track.Metadata.Comment}\" " +
                    (track.Metadata.Artists is {Count: > 0}
                      ? $"--tv \"TPE2={string.Join(';', track.Metadata.Artists)}\" "
                      : string.Empty) +
                    $"{source.Name} "
      })?.WaitForExit();

      return new FileInfo(Path.GetFileNameWithoutExtension(source.FullName) + ".mp3");
    }
  }
}