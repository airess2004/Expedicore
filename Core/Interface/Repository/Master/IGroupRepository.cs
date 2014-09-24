using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IGroupRepository : IRepository<Group>
    {
       IQueryable<Group> GetQueryable();
       Group GetObjectById(int Id);
       Group CreateObject(Group model);
       Group UpdateObject(Group model);
       Group SoftDeleteObject(Group model);
       bool DeleteObject(int Id);  
    }
}