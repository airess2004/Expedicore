using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContactTypeService
    {
        IQueryable<ContactType> GetQueryable();
        ContactType GetObjectById(int Id);
        ContactType UpdateObject(ContactType contactype);
        ContactType SoftDeleteObject(ContactType contactype);
    }
}