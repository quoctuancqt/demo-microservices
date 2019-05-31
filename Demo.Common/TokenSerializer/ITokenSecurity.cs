namespace Common.TokenSerializer
{
    public interface ITokenSecurity
    {
        string SecurityStamp { get; set; }

        string Key { get; }
    }
}
