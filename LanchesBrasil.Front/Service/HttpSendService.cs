using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using LanchesBrasil.Commons.Model;
using LanchesBrasil.Front.Service.Interfaces;
using Microsoft.AspNetCore.Components;

namespace LanchesBrasil.Front.Service
{
    public class HttpSendService : IHttpSendService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigation;

        public HttpSendService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _navigation = navigation;
        }

        public async Task<ResultResponse> SendRequest(string url, Commons.Model.HttpMethod httpMethod, object? objSend = null)
        {
            HttpResponseMessage response;
            try
            {
                await AddAuthorizationHeader();

                response = await ReattemptRequest(url, httpMethod, objSend);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResultResponse>();
                    return result ?? new ResultResponse();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var tokenAtualizado = await RefreshAccessTokenAsync();
                    if (tokenAtualizado)
                    {
                        await AddAuthorizationHeader();

                        response = await ReattemptRequest(url, httpMethod, objSend);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadFromJsonAsync<ResultResponse>();
                            return result ?? new ResultResponse();
                        }
                    }
                    _navigation.NavigateTo("/login");
                }

                return new ResultResponse(false, $"Erro: {response.ReasonPhrase}", response.ReasonPhrase ?? "", (int)response.StatusCode);
            }
            catch (HttpRequestException exception)
            {
                return new ResultResponse(true, "Falha ao efetuar a requisição.", exception.Message ?? "", exception.StatusCode > 0 ? (int)exception.StatusCode : 0);
            }
            catch (Exception exception)
            {
                return new ResultResponse(false, $"Erro: {exception.StackTrace}", exception.Message ?? "");
            }
        }

        private async Task<bool> RefreshAccessTokenAsync()
        {
            var tokenEndpoint = "http://localhost:8083/realms/lanchesbrasil/protocol/openid-connect/token";
            var clientId = "lanchesbrasil-app";
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        });

            var response = await _httpClient.PostAsync(tokenEndpoint, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponseKeycloak>(content);

                if (tokenResponse != null)
                {
                    await _localStorage.SetItemAsync("accessToken", tokenResponse.AccessToken);
                    await _localStorage.SetItemAsync("refreshToken", tokenResponse.RefreshToken);
                    return true;
                }
            }

            return false;
        }

        private async Task<HttpResponseMessage> ReattemptRequest(string url, Commons.Model.HttpMethod httpMethod, object? objSend)
        {
            return httpMethod switch
            {
                Commons.Model.HttpMethod.POST when objSend != null => await _httpClient.PostAsJsonAsync(url, objSend),
                Commons.Model.HttpMethod.GET => await _httpClient.GetAsync(url),
                Commons.Model.HttpMethod.PUT when objSend != null => await _httpClient.PutAsJsonAsync(url, objSend),
                Commons.Model.HttpMethod.DELETE => await _httpClient.DeleteAsync(url),
                _ => throw new ArgumentException("Método HTTP inválido ou objeto de envio nulo para métodos POST/PUT")
            };
        }

        private async Task AddAuthorizationHeader()
        {
            var accessToken = await _localStorage.GetItemAsync<string>("accessToken");

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
