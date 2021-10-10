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

using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Gunloader.Persistence;
using static System.Guid;

namespace Gunloader.Common
{
  public abstract class Persistent
  {
    [JsonIgnore] [XmlIgnore] public FileInfo Storage { get; set; } = new($"{NewGuid()}.ser");

    public abstract void Save(ISerialisation serialisation);
    public abstract void Load(ISerialisation serialisation);
  }
}