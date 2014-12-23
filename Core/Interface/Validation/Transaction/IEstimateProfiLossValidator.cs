using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IEstimateProfitLossValidation
    {
        EstimateProfitLoss VCreateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService, IShipmentOrderService _shipmentOrderService);
        EstimateProfitLoss VUpdateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService, IShipmentOrderService _shipmentOrderService);
        EstimateProfitLoss VConfirmObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService);
        EstimateProfitLoss VUnConfirmObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService,
                          IEstimateProfitLossDetailService _estimateProfitLossDetailService, IInvoiceDetailService _invoiceDetailService,
                          IPaymentRequestDetailService _paymentRequestDetailService);
        EstimateProfitLoss VSoftDeleteObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService);
    }
}
