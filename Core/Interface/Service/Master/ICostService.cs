using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICostService
    {
        IQueryable<Cost> GetQueryable();
        Cost GetObjectById(int Id);
        Cost CreateObject(Cost cost);
        Cost UpdateObject(Cost cost);
        Cost SoftDeleteObject(Cost cost);
        bool IsNameDuplicated(Cost cost);
    }
}