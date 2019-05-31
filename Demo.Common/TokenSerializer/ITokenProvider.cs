namespace Common.TokenSerializer
{
    public interface ITokenProvider
    {
        string GenerateToken<T>(string reason, T model)
            where T : class, ITokenSecurity;

        TokenValidation ValidateToken<T>(string reason, T user, string token)
            where T : class, ITokenSecurity;
    }
}
