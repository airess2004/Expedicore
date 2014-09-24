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
    public class PortRepository : EfRepository<Port>, IPortRepository
    {
        private ExpedicoEntities entities;

        public PortRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Port> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public Port GetObjectById(int Id)
        {
            Port data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public Port CreateObject(Port model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Port UpdateObject(Port model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Port SoftDeleteObject(Port model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Port data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }
    }
}
