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
    public class ShipmentOrderRoutingRepository : EfRepository<ShipmentOrderRouting>, IShipmentOrderRoutingRepository 
    {
        private ExpedicoEntities entities;
         
        public ShipmentOrderRoutingRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<ShipmentOrderRouting> GetQueryable()
        {
            return FindAll();
        }

        public ShipmentOrderRouting GetObjectById(int Id)
        {
            ShipmentOrderRouting data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         

        public ShipmentOrderRouting GetObjectByShipmentOrderId(int Id)
        {
            ShipmentOrderRouting data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
          
        public IList<ShipmentOrderRouting> GetListByShipmentOrderId(int Id)
        { 
            IList<ShipmentOrderRouting> data = FindAll(x => x.ShipmentOrderId == Id && !x.IsDeleted).ToList();
            return data;
        }

        public int GetLastMasterCode(int officeId)
        {
            ShipmentOrderRouting data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public ShipmentOrderRouting CreateObject(ShipmentOrderRouting model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            model.UpdatedBy = null;
            model.UpdatedAt = null;
            return Create(model);
        }

        public ShipmentOrderRouting UpdateObject(ShipmentOrderRouting model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public ShipmentOrderRouting SoftDeleteObject(ShipmentOrderRouting model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            ShipmentOrderRouting data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
