using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using BlazingShop.Client.Authentication;
using BlazingShop.Client.Authentication.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

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

        public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userToBeAuthenticated)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username",userToBeAuthenticated.Email),
                new KeyValuePair<string, string>("password",userToBeAuthenticated.Password)
            });

            var authResult = await _httpClient.PostAsync("https://localhost:5001/token",data);
            var authContent = await authResult.Content.ReadAsStringAsync();

            if (authResult.IsSuccessStatusCode == false)
            {
                return null;
            }

            var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
                authContent,
                new JsonSerializerOptions{PropertyNameCaseInsensitive = true});

            await _localStorage.SetItemAsync("auth_token", result.Access_Token);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Access_Token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("auth_token");

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        }


    }
}
