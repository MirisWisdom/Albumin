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

using Gunloader.Common;
using Gunloader.Persistence;

namespace Gunloader.Programs
{
  public class Toolkit
  {
    public FFmpeg         FFmpeg        { get; set; } = new();
    public YTDL           YTDL          { get; set; } = new();
    public Encoder        Encoder       { get; set; } = new FLAC();
    public ISerialisation Serialisation { get; set; } = new JSON();
  }
}