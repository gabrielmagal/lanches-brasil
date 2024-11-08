namespace LanchesBrasil.Commons.Model
{
    public class AuthenticationKeycloak
    {
        public string? Path { get; set; }
        public string? Realm { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public AuthenticatioType AuthenticatioType { get; set; }

        public AuthenticationKeycloak()
        {
        }

        public AuthenticationKeycloak(string Path, string Realm)
        {
            this.Path = Path;
            this.Realm = Realm;
        }

        public AuthenticationKeycloak(string Path, string Realm, string ClientId, string ClientSecret, AuthenticatioType AuthenticatioType)
        {
            this.Path = Path;
            this.Realm = Realm;
            this.ClientId = ClientId;
            this.ClientSecret = ClientSecret;
            this.AuthenticatioType = AuthenticatioType;
        }

        public AuthenticationKeycloak(string Path, string Realm, string ClientId, string ClientSecret, string Username, string Password, AuthenticatioType AuthenticatioType)
        {
            this.Path = Path;
            this.Realm = Realm;
            this.ClientId = ClientId;
            this.ClientSecret = ClientSecret;
            this.Username = Username;
            this.Password = Password;
            this.AuthenticatioType = AuthenticatioType;
        }
    }

    public enum AuthenticatioType
    {
        CLIENT_CREDENTIALS, PASSWORD
    }
}
