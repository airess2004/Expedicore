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
    public class ShipmentAdviceRepository : EfRepository<ShipmentAdvice>, IShipmentAdviceRepository 
    {
        private ExpedicoEntities entities;
         
        public ShipmentAdviceRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<ShipmentAdvice> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public ShipmentAdvice GetObjectById(int Id)
        {
            ShipmentAdvice data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public ShipmentAdvice GetObjectByShipmentOrderId(int Id)
        {
            ShipmentAdvice data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public ShipmentAdvice CreateObject(ShipmentAdvice model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public ShipmentAdvice UpdateObject(ShipmentAdvice model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public ShipmentAdvice SoftDeleteObject(ShipmentAdvice model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            ShipmentAdvice data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
