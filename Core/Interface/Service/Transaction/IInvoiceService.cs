using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IInvoiceService
    {
        IQueryable<Invoice> GetQueryable();
        Invoice GetObjectById(int Id);
        Invoice GetObjectByShipmentOrderId(int Id);
        Invoice CreateUpdateObject(Invoice invoice);
        Invoice CreateObject(Invoice invoice);
        Invoice UpdateObject(Invoice invoice); 
        Invoice SoftDeleteObject(Invoice invoice);
    }
}