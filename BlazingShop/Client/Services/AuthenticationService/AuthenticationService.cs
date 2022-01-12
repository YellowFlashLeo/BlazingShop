using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazingShop.Client.Authentication;
using BlazingShop.Client.Authentication.Models;
using BlazingShop.Shared.Modals;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BlazingShop.Client.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public async Task RegisterUser(CreateUserModel model)
        {
            var data = new {model.FirstName, model.LastName, model.EmailAddress, model.Password};

            using (HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/Register", data))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userToBeAuthenticated)
        {
            string inputJson = JsonConvert.SerializeObject(userToBeAuthenticated);
            HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            using (var authResult = await _httpClient.PostAsync($"/token", inputContent))
            {
                if (authResult.IsSuccessStatusCode == false)
                {
                    throw new Exception(authResult.ReasonPhrase);
                }
                else
                {
                    var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
                        await authResult.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    await _localStorage.SetItemAsync("auth_token", result.Access_Token);
                    ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Access_Token);

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

                    return result;
                }
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("auth_token");

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
