using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContactTypeRepository : IRepository<ContactType>
    {
       IQueryable<ContactType> GetQueryable();
       ContactType GetObjectById(int Id);
       ContactType CreateObject(ContactType model);
       ContactType UpdateObject(ContactType model);
       ContactType SoftDeleteObject(ContactType model);
       bool DeleteObject(int Id);  
    }
}