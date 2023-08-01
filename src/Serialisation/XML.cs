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

using System.IO;
using System.Xml.Serialization;
using static System.IO.File;

namespace Albumin.Serialisation
{
  public class XML : ISerialisation
  {
    public T Hydrate<T>(FileInfo source)
    {
      if (!source.Exists)
        throw new FileNotFoundException("Could not deserialise XML. Source file not found.");

      var       xmlSerializer = new XmlSerializer(typeof(T));
      using var textReader    = new StringReader(ReadAllText(source.FullName));
      return (T)xmlSerializer.Deserialize(textReader);
    }

    public void Marshal<T>(FileInfo target, T entity)
    {
      var       xmlSerializer = new XmlSerializer(typeof(T));
      using var textWriter    = new StringWriter();
      xmlSerializer.Serialize(textWriter, entity);
      WriteAllText(target.FullName, textWriter.ToString());
    }
  }
}