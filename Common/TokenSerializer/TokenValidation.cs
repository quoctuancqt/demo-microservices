namespace Common.TokenSerializer
{
    using System.Collections.Generic;

    public class TokenValidation
    {
        public bool Validated { get { return Errors.Count == 0; } }
        public readonly List<TokenValidationStatus> Errors = new List<TokenValidationStatus>();
    }

    public enum TokenValidationStatus
    {
        Expired,
        WrongUser,
        WrongPurpose,
        WrongGuid
    }
}
