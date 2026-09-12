using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace BoredButBrokeFE.Auth
{
    public class CookieAuthStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserCookieContainer _cookieContainer;
        private static readonly AuthenticationState _loggedOut = new(new ClaimsPrincipal(new ClaimsIdentity()));
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(20);
        public CookieAuthStateProvider(ILoggerFactory loggerFactory, 
            IHttpClientFactory httpClientFactory, 
            UserCookieContainer cookieContainer) 
            : base(loggerFactory)
        {

            _httpClientFactory = httpClientFactory;
            _cookieContainer = cookieContainer;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return await FetchAuthStateAsync();
        }
        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState state, CancellationToken token)
        {
            var currentPrincipal = state.User;
            if (currentPrincipal.Identity?.IsAuthenticated != true)
            {
                return false;
            }

            var freshState = await FetchAuthStateAsync();
            return freshState.User.Identity?.IsAuthenticated == true;
        }
        public void NotifyUserLoggedIn()
        {
            NotifyAuthenticationStateChanged(FetchAuthStateAsync());
        }
        public void NotifyUserLoggedOut()
        {
            _cookieContainer.CookieHeader = null;
            NotifyAuthenticationStateChanged(Task.FromResult(_loggedOut));
        }
        private async Task<AuthenticationState> FetchAuthStateAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BBBBackEnd");
                var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/me");

                if (!string.IsNullOrEmpty(_cookieContainer.CookieHeader))
                {
                    request.Headers.Add("Cookie", _cookieContainer.CookieHeader);
                }

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _cookieContainer.CookieHeader = null;
                    return _loggedOut;
                }

                var user = await response.Content.ReadFromJsonAsync<UserInfo>();
                if (user is null)
                {
                    _cookieContainer.CookieHeader = null;
                    return _loggedOut;
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.GivenName,  user.FirstName ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                };

                var identity = new ClaimsIdentity(claims, "cookie");
                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            }
            catch
            {
                return _loggedOut;
            }
        }
        private record UserInfo(string? Email, string? FirstName);
    }
}
