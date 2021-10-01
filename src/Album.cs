using System.Text.Json.Serialization;

namespace Gunloader
{
  public class Album
  {
    [JsonPropertyName("source")]  public string Source  { get; set; }
    [JsonPropertyName("records")] public string Records { get; set; }
    [JsonPropertyName("title")]   public string Title   { get; set; }
  }
}