using Newtonsoft.Json;

namespace ProjetoChatGPT04_CSharp.Dtos
{
    public class MessageDTO
    {
        [JsonProperty("role")]
        public string? Role { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }
    }
}
