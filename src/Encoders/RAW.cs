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
  public class RAW : Encoder
  {
    public string Program { get; set; } = "ffmpeg";

    public override FileInfo Encode(Track track, FileInfo audio = null, FileInfo cover = null)
    {
      audio ??= new FileInfo(track.Number + ".wav");
      var output = $"{GetFileNameWithoutExtension(track.Number)}{audio.Extension}";

      if (!audio.Exists)
        throw new FileNotFoundException("Could not extract the track from the audio. Source file not found.");

      /**
       * Split input audio into separate files of the same type, without any form of re-encoding.
       */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"-ss {track.Start} "                                                  +
                    $"{(string.IsNullOrWhiteSpace(track.End) ? "" : $"-to {track.End}")} " +
                    $"-i {audio.Name} "                                                    +
                    "-c:a copy "                                                           +
                    $"{output}"
      })?.WaitForExit();

      return new FileInfo(output);
    }
  }
}