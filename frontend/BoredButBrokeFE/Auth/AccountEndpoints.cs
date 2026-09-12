using BoredButBrokeFE.Components.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Security.Claims;

namespace BoredButBrokeFE.Auth
{
    public static class AccountEndpoints
    {
        public const string BackendCookieClaimType = "BackendCookie";

        public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/account/login", LoginAsync);
            app.MapPost("/account/logout", LogoutAsync);
        }

        private static async Task<IResult> LoginAsync(
            LoginUserRequest request,
            IHttpClientFactory httpClientFactory,
            HttpContext context)
        {
            var client = httpClientFactory.CreateClient("BBBBackEnd");

            HttpResponseMessage loginResponse;
            try
            {
                loginResponse = await client.PostAsJsonAsync("api/auth/login", request);
            }
            catch (HttpRequestException)
            {
                return Results.Problem(statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            if (!loginResponse.IsSuccessStatusCode)
            {
                return Results.Problem(statusCode: (int)loginResponse.StatusCode);
            }

            var backendCookies = new CookieContainer();
            if (loginResponse.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders))
            {
                foreach (var setCookieHeader in setCookieHeaders)
                {
                    backendCookies.SetCookies(client.BaseAddress!, setCookieHeader);
                }
            }

            var backendCookie = backendCookies.GetCookieHeader(client.BaseAddress!);
            if (string.IsNullOrEmpty(backendCookie))
            {
                return Results.Problem(statusCode: StatusCodes.Status502BadGateway);
            }

            var meRequest = new HttpRequestMessage(HttpMethod.Get, "api/auth/me");
            meRequest.Headers.TryAddWithoutValidation("Cookie", backendCookie);
            var meResponse = await client.SendAsync(meRequest);

            var user = meResponse.IsSuccessStatusCode
                ? await meResponse.Content.ReadFromJsonAsync<UserInfoResponse>()
                : null;

            if (user is null)
            {
                return Results.Problem(statusCode: StatusCodes.Status502BadGateway);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(BackendCookieClaimType, backendCookie)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties { IsPersistent = true, AllowRefresh = false };
            var expires = backendCookies.GetCookies(client.BaseAddress!).Min(c => c.Expires);
            if (expires != DateTime.MinValue)
            {
                properties.ExpiresUtc = new DateTimeOffset(expires);
            }

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                properties);

            return Results.Ok();
        }

        private static async Task<IResult> LogoutAsync(IHttpClientFactory httpClientFactory, HttpContext context)
        {
            var backendCookie = context.User.FindFirst(BackendCookieClaimType)?.Value;

            if (!string.IsNullOrEmpty(backendCookie))
            {
                var client = httpClientFactory.CreateClient("BBBBackEnd");
                var logoutRequest = new HttpRequestMessage(HttpMethod.Post, "api/auth/logout");
                logoutRequest.Headers.TryAddWithoutValidation("Cookie", backendCookie);

                try
                {
                    await client.SendAsync(logoutRequest);
                }
                catch (HttpRequestException)
                {

                }
            }

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Results.Ok();
        }
    }
}