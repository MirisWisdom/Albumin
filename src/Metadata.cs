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

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gunloader
{
  public class Metadata
  {
    [JsonPropertyName("album")]   public string       Album   { get; set; } = string.Empty;
    [JsonPropertyName("cover")]   public string       Cover   { get; set; } = string.Empty;
    [JsonPropertyName("genre")]   public string       Genre   { get; set; } = string.Empty;
    [JsonPropertyName("comment")] public string       Comment { get; set; } = string.Empty;
    [JsonPropertyName("artists")] public List<string> Artists { get; set; } = new();
  }
}