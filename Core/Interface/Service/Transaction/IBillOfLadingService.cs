using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBillOfLadingService
    {
        IQueryable<BillOfLading> GetQueryable();
        BillOfLading GetObjectById(int Id);
        BillOfLading GetObjectByShipmentOrderId(int Id);
        BillOfLading CreateUpdateObject(BillOfLading billoflading);
        BillOfLading CreateObject(BillOfLading billoflading);
        BillOfLading UpdateObject(BillOfLading billoflading); 
        BillOfLading SoftDeleteObject(BillOfLading billoflading);
    }
}