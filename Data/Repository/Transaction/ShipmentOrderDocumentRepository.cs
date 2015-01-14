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
    public class ShipmentOrderDocumentRepository : EfRepository<ShipmentOrderDocument>, IShipmentOrderDocumentRepository 
    {
        private ExpedicoEntities entities;
         
        public ShipmentOrderDocumentRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<ShipmentOrderDocument> GetQueryable()
        {
            return FindAll();
        }

        public ShipmentOrderDocument GetObjectById(int Id)
        {
            ShipmentOrderDocument data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         

        public ShipmentOrderDocument GetObjectByShipmentOrderId(int Id)
        {
            ShipmentOrderDocument data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
          
        public IList<ShipmentOrderDocument> GetListByShipmentOrderId(int Id)
        { 
            IList<ShipmentOrderDocument> data = FindAll(x => x.ShipmentOrderId == Id && !x.IsDeleted).ToList();
            return data;
        }

        public int GetLastMasterCode(int officeId)
        {
            //ShipmentOrderDocument data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            //if (data != null)
            //{
            //    return data.MasterCode;
            //}
            //else
            //{
                return 0;
            //}
        }

        public ShipmentOrderDocument CreateObject(ShipmentOrderDocument model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            model.UpdatedBy = null;
            model.UpdatedAt = null;
            return Create(model);
        }

        public ShipmentOrderDocument UpdateObject(ShipmentOrderDocument model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public ShipmentOrderDocument SoftDeleteObject(ShipmentOrderDocument model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            ShipmentOrderDocument data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
