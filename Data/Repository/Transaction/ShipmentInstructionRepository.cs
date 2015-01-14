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
    public class ShipmentInstructionRepository : EfRepository<ShipmentInstruction>, IShipmentInstructionRepository 
    {
        private ExpedicoEntities entities;
         
        public ShipmentInstructionRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<ShipmentInstruction> GetQueryable()
        {
            return FindAll();
        }

        public ShipmentInstruction GetObjectById(int Id)
        {
            ShipmentInstruction data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         

        public ShipmentInstruction GetObjectByShipmentOrderId(int Id)
        {
            ShipmentInstruction data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public ShipmentInstruction CreateObject(ShipmentInstruction model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            model.UpdatedBy = null;
            model.UpdatedAt = null;
            return Create(model);
        }

        public ShipmentInstruction UpdateObject(ShipmentInstruction model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public ShipmentInstruction SoftDeleteObject(ShipmentInstruction model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            ShipmentInstruction data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
