using LanchesBrasil.Commons.Model;

namespace LanchesBrasil.Commons.Service.Interfaces
{
    public interface IKeycloakService
    {
        public ResultResponse GetTokenAsync(AuthenticationKeycloak authenticationKeycloak);
        public ResultResponse CreateUserAsync(UserKeycloak userKeycloak);
        public ResultResponse GetUsersAsync();
        public ResultResponse GetUserAsync(string userId);
        public ResultResponse GetUsersByUsernameAsync(string username);
        public ResultResponse GetUsersByEmailAsync(string email);
        public ResultResponse SendPasswordResetEmailAsync(string email);
        public ResultResponse ValidateCredentialsAsync(string username, string password);
    }
}
