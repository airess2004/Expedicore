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
    public class ExchangeRateRepository : EfRepository<ExchangeRate>, IExchangeRateRepository
    {
        private ExpedicoEntities entities;

        public ExchangeRateRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<ExchangeRate> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public ExchangeRate GetObjectById(int Id)
        {
            ExchangeRate data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public int GetLastMasterCode(int officeId)
        {
            ExchangeRate data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public ExchangeRate CreateObject(ExchangeRate model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public ExchangeRate UpdateObject(ExchangeRate model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public ExchangeRate SoftDeleteObject(ExchangeRate model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            ExchangeRate data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
