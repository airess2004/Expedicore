using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EstimateProfitLossDetailService : IEstimateProfitLossDetailService 
    {  
        private IEstimateProfitLossDetailRepository _repository;
        private IEstimateProfitLossDetailValidation _validator;

        public EstimateProfitLossDetailService(IEstimateProfitLossDetailRepository _estimateprofitlossdetailRepository, IEstimateProfitLossDetailValidation _estimateprofitlossdetailValidation)
        {
            _repository = _estimateprofitlossdetailRepository;
            _validator = _estimateprofitlossdetailValidation;
        }

        public IQueryable<EstimateProfitLossDetail> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public EstimateProfitLossDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EstimateProfitLossDetail GetObjectByEstimateProfitLossId(int Id)
        {
            return _repository.GetObjectByEstimateProfitLossId(Id);
        }

        public EstimateProfitLossDetail CreateObject(EstimateProfitLossDetail estimateprofitlossdetail,IShipmentOrderService _shipmentOrderService
            ,ISeaContainerService _seaContainerService,IEstimateProfitLossService _estimateProfitLossService,IContactService _contactService,ICostService _costService)
        { 
            estimateprofitlossdetail.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(estimateprofitlossdetail,_estimateProfitLossService,_contactService,_costService)))
            {
                EstimateProfitLossDetail newEPLDetail  = new EstimateProfitLossDetail();
                newEPLDetail.CostId = estimateprofitlossdetail.CostId;
                newEPLDetail.AmountCrr = estimateprofitlossdetail.AmountCrr.Value;
                newEPLDetail.Amount = estimateprofitlossdetail.Amount;
                newEPLDetail.CodingQuantity = estimateprofitlossdetail.CodingQuantity;
                newEPLDetail.OfficeId = estimateprofitlossdetail.OfficeId;
                newEPLDetail.CreatedById = estimateprofitlossdetail.CreatedById;
                newEPLDetail.CreatedAt = DateTime.Now;
                newEPLDetail.CustomerId = estimateprofitlossdetail.CustomerId;
                newEPLDetail.CustomerTypeId = estimateprofitlossdetail.CustomerTypeId;
                newEPLDetail.DataFrom = false;
                newEPLDetail.Description = String.IsNullOrEmpty(estimateprofitlossdetail.Description) ? "" : estimateprofitlossdetail.Description.ToUpper();
                newEPLDetail.EstimateProfitLossId = estimateprofitlossdetail.EstimateProfitLossId;
                newEPLDetail.IsIncome = true;
                newEPLDetail.PerQty = estimateprofitlossdetail.PerQty;
                newEPLDetail.Quantity = estimateprofitlossdetail.Quantity;
                newEPLDetail.Sign = estimateprofitlossdetail.Sign;
                newEPLDetail.Type = estimateprofitlossdetail.Type;

                var epl = _estimateProfitLossService.GetObjectById(estimateprofitlossdetail.EstimateProfitLossId);
                if (epl != null)
                {
                    var shipment = _shipmentOrderService.GetObjectById(epl.ShipmentOrderId);
                    if (shipment != null)
                    {
                        // Check Main JOb and Have Sub or NOT
                        if (shipment.SubJobNumber == 0 &&  shipment.TotalSub > 0)
                        {
                            newEPLDetail.DataFrom = estimateprofitlossdetail.IsSplitIncCost;
                            newEPLDetail.IsSplitIncCost = estimateprofitlossdetail.IsSplitIncCost;
                        }

                      
                    }
                }

                estimateprofitlossdetail = _repository.CreateObject(newEPLDetail);

                if (epl != null)
                {
                    var shipment = _shipmentOrderService.GetObjectById(epl.ShipmentOrderId);
                    if (shipment != null)
                    {
                        var mainJob = _shipmentOrderService.GetQueryable().Where(s => s.JobNumber == shipment.JobNumber && s.JobId == shipment.JobId && s.OfficeId == shipment.OfficeId &&
                                                                                 s.SubJobNumber == 0 && s.TotalSub > 0).FirstOrDefault();
                        if (mainJob != null)
                        {
                            var mainEPL = _estimateProfitLossService.GetObjectByShipmentOrderId(mainJob.Id);
                            if (mainEPL != null)
                            {
                                // Create SPLIT ACCOUNT if EPL as Sub Job and Have Split Account
                                SplitEPLAccount(newEPLDetail, _shipmentOrderService, _seaContainerService, _estimateProfitLossService);
                            }
                        }
                    }
                }
                

               
            }
            return estimateprofitlossdetail;
        }

        private void SplitEPLAccount(EstimateProfitLossDetail estimateProfitLossDetail, IShipmentOrderService _shipmentOrderService, ISeaContainerService _seaContainerService, IEstimateProfitLossService _estimateProfitLossService)
        {
            var epl = _repository.Find(e => e.Id == estimateProfitLossDetail.EstimateProfitLossId);
                if (epl != null)
                {
                    var shipment = _shipmentOrderService.GetObjectById(estimateProfitLossDetail.EstimateProfitLossId);
                    if (shipment != null)
                    {
                        // Get Main JOB
                        var shipmentMainJob = _shipmentOrderService.GetQueryable().Where(s => s.JobNumber == shipment.JobNumber && s.JobId == shipment.JobId
                            && s.OfficeId == shipment.OfficeId && s.SubJobNumber == 0 && s.TotalSub > 0).FirstOrDefault();

                        // Get Container Main JOB
                        var mainContainer = (from c in _seaContainerService.GetQueryable().Where(c => c.ShipmentOrderId == shipmentMainJob.Id)
                                             group c by c.Size into cs
                                             select new
                                             {
                                                 Size = cs.Key,
                                                 TotalCBM = cs.Sum(x => x.CBM.HasValue ? x.CBM.Value : 0)
                                             }).ToList();

                        // Get Sub JoB
                        var shipmentSubJob = _shipmentOrderService.GetQueryable().Where(s => s.JobNumber == shipment.JobNumber && s.SubJobNumber > 0 &&
                                                                            s.JobId == shipment.JobId && s.OfficeId == shipment.OfficeId);

                        foreach (var item in shipmentSubJob)
                        {
                            var eplSub = _estimateProfitLossService.GetObjectByShipmentOrderId(item.Id);
                            if (eplSub != null)
                            {
                                // Delete ALL EPL Detail from EPL Sub Job if IsSplitIncCost == True
                                var eplSubDetailList = _repository.GetQueryable().Where(x => x.EstimateProfitLossId == eplSub.Id).ToList();
                                foreach (var itemESDL in eplSubDetailList)
                                {
                                    if (itemESDL.IsSplitIncCost)
                                    {
                                        _repository.DeleteObject(itemESDL.Id);
                                    }
                                }

                                var eplDetailsMainJob = _repository.GetQueryable().Where(x => x.EstimateProfitLossId == estimateProfitLossDetail.EstimateProfitLossId).ToList();
                                foreach (var itemED in eplDetailsMainJob)
                                {
                                    // Cost Split - ONLY Cost SSLine, Cost EMKL, Cost Rebate
                                    if (itemED.IsSplitIncCost == true &&
                                        ((itemED.CustomerTypeId == MasterConstant.ContactType.SSLine && itemED.IsIncome == false) ||
                                        (itemED.CustomerTypeId == MasterConstant.ContactType.EMKL && itemED.IsIncome == false) ||
                                        (itemED.CustomerTypeId == MasterConstant.ContactType.RebateShipper && itemED.IsIncome == false) ||
                                        (itemED.CustomerTypeId == MasterConstant.ContactType.RebateConsignee && itemED.IsIncome == false)))
                                    {
                                        // Get Container per Sub Shipment Order
                                        var subContainer = (from c in _seaContainerService.GetQueryable().Where(c => c.ShipmentOrderId == item.Id).ToList()
                                                            group c by c.Size into cs
                                                            select new
                                                            {
                                                                Size = cs.Key,
                                                                TotalCBM = cs.Sum(x => x.CBM.HasValue ? x.CBM.Value : 0)
                                                            }).ToList();

                                        var totalMainCBM = (from m in mainContainer where m.Size == itemED.Type select m.TotalCBM).FirstOrDefault();
                                        var totalSubCBM = (from s in subContainer where s.Size == itemED.Type select s.TotalCBM).FirstOrDefault();

                                        var quantity = (totalSubCBM / totalMainCBM) * itemED.Quantity;
                                        var amount = quantity * itemED.PerQty;

                                        EstimateProfitLossDetail newEPLDetail = new EstimateProfitLossDetail();
                                        newEPLDetail.CostId = itemED.CostId;
                                        newEPLDetail.AmountCrr = itemED.AmountCrr;
                                        newEPLDetail.Amount = amount;
                                        newEPLDetail.CodingQuantity = itemED.CodingQuantity;
                                        newEPLDetail.OfficeId = estimateProfitLossDetail.OfficeId;
                                        newEPLDetail.CreatedById = estimateProfitLossDetail.CreatedById;
                                        newEPLDetail.CreatedAt = DateTime.Now;
                                        newEPLDetail.CustomerId = itemED.CustomerId;
                                        newEPLDetail.CustomerTypeId = itemED.CustomerTypeId;
                                        newEPLDetail.DataFrom = true;
                                        newEPLDetail.Description = itemED.Description;
                                        newEPLDetail.EstimateProfitLossId = eplSub.Id;
                                        newEPLDetail.IsIncome = itemED.IsIncome;
                                        newEPLDetail.IsSplitIncCost = true;
                                        newEPLDetail.DataFrom = true;
                                        newEPLDetail.PerQty = itemED.PerQty;
                                        newEPLDetail.Quantity = quantity;
                                        newEPLDetail.Sequence = itemED.Sequence;
                                        newEPLDetail.Sign = itemED.Sign;
                                        newEPLDetail.Type = itemED.Type;
                                         
                                        newEPLDetail = _repository.CreateObject(newEPLDetail);

                                        this.CalculateTotalEPL(newEPLDetail,_estimateProfitLossService,_shipmentOrderService);
                                    }
                                }
                            }
                        }
                    }
                }
        }

        #region Calculate Total EPL

        private void CalculateTotalEPL(EstimateProfitLossDetail eplDetail,IEstimateProfitLossService _estimateProfitLossService,IShipmentOrderService _shipmentOrderService)
        {
                var epl = _estimateProfitLossService.GetObjectById(eplDetail.EstimateProfitLossId);
                if (epl != null)
                {
                    // Get Shipment Order
                    var shipment = _shipmentOrderService.GetObjectById(epl.ShipmentOrderId);
                    if (shipment != null)
                    {
                        switch (shipment.JobId)
                        {
                            case MasterConstant.JobType.SeaExport:
                            case MasterConstant.JobType.EMKLSeaExport:
                                CalculateTotalEPLJobSeaExport(epl.Id,_estimateProfitLossService,_shipmentOrderService);
                                break;
                            case MasterConstant.JobType.SeaImport:
                            case MasterConstant.JobType.EMKLSeaImport:
                                CalculateTotalEPLJobSeaImport(epl.Id, _estimateProfitLossService,_shipmentOrderService);
                                break;
                            case MasterConstant.JobType.AirExport:
                            case MasterConstant.JobType.EMKLAirExport:
                                CalculateTotalEPLJobAirExport(epl.Id, _estimateProfitLossService,_shipmentOrderService);
                                break;
                            case MasterConstant.JobType.AirImport:
                            case MasterConstant.JobType.EMKLAirImport:
                                CalculateTotalEPLJobAirImport(epl.Id, _estimateProfitLossService,_shipmentOrderService);
                                break;
                            case MasterConstant.JobType.EMKLDomestic:
                                CalculateTotalEPLJobDomestic(epl.Id, _estimateProfitLossService,_shipmentOrderService);
                                break;
                        }
                    }
                }
            }

        private void CalculateTotalEPLJobSeaExport(int eplId,IEstimateProfitLossService _estimateProfitLossService,IShipmentOrderService _shipmentOrderService)
        {
                var incShipperUSD = GetTotalIncShipperUSD(eplId);
                var incShipperIDR = GetTotalIncShipperIDR(eplId);
                var incAgentUSD = GetTotalIncAgentUSD(eplId);
                var incAgentIDR = GetTotalIncAgentIDR(eplId);
                var costSSLineUSD = GetTotalCostSSLineUSD(eplId);
                var costSSLineIDR = GetTotalCostSSLineIDR(eplId);
                var costEMKLUSD = GetTotalCostEMKLUSD(eplId);
                var costEMKLIDR = GetTotalCostEMKLIDR(eplId);
                var costRebateShipperUSD = GetTotalCostRebateShipperUSD(eplId);
                var costRebateShipperIDR = GetTotalCostRebateShipperIDR(eplId);
                var costAgentUSD = GetTotalCostAgentUSD(eplId);
                var costAgentIDR = GetTotalCostAgentIDR(eplId);
                var costDepoUSD = GetTotalCostDepoUSD(eplId);
                var costDepoIDR = GetTotalCostDepoIDR(eplId);

                decimal shipperUSD = incShipperUSD - (costSSLineUSD + costEMKLUSD + costRebateShipperUSD + costDepoUSD);
                decimal shipperIDR = incShipperIDR - (costSSLineIDR + costEMKLIDR + costRebateShipperIDR + costDepoIDR);
                decimal agentUSD = incAgentUSD - costAgentUSD;
                decimal agentIDR = incAgentIDR - costAgentIDR;

                var epl = _estimateProfitLossService.GetObjectById(eplId);
                if (epl != null)
                {
                    epl.EstUSDShipCons = shipperUSD;
                    epl.EstIDRShipCons = shipperIDR;
                    epl.EstUSDAgent = agentUSD;
                    epl.EstIDRAgent = agentIDR;
                    _estimateProfitLossService.UpdateObject(epl,_shipmentOrderService);
                }
        }

        private void CalculateTotalEPLJobSeaImport(int eplId,IEstimateProfitLossService _estimateProfitLossService,IShipmentOrderService _shipmentOrderService)
        {
                var incConsigneeUSD = GetTotalIncConsigneeUSD(eplId);
                var incConsigneeIDR = GetTotalIncConsigneeIDR(eplId);
                var incAgentUSD = GetTotalIncAgentUSD(eplId);
                var incAgentIDR = GetTotalIncAgentIDR(eplId);
                var costSSLineUSD = GetTotalCostSSLineUSD(eplId);
                var costSSLineIDR = GetTotalCostSSLineIDR(eplId);
                var costEMKLUSD = GetTotalCostEMKLUSD(eplId);
                var costEMKLIDR = GetTotalCostEMKLIDR(eplId);
                var costRebateConsigneeUSD = GetTotalCostRebateConsigneeUSD(eplId);
                var costRebateConsigneeIDR = GetTotalCostRebateConsigneeIDR(eplId);
                var costAgentUSD = GetTotalCostAgentUSD(eplId);
                var costAgentIDR = GetTotalCostAgentIDR(eplId);
                var costDepoUSD = GetTotalCostDepoUSD(eplId);
                var costDepoIDR = GetTotalCostDepoIDR(eplId);

                decimal consigneeUSD = incConsigneeUSD - (costSSLineUSD + costEMKLUSD + costRebateConsigneeUSD + costDepoUSD);
                decimal consigneeIDR = incConsigneeIDR - (costSSLineIDR + costEMKLIDR + costRebateConsigneeIDR + costDepoIDR);
                decimal agentUSD = incAgentUSD - costAgentUSD;
                decimal agentIDR = incAgentIDR - costAgentIDR;

                var epl = _estimateProfitLossService.GetObjectById(eplId);
                if (epl != null)
                {
                    epl.EstUSDShipCons = consigneeUSD;
                    epl.EstIDRShipCons = consigneeIDR;
                    epl.EstUSDAgent = agentUSD;
                    epl.EstIDRAgent = agentIDR;
                    _estimateProfitLossService.UpdateObject(epl,_shipmentOrderService);

                }
        }

        private void CalculateTotalEPLJobAirExport(int eplId, IEstimateProfitLossService _estimateProfitLossService, IShipmentOrderService _shipmentOrderService)
        {
                var incShipperUSD = GetTotalIncShipperUSD(eplId);
                var incShipperIDR = GetTotalIncShipperIDR(eplId);
                var incAgentUSD = GetTotalIncAgentUSD(eplId);
                var incAgentIDR = GetTotalIncAgentIDR(eplId);
                var costIATAUSD = GetTotalCostIATAUSD(eplId);
                var costIATAIDR = GetTotalCostIATAIDR(eplId);
                var costEMKLUSD = GetTotalCostEMKLUSD(eplId);
                var costEMKLIDR = GetTotalCostEMKLIDR(eplId);
                var costRebateShipperUSD = GetTotalCostRebateShipperUSD(eplId);
                var costRebateShipperIDR = GetTotalCostRebateShipperIDR(eplId);
                var costAgentUSD = GetTotalCostAgentUSD(eplId);
                var costAgentIDR = GetTotalCostAgentIDR(eplId);
                var costDepoUSD = GetTotalCostDepoUSD(eplId);
                var costDepoIDR = GetTotalCostDepoIDR(eplId);

                decimal shipperUSD = incShipperUSD - (costIATAUSD + costEMKLUSD + costRebateShipperUSD + costDepoUSD);
                decimal shipperIDR = incShipperIDR - (costIATAIDR + costEMKLIDR + costRebateShipperIDR + costDepoIDR);
                decimal agentUSD = incAgentUSD - costAgentUSD;
                decimal agentIDR = incAgentIDR - costAgentIDR;

                var epl = _estimateProfitLossService.GetObjectById(eplId);

                if (epl != null)
                {
                    epl.EstUSDShipCons = shipperUSD;
                    epl.EstIDRShipCons = shipperIDR;
                    epl.EstUSDAgent = agentUSD;
                    epl.EstIDRAgent = agentIDR;
                    _estimateProfitLossService.UpdateObject(epl,_shipmentOrderService);

                }
        }

        private void CalculateTotalEPLJobAirImport(int eplId, IEstimateProfitLossService _estimateProfitLossService, IShipmentOrderService _shipmentOrderService)
        {
                var incConsigneeUSD = GetTotalIncConsigneeUSD(eplId);
                var incConsigneeIDR = GetTotalIncConsigneeIDR(eplId);
                var incAgentUSD = GetTotalIncAgentUSD(eplId);
                var incAgentIDR = GetTotalIncAgentIDR(eplId);
                var costIATAUSD = GetTotalCostIATAUSD(eplId);
                var costIATAIDR = GetTotalCostIATAIDR(eplId);
                var costEMKLUSD = GetTotalCostEMKLUSD(eplId);
                var costEMKLIDR = GetTotalCostEMKLIDR(eplId);
                var costRebateShipperUSD = GetTotalCostRebateShipperUSD(eplId);
                var costRebateShipperIDR = GetTotalCostRebateShipperIDR(eplId);
                var costAgentUSD = GetTotalCostAgentUSD(eplId);
                var costAgentIDR = GetTotalCostAgentIDR(eplId);
                var costDepoUSD = GetTotalCostDepoUSD(eplId);
                var costDepoIDR = GetTotalCostDepoIDR(eplId);

                decimal consigneeUSD = incConsigneeUSD - (costIATAUSD + costEMKLUSD + costRebateShipperUSD + costDepoUSD);
                decimal consigneeIDR = incConsigneeIDR - (costIATAIDR + costEMKLIDR + costRebateShipperIDR + costDepoIDR);
                decimal agentUSD = incAgentUSD - costAgentUSD;
                decimal agentIDR = incAgentIDR - costAgentIDR;

                var epl = _estimateProfitLossService.GetObjectById(eplId);

                if (epl != null)
                {
                    epl.EstUSDShipCons = consigneeUSD;
                    epl.EstIDRShipCons = consigneeIDR;
                    epl.EstUSDAgent = agentUSD;
                    epl.EstIDRAgent = agentIDR;
                    _estimateProfitLossService.UpdateObject(epl,_shipmentOrderService);

                }
        }

        private void CalculateTotalEPLJobDomestic(int eplId, IEstimateProfitLossService _estimateProfitLossService, IShipmentOrderService _shipmentOrderService)
        {
                var incShipperUSD = GetTotalIncShipperUSD(eplId);
                var incShipperIDR = GetTotalIncShipperIDR(eplId);
                var incConsigneeUSD = GetTotalIncConsigneeUSD(eplId);
                var incConsigneeIDR = GetTotalIncConsigneeIDR(eplId);
                var incAgentUSD = GetTotalIncAgentUSD(eplId);
                var incAgentIDR = GetTotalIncAgentIDR(eplId);
                var costSSLineUSD = GetTotalCostSSLineUSD(eplId);
                var costSSLineIDR = GetTotalCostSSLineIDR(eplId);
                var costEMKLUSD = GetTotalCostEMKLUSD(eplId);
                var costEMKLIDR = GetTotalCostEMKLIDR(eplId);
                var costRebateShipperUSD = GetTotalCostRebateShipperUSD(eplId);
                var costRebateShipperIDR = GetTotalCostRebateShipperIDR(eplId);
                var costRebateConsigneeUSD = GetTotalCostRebateConsigneeUSD(eplId);
                var costRebateConsigneeIDR = GetTotalCostRebateConsigneeIDR(eplId);
                var costAgentUSD = GetTotalCostAgentUSD(eplId);
                var costAgentIDR = GetTotalCostAgentIDR(eplId);
                var costDepoUSD = GetTotalCostDepoUSD(eplId);
                var costDepoIDR = GetTotalCostDepoIDR(eplId);

                decimal shipconsUSD = (incShipperUSD + incConsigneeUSD) - (costSSLineUSD + costEMKLUSD + costRebateShipperUSD + costRebateConsigneeUSD + costDepoUSD);
                decimal shipconsIDR = (incShipperIDR + incConsigneeIDR) - (costSSLineIDR + costEMKLIDR + costRebateShipperIDR + costRebateConsigneeIDR + costDepoIDR);
                decimal agentUSD = incAgentUSD - costAgentUSD;
                decimal agentIDR = incAgentIDR - costAgentIDR;

                var epl = _estimateProfitLossService.GetObjectById(eplId);

                if (epl != null)
                {
                    epl.EstUSDShipCons = shipconsUSD;
                    epl.EstIDRShipCons = shipconsIDR;
                    epl.EstUSDAgent = agentUSD;
                    epl.EstIDRAgent = agentIDR;
                    _estimateProfitLossService.UpdateObject(epl,_shipmentOrderService);

                }
            }

        private decimal GetTotalIncShipperUSD(int eplId)
        {
            var incShipperUSD = (from e in GetQueryable().Where(x=> x.Id == eplId).ToList()
                                 where e.IsIncome == true && e.CustomerTypeId == MasterConstant.ContactType.Shipper && e.AmountCrr == MasterConstant.Currency.USD
                                 select e.Amount).Sum();
            if (!incShipperUSD.HasValue)
                return 0;
            return incShipperUSD.Value;
        }

        private decimal GetTotalIncShipperIDR(int eplId)
        {
            var incShipperIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                 where e.IsIncome == true && e.CustomerTypeId == MasterConstant.ContactType.Shipper && e.AmountCrr == MasterConstant.Currency.IDR
                                 select e.Amount).Sum();
            if (!incShipperIDR.HasValue)
                return 0;
            return incShipperIDR.Value;
        }

        private decimal GetTotalIncAgentUSD(int eplId)
        {
            decimal incAgentUSD = 0;
            var incAgentUSDList = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                   where e.IsIncome == true && e.CustomerTypeId == MasterConstant.ContactType.Agent
                                   select e).ToList();

            foreach (var item in incAgentUSDList)
            {
                int CostId = item.CostId;
                decimal amountUSD = item.Amount.HasValue ? item.Amount.Value : 0;
                // NOT Selling Rate && Buying Rate
                if (CostId != 3 && CostId != 4)
                {
                    if (item.Sign.HasValue && item.Sign.Value == true)
                    {
                        incAgentUSD += amountUSD;
                    }
                    else
                    {
                        incAgentUSD -= amountUSD;
                    }
                }
            }

            return incAgentUSD;
        }

        private decimal GetTotalIncAgentIDR(int eplId)
        {
            decimal incAgentIDR = 0;
            var incAgentIDRList = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                   where e.IsIncome == true && e.CustomerTypeId == MasterConstant.ContactType.Agent
                                   select e).ToList();

            foreach (var item in incAgentIDRList)
            {
                int CostId = item.CostId;
                decimal amountIDR = item.Amount.HasValue ? item.Amount.Value : 0;
                // NOT Selling Rate && Buying Rate
                if (CostId != 3 && CostId != 4)
                {
                    if (item.Sign.HasValue && item.Sign.Value == true)
                    {
                        incAgentIDR += amountIDR;
                    }
                    else
                    {
                        incAgentIDR -= amountIDR;
                    }
                }
            }

            return incAgentIDR;
        }

        private decimal GetTotalIncConsigneeUSD(int eplId)
        {
            var incConsigneeUSD = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                   where e.IsIncome == true && e.CustomerTypeId == MasterConstant.ContactType.Consignee && e.AmountCrr == MasterConstant.Currency.USD
                                   select e.Amount).Sum();
            if (!incConsigneeUSD.HasValue)
                return 0;
            return incConsigneeUSD.Value;
        }

        private decimal GetTotalIncConsigneeIDR(int eplId)
        {
            var incConsigneeIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                   where e.IsIncome == true && e.CustomerTypeId == MasterConstant.ContactType.Consignee && e.AmountCrr == MasterConstant.Currency.IDR
                                   select e.Amount).Sum();
            if (!incConsigneeIDR.HasValue)
                return 0;
            return incConsigneeIDR.Value;
        }

        private decimal GetTotalCostAgentUSD(int eplId)
        {
            decimal costAgentUSD = 0;
            var costAgentUSDList = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                    where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.Agent
                                    select e).ToList();

            foreach (var item in costAgentUSDList)
            {
                int CostId = item.CostId;
                decimal amountUSD = item.Amount.HasValue ? item.Amount.Value : 0;
                // NOT Selling Rate && Buying Rate
                if (CostId != 3 && CostId != 4)
                {
                    if (item.Sign.HasValue && item.Sign.Value == true)
                    {
                        costAgentUSD += amountUSD;
                    }
                    else
                    {
                        costAgentUSD -= amountUSD;
                    }
                }
            }

            return costAgentUSD;
        }

        private decimal GetTotalCostAgentIDR(int eplId)
        {
            decimal costAgentIDR = 0;
            var costAgentIDRList = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                    where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.Agent
                                    select e).ToList();

            foreach (var item in costAgentIDRList)
            {
                int CostId = item.CostId;
                decimal amountIDR = item.Amount.HasValue ? item.Amount.Value : 0;
                // NOT Selling Rate && Buying Rate
                if (CostId != 3 && CostId != 4)
                {
                    if (item.Sign.HasValue && item.Sign.Value == true)
                    {
                        costAgentIDR += amountIDR;
                    }
                    else
                    {
                        costAgentIDR -= amountIDR;
                    }
                }
            }

            return costAgentIDR;
        }

        private decimal GetTotalCostSSLineUSD(int eplId)
        {
            var costSSLineUSD = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                 where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.SSLine && e.AmountCrr == MasterConstant.Currency.USD
                                 select e.Amount).Sum();
            if (!costSSLineUSD.HasValue)
                return 0;
            return costSSLineUSD.Value;
        }

        private decimal GetTotalCostSSLineIDR(int eplId)
        {
            var costSSLineIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                 where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.SSLine && e.AmountCrr == MasterConstant.Currency.IDR
                                 select e.Amount).Sum();
            if (!costSSLineIDR.HasValue)
                return 0;
            return costSSLineIDR.Value;
        }

        private decimal GetTotalCostIATAUSD(int eplId)
        {
            var costIATAUSD = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                               where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.IATA && e.AmountCrr == MasterConstant.Currency.USD
                               select e.Amount).Sum();
            if (!costIATAUSD.HasValue)
                return 0;
            return costIATAUSD.Value;
        }

        private decimal GetTotalCostIATAIDR(int eplId)
        {
            var costIATAIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                               where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.IATA && e.AmountCrr == MasterConstant.Currency.IDR
                               select e.Amount).Sum();
            if (!costIATAIDR.HasValue)
                return 0;
            return costIATAIDR.Value;
        }

        private decimal GetTotalCostEMKLUSD(int eplId)
        {
            var costEMKLUSD = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                               where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.EMKL && e.AmountCrr == MasterConstant.Currency.USD
                               select e.Amount).Sum();
            if (!costEMKLUSD.HasValue)
                return 0;
            return costEMKLUSD.Value;
        }

        private decimal GetTotalCostEMKLIDR(int eplId)
        {
            var costEMKLIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                               where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.EMKL && e.AmountCrr == MasterConstant.Currency.IDR
                               select e.Amount).Sum();
            if (!costEMKLIDR.HasValue)
                return 0;
            return costEMKLIDR.Value;
        }

        private decimal GetTotalCostDepoUSD(int eplId)
        {
            var costDepoUSD = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                               where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.Depo && e.AmountCrr == MasterConstant.Currency.USD
                               select e.Amount).Sum();
            if (!costDepoUSD.HasValue)
                return 0;
            return costDepoUSD.Value;
        }

        private decimal GetTotalCostDepoIDR(int eplId)
        {
            var costDepoIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                               where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.Depo && e.AmountCrr == MasterConstant.Currency.IDR
                               select e.Amount).Sum();
            if (!costDepoIDR.HasValue)
                return 0;
            return costDepoIDR.Value;
        }

        private decimal GetTotalCostRebateShipperUSD(int eplId)
        {
            var costRebateShipperUSD = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                        where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.RebateShipper && e.AmountCrr == MasterConstant.Currency.USD
                                        select e.Amount).Sum();
            if (!costRebateShipperUSD.HasValue)
                return 0;
            return costRebateShipperUSD.Value;
        }

        private decimal GetTotalCostRebateShipperIDR(int eplId)
        {
            var costRebateShipperIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                        where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.RebateShipper && e.AmountCrr == MasterConstant.Currency.IDR
                                        select e.Amount).Sum();
            if (!costRebateShipperIDR.HasValue)
                return 0;
            return costRebateShipperIDR.Value;
        }

        private decimal GetTotalCostRebateConsigneeUSD(int eplId)
        {
            var costRebateConsigneeUSD = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                          where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.RebateConsignee && e.AmountCrr == MasterConstant.Currency.USD
                                          select e.Amount).Sum();
            if (!costRebateConsigneeUSD.HasValue)
                return 0;
            return costRebateConsigneeUSD.Value;
        }

        private decimal GetTotalCostRebateConsigneeIDR(int eplId)
        {
            var costRebateConsigneeIDR = (from e in GetQueryable().Where(x => x.Id == eplId).ToList()
                                          where e.IsIncome == false && e.CustomerTypeId == MasterConstant.ContactType.RebateConsignee && e.AmountCrr == MasterConstant.Currency.IDR
                                          select e.Amount).Sum();
            if (!costRebateConsigneeIDR.HasValue)
                return 0;
            return costRebateConsigneeIDR.Value;
        }
        #endregion


     
        public EstimateProfitLossDetail UpdateObject(EstimateProfitLossDetail estimateprofitlossdetail,IEstimateProfitLossService _estimateProfitLossService,IContactService _contactService,ICostService _costService)
        {
            if (isValid(_validator.VUpdateObject(estimateprofitlossdetail,_estimateProfitLossService,this,_contactService,_costService)))
            {
                estimateprofitlossdetail = _repository.UpdateObject(estimateprofitlossdetail);
            }
            return estimateprofitlossdetail;
        }
         
        public EstimateProfitLossDetail SoftDeleteObject(EstimateProfitLossDetail estimateprofitlossdetail)
        {
            estimateprofitlossdetail = _repository.SoftDeleteObject(estimateprofitlossdetail);
            return estimateprofitlossdetail;
        }


        public bool isValid(EstimateProfitLossDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public EstimateProfitLossDetail ConfirmObject(EstimateProfitLossDetail estimateProfitLossDetail)
        {
            estimateProfitLossDetail = _repository.ConfirmObject(estimateProfitLossDetail);
            return estimateProfitLossDetail;
        }
        
        public EstimateProfitLossDetail UnconfirmObject(EstimateProfitLossDetail estimateProfitLossDetail)
        {
            estimateProfitLossDetail = _repository.UnconfirmObject(estimateProfitLossDetail);
            return estimateProfitLossDetail;
        }
    }
}
