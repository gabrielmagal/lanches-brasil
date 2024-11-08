using System.Text;
using System.Text.Json;
using LanchesBrasil.Commons.Model;
using LanchesBrasil.Commons.Service.Interfaces;

namespace LanchesBrasil.Commons.Service
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient = new();
        private readonly AuthenticationKeycloak AuthenticationKeycloak;
        private readonly ResultResponse Token;

        public KeycloakService()
        {
            AuthenticationKeycloak = new("http://localhost:8083", "lanchesbrasil", "lanchesbrasil-api", "HA6pSOO9BbHUzJIiyfG7iMDnQ6BxZUcp", AuthenticatioType.CLIENT_CREDENTIALS);
            Token = GetTokenAsync(AuthenticationKeycloak);
        }

        public ResultResponse GetTokenAsync(AuthenticationKeycloak authenticationKeycloak)
        {
            var tokenEndpoint = $"{authenticationKeycloak.Path}/realms/{authenticationKeycloak.Realm}/protocol/openid-connect/token";

            StringContent? requestBody = null;
            switch (authenticationKeycloak.AuthenticatioType)
            {
                case AuthenticatioType.CLIENT_CREDENTIALS:
                    requestBody = new StringContent(
                        $"client_id={authenticationKeycloak.ClientId}&client_secret={authenticationKeycloak.ClientSecret}&grant_type=client_credentials",
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded"
                    );
                break;

                case AuthenticatioType.PASSWORD:
                    requestBody = new StringContent(
                        $"client_id={authenticationKeycloak.ClientId}&client_secret={authenticationKeycloak.ClientSecret}&username={authenticationKeycloak.Username}&password={authenticationKeycloak.Password}&grant_type=password",
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded"
                    );
                break;
            }

            var response = _httpClient.PostAsync(tokenEndpoint, requestBody);
            if (response.Result.IsSuccessStatusCode)
            {
                var responseContent = response.Result.Content.ReadAsStringAsync();
                return new ResultResponse(true, "Token Obtido com sucesso.", JsonSerializer.Deserialize<TokenResponseKeycloak>(responseContent.Result)?.AccessToken ?? "", (int)response.Result.StatusCode);
            }
            return new ResultResponse(false, $"[{(int)response.Result.StatusCode}] - Erro ao obter token de acesso.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
        }

        public ResultResponse CreateUserAsync(UserKeycloak userKeycloak)
        {
            var resultResponseByUsername = GetUsersByUsernameAsync(userKeycloak.Username ?? "");

            if (!resultResponseByUsername.Success)
            {
                return resultResponseByUsername;
            }

            var resultResponseByEmail = GetUsersByEmailAsync(userKeycloak.Email ?? "");

            if (!resultResponseByEmail.Success)
            {
                return resultResponseByEmail;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Body);

            var user = new
            {
                username = userKeycloak.Username,
                email = userKeycloak.Email,
                firstName = userKeycloak.FirstName,
                lastName = userKeycloak.LastName,
                enabled = true,
                credentials = new[]
                {
                    new
                    {
                        type = "password",
                        value = userKeycloak.Password,
                        temporary = false
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync($"{AuthenticationKeycloak.Path}/admin/realms/{AuthenticationKeycloak.Realm}/users", content);

            if (response.Result.IsSuccessStatusCode)
            {
                return new ResultResponse(true, "Usuário criado com sucesso.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
            }
            return new ResultResponse(false, "Erro ao criar usuário.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
        }

        public ResultResponse GetUsersAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Body);

            var response = _httpClient.GetAsync($"{AuthenticationKeycloak.Path}/admin/realms/{AuthenticationKeycloak.Realm}/users");

            if (response.Result.IsSuccessStatusCode)
            {
                return new ResultResponse(true, "Usuário(s) retornado(s) com sucesso.", JsonSerializer.Serialize(JsonSerializer.Deserialize<IEnumerable<UserRepresentationKeycloak>>(response.Result.Content.ReadAsStringAsync().Result)), (int)response.Result.StatusCode);
            }

            return new ResultResponse(false, "Erro ao obter usuários.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
        }

        public ResultResponse GetUserAsync(string userId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Body);

            var response = _httpClient.GetAsync($"{AuthenticationKeycloak.Path}/admin/realms/{AuthenticationKeycloak.Realm}/users/{userId}");

            if (response.Result.IsSuccessStatusCode)
            {
                return new ResultResponse(true, "Usuário obtido com sucesso.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
            }
            return new ResultResponse(false, "Erro ao obter usuários.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
        }

        public ResultResponse GetUsersByUsernameAsync(string username)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Body);

            var response = _httpClient.GetAsync($"{AuthenticationKeycloak.Path}/admin/realms/{AuthenticationKeycloak.Realm}/users?username={username}");

            if (response.Result.IsSuccessStatusCode)
            {
                var usersJson = response.Result.Content.ReadAsStringAsync();

                var users = JsonSerializer.Deserialize<IEnumerable<UserRepresentationKeycloak>>(usersJson.Result);

                var usersFiltered = users?.Where(u => username.Equals(u.Username, StringComparison.OrdinalIgnoreCase));

                if (usersFiltered != null && usersFiltered.Any())
                {
                    return new ResultResponse(true, "Usuário(s) retornado(s) com sucesso.", JsonSerializer.Serialize(usersFiltered), (int)response.Result.StatusCode);
                }

                return new ResultResponse(false, "Não foi encontrado nenhum usuário com o e-mail informado.", string.Empty);
            }

            return new ResultResponse(false, "Erro ao obter usuários.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
        }

        public ResultResponse GetUsersByEmailAsync(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Body);

            var response = _httpClient.GetAsync($"{AuthenticationKeycloak.Path}/admin/realms/{AuthenticationKeycloak.Realm}/users?email={email}");

            if (response.Result.IsSuccessStatusCode)
            {
                var usersJson = response.Result.Content.ReadAsStringAsync();

                var users = JsonSerializer.Deserialize<IEnumerable<UserRepresentationKeycloak>>(usersJson.Result);

                var usersFiltered = users?.Where(u => email.Equals(u.Email, StringComparison.OrdinalIgnoreCase));

                if (usersFiltered != null && usersFiltered.Any())
                {
                    return new ResultResponse(true, "Usuário(s) retornado(s) com sucesso.", JsonSerializer.Serialize(usersFiltered), (int)response.Result.StatusCode);
                }

                return new ResultResponse(false, "Não foi encontrado nenhum usuário com o e-mail informado.", string.Empty);
            }

            return new ResultResponse(false, "Erro ao obter usuários.", response.Result.Content.ReadAsStringAsync().Result, (int)response.Result.StatusCode);
        }

        public ResultResponse SendPasswordResetEmailAsync(string email)
        {
            var resultResponseUsers = GetUsersByEmailAsync(email);
            if (resultResponseUsers.Success && !string.IsNullOrEmpty(resultResponseUsers.Body))
            {
                var usersDeserialized = JsonSerializer.Deserialize<IEnumerable<UserRepresentationKeycloak>>(resultResponseUsers.Body);
                if (usersDeserialized != null && usersDeserialized.Any())
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Body);

                    var resetPasswordResponse = _httpClient.PutAsync(
                        $"{AuthenticationKeycloak.Path}/admin/realms/{AuthenticationKeycloak.Realm}/users/{usersDeserialized.First().Id}/execute-actions-email?client_id={AuthenticationKeycloak.ClientId}",
                        new StringContent(JsonSerializer.Serialize(new[] { "UPDATE_PASSWORD" }), Encoding.UTF8, "application/json")
                    );

                    if (!resetPasswordResponse.Result.IsSuccessStatusCode)
                    {
                        return new ResultResponse(false, $"Erro ao enviar solicitação de recuperação de senha.", resetPasswordResponse.Result.Content.ReadAsStringAsync().Result, (int)resetPasswordResponse.Result.StatusCode);
                    }
                    return new ResultResponse(true, "Para prosseguir com a recuperação da sua conta verifique o e-mail enviado para você com instruções.", string.Empty, (int)resetPasswordResponse.Result.StatusCode);
                }
            }
            return resultResponseUsers;
        }

        public ResultResponse ValidateCredentialsAsync(string username, string password)
        {
            AuthenticationKeycloak authenticationKeycloak = new("http://localhost:8083", "lanchesbrasil", AuthenticationKeycloak.ClientId ?? "", AuthenticationKeycloak.ClientSecret ?? "", username, password, AuthenticatioType.PASSWORD);
            var token = GetTokenAsync(authenticationKeycloak);
            return new ResultResponse(token.Success, token.Success == true ? "Login e senha validados com sucesso." : "Login ou senha incorretos.", token.Success.ToString(), token.StatusCode);
        }
    }
}
