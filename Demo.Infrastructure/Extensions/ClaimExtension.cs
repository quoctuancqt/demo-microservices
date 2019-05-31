namespace Core.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class ClaimExtension
    {
        public static string GetValue(this IPrincipal user, string key)
        {
            if (user == null)
            {
                return string.Empty;
            }

            var claim = ((ClaimsIdentity)user.Identity).Claims.FirstOrDefault(x => x.Type.Equals(key));

            if (claim == null)
            {
                return string.Empty;
            }

            return claim.Value;
        }

        public static bool WithRole(this IPrincipal user, string role)
        {
            var claim = ((ClaimsIdentity)user?.Identity)?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role));

            return claim != null && claim.Value.Equals(role);
        }
    }
}
