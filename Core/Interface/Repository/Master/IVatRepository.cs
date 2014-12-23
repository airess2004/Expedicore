using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IVatRepository : IRepository<Vat>
    {
       IQueryable<Vat> GetQueryable();
       Vat GetObjectById(int Id);
       Vat CreateObject(Vat model);
       Vat UpdateObject(Vat model);
       Vat SoftDeleteObject(Vat model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Vat model);
       int GetLastMasterCode(int officeId);
    }
}