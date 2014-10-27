using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContactRepository : IRepository<Contact>
    {
       IQueryable<Contact> GetQueryable();
       Contact GetObjectById(int Id);
       Contact CreateObject(Contact model);
       Contact UpdateObject(Contact model);
       Contact SoftDeleteObject(Contact model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Contact model);
       int GetLastMasterCode(int officeId);
    }
}