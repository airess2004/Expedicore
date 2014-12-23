﻿using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IInvoiceDetailRepository : IRepository<InvoiceDetail>
    { 
       IQueryable<InvoiceDetail> GetQueryable();
       InvoiceDetail GetObjectById(int Id);
       InvoiceDetail CreateObject(InvoiceDetail model);
       InvoiceDetail UpdateObject(InvoiceDetail model);
       InvoiceDetail SoftDeleteObject(InvoiceDetail model);
       InvoiceDetail ConfirmObject(InvoiceDetail model);
       InvoiceDetail UnconfirmObject(InvoiceDetail model);
       bool DeleteObject(int Id);
    }
}