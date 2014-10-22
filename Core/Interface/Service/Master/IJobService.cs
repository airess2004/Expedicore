using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IJobService
    {
        IQueryable<Job> GetQueryable();
        Job GetObjectById(int Id);
        Job CreateObject(Job job);
        Job UpdateObject(Job job);
        Job SoftDeleteObject(Job job);
    }
}