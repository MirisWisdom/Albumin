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

using System;
using Gunloader.Persistence;

namespace Gunloader.Batches
{
  public class Compiler : Common.Compiler
  {
    public Compiler(ISerialisation serialisation) : base(serialisation)
    {
      //
    }

    public void Compile(Hydration hydration, Batch batch)
    {
      if (!hydration.Records.Extension.Contains("txt") || !hydration.Records.Exists)
        throw new ArgumentException("A valid plaintext records file must exist.");

      hydration.Hydrate(batch);
      batch.Save(Serialisation);
    }
  }
}