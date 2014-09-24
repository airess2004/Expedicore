using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IAirlineRepository : IRepository<Airline>
    {
       IQueryable<Airline> GetQueryable();
       Airline GetObjectById(int Id);
       Airline CreateObject(Airline model);
       Airline UpdateObject(Airline model);
       Airline SoftDeleteObject(Airline model);
       bool DeleteObject(int Id);  
    }
}