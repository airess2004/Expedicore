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
    public class CashAdvanceRepository : EfRepository<CashAdvance>, ICashAdvanceRepository 
    {
        private ExpedicoEntities entities;
         
        public CashAdvanceRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<CashAdvance> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public CashAdvance GetObjectById(int Id)
        {
            CashAdvance data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         
        public int GetCashAdvanceNo(int officeId)
        {
            CashAdvance data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.CashAdvanceNo).FirstOrDefault();
            if (data != null)
            {
                return data.CashAdvanceNo;
            }
            else
            {
                return 0;
            }
        }

        public CashAdvance CreateObject(CashAdvance model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public CashAdvance UpdateObject(CashAdvance model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public CashAdvance SoftDeleteObject(CashAdvance model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            CashAdvance data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
