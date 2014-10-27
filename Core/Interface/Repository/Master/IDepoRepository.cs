using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IDepoRepository : IRepository<Depo>
    {
       IQueryable<Depo> GetQueryable();
       Depo GetObjectById(int Id);
       Depo CreateObject(Depo model);
       Depo UpdateObject(Depo model);
       Depo SoftDeleteObject(Depo model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Depo model);
       int GetLastMasterCode(int officeId);
    }
}