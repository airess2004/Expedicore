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
        EstimateProfitLossDetail CreateObject(EstimateProfitLossDetail estimateprofitlossdetail, IShipmentOrderService _shipmentOrderService
            , ISeaContainerService _seaContainerService, IEstimateProfitLossService _estimateProfitLossService, IContactService _contactService, ICostService _costService); 
        EstimateProfitLossDetail SoftDeleteObject(EstimateProfitLossDetail estimateprofitlossdetail);
        EstimateProfitLossDetail ConfirmObject(EstimateProfitLossDetail estimateProfitLossDetail);
        EstimateProfitLossDetail UnconfirmObject(EstimateProfitLossDetail estimateProfitLossDetail);
        bool DeleteObject(int Id);
    }
}