using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IJobRepository : IRepository<Job>
    {
       IQueryable<Job> GetQueryable();
       Job GetObjectById(int Id);
       Job CreateObject(Job model);
       Job UpdateObject(Job model);
       Job SoftDeleteObject(Job model);
       bool DeleteObject(int Id);
    }
}