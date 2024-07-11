using Newtonsoft.Json;

namespace Ciara.Entities;

public record OllamaChatRequest([property: JsonProperty("model")] string Model, [property: JsonProperty("messages")] OllamaChatMessage[] Messages, [property: JsonProperty("stream")] bool Stream = false);

public record OllamaChatResponse([property: JsonProperty("model")] string Model, [property: JsonProperty("message")] OllamaChatMessage Message);
public record OllamaChatMessage([property: JsonProperty("role")] string Role, [property: JsonProperty("content")] string Message, [property: JsonProperty("images")] string[]? Images = null);