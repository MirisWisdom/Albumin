using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gunloader
{
  public class Track
  {
    [JsonPropertyName("track")]   public string       Number  { get; set; } = string.Empty;
    [JsonPropertyName("start")]   public string       Start   { get; set; } = string.Empty;
    [JsonPropertyName("title")]   public string       Title   { get; set; } = string.Empty;
    [JsonPropertyName("album")]   public string       Album   { get; set; } = string.Empty;
    [JsonPropertyName("genre")]   public string       Genre   { get; set; } = string.Empty;
    [JsonPropertyName("comment")] public string       Comment { get; set; } = string.Empty;
    [JsonPropertyName("artists")] public List<string> Artists { get; set; } = new() {"Various Artists"};
  }
}