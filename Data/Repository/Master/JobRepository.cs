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
    public class JobRepository : EfRepository<Job>, IJobRepository
    {
        private ExpedicoEntities entities;

        public JobRepository()
        {
            entities = new ExpedicoEntities();
        }

        public IQueryable<Job> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public Job GetObjectById(int Id)
        {
            Job data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public Job CreateObject(Job model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Job UpdateObject(Job model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Job SoftDeleteObject(Job model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Job data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }
    }
}
