namespace Common.TokenSerializer
{
    using System;
    using System.Linq;
    using System.Text;

    public class TokenProvider : ITokenProvider
    {
        public string GenerateToken<T>(string reason, T model)
            where T : class, ITokenSecurity
        {
            byte[] _time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] _key = Guid.Parse(model.SecurityStamp).ToByteArray();
            byte[] _Id = GetBytes(model.Key);
            byte[] _reason = GetBytes(reason);
            byte[] data = new byte[_time.Length + _key.Length + _reason.Length + _Id.Length];

            Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            Buffer.BlockCopy(_key, 0, data, _time.Length, _key.Length);
            Buffer.BlockCopy(_reason, 0, data, _time.Length + _key.Length, _reason.Length);
            Buffer.BlockCopy(_Id, 0, data, _time.Length + _key.Length + _reason.Length, _Id.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        public TokenValidation ValidateToken<T>(string reason, T user, string token)
            where T : class, ITokenSecurity
        {
            var result = new TokenValidation();
            byte[] data = Convert.FromBase64String(token);
            byte[] _time = data.Take(8).ToArray();
            byte[] _key = data.Skip(8).Take(16).ToArray();
            byte[] _reason = data.Skip(24).Take(reason.Length).ToArray();
            byte[] _Id = data.Skip((24 + reason.Length)).ToArray();

            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(_time, 0));

            if (when < DateTime.UtcNow.AddHours(-24))
            {
                result.Errors.Add(TokenValidationStatus.Expired);
            }

            Guid gKey = new Guid(_key);
            if (gKey.ToString() != user.SecurityStamp)
            {
                result.Errors.Add(TokenValidationStatus.WrongGuid);
            }

            if (reason != GetString(_reason))
            {
                result.Errors.Add(TokenValidationStatus.WrongPurpose);
            }

            if (user.Key.ToString() != GetString(_Id))
            {
                result.Errors.Add(TokenValidationStatus.WrongUser);
            }

            return result;
        }

        private byte[] GetBytes(string someString)
        {
            return Encoding.ASCII.GetBytes(someString);
        }

        private string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
