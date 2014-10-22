using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEstimateProfitLossRepository : IRepository<EstimateProfitLoss>
    { 
       IQueryable<EstimateProfitLoss> GetQueryable();
       EstimateProfitLoss GetObjectById(int Id);
       EstimateProfitLoss GetObjectByShipmentOrderId(int Id);
       EstimateProfitLoss CreateObject(EstimateProfitLoss model);
       EstimateProfitLoss UpdateObject(EstimateProfitLoss model);
       EstimateProfitLoss SoftDeleteObject(EstimateProfitLoss model);
       bool DeleteObject(int Id);
    }
}