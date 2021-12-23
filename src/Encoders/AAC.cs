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
  public class AAC : Encoder
  {
    public string Program { get; set; } = "ffmpeg";

    public override FileInfo Encode(Track track, FileInfo source = null, FileInfo cover = null)
    {
      source ??= new FileInfo(track.Number + ".wav");
      var output = new FileInfo(GetFileNameWithoutExtension(source.FullName) + ".m4a");

      if (!source.Exists)
        throw new FileNotFoundException("Could not encode given track to AAC. Source file not found.");

      /**
       * Encode the input audio into an AAC with embedded metadata.
       */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"-i {source.Name} "                                                +
                    @"-c:a aac "                                                        +
                    @"-q:a 2 "                                                          +
                    $"-metadata title=\"{track.Title}\" "                               +
                    $"-metadata track=\"{track.Number}\" "                              +
                    $"-metadata album=\"{track.Metadata.Album}\" "                      +
                    $"-metadata genre=\"{track.Metadata.Genre}\" "                      +
                    $"-metadata comment=\"{track.Metadata.Comment}\" "                  +
                    $"-metadata artist=\"{string.Join(';', track.Metadata.Artists)}\" " +
                    $"{output.Name}"
      })?.WaitForExit();

      return output;
    }
  }
}