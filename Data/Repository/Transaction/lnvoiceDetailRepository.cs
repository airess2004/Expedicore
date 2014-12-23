using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Data.Repository
{ 
    public class InvoiceDetailRepository : EfRepository<InvoiceDetail>, IInvoiceDetailRepository 
    {
        private ExpedicoEntities entities;
         
        public InvoiceDetailRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<InvoiceDetail> GetQueryable()
        {
            return FindAll();
        }

        public InvoiceDetail GetObjectById(int Id)
        {
            InvoiceDetail data = Find(x => x.Id == Id);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public InvoiceDetail CreateObject(InvoiceDetail model)
        {
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public InvoiceDetail UpdateObject(InvoiceDetail model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }
         
        public InvoiceDetail ConfirmObject(InvoiceDetail model)
        {
            model.IsConfirmed = true;
            Update(model);
            return model;
        }
         
        public InvoiceDetail UnconfirmObject(InvoiceDetail model)
        {
            model.ConfirmationDate = null;
            model.IsConfirmed = false;
            Update(model);
            return model;
        }

        public InvoiceDetail SoftDeleteObject(InvoiceDetail model)
        {
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            InvoiceDetail data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
