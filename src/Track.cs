using System.Text.Json.Serialization;

namespace Gunloader
{
  public class Track
  {
    [JsonPropertyName("track")] public string Number { get; set; }
    [JsonPropertyName("start")] public string Start  { get; set; }
    [JsonPropertyName("title")] public string Title  { get; set; }
  }
}