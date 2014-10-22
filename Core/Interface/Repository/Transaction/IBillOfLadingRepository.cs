using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBillOfLadingRepository : IRepository<BillOfLading>
    { 
       IQueryable<BillOfLading> GetQueryable();
       BillOfLading GetObjectById(int Id);
       BillOfLading GetObjectByShipmentOrderId(int Id);
       BillOfLading CreateObject(BillOfLading model);
       BillOfLading UpdateObject(BillOfLading model);
       BillOfLading SoftDeleteObject(BillOfLading model);
       bool DeleteObject(int Id);
    }
}