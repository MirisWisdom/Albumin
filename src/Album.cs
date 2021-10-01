using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gunloader
{
  public class Album
  {
    [JsonPropertyName("source")]  public string       Source  { get; set; } = string.Empty;
    [JsonPropertyName("records")] public string       Records { get; set; } = string.Empty;
    [JsonPropertyName("title")]   public string       Title   { get; set; } = string.Empty;
    [JsonPropertyName("genre")]   public string       Genre   { get; set; } = string.Empty;
    [JsonPropertyName("comment")] public string       Comment { get; set; } = string.Empty;
    [JsonPropertyName("artists")] public List<string> Artists { get; set; } = new() {"Various Artists"};
  }
}