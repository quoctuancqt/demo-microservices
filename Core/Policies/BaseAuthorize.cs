using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Core.Policies
{
    public abstract class BaseAuthorize<TRequirement> : AuthorizationHandler<TRequirement>, IAuthorizationRequirement
        where TRequirement : IAuthorizationRequirement
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            if (await BeforeAuthorizeAsync(context, requirement))
            {
                if (IsAuthorize(context, requirement))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Fail();
            }
        }

        private async Task<bool> BeforeAuthorizeAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            //var curentRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            //var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            //if (curentRole != null && userId != null)
            //{
            //    var _userManager = ResolverFactory.GetService<UserManager<ApplicationUser>>();

            //    var user = await _userManager.FindByIdAsync(userId.Value);

            //    var result = await _userManager.GetRolesAsync(user);

            //    var dbRole = result.FirstOrDefault();

            //    if (curentRole.Value != dbRole)
            //    {
            //        return false;
            //    }
            //}

            return true;
        }

        protected abstract bool IsAuthorize(AuthorizationHandlerContext context, TRequirement requirement);
    }
}
