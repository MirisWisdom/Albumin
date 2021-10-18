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
using static System.DateTime;
using static System.Diagnostics.Process;
using static System.Globalization.CultureInfo;

namespace Gunloader.Programs
{
  public class FFmpeg
  {
    public string Program { get; set; } = "ffmpeg";
    public bool   Lossy   { get; set; }

    public FileInfo ExtractCover(FileInfo source, Track track)
    {
      if (!source.Exists)
        throw new FileNotFoundException("Could not extract cover from the video. Source file not found.");

      var output = new FileInfo($"{track.Number}.{(Lossy ? "jpg" : "png")}");

      var frame = ParseExact(track.Start, "H:mm:ss", InvariantCulture)
        .AddSeconds(30); /* (start time + 30 seconds) has correct thumbnail */

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"-ss {frame:H:mm:ss} " +
                    $"-y -i {source.Name} " +
                    "-vframes 1 "           +
                    $"{output.Name}"
      })?.WaitForExit();

      return output;
    }

    public FileInfo ExtractAudio(FileInfo source, Track track)
    {
      var output = new FileInfo($"{track.Number}.wav");

      if (!source.Exists)
        throw new FileNotFoundException("Could not extract audio from the video. Source file not found.");

      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"-ss {track.Start} "                                              +
                    $"{(!string.IsNullOrEmpty(track.End) ? $"-to {track.End}" : "")} " +
                    $"-y -i {source.Name} "                                            +
                    $"{track.Number}.wav"
      })?.WaitForExit();

      return output;
    }
  }
}