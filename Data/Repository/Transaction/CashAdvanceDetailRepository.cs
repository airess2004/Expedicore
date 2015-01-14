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
    public class CashAdvanceDetailRepository : EfRepository<CashAdvanceDetail>, ICashAdvanceDetailRepository 
    {
        private ExpedicoEntities entities;
         
        public CashAdvanceDetailRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<CashAdvanceDetail> GetQueryable()
        {
            return FindAll();
        }

        public CashAdvanceDetail GetObjectById(int Id)
        {
            CashAdvanceDetail data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public CashAdvanceDetail GetObjectByCashAdvanceId(int Id)
        {
            CashAdvanceDetail data = Find(x => x.CashAdvanceId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public CashAdvanceDetail CreateObject(CashAdvanceDetail model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public CashAdvanceDetail UpdateObject(CashAdvanceDetail model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public CashAdvanceDetail SoftDeleteObject(CashAdvanceDetail model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            CashAdvanceDetail data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public CashAdvanceDetail ConfirmObject(CashAdvanceDetail model)
        {
            model.IsConfirmed = true;
            Update(model);
            return model;
        }

        public CashAdvanceDetail UnconfirmObject(CashAdvanceDetail model)
        {
            model.ConfirmationDate = null;
            model.IsConfirmed = false;
            Update(model);
            return model;
        }

    }
}
