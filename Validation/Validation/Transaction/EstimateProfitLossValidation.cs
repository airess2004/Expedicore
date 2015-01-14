using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class EstimateProfitLossValidation : IEstimateProfitLossValidation
    {

        public EstimateProfitLoss VvalidEPL(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        {
            EstimateProfitLoss epl = _estimateprofitlossService.GetObjectById(estimateprofitloss.Id);
            if (epl == null)
            {
                estimateprofitloss.Errors.Add("Generic", "Invalid EPL");
            }
            else
            {
                if (estimateprofitloss.OfficeId != epl.OfficeId)
                {
                    estimateprofitloss.Errors.Add("Generic", "Invalid EPL");
                }
            }
            return estimateprofitloss;
        }
         
        public EstimateProfitLoss VIsConfirmed(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        {
            EstimateProfitLoss epl = _estimateprofitlossService.GetObjectById(estimateprofitloss.Id);
            if (epl.IsConfirmed == true)
            {
                estimateprofitloss.Errors.Add("Generic", "EPL is Confirmed");
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss VIsUnconfirmed(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        { 
            EstimateProfitLoss epl = _estimateprofitlossService.GetObjectById(estimateprofitloss.Id);
            if (epl.IsConfirmed == false)
            {
                estimateprofitloss.Errors.Add("Generic", "EPL is Unconfirmed");
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss ValreadyAssignShipmentOrder(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        { 
            EstimateProfitLoss epl = _estimateprofitlossService.GetObjectByShipmentOrderId(estimateprofitloss.ShipmentOrderId);
            if (epl != null)
            {
                estimateprofitloss.Errors.Add("Generic", "ShipmentOrder : " + epl.ShipmentOrder.ShipmentOrderCode + " sudah di buat EPL");
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss VvalidShipmentOrder(EstimateProfitLoss estimateprofitloss, IShipmentOrderService _shipmentOrderService, IEstimateProfitLossService _estimateprofitlossService)
        {
            ShipmentOrder shipmentOrder = _shipmentOrderService.GetObjectById(estimateprofitloss.ShipmentOrderId);
            if (shipmentOrder == null)
            {
                estimateprofitloss.Errors.Add("ShipmentOrder", "Invalid Shipment Order");
            }
            else
            {
                bool eplAsSubJob = false;
                if (shipmentOrder.SubJobNumber > 0 && (shipmentOrder.TotalSub > 0))
                {
                    eplAsSubJob = true;
                }
                if (eplAsSubJob)
                {
                    var shipmentMainJob = _shipmentOrderService.GetQueryable().Where(s => s.JobNumber == shipmentOrder.JobNumber && s.JobId == shipmentOrder.JobId && s.OfficeId == shipmentOrder.OfficeId
                                                                    && s.SubJobNumber == 0 && (s.TotalSub > 0)).FirstOrDefault();
                    if (shipmentMainJob != null)
                    {
                        var eplAsMainJob = _estimateprofitlossService.GetObjectByShipmentOrderId(shipmentMainJob.Id);
                        if (eplAsMainJob == null)
                        {
                            estimateprofitloss.Errors.Add("Generic","EPL untuk Shipment Order :" + shipmentMainJob.ShipmentOrderCode + " belum di buat");
                        }
                    }
                }
            }
            return estimateprofitloss;
        }
         
        public EstimateProfitLoss ValreadyAssignInvoiceOrPaymentRequest(EstimateProfitLoss estimateprofitloss, 
            IEstimateProfitLossDetailService _estimateProfitLossDetailService,IInvoiceDetailService _invoiceDetailService,
            IPaymentRequestDetailService _paymentRequestDetailService)
        {
            IList<EstimateProfitLossDetail> eplDs = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLossId == estimateprofitloss.Id && x.IsDeleted == false).ToList();
            if (eplDs != null)
            {
                foreach (var eplDetail in eplDs)
                {
                    // check invoice use EPL
                    InvoiceDetail invoiceDetail = _invoiceDetailService.GetQueryable().Where(x => x.EPLDetailId == eplDetail.Id && x.IsDeleted == false).FirstOrDefault();
                    if (invoiceDetail != null)
                    {
                       estimateprofitloss.Errors.Add("Generic","EPL telah digunakan di Invoice : " + invoiceDetail.Invoices.InvoicesNo);
                       return estimateprofitloss;
                    }
                    PaymentRequestDetail paymentRequestDetail = _paymentRequestDetailService.GetQueryable().Where(x => x.EPLDetailId == eplDetail.Id && x.IsDeleted == false).FirstOrDefault();
                    if (paymentRequestDetail != null)
                    {
                        estimateprofitloss.Errors.Add("Generic", "EPL telah digunakan di PaymentRequest : " + paymentRequestDetail.PaymentRequest.PRNo);
                        return estimateprofitloss;
                    }
                }
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss VCreateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService, IShipmentOrderService _shipmentOrderService)
        {
            ValreadyAssignShipmentOrder(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            VvalidShipmentOrder(estimateprofitloss,_shipmentOrderService,_estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            return estimateprofitloss;
        }

        public EstimateProfitLoss VUpdateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService, IShipmentOrderService _shipmentOrderService)
        {
            VvalidEPL(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            ValreadyAssignShipmentOrder(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            VvalidShipmentOrder(estimateprofitloss, _shipmentOrderService, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            return estimateprofitloss;
        }
          
        public EstimateProfitLoss VUnConfirmObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService, 
           IEstimateProfitLossDetailService _estimateProfitLossDetailService, IInvoiceDetailService _invoiceDetailService ,
            IPaymentRequestDetailService _paymentRequestDetailService)
        {
            VIsUnconfirmed(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            VvalidEPL(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            //ValreadyAssignInvoiceOrPaymentRequest(estimateprofitloss, _estimateProfitLossDetailService, _invoiceDetailService, _paymentRequestDetailService);
            //if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            return estimateprofitloss;
        }

        public EstimateProfitLoss VConfirmObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        {
            VIsConfirmed(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            VvalidEPL(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            return estimateprofitloss;
        }

        public EstimateProfitLoss VSoftDeleteObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        { 
            VvalidEPL(estimateprofitloss, _estimateprofitlossService);
            if (!isValid(estimateprofitloss)) { return estimateprofitloss; }
            return estimateprofitloss;
        }

        public bool isValid(EstimateProfitLoss obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
