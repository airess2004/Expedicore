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
    public class InvoiceRepository : EfRepository<Invoice>, IInvoiceRepository 
    {
        private ExpedicoEntities entities;
         
        public InvoiceRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Invoice> GetQueryable()
        {
            return FindAll();
        }

        public Invoice GetObjectById(int Id)
        {
            Invoice data = Find(x => x.Id == Id);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         
        public int GetInvoiceNo(int officeId,string debetCredit)
        {
            Invoice data = FindAll(x => x.OfficeId == officeId && x.DebetCredit == debetCredit).OrderByDescending(x => x.InvoicesNo).FirstOrDefault();
            if (data != null)
            {
                return data.InvoicesNo;
            }
            else
            {
                return 0;
            }
        }

        public int GetNewInvoiceStatus(int officeId, int ShipmentOrderId)
        {
            Invoice data = FindAll(b => b.OfficeId == officeId && b.ShipmentOrderId == ShipmentOrderId).OrderByDescending(x => x.InvoiceStatus).FirstOrDefault();
            if (data != null)
            {
                return data.InvoiceStatus.Value;
            }
            else
            {
                return 0;
            }
        }


        public Invoice GetObjectByShipmentOrderId(int Id)
        {
            Invoice data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public Invoice CreateObject(Invoice model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Invoice UpdateObject(Invoice model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Invoice SoftDeleteObject(Invoice model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }
         
        public Invoice ConfirmObject(Invoice model)
        {
            model.IsConfirmed = true;
            Update(model);
            return model;
        }

        public Invoice UnconfirmObject(Invoice model)
        {
            model.ConfirmationDate = null;
            model.IsConfirmed = false;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Invoice data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
