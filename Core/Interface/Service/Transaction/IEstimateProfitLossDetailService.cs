using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEstimateProfitLossDetailService
    {
        IQueryable<EstimateProfitLossDetail> GetQueryable();
        EstimateProfitLossDetail GetObjectById(int Id);
        EstimateProfitLossDetail GetObjectByEstimateProfitLossId(int Id);
        EstimateProfitLossDetail CreateObject(EstimateProfitLossDetail estimateprofitlossdetail);
        EstimateProfitLossDetail UpdateObject(EstimateProfitLossDetail estimateprofitlossdetail); 
        EstimateProfitLossDetail SoftDeleteObject(EstimateProfitLossDetail estimateprofitlossdetail);
    }
}