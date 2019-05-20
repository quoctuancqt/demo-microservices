namespace Core.Services
{
    using Core.Exceptions;
    using Core.Repositories;
    using Core.UnitOfWork;
    using Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Helpers;
    using Microsoft.EntityFrameworkCore;

    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected readonly string _userId;

        protected readonly string _tenantId;

        protected readonly string _displayName;

        protected readonly string _userName;

        protected readonly string _firstName;

        protected readonly string _roleName;

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _userId = _unitOfWork.UserId;

            _displayName = _unitOfWork.DisplayName;

            _userName = _unitOfWork.Email;

            _firstName = _unitOfWork.FirstName;

            _roleName = _unitOfWork.RoleName;

        }

        protected void BadRequest(string msg)
        {
            throw new BadRequestException(msg);
        }

        protected void BadRequest(IDictionary<string, string> errors)
        {
            throw new BadRequestException(errors);
        }

        protected virtual async Task<TEntity> CheckExistingNameAsync<TEntity>(string id, string name)
              where TEntity : EntityBase, IEntity
        {
            var repository = _unitOfWork.GetPropValue<IRepository<TEntity>>($"{typeof(TEntity).Name}Repository");

            var entity = await repository.FindAll()
                .SingleOrDefaultAsync(x => x.GetPropValue<string>("Name").ToLower().Equals(name.ToLower().Trim()));

            if (string.IsNullOrEmpty(id))
            {
                if (entity != null)
                {
                    BadRequest(new Dictionary<string, string>
                    {
                        { "name", "Already exists." }
                    });
                }
            }
            else
            {
                if (entity == null) return await repository.FindByAsync(id);

                if (entity != null && id.Equals(entity.GetPropValue<string>("Id"))) return entity;
                else
                {
                    BadRequest(new Dictionary<string, string>
                    {
                        { "name", "Already exists." }
                    });
                }
            }

            return null;
        }
    }
}
