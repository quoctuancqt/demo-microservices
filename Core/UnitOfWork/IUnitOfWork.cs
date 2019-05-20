namespace Core.UnitOfWork
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        HttpContext HttpContext { get; }

        string DisplayName { get; }

        string RoleName { get; }

        string UserId { get; }

        string Email { get; }

        string FirstName { get; }

        ClaimsPrincipal CurrentUser { get; }

        Task CommitAsync(bool isAudits = true);

        Task CommitAsync(Func<Task> action);
    }
}
