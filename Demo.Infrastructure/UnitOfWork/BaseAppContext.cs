using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.UnitOfWork
{
    public class BaseAppContext : DbContext, IUnitOfWork
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseAppContext(IHttpContextAccessor httpContextAccessor, DbContextOptions options) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges()
        {
            BeforeCommit();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeCommit();

            return base.SaveChangesAsync(cancellationToken);
        }

        public void BeforeCommit()
        {
            var entriesAdded = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity);

            var entriesModified = ChangeTracker.Entries()
                  .Where(e => e.State == EntityState.Modified).Select(e => e.Entity as IAudit);

            if (entriesAdded.Count() > 0) ProcessAudit(entriesAdded, EntityState.Added);

            if (entriesModified.Count() > 0) ProcessAudit(entriesModified, EntityState.Modified);
        }

        private void ProcessAudit(IEnumerable<object> entries, EntityState state)
        {
            foreach (var e in entries.Select(e => e as IAudit))
            {
                if (e != null)
                {
                    if (state == EntityState.Added)
                    {
                        e.CreatedBy = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
                        e.CreatedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        e.ModifiedBy = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
                        e.ModifiedDate = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
