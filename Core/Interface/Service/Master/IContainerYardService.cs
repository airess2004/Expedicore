using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContainerYardService
    {
        IQueryable<ContainerYard> GetQueryable();
        ContainerYard GetObjectById(int Id);
        ContainerYard CreateObject(ContainerYard containeryard);
        ContainerYard UpdateObject(ContainerYard containeryard);
        ContainerYard SoftDeleteObject(ContainerYard containeryard);
        bool IsNameDuplicated(ContainerYard containeryard);
    }
}