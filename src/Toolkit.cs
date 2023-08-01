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

using Albumin.Encoders;
using Albumin.Programs;
using Albumin.Serialisation;

namespace Albumin
{
  public class Toolkit
  {
    public FFmpeg         FFmpeg        { get; set; } = new();
    public YTDL           YTDL          { get; set; } = new();
    public Encoder        Encoder       { get; set; } = new LAME();
    public ISerialisation Serialisation { get; set; } = new JSON();
  }
}