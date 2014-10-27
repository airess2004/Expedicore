using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContainerYardRepository : IRepository<ContainerYard>
    {
       IQueryable<ContainerYard> GetQueryable();
       ContainerYard GetObjectById(int Id);
       ContainerYard CreateObject(ContainerYard model);
       ContainerYard UpdateObject(ContainerYard model);
       ContainerYard SoftDeleteObject(ContainerYard model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(ContainerYard model);
       int GetLastMasterCode(int officeId);
    }
}