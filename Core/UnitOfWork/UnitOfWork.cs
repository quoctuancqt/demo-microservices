﻿namespace Core.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Core.Repositories;
    using Core.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDictionary<Type, object> _repositories;

        private DbContext _context;

        public HttpContext HttpContext { get; private set; }

        public ClaimsPrincipal CurrentUser
        {
            get
            {
                if (HttpContext == null) return null;

                else return HttpContext.User;
            }
        }

        public string DisplayName => CurrentUser.GetValue(ClaimTypes.Name);

        public string RoleName => CurrentUser.GetValue(ClaimTypes.Role);

        public string UserId => CurrentUser.GetValue(ClaimTypes.NameIdentifier);

        public string Email => CurrentUser.GetValue(ClaimTypes.Email);

        public string FirstName => CurrentUser.GetValue(ClaimTypes.GivenName);

        public UnitOfWork(IHttpContextAccessor httpContextAccessor, DbContext context) : this(context)
        {
            HttpContext = httpContextAccessor.HttpContext;
        }

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(bool isAudits = true)
        {
            BeforeCommit(isAudits);

            await _context.SaveChangesAsync();
        }

        public async Task CommitAsync(Func<Task> action)
        {
            BeforeCommit();

            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    //await _context.SaveChangesAsync();

                    await action();

                    transaction.Commit();
                }
            });
        }

        protected void BeforeCommit(bool isAudits = true)
        {
            var entriesAdded = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity);

            var entriesModified = _context.ChangeTracker.Entries()
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
                        e.CreatedBy = UserId;
                        e.CreatedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        e.ModifiedBy = UserId;
                        e.ModifiedDate = DateTime.UtcNow;
                    }
                }
            }
        }

        private T GetRepository<T>()
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (T)_repositories[typeof(T)];
            }
            else
            {
                var repository = (T)Activator.CreateInstance(typeof(T), _context);

                _repositories.Add(typeof(T), repository);

                return repository;
            }

        }

        private GenericRepository<T> GetGenericRepository<T>() where T : EntityBase, IEntity
        {
            if (_repositories.ContainsKey(typeof(GenericRepository<T>)))
            {
                return (GenericRepository<T>)_repositories[typeof(GenericRepository<T>)];
            }
            else
            {
                var repository = new GenericRepository<T>(_context);

                _repositories.Add(typeof(GenericRepository<T>), repository);

                return repository;
            }

        }

        public void Dispose()
        {
            _context?.Dispose();

            _repositories = null;
        }
    }
}
