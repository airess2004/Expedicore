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
    public class DepoRepository : EfRepository<Depo>, IDepoRepository
    {
        private ExpedicoEntities entities;

        public DepoRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Depo> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public Depo GetObjectById(int Id)
        {
            Depo data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         
        public int GetLastMasterCode(int officeId) 
        {
            Depo data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public Depo CreateObject(Depo model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Depo UpdateObject(Depo model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Depo SoftDeleteObject(Depo model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Depo data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Depo model)
        {
            IQueryable<Depo> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }


    }
}
