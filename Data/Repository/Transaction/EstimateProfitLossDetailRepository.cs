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
    public class EstimateProfitLossDetailRepository : EfRepository<EstimateProfitLossDetail>, IEstimateProfitLossDetailRepository 
    {
        private ExpedicoEntities entities;
         
        public EstimateProfitLossDetailRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<EstimateProfitLossDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public EstimateProfitLossDetail GetObjectById(int Id)
        {
            EstimateProfitLossDetail data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public EstimateProfitLossDetail GetObjectByEstimateProfitLossId(int Id)
        {
            EstimateProfitLossDetail data = Find(x => x.EstimateProfitLossId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public EstimateProfitLossDetail CreateObject(EstimateProfitLossDetail model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public EstimateProfitLossDetail UpdateObject(EstimateProfitLossDetail model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public EstimateProfitLossDetail SoftDeleteObject(EstimateProfitLossDetail model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            EstimateProfitLossDetail data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
