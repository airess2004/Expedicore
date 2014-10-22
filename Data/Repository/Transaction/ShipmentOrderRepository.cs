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
    public class ShipmentOrderRepository : EfRepository<ShipmentOrder>, IShipmentOrderRepository 
    {
        private ExpedicoEntities entities;
         
        public ShipmentOrderRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<ShipmentOrder> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }


        public ShipmentOrder GetObjectById(int Id)
        {
            ShipmentOrder data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public int GetLastJobNumber(int officeId,int jobId)
        { 
            int? data = FindAll(x => x.OfficeId == officeId && x.JobId == jobId).Max().JobNumber;
            if (data != null)
            {
                return data.Value;
            }
            else
            {
                return 1;
            }
        }
         
        public int GetLastSubJobNumber(int officeId,int jobId,int subJobNumber)
        { 
            int? data = FindAll(x => x.OfficeId == officeId && x.JobId == jobId && x.SubJobNumber == subJobNumber).Max().JobNumber;
            if (data != null)
            {
                return data.Value;
            }
            else
            {
                return 1;
            }
        }

        public int GetLastMasterCode(int officeId)
        {
            int? data = FindAll(x => x.OfficeId == officeId).Max().MasterCode;
            if (data != null)
            {
                return data.Value;
            }
            else
            {
                return 1;
            }
        }

        public ShipmentOrder CreateObject(ShipmentOrder model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public ShipmentOrder UpdateObject(ShipmentOrder model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public ShipmentOrder SoftDeleteObject(ShipmentOrder model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            ShipmentOrder data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
