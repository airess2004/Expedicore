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
    public class SeaContainerRepository : EfRepository<SeaContainer>, ISeaContainerRepository 
    {
        private ExpedicoEntities entities;
         
        public SeaContainerRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<SeaContainer> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public SeaContainer GetObjectById(int Id)
        {
            SeaContainer data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public SeaContainer GetObjectByShipmentOrderId(int Id)
        {
            SeaContainer data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public SeaContainer CreateObject(SeaContainer model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public SeaContainer UpdateObject(SeaContainer model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public SeaContainer SoftDeleteObject(SeaContainer model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            SeaContainer data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
