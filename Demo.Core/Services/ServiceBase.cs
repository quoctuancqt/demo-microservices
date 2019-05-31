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
    }
}
