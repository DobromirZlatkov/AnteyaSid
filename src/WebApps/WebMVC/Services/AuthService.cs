namespace AnteyaSidOnContainers.WebApps.WebMVC.Services
{
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public AuthService(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor ?? throw new ArgumentNullException(nameof(httpContextAccesor));
        }

        /// <summary>
        /// Method get access token from the http context to authenticate api calls to the microservices
        /// </summary>
        /// <returns>Access token</returns>
        public async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccesor.HttpContext;
            return await context.GetTokenAsync("access_token");
        }
    }
}
