using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContinentService
    {
        IQueryable<Continent> GetQueryable();
        Continent GetObjectById(int Id);
        Continent CreateObject(Continent continent);
        Continent UpdateObject(Continent continent);
        Continent SoftDeleteObject(Continent continent);
        bool IsNameDuplicated(Continent continent);
    }
}