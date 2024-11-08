using LanchesBrasil.Commons.Model;
using LanchesBrasil.Front.Service.Interfaces;

namespace LanchesBrasil.Front.Service
{
    public class AccountService : IAccountService
    {
        private readonly IHttpSendService _httpSendService;

        public AccountService(IHttpSendService httpSendService)
        {
            _httpSendService = httpSendService;
        }

        public async Task<ResultResponse> CreateUser(User user)
        {
            return await _httpSendService.SendRequest("http://localhost:5004/api/Account/createuser", Commons.Model.HttpMethod.POST, user);
        }

        public async Task<ResultResponse> GetUsers()
        {
            return await _httpSendService.SendRequest("http://localhost:5004/api/Account/users", Commons.Model.HttpMethod.GET);
        }

        public Task<ResultResponse> UserLogin(string login, string password)
        {
            return Task.FromResult(new ResultResponse(true, "Usuário logado com sucesso!"));
        }

        public Task<ResultResponse> ForgotPassword(string email)
        {
            return Task.FromResult(new ResultResponse(true, "Para prosseguir com a recuperação da sua conta verifique o e-mail enviado para você com instruções."));
        }
    }
}
