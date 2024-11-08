using LanchesBrasil.Commons.Model;

namespace LanchesBrasil.Front.Service.Interfaces
{
    public interface IHttpSendService
    {
        public Task<ResultResponse> SendRequest(string url, Commons.Model.HttpMethod httpMethod, object? objSend = null);
    }
}
