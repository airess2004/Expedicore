using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IOfficeRepository : IRepository<Office>
    {
       IQueryable<Office> GetQueryable();
       Office GetObjectById(int Id);
       Office CreateObject(Office model);
       Office UpdateObject(Office model);
       Office SoftDeleteObject(Office model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Office model);
    }
}