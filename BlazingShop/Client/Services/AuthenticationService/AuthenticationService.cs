using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazingShop.Client.Authentication;
using BlazingShop.Client.Authentication.Models;
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

        public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userToBeAuthenticated)
        {
            //var data = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("grant_type", "password"),
            //    new KeyValuePair<string, string>("username",userToBeAuthenticated.Email),
            //    new KeyValuePair<string, string>("password",userToBeAuthenticated.Password)
            //});

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
                    //var result = await authResult.Content.ReadFromJsonAsync<AuthenticatedUserModel>();
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
