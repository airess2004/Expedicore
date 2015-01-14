using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IShipmentOrderDocumentRepository : IRepository<ShipmentOrderDocument>
    { 
       IQueryable<ShipmentOrderDocument> GetQueryable();
       ShipmentOrderDocument GetObjectById(int Id);
       ShipmentOrderDocument GetObjectByShipmentOrderId(int Id);
       IList<ShipmentOrderDocument> GetListByShipmentOrderId(int Id);
       ShipmentOrderDocument CreateObject(ShipmentOrderDocument model);
       ShipmentOrderDocument UpdateObject(ShipmentOrderDocument model);
       ShipmentOrderDocument SoftDeleteObject(ShipmentOrderDocument model);
       bool DeleteObject(int Id);
       int GetLastMasterCode(int officeId);
    }
}