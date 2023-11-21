using System.Text.Json.Serialization;

namespace Contracts.Models
{
    public class AuthorizationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; init; }

        [JsonPropertyName("password")]
        public string Password { get; init; }
    }
}
