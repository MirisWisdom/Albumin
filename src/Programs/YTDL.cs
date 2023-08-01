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
using static System.Environment;
using static System.IO.Directory;

namespace Albumin.Programs
{
  public class YTDL
  {
    public string Program { get; set; } = "yt-dlp";

    public FileInfo GetVideo(string url, FileInfo output)
    {
      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"{url} " +
                    $"--output {output.Name}.%(ext)s"
      })?.WaitForExit();

      /**
       * Infer file extension of the downloaded file.
       */

      return new FileInfo(GetFiles(CurrentDirectory, $"{output.Name}*")[0]);
    }

    public FileInfo GetAudio(string url, FileInfo output)
    {
      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"{url} "                 +
                    @"-x --format bestaudio " +
                    $"--output {output.Name}.%(ext)s"
      })?.WaitForExit();

      /**
       * Infer file extension of the downloaded file.
       */

      return new FileInfo(GetFiles(CurrentDirectory, $"{output.Name}*")[0]);
    }

    public FileInfo Metadata(string url, FileInfo output)
    {
      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"{url} "                             +
                    @"--skip-download --write-info-json " +
                    $"--output {output.Name}"
      })?.WaitForExit();

      /**
       * Infer file extension of the downloaded file.
       */

      return new FileInfo(GetFiles(CurrentDirectory, $"{output.Name}*.json")[0]);
    }
  }
}