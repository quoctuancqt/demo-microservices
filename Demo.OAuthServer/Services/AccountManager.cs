namespace OAuthServer.Services
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using JwtTokenServer.Models;
    using JwtTokenServer.Services;

    public class AccountManager : IAccountManager
    {
        public async Task<AccountResult> VerifyAccountAsync(string username, string password, TokenRequest tokenRequest)
        {
            if (!username.Equals("admin") || !password.Equals("admin")) return new AccountResult(new { error = "Invalid user" });

            tokenRequest.Claims.Add(new CustomClaim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()));
            tokenRequest.Claims.Add(new CustomClaim(ClaimTypes.Name, "Admin"));

            tokenRequest.Responses.Add("userId", Guid.Empty.ToString());

            await Task.CompletedTask;

            return new AccountResult(tokenRequest);
        }
    }
}
