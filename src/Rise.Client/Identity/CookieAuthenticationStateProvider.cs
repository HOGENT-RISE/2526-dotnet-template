using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Ardalis.Result;
using Microsoft.AspNetCore.Components.Authorization;
using Rise.Client.Identity.Models;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Identity
{
    /// <summary>
    /// Handles state for cookie-based auth.
    /// </summary>
    /// <remarks>
    /// Create a new instance of the auth provider.
    /// </remarks>
    /// <param name="httpClientFactory">Factory to retrieve auth client.</param>
    public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory, ILogger<CookieAuthenticationStateProvider> logger)
        : AuthenticationStateProvider, IAccountManagement
    {
        /// <summary>
        /// Map the JavaScript-formatted properties to C#-formatted classes.
        /// </summary>
        private readonly JsonSerializerOptions jsonSerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

        /// <summary>
        /// Special auth client.
        /// </summary>
        private readonly HttpClient httpClient = httpClientFactory.CreateClient("Auth");

        /// <summary>
        /// Authentication state.
        /// </summary>
        private bool authenticated = false;

        /// <summary>
        /// Default principal for anonymous (not authenticated) users.
        /// </summary>
        private readonly ClaimsPrincipal unauthenticated = new(new ClaimsIdentity());

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result serialized to a <see cref="FormResult"/>.
        /// </returns>
        public async Task<FormResult> RegisterAsync(string email, string password)
        {
            string[] defaultDetail = [ "An unknown error prevented registration from succeeding." ];

            try
            {
                // make the request
                var result = await httpClient.PostAsJsonAsync(
                    "/api/identity/accounts/register",
                    new AccountRequest.Register
                    {
                        Email = email,
                        Password = password
                    });

                var typedResult = await result.Content.ReadFromJsonAsync<Result>();

                // successful?
                if (result.IsSuccessStatusCode)
                {
                    return new FormResult { Succeeded = true };
                }
                // // body should contain details about why it failed

                return new FormResult
                {
                    Succeeded = false,
                    ErrorList = typedResult!.Errors.ToArray()
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "App error");
            }

            // unknown error
            return new FormResult
            {
                Succeeded = false,
                ErrorList = defaultDetail
            };
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result of the login request serialized to a <see cref="FormResult"/>.</returns>
        public async Task<FormResult> LoginAsync(string email, string password)
        {
            try
            {
                // login with cookies
                var result = await httpClient.PostAsJsonAsync(
                    "/api/identity/accounts/login", new
                    {
                        email,
                        password
                    });
                // success?
                if (result.IsSuccessStatusCode)
                {
                    // need to refresh auth state
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                    // success!
                    return new FormResult { Succeeded = true };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "App error");
            }

            // unknown error
            return new FormResult
            {
                Succeeded = false,
                ErrorList = [ "Invalid email and/or password." ]
            };
        }

        /// <summary>
        /// Get authentication state.
        /// </summary>
        /// <remarks>
        /// Called by Blazor anytime and authentication-based decision needs to be made, then cached
        /// until the changed state notification is raised.
        /// </remarks>
        /// <returns>The authentication state asynchronous request.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            authenticated = false;
            // default to not authenticated
            var user = unauthenticated;

            try
            {
                var result = await httpClient.GetFromJsonAsync<Result<AccountResponse.Info>>("/api/identity/accounts/info");

                if (result!.IsSuccess)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, result.Value.Email),
                        new(ClaimTypes.Email, result.Value.Email)
                    };

                    claims.AddRange(
                        result.Value.Claims
                            .Where(c => c.Key is not (ClaimTypes.Name or ClaimTypes.Email or ClaimTypes.Role))
                            .Select(c => new Claim(c.Key, c.Value))
                    );

                    claims.AddRange(result.Value.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

                    var identity = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                    user = new ClaimsPrincipal(identity);
                    authenticated = true;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode != HttpStatusCode.Unauthorized)
            {
                logger.LogError(ex, "App error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "App error");
            }

            return new AuthenticationState(user);
        }

        public async Task LogoutAsync()
        {
            const string Empty = "{}";
            var emptyContent = new StringContent(Empty, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/identity/logout", emptyContent);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return authenticated;
        }
    }
}
