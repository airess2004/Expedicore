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
    public class AirlineRepository : EfRepository<Airline>, IAirlineRepository
    {
        private ExpedicoEntities entities;

        public AirlineRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Airline> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public Airline GetObjectById(int Id)
        {
            Airline data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public Airline CreateObject(Airline model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Airline UpdateObject(Airline model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Airline SoftDeleteObject(Airline model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Airline data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }
    }
}
