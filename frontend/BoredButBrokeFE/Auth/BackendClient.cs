using Microsoft.AspNetCore.Components.Authorization;

namespace BoredButBrokeFE.Auth
{
    public class BackendClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationStateProvider _authStateProvider;

        public BackendClient(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authStateProvider)
        {
            _httpClientFactory = httpClientFactory;
            _authStateProvider = authStateProvider;
        }

        public async Task<HttpClient> CreateClientAsync()
        {
            var client = _httpClientFactory.CreateClient("BBBBackEnd");

            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var backendCookie = authState.User.FindFirst(AccountEndpoints.BackendCookieClaimType)?.Value;

            if (!string.IsNullOrEmpty(backendCookie))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", backendCookie);
            }

            return client;
        }
    }
}