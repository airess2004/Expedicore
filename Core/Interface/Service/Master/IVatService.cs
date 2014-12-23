using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IVatService
    {
        IQueryable<Vat> GetQueryable();
        Vat GetObjectById(int Id);
        Vat CreateObject(Vat vat);
        Vat UpdateObject(Vat vat);
        Vat SoftDeleteObject(Vat vat);
        bool IsNameDuplicated(Vat vat);

    }
}