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
using System.Text.Json;
using static System.IO.File;
using static System.Text.Json.JsonSerializer;

namespace Gunloader.Serialisation
{
  public class JSON : ISerialisation
  {
    public T Hydrate<T>(FileInfo source)
    {
      if (!source.Exists)
        throw new FileNotFoundException("Could not deserialise JSON. Source file not found.");

      return Deserialize<T>(ReadAllText(source.FullName));
    }

    public void Marshal<T>(FileInfo target, T entity)
    {
      WriteAllText(target.FullName, Serialize(entity, new JsonSerializerOptions { WriteIndented = true }));
    }
  }
}