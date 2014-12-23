using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISeaContainerService
    {
        IQueryable<SeaContainer> GetQueryable();
        SeaContainer GetObjectById(int Id);
        SeaContainer GetObjectByShipmentOrderId(int Id);
        SeaContainer CreateUpdateObject(SeaContainer seacontainer);
        SeaContainer CreateObject(SeaContainer seacontainer);
        SeaContainer UpdateObject(SeaContainer seacontainer); 
        SeaContainer SoftDeleteObject(SeaContainer seacontainer);
    }
}