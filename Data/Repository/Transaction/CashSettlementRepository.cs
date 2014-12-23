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
    public class CashSettlementRepository : EfRepository<CashSettlement>, ICashSettlementRepository 
    {
        private ExpedicoEntities entities;
         
        public CashSettlementRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<CashSettlement> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public CashSettlement GetObjectById(int Id)
        {
            CashSettlement data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         
        public int GetCashSettlementNo(int officeId)
        {
            CashSettlement data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.SettlementNo).FirstOrDefault();
            if (data != null)
            {
                return data.SettlementNo;
            }
            else
            {
                return 0;
            }
        }

        public CashSettlement CreateObject(CashSettlement model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public CashSettlement UpdateObject(CashSettlement model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public CashSettlement SoftDeleteObject(CashSettlement model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            CashSettlement data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
