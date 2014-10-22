using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IOfficeService
    {
        IQueryable<Office> GetQueryable();
        Office GetObjectById(int Id);
        Office CreateObject(Office office);
        Office UpdateObject(Office office);
        Office SoftDeleteObject(Office office);
        bool IsNameDuplicated(Office office);
    }
}