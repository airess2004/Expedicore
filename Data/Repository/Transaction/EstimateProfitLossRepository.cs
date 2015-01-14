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
    public class EstimateProfitLossRepository : EfRepository<EstimateProfitLoss>, IEstimateProfitLossRepository 
    {
        private ExpedicoEntities entities;
         
        public EstimateProfitLossRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<EstimateProfitLoss> GetQueryable()
        {
            return FindAll();
        }

        public EstimateProfitLoss GetObjectById(int Id)
        {
            EstimateProfitLoss data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public EstimateProfitLoss GetObjectByShipmentOrderId(int Id)
        {
            EstimateProfitLoss data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public EstimateProfitLoss CreateObject(EstimateProfitLoss model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public EstimateProfitLoss UpdateObject(EstimateProfitLoss model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public EstimateProfitLoss SoftDeleteObject(EstimateProfitLoss model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            EstimateProfitLoss data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public EstimateProfitLoss ConfirmObject(EstimateProfitLoss model)
        {
            model.IsConfirmed = true;
            Update(model);
            return model;
        }

        public EstimateProfitLoss UnconfirmObject(EstimateProfitLoss model)
        {
            model.ConfirmationDate = null;
            model.IsConfirmed = false;
            Update(model);
            return model;
        }
    }
}

