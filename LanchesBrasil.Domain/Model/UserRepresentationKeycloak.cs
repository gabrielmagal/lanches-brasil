using System.Text.Json.Serialization;

namespace LanchesBrasil.Commons.Model
{
    public class UserRepresentationKeycloak
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("createdTimestamp")]
        public long CreatedTimestamp { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("totp")]
        public bool Totp { get; set; }

        [JsonPropertyName("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("disableableCredentialTypes")]
        public IEnumerable<string>? DisableableCredentialTypes { get; set; }

        [JsonPropertyName("requiredActions")]
        public IEnumerable<string>? RequiredActions { get; set; }

        [JsonPropertyName("notBefore")]
        public int NotBefore { get; set; }

        [JsonPropertyName("access")]
        public AccessRepresentationKeycloak? Access { get; set; }
    }
}
