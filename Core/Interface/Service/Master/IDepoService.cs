using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IDepoService
    {
        IQueryable<Depo> GetQueryable();
        Depo GetObjectById(int Id);
        Depo CreateObject(Depo depo);
        Depo UpdateObject(Depo depo);
        Depo SoftDeleteObject(Depo depo);
        bool IsNameDuplicated(Depo depo);
    }
}