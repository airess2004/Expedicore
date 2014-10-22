using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEstimateProfitLossDetailRepository : IRepository<EstimateProfitLossDetail>
    { 
       IQueryable<EstimateProfitLossDetail> GetQueryable();
       EstimateProfitLossDetail GetObjectById(int Id);
       EstimateProfitLossDetail GetObjectByEstimateProfitLossId(int Id);
       EstimateProfitLossDetail CreateObject(EstimateProfitLossDetail model);
       EstimateProfitLossDetail UpdateObject(EstimateProfitLossDetail model);
       EstimateProfitLossDetail SoftDeleteObject(EstimateProfitLossDetail model);
       bool DeleteObject(int Id);
    }
}