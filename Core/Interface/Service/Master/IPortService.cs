using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPortService
    {
        IQueryable<Port> GetQueryable();
        Port GetObjectById(int Id);
        Port CreateObject(Port port, ICityLocationService _citylocationService);
        Port UpdateObject(Port port, ICityLocationService _citylocationService);
        Port SoftDeleteObject(Port port);
        bool IsNameDuplicated(Port port);
    }
}