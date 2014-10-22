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

        public bool IsNameDuplicated(Airline model)
        { 
            IQueryable<Airline> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }

    }
}
