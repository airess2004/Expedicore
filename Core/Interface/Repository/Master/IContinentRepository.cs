using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContinentRepository : IRepository<Continent>
    {
       IQueryable<Continent> GetQueryable();
       Continent GetObjectById(int Id);
       Continent CreateObject(Continent model);
       Continent UpdateObject(Continent model);
       Continent SoftDeleteObject(Continent model);
       int GetFirstMasterCode(int officeId);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Continent model);
    }
}