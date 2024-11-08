using LanchesBrasil.Commons.Model;

namespace LanchesBrasil.Front.Service.Interfaces
{
    public interface IAccountService
    {
        public Task<ResultResponse> CreateUser(User user);
        public Task<ResultResponse> UserLogin(string login, string password);
        public Task<ResultResponse> GetUsers();
        public Task<ResultResponse> ForgotPassword(string email);
    }
}
