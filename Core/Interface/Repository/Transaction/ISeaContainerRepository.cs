using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISeaContainerRepository : IRepository<SeaContainer>
    { 
       IQueryable<SeaContainer> GetQueryable();
       SeaContainer GetObjectById(int Id);
       SeaContainer GetObjectByShipmentOrderId(int Id);
       SeaContainer CreateObject(SeaContainer model);
       SeaContainer UpdateObject(SeaContainer model);
       SeaContainer SoftDeleteObject(SeaContainer model);
       bool DeleteObject(int Id);
    }
}