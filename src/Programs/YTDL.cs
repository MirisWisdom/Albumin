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
using static System.Environment;
using static System.IO.Directory;

namespace Gunloader.Programs
{
  public class YTDL
  {
    public string Program { get; set; } = "youtube-dl";

    public FileInfo Download(string download, FileInfo output)
    {
      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"{download} " +
                    $"--output {output.Name}"
      })?.WaitForExit();

      /**
       * Infer file extension of the downloaded file.
       */

      return new FileInfo(GetFiles(CurrentDirectory, $"{output.Name}*")[0]);
    }

    public FileInfo Metadata(string download, FileInfo output)
    {
      Start(new ProcessStartInfo
      {
        FileName = Program,
        Arguments = $"{download} "                        +
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