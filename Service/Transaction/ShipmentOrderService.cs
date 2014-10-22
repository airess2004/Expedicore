using Core.DomainModel;
using Core.Constant;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Master
{
    public class ShipmentOrderService : IShipmentOrderService 
    {  
        private IShipmentOrderRepository _repository;
        private IShipmentOrderValidation _validator;

        public ShipmentOrderService(IShipmentOrderRepository _shipmentorderRepository, IShipmentOrderValidation _shipmentorderValidation)
        {
            _repository = _shipmentorderRepository;
            _validator = _shipmentorderValidation;
        }

        public IQueryable<ShipmentOrder> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ShipmentOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public static string Replicate(string codeVal, int length)
        {
            if (String.IsNullOrEmpty(codeVal))
            {
                return codeVal;
            }

            string result = "";
            for (int i = codeVal.Length; i < length; i++)
            {
                result += "0";
            }
            result += codeVal;

            return result;
        }

        public string GenerateShipmentNo(string intCompany, int jobtype, int jobNumber, int subJobNo)
        {  
            string shipmentNo = ""; 
            
            try
            {
                shipmentNo = intCompany + "." + jobtype + "." + Replicate(jobNumber.ToString(), 6) + "-" + Replicate(subJobNo.ToString(), 2);
            }
            catch (Exception ex)
            {
                //LOG.Error("GenerateShipmentNo Failed,", ex);
            }

            return shipmentNo;
        }
        public ShipmentOrder CreateObjectEMKLDomestic(ShipmentOrder shipmentorder)//OfficeId && CreatedBy
        {
            shipmentorder.SubJobNumber = 0;
            shipmentorder.SIReference = ""; 
            shipmentorder.GoodRecDate = DateTime.Today;
            shipmentorder.JobStatus = "FCL";
            shipmentorder.SIDate = DateTime.Today; 
            shipmentorder.LoadStatus = "FCL"; 
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0; 
            shipmentorder.AgentName = ""; 
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0; 
            shipmentorder.TranshipmentName = "";
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0;
            shipmentorder.ShipperName = ""; 
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0;
            shipmentorder.ConsigneeName = ""; 
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0;
            shipmentorder.NPartyName = "";
            shipmentorder.NPartyAddress = "";
            // Default Port
            shipmentorder.LoadingPortId = 0; shipmentorder.LoadingPortName = "";
            shipmentorder.DischargePortId = 0; shipmentorder.DischargePortName = "";
            shipmentorder.ReceiptPlaceId = 0; shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DeliveryPlaceId = 0; shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.OBLStatus = "P"; shipmentorder.OBLCollectId = 0; shipmentorder.OBLPayableId = 0;
            shipmentorder.HBLStatus = "P"; shipmentorder.HBLPayableId = 0; shipmentorder.HBLCollectId = 0;
            shipmentorder.JobStatus = "FCL"; 
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CONSIGNEE IMMEDIATELY UPON ARRIVAL";
            shipmentorder.GoodDescription = ""; 
            shipmentorder.OceanMSTBLNo = ""; 
            shipmentorder.VolumeBL = 0; 
            shipmentorder.VolumeInvoice = 0;
            shipmentorder.SSLineId = 0;
            shipmentorder.BrokerId = 0;
            shipmentorder.DepoId = 0;
            shipmentorder.Truck = "";
            shipmentorder.CreatedAt  = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.EMKLDomestic) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.EMKLDomestic, shipmentorder.JobNumber, shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObjectEMKLAirImport(ShipmentOrder shipmentorder)//OfficeId && CreatedBy
        {
            shipmentorder.SubJobNumber = 0; 
            shipmentorder.JobStatus = "FCL";
            shipmentorder.GoodRecDate = DateTime.Today;
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.SIDate = DateTime.Today;
            shipmentorder.SIReference = "";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0; 
            shipmentorder.AgentName = ""; 
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0; 
            shipmentorder.TranshipmentName = "";
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = ""; 
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0;
            shipmentorder.ConsigneeName = ""; 
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0; 
            shipmentorder.NPartyName = "";
            shipmentorder.NPartyAddress = "";
            shipmentorder.DepartureAirPortId = 0;
            shipmentorder.DepartureAirPortName = "";
            shipmentorder.DestinationAirPortId = 0;
            shipmentorder.DestinationAirPortName = "";
            shipmentorder.ReceiptPlaceId = 0;
            shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DeliveryPlaceId = 0;
            shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.MAWBStatus = "P"; 
            shipmentorder.MAWBCollectId = 0;
            shipmentorder.MAWBPayableId = 0;
            shipmentorder.HAWBStatus = "P"; 
            shipmentorder.HAWBCollectId = 0; 
            shipmentorder.HAWBPayableId = 0;
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CNEE IMMEDIATELY UPON ARRIVAL DOCS ATTACHED TO MAWB";
            shipmentorder.BrokerId = 0;
            shipmentorder.CustomValue = "N C V";
            shipmentorder.CarriageValue = "AS PER INV";
            shipmentorder.IATAId = 0; 
            shipmentorder.ChargeableWeight = 0; 
            shipmentorder.WeightHAWB = 0;
            shipmentorder.MAWBNo = "000 0000 0000";
            shipmentorder.Shipmark = "";
            shipmentorder.GoodNatureQuantity = "";
            shipmentorder.Total = "AS ARRANGED";
            shipmentorder.ChargeRate = 0;
            shipmentorder.ChargeWeight = 0; 
            shipmentorder.KGLB = "KG"; 
            shipmentorder.GrossWeight = 0;
            shipmentorder.PiecesRCP = "";
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.EMKLAirImport) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.EMKLAirImport, shipmentorder.JobNumber, shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObjectEMKLAirExport(ShipmentOrder shipmentorder)//OfficeId && CreatedBy
        {
            shipmentorder.SubJobNumber = 0; 
            shipmentorder.SIReference = "";
            shipmentorder.JobStatus = "FCL"; 
            shipmentorder.GoodRecDate = DateTime.Today;
            shipmentorder.SIDate = DateTime.Today;
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0; 
            shipmentorder.AgentName = "";
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0;
            shipmentorder.TranshipmentName = "";
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = ""; 
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0; 
            shipmentorder.ConsigneeName = ""; 
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0; 
            shipmentorder.NPartyName = ""; 
            shipmentorder.NPartyAddress = "";
            shipmentorder.DepartureAirPortId = 0; 
            shipmentorder.DepartureAirPortName = "";
            shipmentorder.DestinationAirPortId = 0; 
            shipmentorder.DestinationAirPortName = "";
            shipmentorder.ReceiptPlaceId = 0;
            shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DeliveryPlaceId = 0; 
            shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.MAWBStatus = "P";
            shipmentorder.MAWBCollectId = 0; 
            shipmentorder.MAWBPayableId = 0;
            shipmentorder.HAWBStatus = "P"; 
            shipmentorder.HAWBCollectId = 0;
            shipmentorder.HAWBPayableId = 0;
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CNEE IMMEDIATELY UPON ARRIVAL DOCS ATTACHED TO MAWB";
            shipmentorder.BrokerId = 0; 
            shipmentorder.CustomValue = "N C V";
            shipmentorder.CarriageValue = "AS PER INV";
            shipmentorder.IATAId = 0;
            shipmentorder.ChargeableWeight = 0;
            shipmentorder.WeightHAWB = 0;
            shipmentorder.MAWBNo = "000 0000 0000";
            shipmentorder.Shipmark = "";
            shipmentorder.GoodNatureQuantity = ""; 
            shipmentorder.Total = "AS ARRANGED";
            shipmentorder.ChargeRate = 0;
            shipmentorder.ChargeWeight = 0;
            shipmentorder.KGLB = "KG";
            shipmentorder.GrossWeight = 0;
            shipmentorder.PiecesRCP = "";
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.EMKLAirExport) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.EMKLAirExport, shipmentorder.JobNumber, shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObjectEMKLSeaImport(ShipmentOrder shipmentorder)//OfficeId && CreatedBy
        {
            shipmentorder.SubJobNumber = 0;
            shipmentorder.JobStatus = "FCL"; 
            shipmentorder.SIDate = DateTime.Today;
            shipmentorder.LoadStatus = "FCL"; 
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0; 
            shipmentorder.AgentName = ""; 
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0;
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0;
            shipmentorder.TranshipmentName = ""; 
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = ""; 
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0; 
            shipmentorder.ConsigneeName = "";
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0; 
            shipmentorder.NPartyName = ""; 
            shipmentorder.NPartyAddress = "";
            shipmentorder.LoadingPortId = 0;
            shipmentorder.LoadingPortName = "";
            shipmentorder.DischargePortId = 0;
            shipmentorder.DischargePortName = "";
            shipmentorder.ReceiptPlaceId = 0; 
            shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DeliveryPlaceId = 0; 
            shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.OBLStatus = "P"; 
            shipmentorder.OBLCollectId = 0;
            shipmentorder.OBLPayableId = 0;
            shipmentorder.HBLStatus = "P"; 
            shipmentorder.HBLPayableId = 0;
            shipmentorder.HBLCollectId = 0;
            shipmentorder.GoodRecDate = DateTime.Today; 
            shipmentorder.JobStatus = "FCL"; 
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CONSIGNEE IMMEDIATELY UPON ARRIVAL";
            shipmentorder.GoodDescription = "";
            shipmentorder.OceanMSTBLNo = ""; 
            shipmentorder.VolumeBL = 0; 
            shipmentorder.VolumeInvoice = 0;
            shipmentorder.SSLineId = 0;
            shipmentorder.BrokerId = 0; 
            shipmentorder.DepoId = 0;
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.EMKLSeaImport) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.EMKLSeaImport, shipmentorder.JobNumber, shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObjectEMKLSeaExport(ShipmentOrder shipmentorder)//OfficeId && CreatedBy
        {
            shipmentorder.SubJobNumber = 0;
            shipmentorder.SIReference = ""; 
            shipmentorder.JobStatus = "FCL";
            shipmentorder.SIDate = DateTime.Today; 
            shipmentorder.LoadStatus = "FCL"; 
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0;
            shipmentorder.AgentName = ""; 
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0; 
            shipmentorder.TranshipmentName = ""; 
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = ""; 
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0;
            shipmentorder.ConsigneeName = ""; 
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0;
            shipmentorder.NPartyName = ""; 
            shipmentorder.NPartyAddress = "";
            shipmentorder.LoadingPortId = 0;
            shipmentorder.LoadingPortName = "";
            shipmentorder.DischargePortId = 0; 
            shipmentorder.DischargePortName = "";
            shipmentorder.ReceiptPlaceId = 0; 
            shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DeliveryPlaceId = 0; 
            shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.OBLStatus = "P"; 
            shipmentorder.OBLCollectId = 0; 
            shipmentorder.OBLPayableId = 0;
            shipmentorder.HBLStatus = "P"; 
            shipmentorder.HBLPayableId = 0;
            shipmentorder.HBLCollectId = 0;
            shipmentorder.GoodRecDate = DateTime.Today; 
            shipmentorder.JobStatus = "FCL"; 
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CONSIGNEE IMMEDIATELY UPON ARRIVAL";
            shipmentorder.GoodDescription = ""; 
            shipmentorder.OceanMSTBLNo = ""; 
            shipmentorder.VolumeBL = 0; 
            shipmentorder.VolumeInvoice = 0;
            shipmentorder.SSLineId = 0; 
            shipmentorder.BrokerId = 0; 
            shipmentorder.DepoId = 0;
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.EMKLSeaExport) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.EMKLSeaExport, shipmentorder.JobNumber, shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObjectSeaExport(ShipmentOrder shipmentorder)//OfficeId && CreatedBy
        {
            shipmentorder.SubJobNumber = 0;
            shipmentorder.TotalSub = 0;
            shipmentorder.SIReference = "";
            shipmentorder.SIDate = DateTime.Today;
            shipmentorder.LoadStatus = "FCL";
            shipmentorder.ServiceNoID = 1;
            shipmentorder.ContainerStatus = "N";
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0;
            shipmentorder.AgentName = ""; 
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0; 
            shipmentorder.TranshipmentName = ""; 
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = ""; 
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0; 
            shipmentorder.ConsigneeName = ""; 
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0; 
            shipmentorder.NPartyName = ""; 
            shipmentorder.NPartyAddress = "";
            shipmentorder.LoadingPortId = 0; 
            shipmentorder.LoadingPortName = "";
            shipmentorder.DischargePortId = 0;
            shipmentorder.DischargePortName = "";
            shipmentorder.ReceiptPlaceId = 0; 
            shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DeliveryPlaceId = 0; 
            shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.OBLStatus = "P";
            shipmentorder.OBLCollectId = 0; 
            shipmentorder.OBLPayableId = 0;
            shipmentorder.HBLStatus = "P"; 
            shipmentorder.HBLPayableId = 0; 
            shipmentorder.HBLCollectId = 0;
            shipmentorder.GoodRecDate = DateTime.Today;
            shipmentorder.JobStatus = "FCL";
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CONSIGNEE IMMEDIATELY UPON ARRIVAL";
            shipmentorder.GoodDescription = ""; 
            shipmentorder.OceanMSTBLNo = ""; 
            shipmentorder.VolumeBL = 0; 
            shipmentorder.VolumeInvoice = 0;
            shipmentorder.SSLineId = 0; 
            shipmentorder.BrokerId = 0; 
            shipmentorder.DepoId = 0;
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.SeaExport) + 1 ;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.SeaExport,shipmentorder.JobNumber,shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }
         
        public ShipmentOrder CreateObjectSeaImport(ShipmentOrder shipmentorder)//OfficeId && CreatedBy
        {
            shipmentorder.SubJobNumber = 0;
            shipmentorder.TotalSub = 0;
            shipmentorder.LoadStatus = "FCL";
            shipmentorder.ServiceNoID = 1;
            shipmentorder.ContainerStatus = "N";
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0; 
            shipmentorder.AgentName = "";
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0; 
            shipmentorder.TranshipmentName = ""; 
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = "";
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0; 
            shipmentorder.ConsigneeName = ""; 
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0; 
            shipmentorder.NPartyName = ""; 
            shipmentorder.NPartyAddress = "";
            shipmentorder.LoadingPortId = 0;
            shipmentorder.LoadingPortName = "";
            shipmentorder.DischargePortId = 0; 
            shipmentorder.DischargePortName = "";
            shipmentorder.ReceiptPlaceId = 0; 
            shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DeliveryPlaceId = 0; 
            shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.OBLStatus = "P"; 
            shipmentorder.OBLCollectId = 0; 
            shipmentorder.OBLPayableId = 0;
            shipmentorder.HBLStatus = "P"; 
            shipmentorder.HBLPayableId = 0; 
            shipmentorder.HBLCollectId = 0;
            shipmentorder.GoodRecDate = DateTime.Today;
            shipmentorder.JobStatus = "FCL";
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CONSIGNEE IMMEDIATELY UPON ARRIVAL";
            shipmentorder.GoodDescription = "";
            shipmentorder.OceanMSTBLNo = ""; 
            shipmentorder.VolumeBL = 0; 
            shipmentorder.VolumeInvoice = 0;
            shipmentorder.SSLineId = 0; 
            shipmentorder.BrokerId = 0; 
            shipmentorder.DepoId = 0;
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.SeaImport) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.SeaImport, shipmentorder.JobNumber, shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObjectAirImport(ShipmentOrder shipmentorder) //OfficeId && CreatedBy && JobID
        {
            shipmentorder.SubJobNumber = 0;
            shipmentorder.TotalSub = 0;
            shipmentorder.SIReference = "";
            shipmentorder.SIDate = DateTime.Today;
            shipmentorder.FreightStatus = "N"; 
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0; 
            shipmentorder.AgentName = ""; 
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0;
            shipmentorder.TranshipmentName = "";
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = ""; 
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0;
            shipmentorder.ConsigneeName = "";
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0; 
            shipmentorder.NPartyName = ""; 
            shipmentorder.NPartyAddress = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.MAWBStatus = "P"; shipmentorder.MAWBCollectId = 0; shipmentorder.MAWBPayableId = 0;
            shipmentorder.HAWBStatus = "P"; shipmentorder.HAWBCollectId = 0; shipmentorder.HAWBPayableId = 0; shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CNEE IMMEDIATELY UPON ARRIVAL DOCS ATTACHED TO MAWB";
            shipmentorder.BrokerId = 0; shipmentorder.CustomValue = "N C V"; shipmentorder.CarriageValue = "AS PER INV";
            shipmentorder.IATAId = 0; shipmentorder.ChargeableWeight = 0; shipmentorder.WeightHAWB = 0;
            shipmentorder.MAWBNo = "000 0000 0000"; shipmentorder.Shipmark = ""; shipmentorder.GoodNatureQuantity = ""; shipmentorder.Total = "AS ARRANGED";
            shipmentorder.ChargeRate = 0; shipmentorder.ChargeWeight = 0; shipmentorder.KGLB = "KG"; shipmentorder.GrossWeight = 0; shipmentorder.PiecesRCP = "";
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.AirImport) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.AirImport, shipmentorder.JobNumber, shipmentorder.SubJobNumber);
            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObjectAirExport(ShipmentOrder shipmentorder) //OfficeId && CreatedBy && JobID
        {
            shipmentorder.SubJobNumber = 0;
            shipmentorder.TotalSub = 0;
            shipmentorder.SIReference = "";
            shipmentorder.SIDate = DateTime.Today;
            shipmentorder.FreightStatus = "N"; 
            shipmentorder.ShipmentStatus = "N";
            shipmentorder.MarketId = 0;
            shipmentorder.MarketCompanyId = 0;
            shipmentorder.AgentId = 0; 
            shipmentorder.AgentName = ""; 
            shipmentorder.AgentAddress = "";
            shipmentorder.DeliveryId = 0; 
            shipmentorder.DeliveryName = ""; 
            shipmentorder.DeliveryAddress = "";
            shipmentorder.TranshipmentId = 0;
            shipmentorder.TranshipmentName = ""; 
            shipmentorder.TranshipmentAddress = "";
            shipmentorder.ShipperId = 0; 
            shipmentorder.ShipperName = "";
            shipmentorder.ShipperAddress = "";
            shipmentorder.ConsigneeId = 0; 
            shipmentorder.ConsigneeName = ""; 
            shipmentorder.ConsigneeAddress = "";
            shipmentorder.NPartyId = 0; 
            shipmentorder.NPartyName = ""; 
            shipmentorder.NPartyAddress = "";
            shipmentorder.DepartureAirPortId = 0; 
            shipmentorder.DepartureAirPortName = "";
            shipmentorder.DestinationAirPortId = 0; 
            shipmentorder.DestinationAirPortName = "";
            shipmentorder.ReceiptPlaceId = 0; 
            shipmentorder.ReceiptPlaceName = "";
            shipmentorder.DestinationAirPortId = 0; 
            shipmentorder.DeliveryPlaceName = "";
            shipmentorder.ETD = DateTime.Today;
            shipmentorder.ETA = DateTime.Today;
            shipmentorder.MAWBStatus = "P"; 
            shipmentorder.MAWBCollectId = 0;
            shipmentorder.MAWBPayableId = 0;
            shipmentorder.HAWBStatus = "P"; 
            shipmentorder.HAWBCollectId = 0; 
            shipmentorder.HAWBPayableId = 0; 
            shipmentorder.Currency = "USD";
            shipmentorder.HandlingInfo = "PLEASE CONTACT CNEE IMMEDIATELY UPON ARRIVAL DOCS ATTACHED TO MAWB";
            shipmentorder.BrokerId = 0; 
            shipmentorder.CustomValue = "N C V"; 
            shipmentorder.CarriageValue = "AS PER INV";
            shipmentorder.IATAId = 0; 
            shipmentorder.ChargeableWeight = 0; 
            shipmentorder.WeightHAWB = 0;
            shipmentorder.MAWBNo = "000 0000 0000"; 
            shipmentorder.Shipmark = "";
            shipmentorder.GoodNatureQuantity = ""; 
            shipmentorder.Total = "AS ARRANGED";
            shipmentorder.ChargeRate = 0; 
            shipmentorder.ChargeWeight = 0; 
            shipmentorder.KGLB = "KG"; 
            shipmentorder.GrossWeight = 0; 
            shipmentorder.PiecesRCP = "";
            shipmentorder.CreatedAt = DateTime.Now;
            shipmentorder.MasterCode = _repository.GetLastMasterCode(shipmentorder.OfficeId) + 1;
            shipmentorder.JobNumber = _repository.GetLastJobNumber(shipmentorder.OfficeId, MasterConstant.JobType.AirExport) + 1;
            shipmentorder.ShipmentOrderId = GenerateShipmentNo(shipmentorder.Office.InitialCompany, MasterConstant.JobType.AirExport, shipmentorder.JobNumber, shipmentorder.SubJobNumber);

            shipmentorder = _repository.CreateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder CreateObject(ShipmentOrder shipmentorder,IOfficeService _officeService)
        {
            shipmentorder.Errors = new Dictionary<String, String>();
            shipmentorder.Office = _officeService.GetObjectById(shipmentorder.OfficeId);
            if (!isValid(_validator.VCreateObject(shipmentorder,this)))
            {
                switch (shipmentorder.JobId)
                {
                    case MasterConstant.JobType.SeaExport:
                        shipmentorder = this.CreateObjectSeaExport(shipmentorder);
                        break;
                    case MasterConstant.JobType.SeaImport:
                        shipmentorder = this.CreateObjectSeaImport(shipmentorder);
                        break;
                    case MasterConstant.JobType.AirExport:
                        shipmentorder = this.CreateObjectAirExport(shipmentorder);
                        break;
                    case MasterConstant.JobType.AirImport:
                        shipmentorder = this.CreateObjectAirImport(shipmentorder);
                        break;
                    case MasterConstant.JobType.EMKLSeaExport:
                        shipmentorder = this.CreateObjectEMKLSeaExport(shipmentorder);
                        break;
                    case MasterConstant.JobType.EMKLSeaImport:
                        shipmentorder = this.CreateObjectEMKLSeaImport(shipmentorder);
                        break;
                    case MasterConstant.JobType.EMKLAirExport:
                        shipmentorder = this.CreateObjectEMKLAirExport(shipmentorder);
                        break;
                    case MasterConstant.JobType.EMKLAirImport:
                        shipmentorder = this.CreateObjectEMKLAirImport(shipmentorder);
                        break;
                    case MasterConstant.JobType.EMKLDomestic:
                        shipmentorder = this.CreateObjectEMKLDomestic(shipmentorder);
                        break;
                }
              
            }
            return shipmentorder;
        }

        public ShipmentOrder UpdateObject(ShipmentOrder shipmentorder)
        {

            shipmentorder = _repository.UpdateObject(shipmentorder);
            return shipmentorder;
        }

        public ShipmentOrder UpdateAsSubOrderAirImport(ShipmentOrder model, IContactService _contactService,
        ShipmentOrderRouting shipmentorderrouting, IShipmentOrderRoutingService _shipmentorderroutingService)
        {

            var subJobNo = _repository.GetLastSubJobNumber(model.OfficeId, model.JobId, model.JobNumber) + 1;

            model.TotalSub = model.TotalSub + 1;
            if (subJobNo == 1)
            {


                ShipmentOrder newShipmentOrder = new ShipmentOrder(); ;
                newShipmentOrder.FreightStatus = "G";

                newShipmentOrder.JobId = model.JobId;
                newShipmentOrder.JobNumber = model.JobNumber;
                newShipmentOrder.SubJobNumber = subJobNo;
                newShipmentOrder.OfficeId = model.OfficeId;
                newShipmentOrder.TotalSub = model.TotalSub;
                newShipmentOrder.JobStatus = model.JobStatus;
                newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                newShipmentOrder.MarketId = model.MarketId;
                newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                newShipmentOrder.QuotationNo = model.QuotationNo;
                newShipmentOrder.AgentId = model.AgentId;
                newShipmentOrder.AgentName = model.AgentName;
                newShipmentOrder.AgentAddress = model.AgentAddress;
                newShipmentOrder.ShipperId = model.ShipperId;
                newShipmentOrder.ShipperName = model.ShipperName;
                newShipmentOrder.ShipperAddress = model.ShipperAddress;
                newShipmentOrder.ConsigneeId = model.ConsigneeId;
                newShipmentOrder.ConsigneeName = model.ConsigneeName;
                newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                newShipmentOrder.NPartyId = model.NPartyId;
                newShipmentOrder.NPartyName = model.NPartyName;
                newShipmentOrder.NPartyAddress = model.NPartyAddress;
                newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId;
                newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                newShipmentOrder.DepartureAirPortId = model.DepartureAirPortId;
                newShipmentOrder.DepartureAirPortName = model.DepartureAirPortName;
                newShipmentOrder.ETD = model.ETD;
                newShipmentOrder.ETA = model.ETA;
                newShipmentOrder.DestinationAirPortId = model.DestinationAirPortId;
                newShipmentOrder.DestinationAirPortName = model.DestinationAirPortName;
                newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId;
                newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                newShipmentOrder.MAWBStatus = model.MAWBStatus;
                newShipmentOrder.MAWBCollectId = model.MAWBCollectId;
                newShipmentOrder.MAWBPayableId = model.MAWBPayableId;
                newShipmentOrder.MAWBCurrency = model.MAWBCurrency;
                newShipmentOrder.MAWBRate = model.MAWBRate;
                newShipmentOrder.HAWBStatus = model.HAWBStatus;
                newShipmentOrder.HAWBCollectId = model.HAWBCollectId;
                newShipmentOrder.HAWBPayableId = model.HAWBPayableId;
                newShipmentOrder.HAWBCurrency = model.HAWBCurrency;
                newShipmentOrder.HAWBRate = model.HAWBRate;
                newShipmentOrder.HandlingInfo = model.HandlingInfo;
                newShipmentOrder.PiecesRCP = model.PiecesRCP;
                newShipmentOrder.GrossWeight = model.GrossWeight;
                newShipmentOrder.KGLB = model.KGLB;
                newShipmentOrder.ChargeWeight = model.ChargeWeight;
                newShipmentOrder.ChargeRate = model.ChargeRate;
                newShipmentOrder.Total = model.Total;
                newShipmentOrder.GoodNatureQuantity = model.GoodNatureQuantity;
                newShipmentOrder.Shipmark = model.Shipmark;
                newShipmentOrder.MAWBNo = model.MAWBNo;
                newShipmentOrder.WeightHAWB = model.WeightHAWB;
                newShipmentOrder.ChargeableWeight = model.ChargeableWeight;
                newShipmentOrder.IATAId = model.IATAId;
                newShipmentOrder.CarriageValue = model.CarriageValue;
                newShipmentOrder.CustomValue = model.CustomValue;
                newShipmentOrder.BrokerId = model.BrokerId;
                newShipmentOrder.DeletedAt = model.DeletedAt;
                newShipmentOrder.IsDeleted = model.IsDeleted;
                newShipmentOrder.JobClosed = model.JobClosed;
                newShipmentOrder.JobClosedOn = model.JobClosedOn;
                newShipmentOrder.PackagingCode = model.PackagingCode;
                newShipmentOrder.Commodity = model.Commodity;
                newShipmentOrder.GoodRecDate = model.GoodRecDate;
                newShipmentOrder.CreatedById = model.CreatedById;
                newShipmentOrder.CreatedAt = DateTime.Now;
                newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.AirImport, model.JobNumber, model.SubJobNumber);

                ShipmentOrder shipmentorder = GetObjectById(model.Id);
                shipmentorder.TotalSub = model.TotalSub;
                UpdateObject(shipmentorder);

                model = _repository.CreateObject(newShipmentOrder);
                if (model != null)
                {
                    IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                    if (shipmentRoutingList != null)
                    {
                        foreach (var item in shipmentRoutingList)
                        {
                            ShipmentOrderRouting route = new ShipmentOrderRouting();
                            route.AirportFrom = item.AirportFrom;
                            route.AirportTo = item.AirportTo;
                            route.CityId = item.CityId;
                            route.OfficeId = item.OfficeId;
                            route.CreatedById = model.CreatedById;
                            route.CreatedAt = DateTime.Now;
                            route.ETD = item.ETD;
                            route.FlightId = item.FlightId;
                            route.FlightNo = item.FlightNo;
                            route.PortId = item.PortId;
                            route.ShipmentOrderId = model.Id;
                            route.VesselId = item.VesselId;
                            route.VesselName = item.VesselName;
                            route.VesselType = item.VesselType;
                            route.Voyage = item.Voyage;
                            _shipmentorderroutingService.CreateObject(route);
                        }
                    }
                }
                return model;
            }
            else
            {
                var tempShipmentOrderId = GetQueryable().Where(x => x.JobId == model.JobId && x.OfficeId == model.OfficeId
                                                               && x.JobNumber == model.JobNumber && x.SubJobNumber == 1).Select(x => x.Id).FirstOrDefault();
                ShipmentOrder subJobSE = GetObjectById(tempShipmentOrderId);
                ShipmentOrder firstJobShipmentOrder = GetObjectById(model.Id);
                ShipmentOrder newShipmentOrder = new ShipmentOrder();
                newShipmentOrder.JobId = model.JobId;
                newShipmentOrder.JobNumber = model.JobNumber;
                newShipmentOrder.SubJobNumber = subJobNo;
                newShipmentOrder.OfficeId = model.OfficeId;
                newShipmentOrder.TotalSub = model.TotalSub;
                newShipmentOrder.SIReference = model.SIReference;
                newShipmentOrder.SIDate = model.SIDate;
                newShipmentOrder.JobStatus = model.JobStatus;
                // From Sub Job
                // End From Sub Job

                newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                newShipmentOrder.MarketId = model.MarketId;
                newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                newShipmentOrder.QuotationNo = model.QuotationNo;
                newShipmentOrder.AgentId = model.AgentId;
                newShipmentOrder.AgentName = model.AgentName;
                newShipmentOrder.AgentAddress = model.AgentAddress;
                newShipmentOrder.ShipperId = model.ShipperId;
                newShipmentOrder.ShipperName = model.ShipperName;
                newShipmentOrder.ShipperAddress = model.ShipperAddress;
                newShipmentOrder.ConsigneeId = model.ConsigneeId;
                newShipmentOrder.ConsigneeName = model.ConsigneeName;
                newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                newShipmentOrder.NPartyId = model.NPartyId;
                newShipmentOrder.NPartyName = model.NPartyName;
                newShipmentOrder.NPartyAddress = model.NPartyAddress;
                newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId;
                newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                newShipmentOrder.DepartureAirPortId = model.DepartureAirPortId;
                newShipmentOrder.DepartureAirPortName = model.DepartureAirPortName;
                newShipmentOrder.ETD = model.ETD;
                newShipmentOrder.ETA = model.ETA;
                newShipmentOrder.DestinationAirPortId = model.DestinationAirPortId;
                newShipmentOrder.DestinationAirPortName = model.DestinationAirPortName;
                newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId;
                newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                newShipmentOrder.MAWBStatus = model.MAWBStatus;
                newShipmentOrder.MAWBCollectId = model.MAWBCollectId;
                newShipmentOrder.MAWBPayableId = model.MAWBPayableId;
                newShipmentOrder.MAWBCurrency = model.MAWBCurrency;
                newShipmentOrder.MAWBRate = model.MAWBRate;
                newShipmentOrder.HAWBStatus = model.HAWBStatus;
                newShipmentOrder.HAWBCollectId = model.HAWBCollectId;
                newShipmentOrder.HAWBPayableId = model.HAWBPayableId;
                newShipmentOrder.HAWBCurrency = model.HAWBCurrency;
                newShipmentOrder.HAWBRate = model.HAWBRate;
                newShipmentOrder.HandlingInfo = model.HandlingInfo;
                newShipmentOrder.PiecesRCP = model.PiecesRCP;
                newShipmentOrder.GrossWeight = model.GrossWeight;
                newShipmentOrder.KGLB = model.KGLB;
                newShipmentOrder.ChargeWeight = model.ChargeWeight;
                newShipmentOrder.ChargeRate = model.ChargeRate;
                newShipmentOrder.Total = model.Total;
                newShipmentOrder.GoodNatureQuantity = model.GoodNatureQuantity;
                newShipmentOrder.Shipmark = model.Shipmark;
                newShipmentOrder.MAWBNo = model.MAWBNo;
                newShipmentOrder.WeightHAWB = model.WeightHAWB;
                newShipmentOrder.ChargeableWeight = model.ChargeableWeight;
                newShipmentOrder.IATAId = model.IATAId;
                newShipmentOrder.CarriageValue = model.CarriageValue;
                newShipmentOrder.CustomValue = model.CustomValue;
                newShipmentOrder.BrokerId = model.BrokerId;
                newShipmentOrder.DeletedAt = model.DeletedAt;
                newShipmentOrder.IsDeleted = model.IsDeleted;
                newShipmentOrder.JobClosed = model.JobClosed;
                newShipmentOrder.JobClosedOn = model.JobClosedOn;
                newShipmentOrder.PackagingCode = model.PackagingCode;
                newShipmentOrder.Commodity = model.Commodity;
                newShipmentOrder.GoodRecDate = model.GoodRecDate;
                newShipmentOrder.CreatedById = model.CreatedById;
                newShipmentOrder.CreatedAt = DateTime.Now;
                newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.AirImport, model.JobNumber, model.SubJobNumber);

                UpdateObject(firstJobShipmentOrder);
                model = _repository.CreateObject(newShipmentOrder);
                if (model != null)
                {
                    IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                    if (shipmentRoutingList != null)
                    {
                        foreach (var item in shipmentRoutingList)
                        {
                            ShipmentOrderRouting route = new ShipmentOrderRouting();
                            route.AirportFrom = item.AirportFrom;
                            route.AirportTo = item.AirportTo;
                            route.CityId = item.CityId;
                            route.OfficeId = item.OfficeId;
                            route.CreatedById = model.CreatedById;
                            route.CreatedAt = DateTime.Now;
                            route.ETD = item.ETD;
                            route.FlightId = item.FlightId;
                            route.FlightNo = item.FlightNo;
                            route.PortId = item.PortId;
                            route.ShipmentOrderId = model.Id;
                            route.VesselId = item.VesselId;
                            route.VesselName = item.VesselName;
                            route.VesselType = item.VesselType;
                            route.Voyage = item.Voyage;
                            _shipmentorderroutingService.CreateObject(route);
                        }
                    }
                }
                return model;
            }
        } 

        public ShipmentOrder UpdateAsSubOrderAirExport(ShipmentOrder model, IContactService _contactService,
         ShipmentOrderRouting shipmentorderrouting, IShipmentOrderRoutingService _shipmentorderroutingService)
        { 

            var subJobNo = _repository.GetLastSubJobNumber(model.OfficeId, model.JobId, model.JobNumber) + 1;

            model.TotalSub = model.TotalSub + 1;
            if (subJobNo == 1)
            {

                int serviceNo = model.LoadStatus == "FCL" ? 5 : 4;
                string containerStatus = model.LoadStatus == "FCL" ? "G" : "N";

                ShipmentOrder newShipmentOrder = new ShipmentOrder(); ;
                newShipmentOrder.FreightStatus = "G";

                newShipmentOrder.JobId = model.JobId;
                newShipmentOrder.JobNumber = model.JobNumber;
                newShipmentOrder.SubJobNumber = subJobNo;
                newShipmentOrder.OfficeId = model.OfficeId;
                newShipmentOrder.TotalSub = model.TotalSub;
                newShipmentOrder.SIReference = model.SIReference;
                newShipmentOrder.SIDate = model.SIDate;
                newShipmentOrder.JobStatus = model.JobStatus;
                newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                newShipmentOrder.MarketId = model.MarketId;
                newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                newShipmentOrder.QuotationNo = model.QuotationNo;
                newShipmentOrder.AgentId = model.AgentId;
                newShipmentOrder.AgentName = model.AgentName;
                newShipmentOrder.AgentAddress = model.AgentAddress;
                newShipmentOrder.DeliveryId = model.DeliveryId;
                newShipmentOrder.DeliveryName = model.DeliveryName;
                newShipmentOrder.DeliveryAddress = model.DeliveryAddress;
                newShipmentOrder.ShipperId = model.ShipperId;
                newShipmentOrder.ShipperName = model.ShipperName;
                newShipmentOrder.ShipperAddress = model.ShipperAddress;
                newShipmentOrder.ConsigneeId = model.ConsigneeId;
                newShipmentOrder.ConsigneeName = model.ConsigneeName;
                newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                newShipmentOrder.NPartyId = model.NPartyId;
                newShipmentOrder.NPartyName = model.NPartyName;
                newShipmentOrder.NPartyAddress = model.NPartyAddress;
                newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId;
                newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                newShipmentOrder.DepartureAirPortId = model.DepartureAirPortId;
                newShipmentOrder.DepartureAirPortName = model.DepartureAirPortName;
                newShipmentOrder.ETD = model.ETD;
                newShipmentOrder.ETA = model.ETA;
                newShipmentOrder.DestinationAirPortId = model.DestinationAirPortId;
                newShipmentOrder.DestinationAirPortName = model.DestinationAirPortName;
                newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId;
                newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                newShipmentOrder.MAWBStatus = model.MAWBStatus;
                newShipmentOrder.MAWBCollectId = model.MAWBCollectId;
                newShipmentOrder.MAWBPayableId = model.MAWBPayableId;
                newShipmentOrder.HAWBStatus = model.HAWBStatus;
                newShipmentOrder.HAWBCollectId = model.HAWBCollectId;
                newShipmentOrder.HAWBPayableId = model.HAWBPayableId;
                newShipmentOrder.Currency = model.Currency;
                newShipmentOrder.HandlingInfo = model.HandlingInfo;
                newShipmentOrder.PiecesRCP = model.PiecesRCP;
                newShipmentOrder.GrossWeight = model.GrossWeight;
                newShipmentOrder.KGLB = model.KGLB;
                newShipmentOrder.ChargeWeight = model.ChargeWeight;
                newShipmentOrder.ChargeRate = model.ChargeRate;
                newShipmentOrder.Total = model.Total;
                newShipmentOrder.GoodNatureQuantity = model.GoodNatureQuantity;
                newShipmentOrder.Shipmark = model.Shipmark;
                newShipmentOrder.MAWBNo = model.MAWBNo;
                newShipmentOrder.WeightHAWB = model.WeightHAWB;
                newShipmentOrder.ChargeableWeight = model.ChargeableWeight; 
                newShipmentOrder.IATAId = model.IATAId; newShipmentOrder.CarriageValue = model.CarriageValue;
                newShipmentOrder.CustomValue = model.CustomValue;
                newShipmentOrder.BrokerId = model.BrokerId;
                newShipmentOrder.DeletedAt = model.DeletedAt; 
                newShipmentOrder.IsDeleted = model.IsDeleted;
                newShipmentOrder.JobClosed = model.JobClosed;
                newShipmentOrder.JobClosedOn = model.JobClosedOn;
                newShipmentOrder.PackagingCode = model.PackagingCode;
                newShipmentOrder.Commodity = model.Commodity;
                newShipmentOrder.GoodRecDate = model.GoodRecDate;
                newShipmentOrder.CreatedById = model.CreatedById;
                newShipmentOrder.CreatedAt = DateTime.Now;
                newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.AirExport, model.JobNumber, model.SubJobNumber);

                ShipmentOrder shipmentorder = GetObjectById(model.Id);
                shipmentorder.TotalSub = model.TotalSub;
                UpdateObject(shipmentorder);

                model = _repository.CreateObject(newShipmentOrder);
                if (model != null)
                {
                    IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                    if (shipmentRoutingList != null)
                    {
                        foreach (var item in shipmentRoutingList)
                        {
                            ShipmentOrderRouting route = new ShipmentOrderRouting();
                            route.AirportFrom = item.AirportFrom;
                            route.AirportTo = item.AirportTo;
                            route.CityId = item.CityId;
                            route.OfficeId = item.OfficeId;
                            route.CreatedById = model.CreatedById;
                            route.CreatedAt = DateTime.Now;
                            route.ETD = item.ETD;
                            route.FlightId = item.FlightId;
                            route.FlightNo = item.FlightNo;
                            route.PortId = item.PortId;
                            route.ShipmentOrderId = model.Id;
                            route.VesselId = item.VesselId;
                            route.VesselName = item.VesselName;
                            route.VesselType = item.VesselType;
                            route.Voyage = item.Voyage;
                            _shipmentorderroutingService.CreateObject(route);
                        }
                    }
                }
                return model;
            }
            else
            {
                var tempShipmentOrderId = GetQueryable().Where(x => x.JobId == model.JobId && x.OfficeId == model.OfficeId
                                                               && x.JobNumber == model.JobNumber && x.SubJobNumber == 1).Select(x => x.Id).FirstOrDefault();
                ShipmentOrder subJobSE = GetObjectById(tempShipmentOrderId);
                ShipmentOrder firstJobShipmentOrder = GetObjectById(model.Id);
                ShipmentOrder newShipmentOrder = new ShipmentOrder();
                newShipmentOrder.JobId = model.JobId;
                newShipmentOrder.JobNumber = model.JobNumber;
                newShipmentOrder.SubJobNumber = subJobNo;
                newShipmentOrder.OfficeId = model.OfficeId;
                newShipmentOrder.TotalSub = model.TotalSub;
                newShipmentOrder.SIReference = model.SIReference;
                newShipmentOrder.SIDate = model.SIDate;
                newShipmentOrder.JobStatus = model.JobStatus;
                // From Sub Job
                // End From Sub Job

                newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                newShipmentOrder.MarketId = model.MarketId;
                newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                newShipmentOrder.QuotationNo = model.QuotationNo;
                newShipmentOrder.AgentId = model.AgentId;
                newShipmentOrder.AgentName = model.AgentName;
                newShipmentOrder.AgentAddress = model.AgentAddress;
                newShipmentOrder.DeliveryId = model.DeliveryId;
                newShipmentOrder.DeliveryName = model.DeliveryName;
                newShipmentOrder.DeliveryAddress = model.DeliveryAddress;
                newShipmentOrder.ShipperId = model.ShipperId;
                newShipmentOrder.ShipperName = model.ShipperName;
                newShipmentOrder.ShipperAddress = model.ShipperAddress;
                newShipmentOrder.ConsigneeId = model.ConsigneeId;
                newShipmentOrder.ConsigneeName = model.ConsigneeName;
                newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                newShipmentOrder.NPartyId = model.NPartyId;
                newShipmentOrder.NPartyName = model.NPartyName;
                newShipmentOrder.NPartyAddress = model.NPartyAddress;
                newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId;
                newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                newShipmentOrder.DepartureAirPortId = model.DepartureAirPortId;
                newShipmentOrder.DepartureAirPortName = model.DepartureAirPortName;
                newShipmentOrder.ETD = model.ETD;
                newShipmentOrder.ETA = model.ETA;
                newShipmentOrder.DestinationAirPortId = model.DestinationAirPortId;
                newShipmentOrder.DestinationAirPortName = model.DestinationAirPortName;
                newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId;
                newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                newShipmentOrder.MAWBStatus = model.MAWBStatus;
                newShipmentOrder.MAWBCollectId = model.MAWBCollectId;
                newShipmentOrder.MAWBPayableId = model.MAWBPayableId;
                newShipmentOrder.HAWBStatus = model.HAWBStatus;
                newShipmentOrder.HAWBCollectId = model.HAWBCollectId;
                newShipmentOrder.HAWBPayableId = model.HAWBPayableId;
                newShipmentOrder.Currency = model.Currency;
                newShipmentOrder.HandlingInfo = model.HandlingInfo;
                newShipmentOrder.PiecesRCP = model.PiecesRCP;
                newShipmentOrder.GrossWeight = model.GrossWeight;
                newShipmentOrder.KGLB = model.KGLB;
                newShipmentOrder.ChargeWeight = model.ChargeWeight;
                newShipmentOrder.ChargeRate = model.ChargeRate;
                newShipmentOrder.Total = model.Total;
                newShipmentOrder.GoodNatureQuantity = model.GoodNatureQuantity;
                newShipmentOrder.Shipmark = model.Shipmark;
                newShipmentOrder.MAWBNo = model.MAWBNo;
                newShipmentOrder.WeightHAWB = model.WeightHAWB;
                newShipmentOrder.ChargeableWeight = model.ChargeableWeight;
                newShipmentOrder.IATAId = model.IATAId; newShipmentOrder.CarriageValue = model.CarriageValue;
                newShipmentOrder.CustomValue = model.CustomValue;
                newShipmentOrder.BrokerId = model.BrokerId;
                newShipmentOrder.DeletedAt = model.DeletedAt;
                newShipmentOrder.IsDeleted = model.IsDeleted;
                newShipmentOrder.JobClosed = model.JobClosed;
                newShipmentOrder.JobClosedOn = model.JobClosedOn;
                newShipmentOrder.PackagingCode = model.PackagingCode;
                newShipmentOrder.Commodity = model.Commodity;
                newShipmentOrder.GoodRecDate = model.GoodRecDate;
                newShipmentOrder.CreatedById = model.CreatedById;
                newShipmentOrder.CreatedAt = DateTime.Now;
                newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.AirExport, model.JobNumber, model.SubJobNumber);

                UpdateObject(firstJobShipmentOrder);
                model = _repository.CreateObject(newShipmentOrder);
                if (model != null)
                {
                    IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                    if (shipmentRoutingList != null)
                    {
                        foreach (var item in shipmentRoutingList)
                        {
                            ShipmentOrderRouting route = new ShipmentOrderRouting();
                            route.AirportFrom = item.AirportFrom;
                            route.AirportTo = item.AirportTo;
                            route.CityId = item.CityId;
                            route.OfficeId = item.OfficeId;
                            route.CreatedById = model.CreatedById;
                            route.CreatedAt = DateTime.Now;
                            route.ETD = item.ETD;
                            route.FlightId = item.FlightId;
                            route.FlightNo = item.FlightNo;
                            route.PortId = item.PortId;
                            route.ShipmentOrderId = model.Id;
                            route.VesselId = item.VesselId;
                            route.VesselName = item.VesselName;
                            route.VesselType = item.VesselType;
                            route.Voyage = item.Voyage;
                            _shipmentorderroutingService.CreateObject(route);
                        }
                    }
                }
                return model;
            }
        } 

        public ShipmentOrder UpdateAsSubOrderSeaImport(ShipmentOrder model, IContactService _contactService,
            ShipmentOrderRouting shipmentorderrouting,IShipmentOrderRoutingService _shipmentorderroutingService)
        { 
               
                var subJobNo = _repository.GetLastSubJobNumber(model.OfficeId, model.JobId, model.JobNumber) + 1;
               
                model.TotalSub = model.TotalSub +1;
                if (subJobNo == 1)
                {

                    int serviceNo = model.LoadStatus == "FCL" ? 5 : 4;
                    string containerStatus = model.LoadStatus == "FCL" ? "G" : "N";

                    ShipmentOrder newShipmentOrder = new ShipmentOrder(); ;
                    newShipmentOrder.JobId = model.JobId;
                    newShipmentOrder.JobNumber = model.JobNumber;
                    newShipmentOrder.SubJobNumber = subJobNo;
                    newShipmentOrder.OfficeId = model.OfficeId;
                    newShipmentOrder.TotalSub = model.TotalSub;
                    newShipmentOrder.Conversion = model.Conversion;
                    newShipmentOrder.LoadStatus = "LCL";
                    newShipmentOrder.ServiceNoID = serviceNo;
                    newShipmentOrder.ContainerStatus = containerStatus; 
                    newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                    newShipmentOrder.MarketId = model.MarketId; 
                    newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                    newShipmentOrder.QuotationNo = model.QuotationNo;
                    newShipmentOrder.AgentId = model.AgentId; 
                    newShipmentOrder.AgentName = model.AgentName; 
                    newShipmentOrder.AgentAddress = model.AgentAddress;
                    newShipmentOrder.TranshipmentId = model.TranshipmentId; 
                    newShipmentOrder.TranshipmentName = model.TranshipmentName; 
                    newShipmentOrder.TranshipmentAddress = model.TranshipmentAddress;
                    newShipmentOrder.ShipperId = model.ShipperId; 
                    newShipmentOrder.ShipperName = model.ShipperName; 
                    newShipmentOrder.ShipperAddress = model.ShipperAddress;
                    newShipmentOrder.ConsigneeId = model.ConsigneeId; 
                    newShipmentOrder.ConsigneeName = model.ConsigneeName; 
                    newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                    newShipmentOrder.NPartyId = model.NPartyId; 
                    newShipmentOrder.NPartyName = model.NPartyName; 
                    newShipmentOrder.NPartyAddress = model.NPartyAddress;
                    newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId; 
                    newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                    newShipmentOrder.LoadingPortId = model.LoadingPortId; 
                    newShipmentOrder.LoadingPortName = model.LoadingPortName;
                    newShipmentOrder.ETD = model.ETD;
                    newShipmentOrder.ETA = model.ETA;
                    newShipmentOrder.DischargePortId = model.DischargePortId; 
                    newShipmentOrder.DischargePortName = model.DischargePortName;
                    newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId; 
                    newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                    newShipmentOrder.OBLStatus = model.OBLStatus; 
                    newShipmentOrder.OBLCollectId = model.OBLCollectId;
                    newShipmentOrder.OBLPayableId = model.OBLPayableId;
                    newShipmentOrder.HBLStatus = model.HBLStatus; 
                    newShipmentOrder.HBLCollectId = model.HBLCollectId;
                    newShipmentOrder.HBLPayableId = model.HBLPayableId;
                    newShipmentOrder.OBLCurrency = model.OBLCurrency;
                    newShipmentOrder.HBLCurrency = model.HBLCurrency;
                    newShipmentOrder.OBLAmount = model.OBLAmount; 
                    newShipmentOrder.HBLAmount = model.HBLAmount;
                    newShipmentOrder.GoodDescription = model.GoodDescription; 
                    newShipmentOrder.OceanMSTBLNo = model.OceanMSTBLNo;
                    newShipmentOrder.VolumeBL = model.VolumeBL; 
                    newShipmentOrder.VolumeInvoice = model.VolumeInvoice;
                    newShipmentOrder.SSLineId = model.SSLineId; 
                    newShipmentOrder.BrokerId = model.BrokerId; 
                    newShipmentOrder.DepoId = model.DepoId;
                    newShipmentOrder.WareHouseName = model.WareHouseName; 
                    newShipmentOrder.KINS = model.KINS; newShipmentOrder.CFName = model.CFName;
                    newShipmentOrder.DeletedAt = model.DeletedAt;
                    newShipmentOrder.IsDeleted = model.IsDeleted;
                    newShipmentOrder.JobClosed = model.JobClosed; 
                    newShipmentOrder.JobClosedOn = model.JobClosedOn;
                    newShipmentOrder.GoodRecDate = model.GoodRecDate;
                    newShipmentOrder.CreatedById = model.CreatedById;
                    newShipmentOrder.CreatedAt = DateTime.Now;
                    newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.SeaImport, model.JobNumber, model.SubJobNumber);
                    
                    ShipmentOrder shipmentorder = GetObjectById(model.Id);
                    shipmentorder.TotalSub = model.TotalSub;
                    if (model.LoadStatus == "FCL")
                    {
                        model.ContainerStatus = "G";
                    }
                    UpdateObject(shipmentorder);

                    model = _repository.CreateObject(newShipmentOrder);
                    if (model != null)
                    {
                        IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                        if (shipmentRoutingList != null)
                        {
                            foreach (var item in shipmentRoutingList)
                            {
                                ShipmentOrderRouting route = new ShipmentOrderRouting();
                                route.AirportFrom = item.AirportFrom;
                                route.AirportTo = item.AirportTo;
                                route.CityId = item.CityId;
                                route.OfficeId = item.OfficeId;
                                route.CreatedById = model.CreatedById;
                                route.CreatedAt = DateTime.Now;
                                route.ETD = item.ETD;
                                route.FlightId = item.FlightId;
                                route.FlightNo = item.FlightNo;
                                route.PortId = item.PortId;
                                route.ShipmentOrderId = model.Id;
                                route.VesselId = item.VesselId;
                                route.VesselName = item.VesselName;
                                route.VesselType = item.VesselType;
                                route.Voyage = item.Voyage;
                                _shipmentorderroutingService.CreateObject(route);
                            }
                        }
                    }
                return model;
                }
                else
                {
                    var tempShipmentOrderId = GetQueryable().Where(x => x.JobId == model.JobId && x.OfficeId == model.OfficeId
                                                                   && x.JobNumber == model.JobNumber && x.SubJobNumber == 1).Select(x => x.Id).FirstOrDefault();
                    ShipmentOrder subJobSE = GetObjectById(tempShipmentOrderId);
                    ShipmentOrder firstJobShipmentOrder = GetObjectById(model.Id);
                    ShipmentOrder newShipmentOrder = new ShipmentOrder();
                    newShipmentOrder.JobId = model.JobId;
                    newShipmentOrder.JobNumber = model.JobNumber;
                    newShipmentOrder.SubJobNumber = subJobNo;
                    newShipmentOrder.OfficeId = model.OfficeId;
                    newShipmentOrder.TotalSub = model.TotalSub;
                    newShipmentOrder.Conversion = model.Conversion;
                    // From Sub Job
                    newShipmentOrder.LoadStatus = subJobSE.LoadStatus;
                    newShipmentOrder.ServiceNoID = subJobSE.ServiceNoID;
                    newShipmentOrder.ContainerStatus = subJobSE.ContainerStatus;
                    // End From Sub Job
                    newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                    newShipmentOrder.MarketId = model.MarketId;
                    newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                    newShipmentOrder.QuotationNo = model.QuotationNo;
                    newShipmentOrder.AgentId = model.AgentId; 
                    newShipmentOrder.AgentName = model.AgentName; 
                    newShipmentOrder.AgentAddress = model.AgentAddress;
                    newShipmentOrder.TranshipmentId = model.TranshipmentId; 
                    newShipmentOrder.TranshipmentName = model.TranshipmentName; 
                    newShipmentOrder.TranshipmentAddress = model.TranshipmentAddress;
                    newShipmentOrder.ShipperId = model.ShipperId; 
                    newShipmentOrder.ShipperName = model.ShipperName; 
                    newShipmentOrder.ShipperAddress = model.ShipperAddress;
                    newShipmentOrder.ConsigneeId = model.ConsigneeId; 
                    newShipmentOrder.ConsigneeName = model.ConsigneeName; 
                    newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                    newShipmentOrder.NPartyId = model.NPartyId; 
                    newShipmentOrder.NPartyName = model.NPartyName; 
                    newShipmentOrder.NPartyAddress = model.NPartyAddress;
                    newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId; 
                    newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                    newShipmentOrder.LoadingPortId = model.LoadingPortId; 
                    newShipmentOrder.LoadingPortName = model.LoadingPortName;
                    newShipmentOrder.ETA = model.ETA;
                    newShipmentOrder.ETD = model.ETD;
                    newShipmentOrder.DischargePortId = model.DischargePortId;
                    newShipmentOrder.DischargePortName = model.DischargePortName;
                    newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId;
                    newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                    newShipmentOrder.OBLStatus = model.OBLStatus; 
                    newShipmentOrder.OBLCollectId = model.OBLCollectId;
                    newShipmentOrder.OBLPayableId = model.OBLPayableId;
                    newShipmentOrder.HBLStatus = model.HBLStatus;
                    newShipmentOrder.HBLCollectId = model.HBLCollectId; 
                    newShipmentOrder.HBLPayableId = model.HBLPayableId;
                    newShipmentOrder.OBLCurrency = model.OBLCurrency;
                    newShipmentOrder.HBLCurrency = model.HBLCurrency;
                    newShipmentOrder.OBLAmount = model.OBLAmount; 
                    newShipmentOrder.HBLAmount = model.HBLAmount;
                    newShipmentOrder.GoodDescription = model.GoodDescription; 
                    newShipmentOrder.OceanMSTBLNo = model.OceanMSTBLNo;
                    newShipmentOrder.VolumeBL = model.VolumeBL;
                    newShipmentOrder.VolumeInvoice = model.VolumeInvoice;
                    newShipmentOrder.SSLineId = model.SSLineId;
                    newShipmentOrder.BrokerId = model.BrokerId; newShipmentOrder.DepoId = model.DepoId;
                    newShipmentOrder.WareHouseName = model.WareHouseName; 
                    newShipmentOrder.KINS = model.KINS; newShipmentOrder.CFName = model.CFName;
                    newShipmentOrder.DeletedAt = model.DeletedAt;
                    newShipmentOrder.IsDeleted = model.IsDeleted;
                    newShipmentOrder.JobClosed = model.JobClosed; 
                    newShipmentOrder.JobClosedOn = model.JobClosedOn;
                    newShipmentOrder.GoodRecDate = model.GoodRecDate;
                    newShipmentOrder.CreatedById = model.CreatedById;
                    newShipmentOrder.CreatedAt = DateTime.Now;
                    newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.SeaImport, model.JobNumber, model.SubJobNumber);

                    UpdateObject(firstJobShipmentOrder);
                    model = _repository.CreateObject(newShipmentOrder);
                    if (model != null)
                    {
                        IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                        if (shipmentRoutingList != null)
                        {
                            foreach (var item in shipmentRoutingList)
                            {
                                ShipmentOrderRouting route = new ShipmentOrderRouting();
                                route.AirportFrom = item.AirportFrom;
                                route.AirportTo = item.AirportTo;
                                route.CityId = item.CityId;
                                route.OfficeId = item.OfficeId;
                                route.CreatedById = model.CreatedById;
                                route.CreatedAt = DateTime.Now;
                                route.ETD = item.ETD;
                                route.FlightId = item.FlightId;
                                route.FlightNo = item.FlightNo;
                                route.PortId = item.PortId;
                                route.ShipmentOrderId = model.Id;
                                route.VesselId = item.VesselId;
                                route.VesselName = item.VesselName;
                                route.VesselType = item.VesselType;
                                route.Voyage = item.Voyage;
                                _shipmentorderroutingService.CreateObject(route);
                            }
                        }
                    }
                    return model;
                    }
                }

        public ShipmentOrder UpdateAsSubOrderSeaExport(ShipmentOrder model, IContactService _contactService,
          ShipmentOrderRouting shipmentorderrouting, IShipmentOrderRoutingService _shipmentorderroutingService)
        {
             
            var subJobNo = _repository.GetLastSubJobNumber(model.OfficeId, model.JobId, model.JobNumber) + 1;

            model.TotalSub = model.TotalSub + 1;
            if (subJobNo == 1)
            {

                int serviceNo = model.LoadStatus == "FCL" ? 5 : 4;
                string containerStatus = model.LoadStatus == "FCL" ? "G" : "N";

                ShipmentOrder newShipmentOrder = new ShipmentOrder(); ;
                newShipmentOrder.JobId = model.JobId;
                newShipmentOrder.JobNumber = model.JobNumber;
                newShipmentOrder.SubJobNumber = subJobNo;
                newShipmentOrder.OfficeId = model.OfficeId;
                newShipmentOrder.TotalSub = model.TotalSub;
                newShipmentOrder.SIReference = model.SIReference;
                newShipmentOrder.SIDate = model.SIDate;
                newShipmentOrder.LoadStatus = "LCL";
                newShipmentOrder.ServiceNoID = serviceNo;
                newShipmentOrder.ContainerStatus = containerStatus;
                newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                newShipmentOrder.MarketId = model.MarketId;
                newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                newShipmentOrder.QuotationNo = model.QuotationNo;
                newShipmentOrder.AgentId = model.AgentId;
                newShipmentOrder.AgentName = model.AgentName;
                newShipmentOrder.AgentAddress = model.AgentAddress;
                newShipmentOrder.DeliveryId = model.DeliveryId;
                newShipmentOrder.DeliveryName = model.DeliveryName;
                newShipmentOrder.DeliveryAddress = model.DeliveryAddress;
                newShipmentOrder.TranshipmentId = model.TranshipmentId;
                newShipmentOrder.TranshipmentName = model.TranshipmentName;
                newShipmentOrder.TranshipmentAddress = model.TranshipmentAddress;
                newShipmentOrder.ShipperId = model.ShipperId;
                newShipmentOrder.ShipperName = model.ShipperName;
                newShipmentOrder.ShipperAddress = model.ShipperAddress;
                newShipmentOrder.ConsigneeId = model.ConsigneeId;
                newShipmentOrder.ConsigneeName = model.ConsigneeName;
                newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                newShipmentOrder.NPartyId = model.NPartyId;
                newShipmentOrder.NPartyName = model.NPartyName;
                newShipmentOrder.NPartyAddress = model.NPartyAddress;
                newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId;
                newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                newShipmentOrder.LoadingPortId = model.LoadingPortId;
                newShipmentOrder.LoadingPortName = model.LoadingPortName;
                newShipmentOrder.ETD = model.ETD;
                newShipmentOrder.ETA = model.ETA;
                newShipmentOrder.DischargePortId = model.DischargePortId;
                newShipmentOrder.DischargePortName = model.DischargePortName;
                newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId;
                newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                newShipmentOrder.OBLStatus = model.OBLStatus;
                newShipmentOrder.OBLCollectId = model.OBLCollectId;
                newShipmentOrder.OBLPayableId = model.OBLPayableId;
                newShipmentOrder.HBLStatus = model.HBLStatus;
                newShipmentOrder.HBLCollectId = model.HBLCollectId;
                newShipmentOrder.HBLPayableId = model.HBLPayableId;
                newShipmentOrder.Currency = model.Currency;
                newShipmentOrder.HandlingInfo = model.HandlingInfo;
                newShipmentOrder.GoodDescription = model.GoodDescription;
                newShipmentOrder.OceanMSTBLNo = model.OceanMSTBLNo;
                newShipmentOrder.VolumeBL = model.VolumeBL;
                newShipmentOrder.VolumeInvoice = model.VolumeInvoice;
                newShipmentOrder.SSLineId = model.SSLineId;
                newShipmentOrder.BrokerId = model.BrokerId;
                newShipmentOrder.DepoId = model.DepoId;
                newShipmentOrder.DeletedAt = model.DeletedAt;   
                newShipmentOrder.IsDeleted = model.IsDeleted;
                newShipmentOrder.JobClosed = model.JobClosed;
                newShipmentOrder.JobClosedOn = model.JobClosedOn;
                newShipmentOrder.GoodRecDate = model.GoodRecDate;
                newShipmentOrder.CreatedById = model.CreatedById;
                newShipmentOrder.CreatedAt = DateTime.Now;
                newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.SeaExport, model.JobNumber, model.SubJobNumber);

                ShipmentOrder shipmentorder = GetObjectById(model.Id);
                shipmentorder.TotalSub = model.TotalSub;
                if (model.LoadStatus == "FCL")
                {
                    model.ContainerStatus = "G";
                }
                UpdateObject(shipmentorder);
                model = _repository.CreateObject(newShipmentOrder);
                if (model != null)
                {
                    IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                    if (shipmentRoutingList != null)
                    {
                        foreach (var item in shipmentRoutingList)
                        {
                            ShipmentOrderRouting route = new ShipmentOrderRouting();
                            route.AirportFrom = item.AirportFrom;
                            route.AirportTo = item.AirportTo;
                            route.CityId = item.CityId;
                            route.OfficeId = item.OfficeId;
                            route.CreatedById = model.CreatedById;
                            route.CreatedAt = DateTime.Now;
                            route.ETD = item.ETD;
                            route.FlightId = item.FlightId;
                            route.FlightNo = item.FlightNo;
                            route.PortId = item.PortId;
                            route.ShipmentOrderId = model.Id;
                            route.VesselId = item.VesselId;
                            route.VesselName = item.VesselName;
                            route.VesselType = item.VesselType;
                            route.Voyage = item.Voyage;
                            _shipmentorderroutingService.CreateObject(route);
                        }
                    }
                }
                return model;
            }
            else
            {
                var tempShipmentOrderId = GetQueryable().Where(x => x.JobId == model.JobId && x.OfficeId == model.OfficeId
                                                               && x.JobNumber == model.JobNumber && x.SubJobNumber == 1).Select(x => x.Id).FirstOrDefault();
                ShipmentOrder subJobSE = GetObjectById(tempShipmentOrderId);
                ShipmentOrder firstJobShipmentOrder = GetObjectById(model.Id);
                ShipmentOrder newShipmentOrder = new ShipmentOrder();
                newShipmentOrder.JobId = model.JobId;
                newShipmentOrder.JobNumber = model.JobNumber;
                newShipmentOrder.SubJobNumber = subJobNo;
                newShipmentOrder.OfficeId = model.OfficeId;
                newShipmentOrder.TotalSub = model.TotalSub;
                newShipmentOrder.SIReference = model.SIReference;
                newShipmentOrder.SIDate = model.SIDate; 
                // From Sub Job
                newShipmentOrder.LoadStatus = subJobSE.LoadStatus;
                newShipmentOrder.ServiceNoID = subJobSE.ServiceNoID;
                newShipmentOrder.ContainerStatus = subJobSE.ContainerStatus;
                // End From Sub Job
                newShipmentOrder.ShipmentStatus = model.ShipmentStatus;
                newShipmentOrder.MarketId = model.MarketId;
                newShipmentOrder.MarketCompanyId = model.MarketCompanyId;
                newShipmentOrder.QuotationNo = model.QuotationNo;
                newShipmentOrder.AgentId = model.AgentId;
                newShipmentOrder.AgentName = model.AgentName;
                newShipmentOrder.AgentAddress = model.AgentAddress;
                newShipmentOrder.DeliveryId = model.DeliveryId;
                newShipmentOrder.DeliveryName = model.DeliveryName;
                newShipmentOrder.DeliveryAddress = model.DeliveryAddress;
                newShipmentOrder.TranshipmentId = model.TranshipmentId;
                newShipmentOrder.TranshipmentName = model.TranshipmentName;
                newShipmentOrder.TranshipmentAddress = model.TranshipmentAddress;
                newShipmentOrder.ShipperId = model.ShipperId;
                newShipmentOrder.ShipperName = model.ShipperName;
                newShipmentOrder.ShipperAddress = model.ShipperAddress;
                newShipmentOrder.ConsigneeId = model.ConsigneeId;
                newShipmentOrder.ConsigneeName = model.ConsigneeName;
                newShipmentOrder.ConsigneeAddress = model.ConsigneeAddress;
                newShipmentOrder.NPartyId = model.NPartyId;
                newShipmentOrder.NPartyName = model.NPartyName;
                newShipmentOrder.NPartyAddress = model.NPartyAddress;
                newShipmentOrder.ReceiptPlaceId = model.ReceiptPlaceId;
                newShipmentOrder.ReceiptPlaceName = model.ReceiptPlaceName;
                newShipmentOrder.LoadingPortId = model.LoadingPortId;
                newShipmentOrder.LoadingPortName = model.LoadingPortName;
                newShipmentOrder.ETD = model.ETD;
                newShipmentOrder.ETA = model.ETA;
                newShipmentOrder.DischargePortId = model.DischargePortId;
                newShipmentOrder.DischargePortName = model.DischargePortName;
                newShipmentOrder.DeliveryPlaceId = model.DeliveryPlaceId;
                newShipmentOrder.DeliveryPlaceName = model.DeliveryPlaceName;
                newShipmentOrder.OBLStatus = model.OBLStatus;
                newShipmentOrder.OBLCollectId = model.OBLCollectId;
                newShipmentOrder.OBLPayableId = model.OBLPayableId;
                newShipmentOrder.HBLStatus = model.HBLStatus;
                newShipmentOrder.HBLCollectId = model.HBLCollectId;
                newShipmentOrder.HBLPayableId = model.HBLPayableId;
                newShipmentOrder.Currency = model.Currency;
                newShipmentOrder.HandlingInfo = model.HandlingInfo;
                newShipmentOrder.GoodDescription = model.GoodDescription;
                newShipmentOrder.OceanMSTBLNo = model.OceanMSTBLNo;
                newShipmentOrder.VolumeBL = model.VolumeBL;
                newShipmentOrder.VolumeInvoice = model.VolumeInvoice;
                newShipmentOrder.SSLineId = model.SSLineId;
                newShipmentOrder.BrokerId = model.BrokerId;
                newShipmentOrder.DepoId = model.DepoId;
                newShipmentOrder.DeletedAt = model.DeletedAt;
                newShipmentOrder.IsDeleted = model.IsDeleted;
                newShipmentOrder.JobClosed = model.JobClosed;
                newShipmentOrder.JobClosedOn = model.JobClosedOn;
                newShipmentOrder.GoodRecDate = model.GoodRecDate;
                newShipmentOrder.CreatedById = model.CreatedById;
                newShipmentOrder.CreatedAt = DateTime.Now;
                newShipmentOrder.ShipmentOrderId = GenerateShipmentNo(model.Office.InitialCompany, MasterConstant.JobType.SeaExport, model.JobNumber, model.SubJobNumber);

                UpdateObject(firstJobShipmentOrder);
                model = _repository.CreateObject(newShipmentOrder);
                if (model != null)
                {
                    IList<ShipmentOrderRouting> shipmentRoutingList = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                    if (shipmentRoutingList != null)
                    {
                        foreach (var item in shipmentRoutingList)
                        {
                            ShipmentOrderRouting route = new ShipmentOrderRouting();
                            route.AirportFrom = item.AirportFrom;
                            route.AirportTo = item.AirportTo;
                            route.CityId = item.CityId;
                            route.OfficeId = item.OfficeId;
                            route.CreatedById = model.CreatedById;
                            route.CreatedAt = DateTime.Now;
                            route.ETD = item.ETD;
                            route.FlightId = item.FlightId;
                            route.FlightNo = item.FlightNo;
                            route.PortId = item.PortId;
                            route.ShipmentOrderId = model.Id;
                            route.VesselId = item.VesselId;
                            route.VesselName = item.VesselName;
                            route.VesselType = item.VesselType;
                            route.Voyage = item.Voyage;
                            _shipmentorderroutingService.CreateObject(route);
                        }
                    }
                }
                return model;
            }
        } 

        public ShipmentOrder UpdateObjectEMKLDomestic(ShipmentOrder model, IContactService _contactService,
            ShipmentOrderRouting shipmentorderrouting,IShipmentOrderRoutingService _shipmentorderroutingService)
        { 
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = String.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.FreightStatus = model.FreightStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                shipmentorder.ETD = model.ETD;
                shipmentorder.ETA = model.ETA;

                // Freight
                shipmentorder.HBLStatus = model.HBLStatus.ToUpper();
                shipmentorder.HBLCollectId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLCollectId;
                shipmentorder.HBLPayableId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLPayableId;
                shipmentorder.Currency = model.Currency;

                // Description
                shipmentorder.GoodDescription = String.IsNullOrEmpty(model.GoodDescription) ? "" : model.GoodDescription.ToUpper();

                shipmentorder.OceanMSTBLNo = String.IsNullOrEmpty(model.OceanMSTBLNo) ? "" : model.OceanMSTBLNo.ToUpper();
                shipmentorder.VolumeBL = model.VolumeBL;
                shipmentorder.VolumeInvoice = model.VolumeInvoice;
                shipmentorder.CarriageValue = String.IsNullOrEmpty(model.CarriageValue) ? "" : model.CarriageValue.ToUpper();
                shipmentorder.CustomValue = String.IsNullOrEmpty(model.CustomValue) ? "" : model.CustomValue.ToUpper();
                shipmentorder.SSLineId = model.SSLineId;
                shipmentorder.BrokerId = model.BrokerId;
                shipmentorder.DepoId = model.DepoId;
                shipmentorder.Truck = String.IsNullOrEmpty(model.Truck) ? "" : model.Truck.ToUpper();
                shipmentorder.VesselFlight = String.IsNullOrEmpty(model.VesselFlight) ? "N" : model.VesselFlight.ToUpper();

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;
                 
                //ShipmentOrder Routing 
                IList<ShipmentOrderRouting> routing = _shipmentorderroutingService.GetListByShipmentOrderId(model.Id);
                foreach (var item in routing)
                {
                    _shipmentorderroutingService.DeleteObject(item.Id);
                }

                switch (shipmentorder.VesselFlight)
                { 
                    case "V":
                        ShipmentOrderRouting tVRoute = new ShipmentOrderRouting();
                        tVRoute.OfficeId = model.OfficeId;
                        tVRoute.CreatedById = model.CreatedById;
                        tVRoute.CreatedAt = DateTime.Today;
                        tVRoute.PortId = shipmentorderrouting.PortId;
                        tVRoute.ShipmentOrderId = model.Id;
                        tVRoute.VesselId = shipmentorderrouting.VesselId;
                        tVRoute.VesselName = shipmentorderrouting.VesselName;
                        tVRoute.VesselType = MasterConstant.VesselType.VesselFeeder;
                        tVRoute.Voyage = shipmentorderrouting.Voyage;
                        _shipmentorderroutingService.CreateObject(tVRoute);
                        break;
                    case "F":
                        ShipmentOrderRouting tFRoute = new ShipmentOrderRouting();
                        tFRoute.OfficeId = model.OfficeId;
                        tFRoute.CreatedById = model.CreatedById;
                        tFRoute.CreatedAt = DateTime.Today;
                        tFRoute.AirportFrom = shipmentorderrouting.AirportFrom;
                        tFRoute.ShipmentOrderId = model.Id;
                        tFRoute.FlightId = shipmentorderrouting.FlightId;
                        tFRoute.FlightNo = shipmentorderrouting.FlightNo;
                        _shipmentorderroutingService.CreateObject(tFRoute);
                        break;
                    case "N": break;
                }

                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLDomestic
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLDomestic
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLDomestic
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLDomestic
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLDomestic
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLDomestic
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */
            }
            return model;
        }


        public ShipmentOrder UpdateObjectEMKLAirImport(ShipmentOrder model, IContactService _contactService)
        {
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = string.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.LoadStatus = model.LoadStatus;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.FreightStatus = model.FreightStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                // Agent, Delivery, Transhipment, Shipper, COnsignee, NParty
                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId;
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.DepartureAirPortId = model.DepartureAirPortId;
                shipmentorder.DepartureAirPortName = String.IsNullOrEmpty(model.DepartureAirPortName) ? "" : model.DepartureAirPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DestinationAirPortId = model.DestinationAirPortId;
                shipmentorder.DestinationAirPortName = String.IsNullOrEmpty(model.DestinationAirPortName) ? "" : model.DestinationAirPortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                shipmentorder.ETD = model.ETD;
                shipmentorder.ETA = model.ETA;

                // Freight
                shipmentorder.HAWBStatus = model.HAWBStatus.ToUpper();
                shipmentorder.HAWBCollectId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBCollectId;
                shipmentorder.HAWBPayableId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBPayableId;
                shipmentorder.Currency = model.Currency;
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();


                // Description
                shipmentorder.PiecesRCP = String.IsNullOrEmpty(model.PiecesRCP) ? "" : model.PiecesRCP.ToUpper();
                shipmentorder.GrossWeight = model.GrossWeight;
                shipmentorder.KGLB = String.IsNullOrEmpty(model.KGLB) ? "" : model.KGLB.ToUpper();
                shipmentorder.ChargeWeight = model.ChargeWeight;
                shipmentorder.ChargeRate = model.ChargeRate;
                shipmentorder.Total = String.IsNullOrEmpty(model.Total) ? "" : model.Total.ToUpper();
                shipmentorder.GoodNatureQuantity = String.IsNullOrEmpty(model.GoodNatureQuantity) ? "" : model.GoodNatureQuantity.ToUpper();
                shipmentorder.Shipmark = String.IsNullOrEmpty(model.Shipmark) ? "" : model.Shipmark.ToUpper();
                shipmentorder.Commodity = String.IsNullOrEmpty(model.Commodity) ? "" : model.Commodity.ToUpper();
                shipmentorder.PackagingCode = model.PackagingCode;

                shipmentorder.HAWBNo = String.IsNullOrEmpty(model.HAWBNo) ? "" : model.HAWBNo.ToUpper();
                shipmentorder.MAWBNo = String.IsNullOrEmpty(model.MAWBNo) ? "" : model.MAWBNo.ToUpper();
                shipmentorder.ChargeableWeight = model.ChargeableWeight;
                shipmentorder.WeightHAWB = model.WeightHAWB;
                shipmentorder.CarriageValue = String.IsNullOrEmpty(model.CarriageValue) ? "" : model.CarriageValue.ToUpper();
                shipmentorder.CustomValue = String.IsNullOrEmpty(model.CustomValue) ? "" : model.CustomValue.ToUpper();
                shipmentorder.IATAId = model.IATAId;
                shipmentorder.BrokerId = model.BrokerId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */

            }
            return model;
        }

        public ShipmentOrder UpdateObjectEMKLAirExport(ShipmentOrder model, BillOfLading billoflading, ShipmentInstruction shipmentinstruction,
             TelexRelease telexrelease, IContactService _contactService, IBillOfLadingService _billofladingService
            , IShipmentInstructionService _shipmentInstructionService, ITelexReleaseService _telexreleaseService)
        {
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = string.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.FreightStatus = model.FreightStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                // Agent, Delivery, Transhipment, Shipper, COnsignee, NParty
                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId;
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.DepartureAirPortId = model.DepartureAirPortId;
                shipmentorder.DepartureAirPortName = String.IsNullOrEmpty(model.DepartureAirPortName) ? "" : model.DepartureAirPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DestinationAirPortId = model.DestinationAirPortId;
                shipmentorder.DestinationAirPortName = String.IsNullOrEmpty(model.DestinationAirPortName) ? "" : model.DestinationAirPortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                shipmentorder.ETD = model.ETD;
                shipmentorder.ETA = model.ETA;

                // Freight
                shipmentorder.MAWBStatus = model.MAWBStatus.ToUpper();
                shipmentorder.MAWBCollectId = model.MAWBStatus.ToUpper() == "P" ? 0 : model.MAWBCollectId;
                shipmentorder.MAWBPayableId = model.MAWBStatus.ToUpper() == "P" ? 0 : model.MAWBPayableId;
                shipmentorder.HAWBStatus = model.HAWBStatus.ToUpper();
                shipmentorder.HAWBCollectId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBCollectId;
                shipmentorder.HAWBPayableId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBPayableId;
                shipmentorder.Currency = model.Currency;
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();


                // Description
                shipmentorder.PiecesRCP = String.IsNullOrEmpty(model.PiecesRCP) ? "" : model.PiecesRCP.ToUpper();
                shipmentorder.GrossWeight = model.GrossWeight;
                shipmentorder.KGLB = String.IsNullOrEmpty(model.KGLB) ? "" : model.KGLB.ToUpper();
                shipmentorder.ChargeWeight = model.ChargeWeight;
                shipmentorder.ChargeRate = model.ChargeRate;
                shipmentorder.Total = String.IsNullOrEmpty(model.Total) ? "" : model.Total.ToUpper();
                shipmentorder.GoodNatureQuantity = String.IsNullOrEmpty(model.GoodNatureQuantity) ? "" : model.GoodNatureQuantity.ToUpper();
                shipmentorder.Shipmark = String.IsNullOrEmpty(model.Shipmark) ? "" : model.Shipmark.ToUpper();
                shipmentorder.Commodity = String.IsNullOrEmpty(model.Commodity) ? "" : model.Commodity.ToUpper();
                shipmentorder.PackagingCode = model.PackagingCode;

                shipmentorder.MAWBNo = String.IsNullOrEmpty(model.MAWBNo) ? "" : model.MAWBNo.ToUpper();
                shipmentorder.ChargeableWeight = model.ChargeableWeight;
                shipmentorder.WeightHAWB = model.WeightHAWB;
                shipmentorder.CarriageValue = String.IsNullOrEmpty(model.CarriageValue) ? "" : model.CarriageValue.ToUpper();
                shipmentorder.CustomValue = String.IsNullOrEmpty(model.CustomValue) ? "" : model.CustomValue.ToUpper();
                shipmentorder.IATAId = model.IATAId;
                shipmentorder.BrokerId = model.BrokerId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLAirExport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLAirExport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLAirExport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLAirExport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLAirExport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */

                /* ==================================================== Bill Of Lading ==================================== */
                BillOfLading newBillOfLading = _billofladingService.GetObjectByShipmentOrderId(model.Id);
                if (newBillOfLading == null)
                {
                    newBillOfLading = new BillOfLading();
                    newBillOfLading.CreatedById = billoflading.CreatedById;
                }
                newBillOfLading.AgentId = billoflading.AgentId;
                newBillOfLading.AgentName = String.IsNullOrEmpty(billoflading.AgentName) ? "" : billoflading.AgentName.ToUpper();
                newBillOfLading.AgentAddress = String.IsNullOrEmpty(billoflading.AgentAddress) ? "" : billoflading.AgentAddress.ToUpper();
                newBillOfLading.AmountInsurance = String.IsNullOrEmpty(billoflading.AmountInsurance) ? "" : billoflading.AmountInsurance.ToUpper();
                newBillOfLading.BLNumber = String.IsNullOrEmpty(billoflading.BLNumber) ? "" : billoflading.BLNumber.ToUpper();
                newBillOfLading.CargoInsurance = String.IsNullOrEmpty(billoflading.CargoInsurance) ? "" : billoflading.CargoInsurance.ToUpper();
                newBillOfLading.OfficeId = billoflading.OfficeId;
                newBillOfLading.ConsigneeAddress = String.IsNullOrEmpty(billoflading.ConsigneeAddress) ? "" : billoflading.ConsigneeAddress.ToUpper();
                newBillOfLading.ConsigneeId = billoflading.ConsigneeId;
                newBillOfLading.ConsigneeName = String.IsNullOrEmpty(billoflading.ConsigneeName) ? "" : billoflading.ConsigneeName.ToUpper();
                newBillOfLading.Descriptions = String.IsNullOrEmpty(billoflading.Descriptions) ? "" : billoflading.Descriptions.ToUpper();
                newBillOfLading.FreightAmount = String.IsNullOrEmpty(billoflading.FreightAmount) ? "" : billoflading.FreightAmount.ToUpper();
                newBillOfLading.FreightPayableAt = String.IsNullOrEmpty(billoflading.FreightPayableAt) ? "" : billoflading.FreightPayableAt.ToUpper();
                newBillOfLading.HAWBFee = decimal.Parse(billoflading.HAWBFee.ToString());
                newBillOfLading.MasterBLId = billoflading.MasterBLId;
                newBillOfLading.UpdatedById = billoflading.UpdatedById;
                newBillOfLading.UpdatedAt = DateTime.Now;
                newBillOfLading.NoOfBL = String.IsNullOrEmpty(billoflading.NoOfBL) ? "" : billoflading.NoOfBL.ToUpper();
                newBillOfLading.NPartyAddress = String.IsNullOrEmpty(billoflading.NPartyAddress) ? "" : billoflading.NPartyAddress.ToUpper();
                newBillOfLading.NPartyId = billoflading.NPartyId;
                newBillOfLading.NPartyName = String.IsNullOrEmpty(billoflading.NPartyName) ? "" : billoflading.NPartyName.ToUpper();
                newBillOfLading.PlaceDateOfIssue = billoflading.PlaceDateOfIssue;
                newBillOfLading.ShipmentOrderId = model.Id;
                newBillOfLading.ShipmentOnBoard = billoflading.ShipmentOnBoard;
                newBillOfLading.ShipperAddress = String.IsNullOrEmpty(billoflading.ShipperAddress) ? "" : billoflading.ShipperAddress.ToUpper();
                newBillOfLading.ShipperId = billoflading.ShipperId;
                newBillOfLading.ShipperName = String.IsNullOrEmpty(billoflading.ShipperName) ? "" : billoflading.ShipperName.ToUpper();
                newBillOfLading.TotalNoOfContainer = String.IsNullOrEmpty(billoflading.TotalNoOfContainer) ? "" : billoflading.TotalNoOfContainer.ToUpper();
                _billofladingService.CreateUpdateObject(newBillOfLading);
                /* ==================================================== END Bill Of Lading ==================================== */

                /* ==================================================== Shipping Instruction ==================================== */

                ShipmentInstruction newshipmentinstruction = _shipmentInstructionService.GetObjectByShipmentOrderId(model.Id);
                if (newshipmentinstruction == null)
                {
                    newshipmentinstruction = new ShipmentInstruction();
                    newshipmentinstruction.CreatedById = shipmentinstruction.CreatedById;
                }
                newshipmentinstruction.Attention = String.IsNullOrEmpty(shipmentinstruction.Attention) ? "" : shipmentinstruction.Attention.ToUpper();
                newshipmentinstruction.CollectAddress = String.IsNullOrEmpty(shipmentinstruction.CollectAddress) ? "" : shipmentinstruction.CollectAddress.ToUpper();
                newshipmentinstruction.CollectAt = shipmentinstruction.CollectAt;
                newshipmentinstruction.CollectName = String.IsNullOrEmpty(shipmentinstruction.CollectName) ? "" : shipmentinstruction.CollectName.ToUpper();
                newshipmentinstruction.OfficeId = shipmentinstruction.OfficeId;
                newshipmentinstruction.ConsigneeAddress = String.IsNullOrEmpty(shipmentinstruction.ConsigneeAddress) ? "" : shipmentinstruction.ConsigneeAddress.ToUpper();
                newshipmentinstruction.ConsigneeId = shipmentinstruction.ConsigneeId;
                newshipmentinstruction.ConsigneeName = String.IsNullOrEmpty(shipmentinstruction.ConsigneeName) ? "" : shipmentinstruction.ConsigneeName.ToUpper();
                newshipmentinstruction.FreightAgreed = String.IsNullOrEmpty(shipmentinstruction.FreightAgreed) ? "" : shipmentinstruction.FreightAgreed.ToUpper();
               // newshipmentinstruction.GoodDescription = String.IsNullOrEmpty(shipmentinstruction.GoodDescription) ? "" : shipmentinstruction.GoodDescription.ToUpper();
                newshipmentinstruction.UpdatedById = shipmentinstruction.UpdatedById;
                newshipmentinstruction.UpdatedAt = DateTime.Now;
                newshipmentinstruction.NPartyAddress = String.IsNullOrEmpty(shipmentinstruction.NPartyAddress) ? "" : shipmentinstruction.NPartyAddress.ToUpper();
                newshipmentinstruction.NPartyId = shipmentinstruction.NPartyId;
                newshipmentinstruction.NPartyName = String.IsNullOrEmpty(shipmentinstruction.NPartyName) ? "" : shipmentinstruction.NPartyName.ToUpper();
                newshipmentinstruction.OriginalBL = String.IsNullOrEmpty(shipmentinstruction.OriginalBL) ? "" : shipmentinstruction.OriginalBL.ToUpper();
                newshipmentinstruction.ShipmentOrderId = model.Id;
                newshipmentinstruction.ShipperAddress = String.IsNullOrEmpty(shipmentinstruction.ShipperAddress) ? "" : shipmentinstruction.ShipperAddress.ToUpper();
                newshipmentinstruction.ShipperId = shipmentinstruction.ShipperId;
                newshipmentinstruction.ShipperName = String.IsNullOrEmpty(shipmentinstruction.ShipperName) ? "" : shipmentinstruction.ShipperName.ToUpper();
                newshipmentinstruction.SIReference = String.IsNullOrEmpty(shipmentinstruction.SIReference) ? "" : shipmentinstruction.SIReference.ToUpper();
                newshipmentinstruction.SpecialInstruction = String.IsNullOrEmpty(shipmentinstruction.SpecialInstruction) ? "" : shipmentinstruction.SpecialInstruction.ToUpper();

                // Description
                newshipmentinstruction.PiecesRCP = String.IsNullOrEmpty(shipmentinstruction.PiecesRCP) ? "" : shipmentinstruction.PiecesRCP.ToUpper();
                newshipmentinstruction.GrossWeight2 = shipmentinstruction.GrossWeight2;
                newshipmentinstruction.KGLB = String.IsNullOrEmpty(shipmentinstruction.KGLB) ? "" : shipmentinstruction.KGLB.ToUpper();
                newshipmentinstruction.ChargeWeight = shipmentinstruction.ChargeWeight;
                newshipmentinstruction.ChargeRate = shipmentinstruction.ChargeRate;
                newshipmentinstruction.Total = String.IsNullOrEmpty(shipmentinstruction.Total) ? "" : shipmentinstruction.Total.ToUpper();
                newshipmentinstruction.GoodNatureQuantity = String.IsNullOrEmpty(shipmentinstruction.GoodNatureQuantity) ? "" : shipmentinstruction.GoodNatureQuantity.ToUpper();

                _shipmentInstructionService.CreateUpdateObject(newshipmentinstruction);
                /* ==================================================== END Shipping Instruction ==================================== */

                /* ==================================================== Telex Release ==================================== */
                TelexRelease newTelexRelease = _telexreleaseService.GetObjectByShipmentOrderId(model.Id);
                if (newTelexRelease == null)
                {
                    newTelexRelease = new TelexRelease();
                    newTelexRelease.CreatedAt = newTelexRelease.CreatedAt;
                    newTelexRelease.CreatedById = newTelexRelease.CreatedById;
                }
                newTelexRelease.OfficeId = newTelexRelease.OfficeId;
                newTelexRelease.UpdatedById = newTelexRelease.UpdatedById;
                newTelexRelease.UpdatedAt = DateTime.Now;
                newTelexRelease.Original = String.IsNullOrEmpty(newTelexRelease.Original) ? "" : newTelexRelease.Original.ToUpper();
                newTelexRelease.SeaWaybill = String.IsNullOrEmpty(newTelexRelease.SeaWaybill) ? "" : newTelexRelease.SeaWaybill.ToUpper();
                newTelexRelease.ShipmentOrderId = model.Id;
                _telexreleaseService.CreateUpdateObject(newTelexRelease);
                /* ==================================================== END Telex Release ==================================== */
            }
            return model;
        }

        public ShipmentOrder UpdateObjectEMKLSeaImport(ShipmentOrder model, IContactService _contactService)
        {
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = String.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.FreightStatus = model.FreightStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId;
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.TranshipmentId = model.TranshipmentId;
                shipmentorder.TranshipmentName = model.TranshipmentName.ToUpper();
                shipmentorder.TranshipmentAddress = String.IsNullOrEmpty(model.TranshipmentAddress) ? "" : model.TranshipmentAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.LoadingPortId = model.LoadingPortId;
                shipmentorder.LoadingPortName = String.IsNullOrEmpty(model.LoadingPortName) ? "" : model.LoadingPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DischargePortId = model.DischargePortId;
                shipmentorder.DischargePortName = String.IsNullOrEmpty(model.DischargePortName) ? "" : model.DischargePortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                // Freight
                shipmentorder.HBLStatus = model.HBLStatus.ToUpper();
                shipmentorder.HBLCollectId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLCollectId;
                shipmentorder.HBLPayableId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLPayableId;
                shipmentorder.Currency = model.Currency;
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();
                shipmentorder.HBLCurrency = model.HBLCurrency;
                shipmentorder.HBLAmount = model.HBLAmount;

                // Description
                shipmentorder.GoodDescription = String.IsNullOrEmpty(model.GoodDescription) ? "" : model.GoodDescription.ToUpper();

                shipmentorder.OceanMSTBLNo = String.IsNullOrEmpty(model.OceanMSTBLNo) ? "" : model.OceanMSTBLNo.ToUpper();
                shipmentorder.HouseBLNo = String.IsNullOrEmpty(model.HouseBLNo) ? "" : model.HouseBLNo.ToUpper();
                shipmentorder.SecondBLNo = String.IsNullOrEmpty(model.SecondBLNo) ? "" : model.SecondBLNo.ToUpper();
                shipmentorder.VolumeBL = model.VolumeBL;
                shipmentorder.VolumeInvoice = model.VolumeInvoice;
                shipmentorder.WareHouseName = String.IsNullOrEmpty(model.WareHouseName) ? "" : model.WareHouseName.ToUpper();
                shipmentorder.KINS = String.IsNullOrEmpty(model.KINS) ? "" : model.KINS.ToUpper();
                shipmentorder.CFName = String.IsNullOrEmpty(model.CFName) ? "" : model.CFName.ToUpper();
                shipmentorder.VolumeBL = model.VolumeBL;
                shipmentorder.VolumeInvoice = model.VolumeInvoice;
                shipmentorder.SSLineId = model.SSLineId;
                shipmentorder.BrokerId = model.BrokerId;
                shipmentorder.DepoId = model.DepoId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaImport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaImport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaImport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaImport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaImport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaImport
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */
            }
            return model;
        }

        public ShipmentOrder UpdateObjectEMKLSeaExport(ShipmentOrder model, BillOfLading billoflading, ShipmentInstruction shipmentinstruction,
           ShipmentAdvice shipmentadvice, TelexRelease telexrelease, IContactService _contactService, IBillOfLadingService _billofladingService
           , IShipmentInstructionService _shipmentInstructionService, IShipmentAdviceService _shipmentAdviceService, ITelexReleaseService _telexreleaseService)
        {
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = string.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.LoadStatus = model.LoadStatus;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.ContainerStatus = model.ContainerStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId;
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.TranshipmentId = model.TranshipmentId;
                shipmentorder.TranshipmentName = model.TranshipmentName.ToUpper();
                shipmentorder.TranshipmentAddress = String.IsNullOrEmpty(model.TranshipmentAddress) ? "" : model.TranshipmentAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.LoadingPortId = model.LoadingPortId;
                shipmentorder.LoadingPortName = String.IsNullOrEmpty(model.LoadingPortName) ? "" : model.LoadingPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DischargePortId = model.DischargePortId;
                shipmentorder.DischargePortName = String.IsNullOrEmpty(model.DischargePortName) ? "" : model.DischargePortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                shipmentorder.ETD = model.ETD;
                shipmentorder.ETA = model.ETA;

                // Freight
                shipmentorder.HBLStatus = model.HBLStatus.ToUpper();
                shipmentorder.HBLCollectId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLCollectId;
                shipmentorder.HBLPayableId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLPayableId;
                shipmentorder.Currency = model.Currency;
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();

                // Description
                shipmentorder.GoodDescription = String.IsNullOrEmpty(model.GoodDescription) ? "" : model.GoodDescription.ToUpper();

                shipmentorder.OceanMSTBLNo = String.IsNullOrEmpty(model.OceanMSTBLNo) ? "" : model.OceanMSTBLNo.ToUpper();
                shipmentorder.VolumeBL = model.VolumeBL;
                shipmentorder.VolumeInvoice = model.VolumeInvoice;
                shipmentorder.SSLineId = model.SSLineId;
                shipmentorder.BrokerId = model.BrokerId;
                shipmentorder.DepoId = model.DepoId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaExport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaExport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaExport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaExport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaExport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.EMKLSeaExport
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */

                /* ==================================================== Bill Of Lading ==================================== */
                BillOfLading newBillOfLading = _billofladingService.GetObjectByShipmentOrderId(model.Id);
                if (newBillOfLading == null)
                {
                    newBillOfLading = new BillOfLading();
                    newBillOfLading.CreatedById = billoflading.CreatedById;
                }
                newBillOfLading.AgentId = billoflading.AgentId;
                newBillOfLading.AgentName = String.IsNullOrEmpty(billoflading.AgentName) ? "" : billoflading.AgentName.ToUpper();
                newBillOfLading.AgentAddress = String.IsNullOrEmpty(billoflading.AgentAddress) ? "" : billoflading.AgentAddress.ToUpper();
                newBillOfLading.AmountInsurance = String.IsNullOrEmpty(billoflading.AmountInsurance) ? "" : billoflading.AmountInsurance.ToUpper();
                newBillOfLading.BLNumber = String.IsNullOrEmpty(billoflading.BLNumber) ? "" : billoflading.BLNumber.ToUpper();
                newBillOfLading.CargoInsurance = String.IsNullOrEmpty(billoflading.CargoInsurance) ? "" : billoflading.CargoInsurance.ToUpper();
                newBillOfLading.OfficeId = billoflading.OfficeId;
                newBillOfLading.ConsigneeAddress = String.IsNullOrEmpty(billoflading.ConsigneeAddress) ? "" : billoflading.ConsigneeAddress.ToUpper();
                newBillOfLading.ConsigneeId = billoflading.ConsigneeId;
                newBillOfLading.ConsigneeName = String.IsNullOrEmpty(billoflading.ConsigneeName) ? "" : billoflading.ConsigneeName.ToUpper();
                newBillOfLading.Descriptions = String.IsNullOrEmpty(billoflading.Descriptions) ? "" : billoflading.Descriptions.ToUpper();
                newBillOfLading.FreightAmount = String.IsNullOrEmpty(billoflading.FreightAmount) ? "" : billoflading.FreightAmount.ToUpper();
                newBillOfLading.FreightPayableAt = String.IsNullOrEmpty(billoflading.FreightPayableAt) ? "" : billoflading.FreightPayableAt.ToUpper();
                newBillOfLading.HAWBFee = decimal.Parse(billoflading.HAWBFee.ToString());
                newBillOfLading.MasterBLId = billoflading.MasterBLId;
                newBillOfLading.UpdatedById = billoflading.UpdatedById;
                newBillOfLading.UpdatedAt = DateTime.Now;
                newBillOfLading.NoOfBL = String.IsNullOrEmpty(billoflading.NoOfBL) ? "" : billoflading.NoOfBL.ToUpper();
                newBillOfLading.NPartyAddress = String.IsNullOrEmpty(billoflading.NPartyAddress) ? "" : billoflading.NPartyAddress.ToUpper();
                newBillOfLading.NPartyId = billoflading.NPartyId;
                newBillOfLading.NPartyName = String.IsNullOrEmpty(billoflading.NPartyName) ? "" : billoflading.NPartyName.ToUpper();
                newBillOfLading.PlaceDateOfIssue = billoflading.PlaceDateOfIssue;
                newBillOfLading.ShipmentOrderId = model.Id;
                newBillOfLading.ShipmentOnBoard = billoflading.ShipmentOnBoard;
                newBillOfLading.ShipperAddress = String.IsNullOrEmpty(billoflading.ShipperAddress) ? "" : billoflading.ShipperAddress.ToUpper();
                newBillOfLading.ShipperId = billoflading.ShipperId;
                newBillOfLading.ShipperName = String.IsNullOrEmpty(billoflading.ShipperName) ? "" : billoflading.ShipperName.ToUpper();
                newBillOfLading.TotalNoOfContainer = String.IsNullOrEmpty(billoflading.TotalNoOfContainer) ? "" : billoflading.TotalNoOfContainer.ToUpper();
                _billofladingService.CreateUpdateObject(newBillOfLading);
                /* ==================================================== END Bill Of Lading ==================================== */

                /* ==================================================== Shipping Instruction ==================================== */

                ShipmentInstruction newshipmentinstruction = _shipmentInstructionService.GetObjectByShipmentOrderId(model.Id);
                if (newshipmentinstruction == null)
                {
                    newshipmentinstruction = new ShipmentInstruction();
                    newshipmentinstruction.CreatedById = shipmentinstruction.CreatedById;
                }
                newshipmentinstruction.Attention = String.IsNullOrEmpty(shipmentinstruction.Attention) ? "" : shipmentinstruction.Attention.ToUpper();
                newshipmentinstruction.CollectAddress = String.IsNullOrEmpty(shipmentinstruction.CollectAddress) ? "" : shipmentinstruction.CollectAddress.ToUpper();
                newshipmentinstruction.CollectAt = shipmentinstruction.CollectAt;
                newshipmentinstruction.CollectName = String.IsNullOrEmpty(shipmentinstruction.CollectName) ? "" : shipmentinstruction.CollectName.ToUpper();
                newshipmentinstruction.OfficeId = shipmentinstruction.OfficeId;
                newshipmentinstruction.ConsigneeAddress = String.IsNullOrEmpty(shipmentinstruction.ConsigneeAddress) ? "" : shipmentinstruction.ConsigneeAddress.ToUpper();
                newshipmentinstruction.ConsigneeId = shipmentinstruction.ConsigneeId;
                newshipmentinstruction.ConsigneeName = String.IsNullOrEmpty(shipmentinstruction.ConsigneeName) ? "" : shipmentinstruction.ConsigneeName.ToUpper();
                newshipmentinstruction.FreightAgreed = String.IsNullOrEmpty(shipmentinstruction.FreightAgreed) ? "" : shipmentinstruction.FreightAgreed.ToUpper();
                newshipmentinstruction.GoodDescription = String.IsNullOrEmpty(shipmentinstruction.GoodDescription) ? "" : shipmentinstruction.GoodDescription.ToUpper();
                newshipmentinstruction.UpdatedById = shipmentinstruction.UpdatedById;
                newshipmentinstruction.UpdatedAt = DateTime.Now;
                newshipmentinstruction.NPartyAddress = String.IsNullOrEmpty(shipmentinstruction.NPartyAddress) ? "" : shipmentinstruction.NPartyAddress.ToUpper();
                newshipmentinstruction.NPartyId = shipmentinstruction.NPartyId;
                newshipmentinstruction.NPartyName = String.IsNullOrEmpty(shipmentinstruction.NPartyName) ? "" : shipmentinstruction.NPartyName.ToUpper();
                newshipmentinstruction.OriginalBL = String.IsNullOrEmpty(shipmentinstruction.OriginalBL) ? "" : shipmentinstruction.OriginalBL.ToUpper();
                newshipmentinstruction.ShipmentOrderId = model.Id;
                newshipmentinstruction.ShipperAddress = String.IsNullOrEmpty(shipmentinstruction.ShipperAddress) ? "" : shipmentinstruction.ShipperAddress.ToUpper();
                newshipmentinstruction.ShipperId = shipmentinstruction.ShipperId;
                newshipmentinstruction.ShipperName = String.IsNullOrEmpty(shipmentinstruction.ShipperName) ? "" : shipmentinstruction.ShipperName.ToUpper();
                newshipmentinstruction.SIReference = String.IsNullOrEmpty(shipmentinstruction.SIReference) ? "" : shipmentinstruction.SIReference.ToUpper();
                newshipmentinstruction.SpecialInstruction = String.IsNullOrEmpty(shipmentinstruction.SpecialInstruction) ? "" : shipmentinstruction.SpecialInstruction.ToUpper();
                _shipmentInstructionService.CreateUpdateObject(newshipmentinstruction);
                /* ==================================================== END Shipping Instruction ==================================== */

                /* ==================================================== Shipment Advice ==================================== */
                ShipmentAdvice newShipmentAdvice = _shipmentAdviceService.GetObjectByShipmentOrderId(model.Id);
                if (newShipmentAdvice == null)
                {
                    newShipmentAdvice = new ShipmentAdvice();
                    newShipmentAdvice.CreatedAt = shipmentadvice.CreatedAt;
                    newShipmentAdvice.CreatedById = shipmentadvice.CreatedById;
                }
                newShipmentAdvice.OfficeId = shipmentadvice.OfficeId;
                newShipmentAdvice.UpdatedById = shipmentadvice.UpdatedById;
                newShipmentAdvice.UpdatedAt = DateTime.Now;
                newShipmentAdvice.Reference = String.IsNullOrEmpty(shipmentadvice.Reference) ? "" : shipmentadvice.Reference.ToUpper();
                newShipmentAdvice.Remarks = String.IsNullOrEmpty(shipmentadvice.Remarks) ? "" : shipmentadvice.Remarks.ToUpper();
                newShipmentAdvice.ShipmentOrderId = model.Id;
                _shipmentAdviceService.CreateUpdateObject(newShipmentAdvice);
                /* ==================================================== END Shipment Advice ==================================== */

                /* ==================================================== Telex Release ==================================== */
                TelexRelease newTelexRelease = _telexreleaseService.GetObjectByShipmentOrderId(model.Id);
                if (newTelexRelease == null)
                {
                    newTelexRelease = new TelexRelease();
                    newTelexRelease.CreatedAt = newTelexRelease.CreatedAt;
                    newTelexRelease.CreatedById = newTelexRelease.CreatedById;
                }
                newTelexRelease.OfficeId = newTelexRelease.OfficeId;
                newTelexRelease.UpdatedById = newTelexRelease.UpdatedById;
                newTelexRelease.UpdatedAt = DateTime.Now;
                newTelexRelease.Original = String.IsNullOrEmpty(newTelexRelease.Original) ? "" : newTelexRelease.Original.ToUpper();
                newTelexRelease.SeaWaybill = String.IsNullOrEmpty(newTelexRelease.SeaWaybill) ? "" : newTelexRelease.SeaWaybill.ToUpper();
                newTelexRelease.ShipmentOrderId = model.Id;
                _telexreleaseService.CreateUpdateObject(newTelexRelease);
                /* ==================================================== END Telex Release ==================================== */
            }
            return model;
        }

      
        public ShipmentOrder UpdateObjectAirImport(ShipmentOrder model, IContactService _contactService)
        { 
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = string.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.LoadStatus = model.LoadStatus;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.FreightStatus = model.FreightStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                // Agent, Delivery, Transhipment, Shipper, COnsignee, NParty
                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId;
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.TranshipmentId = model.TranshipmentId;
                shipmentorder.TranshipmentName = model.TranshipmentName.ToUpper();
                shipmentorder.TranshipmentAddress = String.IsNullOrEmpty(model.TranshipmentAddress) ? "" : model.TranshipmentAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.DepartureAirPortId = model.DepartureAirPortId;
                shipmentorder.DepartureAirPortName = String.IsNullOrEmpty(model.DepartureAirPortName) ? "" : model.DepartureAirPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DestinationAirPortId = model.DestinationAirPortId;
                shipmentorder.DestinationAirPortName = String.IsNullOrEmpty(model.DestinationAirPortName) ? "" : model.DestinationAirPortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                // Freight
                shipmentorder.MAWBStatus = model.MAWBStatus.ToUpper();
                shipmentorder.MAWBCollectId = model.MAWBStatus.ToUpper() == "P" ? 0 : model.MAWBCollectId;
                shipmentorder.MAWBPayableId = model.MAWBStatus.ToUpper() == "P" ? 0 : model.MAWBPayableId;
                shipmentorder.HAWBStatus = model.HAWBStatus.ToUpper();
                shipmentorder.HAWBCollectId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBCollectId;
                shipmentorder.HAWBPayableId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBPayableId;
                shipmentorder.Currency = model.Currency;
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();


                // Description
                shipmentorder.PiecesRCP = String.IsNullOrEmpty(model.PiecesRCP) ? "" : model.PiecesRCP.ToUpper();
                shipmentorder.GrossWeight = model.GrossWeight;
                shipmentorder.KGLB = String.IsNullOrEmpty(model.KGLB) ? "" : model.KGLB.ToUpper();
                shipmentorder.ChargeWeight = model.ChargeWeight;
                shipmentorder.ChargeRate = model.ChargeRate;
                shipmentorder.Total = String.IsNullOrEmpty(model.Total) ? "" : model.Total.ToUpper();
                shipmentorder.GoodNatureQuantity = String.IsNullOrEmpty(model.GoodNatureQuantity) ? "" : model.GoodNatureQuantity.ToUpper();
                shipmentorder.Shipmark = String.IsNullOrEmpty(model.Shipmark) ? "" : model.Shipmark.ToUpper();
                shipmentorder.Commodity = String.IsNullOrEmpty(model.Commodity) ? "" : model.Commodity.ToUpper();
                shipmentorder.PackagingCode = model.PackagingCode;

                shipmentorder.HAWBNo = String.IsNullOrEmpty(model.HAWBNo) ? "" : model.HAWBNo.ToUpper();
                shipmentorder.MAWBNo = String.IsNullOrEmpty(model.MAWBNo) ? "" : model.MAWBNo.ToUpper();
                shipmentorder.ChargeableWeight = model.ChargeableWeight;
                shipmentorder.WeightHAWB = model.WeightHAWB;
                shipmentorder.CarriageValue = String.IsNullOrEmpty(model.CarriageValue) ? "" : model.CarriageValue.ToUpper();
                shipmentorder.CustomValue = String.IsNullOrEmpty(model.CustomValue) ? "" : model.CustomValue.ToUpper();
                shipmentorder.IATAId = model.IATAId;
                shipmentorder.BrokerId = model.BrokerId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update SubShipmentOrder
                if (shipmentorder.SubJobNumber == 0)
                {
                    shipmentorder.ETD = model.ETD;
                    shipmentorder.ETA = model.ETA;
                    this.UpdateSubShipmentOrder(shipmentorder);
                }
                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirImport
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */

            }
            return model;
        }


        public ShipmentOrder UpdateObjectAirExport(ShipmentOrder model, BillOfLading billoflading, ShipmentInstruction shipmentinstruction,
             TelexRelease telexrelease, IContactService _contactService, IBillOfLadingService _billofladingService
            , IShipmentInstructionService _shipmentInstructionService, ITelexReleaseService _telexreleaseService)
        {
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = string.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.LoadStatus = model.LoadStatus;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.FreightStatus = model.FreightStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                // Agent, Delivery, Transhipment, Shipper, COnsignee, NParty
                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId;
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.DepartureAirPortId = model.DepartureAirPortId;
                shipmentorder.DepartureAirPortName = String.IsNullOrEmpty(model.DepartureAirPortName) ? "" : model.DepartureAirPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DestinationAirPortId = model.DestinationAirPortId;
                shipmentorder.DestinationAirPortName = String.IsNullOrEmpty(model.DestinationAirPortName) ? "" : model.DestinationAirPortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                // Freight
                shipmentorder.MAWBStatus = model.MAWBStatus.ToUpper();
                shipmentorder.MAWBCollectId = model.MAWBStatus.ToUpper() == "P" ? 0 : model.MAWBCollectId;
                shipmentorder.MAWBPayableId = model.MAWBStatus.ToUpper() == "P" ? 0 : model.MAWBPayableId;
                shipmentorder.HAWBStatus = model.HAWBStatus.ToUpper();
                shipmentorder.HAWBCollectId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBCollectId;
                shipmentorder.HAWBPayableId = model.HAWBStatus.ToUpper() == "P" ? 0 : model.HAWBPayableId;
                shipmentorder.Currency = model.Currency; 
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();


                  // Description
                shipmentorder.PiecesRCP = String.IsNullOrEmpty(model.PiecesRCP) ? "" : model.PiecesRCP.ToUpper();
                shipmentorder.GrossWeight = model.GrossWeight;
                shipmentorder.KGLB = String.IsNullOrEmpty(model.KGLB) ? "" : model.KGLB.ToUpper();
                shipmentorder.ChargeWeight = model.ChargeWeight;
                shipmentorder.ChargeRate = model.ChargeRate;
                shipmentorder.Total = String.IsNullOrEmpty(model.Total) ? "" : model.Total.ToUpper();
                shipmentorder.GoodNatureQuantity = String.IsNullOrEmpty(model.GoodNatureQuantity) ? "" : model.GoodNatureQuantity.ToUpper();
                shipmentorder.Shipmark = String.IsNullOrEmpty(model.Shipmark) ? "" : model.Shipmark.ToUpper();
                shipmentorder.Commodity = String.IsNullOrEmpty(model.Commodity) ? "" : model.Commodity.ToUpper();
                shipmentorder.PackagingCode = model.PackagingCode;

                shipmentorder.MAWBNo = String.IsNullOrEmpty(model.MAWBNo) ? "" : model.MAWBNo.ToUpper();
                shipmentorder.ChargeableWeight = model.ChargeableWeight;
                shipmentorder.WeightHAWB = model.WeightHAWB;
                shipmentorder.CarriageValue = String.IsNullOrEmpty(model.CarriageValue) ? "" : model.CarriageValue.ToUpper();
                shipmentorder.CustomValue = String.IsNullOrEmpty(model.CustomValue) ? "" : model.CustomValue.ToUpper();
                shipmentorder.IATAId = model.IATAId;
                shipmentorder.BrokerId = model.BrokerId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update SubShipmentOrder
                if (shipmentorder.SubJobNumber == 0)
                {
                    shipmentorder.ETD = model.ETD;
                    shipmentorder.ETA = model.ETA;
                    this.UpdateSubShipmentOrder(shipmentorder);
                }
                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirExport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirExport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirExport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirExport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirExport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.AirExport
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */

                /* ==================================================== Bill Of Lading ==================================== */
                BillOfLading newBillOfLading = _billofladingService.GetObjectByShipmentOrderId(model.Id);
                if (newBillOfLading == null)
                {
                    newBillOfLading = new BillOfLading();
                    newBillOfLading.CreatedById = billoflading.CreatedById;
                }
                newBillOfLading.AgentId = billoflading.AgentId;
                newBillOfLading.AgentName = String.IsNullOrEmpty(billoflading.AgentName) ? "" : billoflading.AgentName.ToUpper();
                newBillOfLading.AgentAddress = String.IsNullOrEmpty(billoflading.AgentAddress) ? "" : billoflading.AgentAddress.ToUpper();
                newBillOfLading.AmountInsurance = String.IsNullOrEmpty(billoflading.AmountInsurance) ? "" : billoflading.AmountInsurance.ToUpper();
                newBillOfLading.BLNumber = String.IsNullOrEmpty(billoflading.BLNumber) ? "" : billoflading.BLNumber.ToUpper();
                newBillOfLading.CargoInsurance = String.IsNullOrEmpty(billoflading.CargoInsurance) ? "" : billoflading.CargoInsurance.ToUpper();
                newBillOfLading.OfficeId = billoflading.OfficeId;
                newBillOfLading.ConsigneeAddress = String.IsNullOrEmpty(billoflading.ConsigneeAddress) ? "" : billoflading.ConsigneeAddress.ToUpper();
                newBillOfLading.ConsigneeId = billoflading.ConsigneeId;
                newBillOfLading.ConsigneeName = String.IsNullOrEmpty(billoflading.ConsigneeName) ? "" : billoflading.ConsigneeName.ToUpper();
                newBillOfLading.Descriptions = String.IsNullOrEmpty(billoflading.Descriptions) ? "" : billoflading.Descriptions.ToUpper();
                newBillOfLading.FreightAmount = String.IsNullOrEmpty(billoflading.FreightAmount) ? "" : billoflading.FreightAmount.ToUpper();
                newBillOfLading.FreightPayableAt = String.IsNullOrEmpty(billoflading.FreightPayableAt) ? "" : billoflading.FreightPayableAt.ToUpper();
                newBillOfLading.HAWBFee = decimal.Parse(billoflading.HAWBFee.ToString());   
                newBillOfLading.MasterBLId = billoflading.MasterBLId;
                newBillOfLading.UpdatedById = billoflading.UpdatedById;
                newBillOfLading.UpdatedAt = DateTime.Now;
                newBillOfLading.NoOfBL = String.IsNullOrEmpty(billoflading.NoOfBL) ? "" : billoflading.NoOfBL.ToUpper();
                newBillOfLading.NPartyAddress = String.IsNullOrEmpty(billoflading.NPartyAddress) ? "" : billoflading.NPartyAddress.ToUpper();
                newBillOfLading.NPartyId = billoflading.NPartyId;
                newBillOfLading.NPartyName = String.IsNullOrEmpty(billoflading.NPartyName) ? "" : billoflading.NPartyName.ToUpper();
                newBillOfLading.PlaceDateOfIssue = billoflading.PlaceDateOfIssue;
                newBillOfLading.ShipmentOrderId = model.Id;
                newBillOfLading.ShipmentOnBoard = billoflading.ShipmentOnBoard;
                newBillOfLading.ShipperAddress = String.IsNullOrEmpty(billoflading.ShipperAddress) ? "" : billoflading.ShipperAddress.ToUpper();
                newBillOfLading.ShipperId = billoflading.ShipperId;
                newBillOfLading.ShipperName = String.IsNullOrEmpty(billoflading.ShipperName) ? "" : billoflading.ShipperName.ToUpper();
                newBillOfLading.TotalNoOfContainer = String.IsNullOrEmpty(billoflading.TotalNoOfContainer) ? "" : billoflading.TotalNoOfContainer.ToUpper();
                _billofladingService.CreateUpdateObject(newBillOfLading);
                /* ==================================================== END Bill Of Lading ==================================== */

                /* ==================================================== Shipping Instruction ==================================== */

                ShipmentInstruction newshipmentinstruction = _shipmentInstructionService.GetObjectByShipmentOrderId(model.Id);
                if (newshipmentinstruction == null)
                {
                    newshipmentinstruction = new ShipmentInstruction();
                    newshipmentinstruction.CreatedById = shipmentinstruction.CreatedById;
                }
                newshipmentinstruction.Attention = String.IsNullOrEmpty(shipmentinstruction.Attention) ? "" : shipmentinstruction.Attention.ToUpper();
                newshipmentinstruction.CollectAddress = String.IsNullOrEmpty(shipmentinstruction.CollectAddress) ? "" : shipmentinstruction.CollectAddress.ToUpper();
                newshipmentinstruction.CollectAt = shipmentinstruction.CollectAt;
                newshipmentinstruction.CollectName = String.IsNullOrEmpty(shipmentinstruction.CollectName) ? "" : shipmentinstruction.CollectName.ToUpper();
                newshipmentinstruction.OfficeId = shipmentinstruction.OfficeId;
                newshipmentinstruction.ConsigneeAddress = String.IsNullOrEmpty(shipmentinstruction.ConsigneeAddress) ? "" : shipmentinstruction.ConsigneeAddress.ToUpper();
                newshipmentinstruction.ConsigneeId = shipmentinstruction.ConsigneeId;
                newshipmentinstruction.ConsigneeName = String.IsNullOrEmpty(shipmentinstruction.ConsigneeName) ? "" : shipmentinstruction.ConsigneeName.ToUpper();
                newshipmentinstruction.FreightAgreed = String.IsNullOrEmpty(shipmentinstruction.FreightAgreed) ? "" : shipmentinstruction.FreightAgreed.ToUpper();
                newshipmentinstruction.GoodDescription = String.IsNullOrEmpty(shipmentinstruction.GoodDescription) ? "" : shipmentinstruction.GoodDescription.ToUpper();
                newshipmentinstruction.UpdatedById = shipmentinstruction.UpdatedById;
                newshipmentinstruction.UpdatedAt = DateTime.Now;
                newshipmentinstruction.NPartyAddress = String.IsNullOrEmpty(shipmentinstruction.NPartyAddress) ? "" : shipmentinstruction.NPartyAddress.ToUpper();
                newshipmentinstruction.NPartyId = shipmentinstruction.NPartyId;
                newshipmentinstruction.NPartyName = String.IsNullOrEmpty(shipmentinstruction.NPartyName) ? "" : shipmentinstruction.NPartyName.ToUpper();
                newshipmentinstruction.OriginalBL = String.IsNullOrEmpty(shipmentinstruction.OriginalBL) ? "" : shipmentinstruction.OriginalBL.ToUpper();
                newshipmentinstruction.ShipmentOrderId = model.Id;
                newshipmentinstruction.ShipperAddress = String.IsNullOrEmpty(shipmentinstruction.ShipperAddress) ? "" : shipmentinstruction.ShipperAddress.ToUpper();
                newshipmentinstruction.ShipperId = shipmentinstruction.ShipperId;
                newshipmentinstruction.ShipperName = String.IsNullOrEmpty(shipmentinstruction.ShipperName) ? "" : shipmentinstruction.ShipperName.ToUpper();
                newshipmentinstruction.SIReference = String.IsNullOrEmpty(shipmentinstruction.SIReference) ? "" : shipmentinstruction.SIReference.ToUpper();
                newshipmentinstruction.SpecialInstruction = String.IsNullOrEmpty(shipmentinstruction.SpecialInstruction) ? "" : shipmentinstruction.SpecialInstruction.ToUpper();

                // Description
                newshipmentinstruction.PiecesRCP = String.IsNullOrEmpty(shipmentinstruction.PiecesRCP) ? "" : shipmentinstruction.PiecesRCP.ToUpper();
                newshipmentinstruction.GrossWeight2 = shipmentinstruction.GrossWeight2;
                newshipmentinstruction.KGLB = String.IsNullOrEmpty(shipmentinstruction.KGLB) ? "" : shipmentinstruction.KGLB.ToUpper();
                newshipmentinstruction.ChargeWeight = shipmentinstruction.ChargeWeight;
                newshipmentinstruction.ChargeRate = shipmentinstruction.ChargeRate;
                newshipmentinstruction.Total = String.IsNullOrEmpty(shipmentinstruction.Total) ? "" : shipmentinstruction.Total.ToUpper();
                newshipmentinstruction.GoodNatureQuantity = String.IsNullOrEmpty(shipmentinstruction.GoodNatureQuantity) ? "" : shipmentinstruction.GoodNatureQuantity.ToUpper();

                _shipmentInstructionService.CreateUpdateObject(newshipmentinstruction);
                /* ==================================================== END Shipping Instruction ==================================== */

                /* ==================================================== Telex Release ==================================== */
                TelexRelease newTelexRelease = _telexreleaseService.GetObjectByShipmentOrderId(model.Id);
                if (newTelexRelease == null)
                {
                    newTelexRelease = new TelexRelease();
                    newTelexRelease.CreatedAt = newTelexRelease.CreatedAt;
                    newTelexRelease.CreatedById = newTelexRelease.CreatedById;
                }
                newTelexRelease.OfficeId = newTelexRelease.OfficeId;
                newTelexRelease.UpdatedById = newTelexRelease.UpdatedById;
                newTelexRelease.UpdatedAt = DateTime.Now;
                newTelexRelease.Original = String.IsNullOrEmpty(newTelexRelease.Original) ? "" : newTelexRelease.Original.ToUpper();
                newTelexRelease.SeaWaybill = String.IsNullOrEmpty(newTelexRelease.SeaWaybill) ? "" : newTelexRelease.SeaWaybill.ToUpper();
                newTelexRelease.ShipmentOrderId = model.Id;
                _telexreleaseService.CreateUpdateObject(newTelexRelease);
                /* ==================================================== END Telex Release ==================================== */
            }
            return model;
        }

        public ShipmentOrder UpdateObjectSeaExport(ShipmentOrder model,BillOfLading billoflading,ShipmentInstruction shipmentinstruction,
            ShipmentAdvice shipmentadvice,TelexRelease telexrelease, IContactService _contactService,IBillOfLadingService _billofladingService
            ,IShipmentInstructionService _shipmentInstructionService,IShipmentAdviceService _shipmentAdviceService,ITelexReleaseService _telexreleaseService)
        {
            if (!isValid(_validator.VUpdateObjectSeaExport(model, this, _contactService)))
            { 
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */
               
                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = string.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.LoadStatus = model.LoadStatus;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.ContainerStatus = model.ContainerStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                shipmentorder.AgentId = model.AgentId; 
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId; 
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.TranshipmentId = model.TranshipmentId;
                shipmentorder.TranshipmentName = model.TranshipmentName.ToUpper();
                shipmentorder.TranshipmentAddress = String.IsNullOrEmpty(model.TranshipmentAddress) ? "" : model.TranshipmentAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId; 
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.LoadingPortId = model.LoadingPortId;
                shipmentorder.LoadingPortName = String.IsNullOrEmpty(model.LoadingPortName) ? "" : model.LoadingPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DischargePortId = model.DischargePortId;
                shipmentorder.DischargePortName = String.IsNullOrEmpty(model.DischargePortName) ? "" : model.DischargePortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();
               
                // Freight
                shipmentorder.OBLStatus = model.OBLStatus.ToUpper(); 
                shipmentorder.OBLCollectId = model.OBLStatus.ToUpper() == "P" ? 0 : model.OBLCollectId;
                shipmentorder.OBLPayableId = model.OBLStatus.ToUpper() == "P" ? 0 : model.OBLPayableId;
                shipmentorder.HBLStatus = model.HBLStatus.ToUpper();
                shipmentorder.HBLCollectId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLCollectId;
                shipmentorder.HBLPayableId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLPayableId;
                shipmentorder.Currency = model.Currency;
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();

                // Description
                shipmentorder.GoodDescription = String.IsNullOrEmpty(model.GoodDescription) ? "" : model.GoodDescription.ToUpper();

                shipmentorder.OceanMSTBLNo = String.IsNullOrEmpty(model.OceanMSTBLNo) ? "" : model.OceanMSTBLNo.ToUpper();
                shipmentorder.VolumeBL = model.VolumeBL;
                shipmentorder.VolumeInvoice = model.VolumeInvoice;
                shipmentorder.SSLineId = model.SSLineId;
                shipmentorder.BrokerId = model.BrokerId;
                shipmentorder.DepoId = model.DepoId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update SubShipmentOrder
                if (shipmentorder.SubJobNumber == 0)
                {
                    shipmentorder.ETD = model.ETD;
                    shipmentorder.ETA = model.ETA;
                    this.UpdateSubShipmentOrder(shipmentorder);
                }
                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                { 
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaExport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaExport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaExport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaExport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaExport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaExport
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */
                    
                /* ==================================================== Bill Of Lading ==================================== */
                    BillOfLading newBillOfLading = _billofladingService.GetObjectByShipmentOrderId(model.Id);
                    if (newBillOfLading == null)
                    {
                        newBillOfLading = new BillOfLading();
                        newBillOfLading.CreatedById = billoflading.CreatedById;
                    }
                    newBillOfLading.AgentId = billoflading.AgentId;
                    newBillOfLading.AgentName = String.IsNullOrEmpty(billoflading.AgentName) ? "" : billoflading.AgentName.ToUpper();
                    newBillOfLading.AgentAddress = String.IsNullOrEmpty(billoflading.AgentAddress) ? "" : billoflading.AgentAddress.ToUpper();
                    newBillOfLading.AmountInsurance = String.IsNullOrEmpty(billoflading.AmountInsurance) ? "" : billoflading.AmountInsurance.ToUpper();
                    newBillOfLading.BLNumber = String.IsNullOrEmpty(billoflading.BLNumber) ? "" : billoflading.BLNumber.ToUpper();
                    newBillOfLading.CargoInsurance = String.IsNullOrEmpty(billoflading.CargoInsurance) ? "" : billoflading.CargoInsurance.ToUpper();
                    newBillOfLading.OfficeId = billoflading.OfficeId;
                    newBillOfLading.ConsigneeAddress = String.IsNullOrEmpty(billoflading.ConsigneeAddress) ? "" : billoflading.ConsigneeAddress.ToUpper();
                    newBillOfLading.ConsigneeId = billoflading.ConsigneeId;
                    newBillOfLading.ConsigneeName = String.IsNullOrEmpty(billoflading.ConsigneeName) ? "" : billoflading.ConsigneeName.ToUpper();
                    newBillOfLading.Descriptions = String.IsNullOrEmpty(billoflading.Descriptions) ? "" : billoflading.Descriptions.ToUpper();
                    newBillOfLading.FreightAmount = String.IsNullOrEmpty(billoflading.FreightAmount) ? "" : billoflading.FreightAmount.ToUpper();
                    newBillOfLading.FreightPayableAt = String.IsNullOrEmpty(billoflading.FreightPayableAt) ? "" : billoflading.FreightPayableAt.ToUpper();
                    newBillOfLading.HAWBFee = decimal.Parse(billoflading.HAWBFee.ToString());
                    newBillOfLading.MasterBLId = billoflading.MasterBLId;
                    newBillOfLading.UpdatedById = billoflading.UpdatedById;
                    newBillOfLading.UpdatedAt = DateTime.Now;
                    newBillOfLading.NoOfBL = String.IsNullOrEmpty(billoflading.NoOfBL) ? "" : billoflading.NoOfBL.ToUpper();
                    newBillOfLading.NPartyAddress = String.IsNullOrEmpty(billoflading.NPartyAddress) ? "" : billoflading.NPartyAddress.ToUpper();
                    newBillOfLading.NPartyId = billoflading.NPartyId;
                    newBillOfLading.NPartyName = String.IsNullOrEmpty(billoflading.NPartyName) ? "" : billoflading.NPartyName.ToUpper();
                    newBillOfLading.PlaceDateOfIssue = billoflading.PlaceDateOfIssue;
                    newBillOfLading.ShipmentOrderId = model.Id;
                    newBillOfLading.ShipmentOnBoard = billoflading.ShipmentOnBoard;
                    newBillOfLading.ShipperAddress = String.IsNullOrEmpty(billoflading.ShipperAddress) ? "" : billoflading.ShipperAddress.ToUpper();
                    newBillOfLading.ShipperId = billoflading.ShipperId;
                    newBillOfLading.ShipperName = String.IsNullOrEmpty(billoflading.ShipperName) ? "" : billoflading.ShipperName.ToUpper();
                    newBillOfLading.TotalNoOfContainer = String.IsNullOrEmpty(billoflading.TotalNoOfContainer) ? "" : billoflading.TotalNoOfContainer.ToUpper();
                    _billofladingService.CreateUpdateObject(newBillOfLading);
                    /* ==================================================== END Bill Of Lading ==================================== */

                    /* ==================================================== Shipping Instruction ==================================== */

                    ShipmentInstruction newshipmentinstruction = _shipmentInstructionService.GetObjectByShipmentOrderId(model.Id);
                    if (newshipmentinstruction == null)
                    {
                        newshipmentinstruction = new ShipmentInstruction();
                        newshipmentinstruction.CreatedById = shipmentinstruction.CreatedById;
                    }
                    newshipmentinstruction.Attention = String.IsNullOrEmpty(shipmentinstruction.Attention) ? "" : shipmentinstruction.Attention.ToUpper();
                    newshipmentinstruction.CollectAddress = String.IsNullOrEmpty(shipmentinstruction.CollectAddress) ? "" : shipmentinstruction.CollectAddress.ToUpper();
                    newshipmentinstruction.CollectAt = shipmentinstruction.CollectAt;
                    newshipmentinstruction.CollectName = String.IsNullOrEmpty(shipmentinstruction.CollectName) ? "" : shipmentinstruction.CollectName.ToUpper();
                    newshipmentinstruction.OfficeId = shipmentinstruction.OfficeId;
                    newshipmentinstruction.ConsigneeAddress = String.IsNullOrEmpty(shipmentinstruction.ConsigneeAddress) ? "" : shipmentinstruction.ConsigneeAddress.ToUpper();
                    newshipmentinstruction.ConsigneeId = shipmentinstruction.ConsigneeId;
                    newshipmentinstruction.ConsigneeName = String.IsNullOrEmpty(shipmentinstruction.ConsigneeName) ? "" : shipmentinstruction.ConsigneeName.ToUpper();
                    newshipmentinstruction.FreightAgreed = String.IsNullOrEmpty(shipmentinstruction.FreightAgreed) ? "" : shipmentinstruction.FreightAgreed.ToUpper();
                    newshipmentinstruction.GoodDescription = String.IsNullOrEmpty(shipmentinstruction.GoodDescription) ? "" : shipmentinstruction.GoodDescription.ToUpper();
                    newshipmentinstruction.UpdatedById = shipmentinstruction.UpdatedById;
                    newshipmentinstruction.UpdatedAt = DateTime.Now;
                    newshipmentinstruction.NPartyAddress = String.IsNullOrEmpty(shipmentinstruction.NPartyAddress) ? "" : shipmentinstruction.NPartyAddress.ToUpper();
                    newshipmentinstruction.NPartyId = shipmentinstruction.NPartyId;
                    newshipmentinstruction.NPartyName = String.IsNullOrEmpty(shipmentinstruction.NPartyName) ? "" : shipmentinstruction.NPartyName.ToUpper();
                    newshipmentinstruction.OriginalBL = String.IsNullOrEmpty(shipmentinstruction.OriginalBL) ? "" : shipmentinstruction.OriginalBL.ToUpper();
                    newshipmentinstruction.ShipmentOrderId = model.Id;
                    newshipmentinstruction.ShipperAddress = String.IsNullOrEmpty(shipmentinstruction.ShipperAddress) ? "" : shipmentinstruction.ShipperAddress.ToUpper();
                    newshipmentinstruction.ShipperId = shipmentinstruction.ShipperId;
                    newshipmentinstruction.ShipperName = String.IsNullOrEmpty(shipmentinstruction.ShipperName) ? "" : shipmentinstruction.ShipperName.ToUpper();
                    newshipmentinstruction.SIReference = String.IsNullOrEmpty(shipmentinstruction.SIReference) ? "" : shipmentinstruction.SIReference.ToUpper();
                    newshipmentinstruction.SpecialInstruction = String.IsNullOrEmpty(shipmentinstruction.SpecialInstruction) ? "" : shipmentinstruction.SpecialInstruction.ToUpper();
                    _shipmentInstructionService.CreateUpdateObject(newshipmentinstruction);
                    /* ==================================================== END Shipping Instruction ==================================== */

                    /* ==================================================== Shipment Advice ==================================== */
                    ShipmentAdvice newShipmentAdvice = _shipmentAdviceService.GetObjectByShipmentOrderId(model.Id);
                    if (newShipmentAdvice == null)
                    {
                        newShipmentAdvice = new ShipmentAdvice();
                        newShipmentAdvice.CreatedAt = shipmentadvice.CreatedAt;
                        newShipmentAdvice.CreatedById = shipmentadvice.CreatedById;
                    }
                    newShipmentAdvice.OfficeId = shipmentadvice.OfficeId ;
                    newShipmentAdvice.UpdatedById =shipmentadvice.UpdatedById;
                    newShipmentAdvice.UpdatedAt = DateTime.Now;
                    newShipmentAdvice.Reference = String.IsNullOrEmpty(shipmentadvice.Reference) ? "" : shipmentadvice.Reference.ToUpper();
                    newShipmentAdvice.Remarks = String.IsNullOrEmpty(shipmentadvice.Remarks) ? "" : shipmentadvice.Remarks.ToUpper();
                    newShipmentAdvice.ShipmentOrderId = model.Id;
                    _shipmentAdviceService.CreateUpdateObject(newShipmentAdvice);
                    /* ==================================================== END Shipment Advice ==================================== */

                    /* ==================================================== Telex Release ==================================== */
                    TelexRelease newTelexRelease = _telexreleaseService.GetObjectByShipmentOrderId(model.Id);
                    if (newTelexRelease == null)
                    {
                        newTelexRelease = new TelexRelease();
                        newTelexRelease.CreatedAt = newTelexRelease.CreatedAt;
                        newTelexRelease.CreatedById = newTelexRelease.CreatedById;
                    }
                    newTelexRelease.OfficeId = newTelexRelease.OfficeId;
                    newTelexRelease.UpdatedById = newTelexRelease.UpdatedById;
                    newTelexRelease.UpdatedAt = DateTime.Now;
                    newTelexRelease.Original = String.IsNullOrEmpty(newTelexRelease.Original) ? "" : newTelexRelease.Original.ToUpper();
                    newTelexRelease.SeaWaybill = String.IsNullOrEmpty(newTelexRelease.SeaWaybill) ? "" : newTelexRelease.SeaWaybill.ToUpper();
                    newTelexRelease.ShipmentOrderId = model.Id;
                    _telexreleaseService.CreateUpdateObject(newTelexRelease);
                    /* ==================================================== END Telex Release ==================================== */
            }
            return model;
        }

        public ShipmentOrder UpdateObjectSeaImport(ShipmentOrder model, IContactService _contactService)
        {
            if (!isValid(_validator.VUpdateObject(model, this, _contactService)))
            {
                ShipmentOrder shipmentorder = this.GetObjectById(model.Id);
                int oldAgentId = shipmentorder.AgentId.HasValue ? shipmentorder.AgentId.Value : 0;
                int oldShipperId = shipmentorder.ShipperId.HasValue ? shipmentorder.ShipperId.Value : 0;
                int oldConsigneeId = shipmentorder.ConsigneeId.HasValue ? shipmentorder.ConsigneeId.Value : 0;
                int oldSSLineId = shipmentorder.SSLineId.HasValue ? shipmentorder.SSLineId.Value : 0;
                int oldEMKLId = shipmentorder.BrokerId.HasValue ? shipmentorder.BrokerId.Value : 0;
                int oldDepoId = shipmentorder.DepoId.HasValue ? shipmentorder.DepoId.Value : 0;

                /* ==================================================== Shipment Order ==================================== */

                shipmentorder.Id = model.Id;
                shipmentorder.JobId = model.JobId;
                shipmentorder.SIReference = String.IsNullOrEmpty(model.SIReference) ? "" : model.SIReference.ToUpper();
                shipmentorder.SIDate = model.SIDate;
                shipmentorder.ServiceNoID = model.ServiceNoID;
                shipmentorder.JobStatus = model.JobStatus;
                shipmentorder.FreightStatus = model.FreightStatus;
                shipmentorder.ShipmentStatus = model.ShipmentStatus;
                shipmentorder.GoodRecDate = model.GoodRecDate;
                shipmentorder.MarketId = model.MarketId;
                shipmentorder.MarketCompanyId = model.MarketCompanyId;

                shipmentorder.AgentId = model.AgentId;
                shipmentorder.AgentName = model.AgentName.ToUpper();
                shipmentorder.AgentAddress = String.IsNullOrEmpty(model.AgentAddress) ? "" : model.AgentAddress.ToUpper();
                shipmentorder.DeliveryId = model.DeliveryId;
                shipmentorder.DeliveryName = model.DeliveryName.ToUpper();
                shipmentorder.DeliveryAddress = String.IsNullOrEmpty(model.DeliveryAddress) ? "" : model.DeliveryAddress.ToUpper();
                shipmentorder.TranshipmentId = model.TranshipmentId;
                shipmentorder.TranshipmentName = model.TranshipmentName.ToUpper();
                shipmentorder.TranshipmentAddress = String.IsNullOrEmpty(model.TranshipmentAddress) ? "" : model.TranshipmentAddress.ToUpper();
                shipmentorder.ShipperId = model.ShipperId;
                shipmentorder.ShipperName = model.ShipperName.ToUpper();
                shipmentorder.ShipperAddress = String.IsNullOrEmpty(model.ShipperAddress) ? "" : model.ShipperAddress.ToUpper();
                shipmentorder.ConsigneeId = model.ConsigneeId;
                shipmentorder.ConsigneeName = model.ConsigneeName.ToUpper();
                shipmentorder.ConsigneeAddress = String.IsNullOrEmpty(model.ConsigneeAddress) ? "" : model.ConsigneeAddress.ToUpper();
                shipmentorder.NPartyId = model.NPartyId;
                shipmentorder.NPartyName = String.IsNullOrEmpty(model.NPartyName) ? "" : model.NPartyName.ToUpper();
                shipmentorder.NPartyAddress = String.IsNullOrEmpty(model.NPartyAddress) ? "" : model.NPartyAddress.ToUpper();

                shipmentorder.LoadingPortId = model.LoadingPortId;
                shipmentorder.LoadingPortName = String.IsNullOrEmpty(model.LoadingPortName) ? "" : model.LoadingPortName.ToUpper();
                shipmentorder.ReceiptPlaceId = model.ReceiptPlaceId;
                shipmentorder.ReceiptPlaceName = String.IsNullOrEmpty(model.ReceiptPlaceName) ? "" : model.ReceiptPlaceName.ToUpper();
                shipmentorder.DischargePortId = model.DischargePortId;
                shipmentorder.DischargePortName = String.IsNullOrEmpty(model.DischargePortName) ? "" : model.DischargePortName.ToUpper();
                shipmentorder.DeliveryPlaceId = model.DeliveryPlaceId;
                shipmentorder.DeliveryPlaceName = String.IsNullOrEmpty(model.DeliveryPlaceName) ? "" : model.DeliveryPlaceName.ToUpper();

                // Freight
                shipmentorder.OBLStatus = model.OBLStatus.ToUpper();
                shipmentorder.OBLCollectId = model.OBLStatus.ToUpper() == "P" ? 0 : model.OBLCollectId;
                shipmentorder.OBLPayableId = model.OBLStatus.ToUpper() == "P" ? 0 : model.OBLPayableId;
                shipmentorder.HBLStatus = model.HBLStatus.ToUpper();
                shipmentorder.HBLCollectId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLCollectId;
                shipmentorder.HBLPayableId = model.HBLStatus.ToUpper() == "P" ? 0 : model.HBLPayableId;
                shipmentorder.Currency = model.Currency;
                shipmentorder.HandlingInfo = String.IsNullOrEmpty(model.HandlingInfo) ? "" : model.HandlingInfo.ToUpper();
                shipmentorder.OBLCurrency = model.OBLCurrency;
                shipmentorder.HBLCurrency = model.HBLCurrency;
                shipmentorder.OBLAmount = model.OBLAmount;
                shipmentorder.HBLAmount = model.HBLAmount; 

                // Description
                shipmentorder.GoodDescription = String.IsNullOrEmpty(model.GoodDescription) ? "" : model.GoodDescription.ToUpper();

                shipmentorder.OceanMSTBLNo = String.IsNullOrEmpty(model.OceanMSTBLNo) ? "" : model.OceanMSTBLNo.ToUpper();
                shipmentorder.HouseBLNo = String.IsNullOrEmpty(model.HouseBLNo) ? "" : model.HouseBLNo.ToUpper();
                shipmentorder.SecondBLNo = String.IsNullOrEmpty(model.SecondBLNo) ? "" : model.SecondBLNo.ToUpper();
                shipmentorder.VolumeBL = model.VolumeBL;
                shipmentorder.VolumeInvoice = model.VolumeInvoice;
                shipmentorder.WareHouseName = String.IsNullOrEmpty(model.WareHouseName) ? "" : model.WareHouseName.ToUpper();
                shipmentorder.KINS = String.IsNullOrEmpty(model.KINS) ? "" : model.KINS.ToUpper();
                shipmentorder.CFName = String.IsNullOrEmpty(model.CFName) ? "" : model.CFName.ToUpper();
                shipmentorder.BrokerId = model.BrokerId;
                shipmentorder.DepoId = model.DepoId;

                shipmentorder.OfficeId = model.OfficeId;
                shipmentorder.UpdatedById = model.UpdatedById;
                shipmentorder.UpdatedAt = DateTime.Now;

                //Update SubShipmentOrder
                if (shipmentorder.SubJobNumber == 0)
                {
                    shipmentorder.ETD = model.ETD;
                    shipmentorder.ETA = model.ETA;
                    this.UpdateSubShipmentOrder(shipmentorder);
                }
                //Update LastShipment
                //Agent
                Contact agent = _contactService.GetObjectById(model.AgentId.Value);
                if (agent != null)
                {
                    agent.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(agent);
                }
                // Revert Last Shipment Date for Old Record
                if (oldAgentId != model.AgentId)
                {
                    Contact oldagent = _contactService.GetObjectById(oldAgentId);
                    if (oldagent != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaImport
                            && x.AgentId == oldAgentId).Max(x => x.ETD);
                        oldagent.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldagent);
                    }
                }

                //Shipper
                Contact shipper = _contactService.GetObjectById(model.ShipperId.Value);
                if (shipper != null)
                {
                    shipper.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(shipper);
                }
                // Revert Last Shipment Date for Old Record
                if (oldShipperId != model.ShipperId)
                {
                    Contact oldshipper = _contactService.GetObjectById(oldShipperId);
                    if (oldshipper != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaImport
                            && x.ShipperId == oldShipperId).Max(x => x.ETD);
                        oldshipper.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldshipper);
                    }
                }

                //Consignee
                Contact consignee = _contactService.GetObjectById(model.ConsigneeId.Value);
                if (consignee != null)
                {
                    consignee.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(consignee);
                }
                // Revert Last Shipment Date for Old Record
                if (oldConsigneeId != model.ConsigneeId)
                {
                    Contact oldconsignee = _contactService.GetObjectById(oldConsigneeId);
                    if (oldconsignee != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaImport
                            && x.ConsigneeId == oldConsigneeId).Max(x => x.ETD);
                        oldconsignee.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldconsignee);
                    }
                }

                //SSLine
                Contact ssline = _contactService.GetObjectById(model.SSLineId.Value);
                if (ssline != null)
                {
                    ssline.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(ssline);
                }
                // Revert Last Shipment Date for Old Record
                if (oldSSLineId != model.SSLineId)
                {
                    Contact oldssline = _contactService.GetObjectById(oldSSLineId);
                    if (oldssline != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaImport
                            && x.SSLineId == oldSSLineId).Max(x => x.ETD);
                        oldssline.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldssline);
                    }
                }

                //Broker
                Contact emkl = _contactService.GetObjectById(model.BrokerId.Value);
                if (emkl != null)
                {
                    emkl.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(emkl);
                }
                // Revert Last Shipment Date for Old Record
                if (oldEMKLId != model.BrokerId)
                {
                    Contact oldemkl = _contactService.GetObjectById(oldEMKLId);
                    if (oldemkl != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaImport
                            && x.BrokerId == oldEMKLId).Max(x => x.ETD);
                        oldemkl.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(oldemkl);
                    }
                }

                Contact depo = _contactService.GetObjectById(model.DepoId.Value);
                if (depo != null)
                {
                    depo.LastShipmentDate = model.ETD.Value;
                    _contactService.UpdateLastShipment(depo);
                }
                // Revert Last Shipment Date for Old Record
                if (oldDepoId != model.DepoId)
                {
                    Contact olddepo = _contactService.GetObjectById(oldDepoId);
                    if (olddepo != null)
                    {
                        var shipmentETD = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == MasterConstant.JobType.SeaImport
                            && x.DepoId == oldDepoId).Max(x => x.ETD);
                        olddepo.LastShipmentDate = shipmentETD == null ? null : shipmentETD;
                        _contactService.UpdateLastShipment(olddepo);
                    }
                }
                model = _repository.UpdateObject(shipmentorder);
                /* ==================================================== END Shipment Order ==================================== */
            }
            return model;
        }
 
        public ShipmentOrder UpdateSubShipmentOrder(ShipmentOrder shipmentorder)
        {
            IList<ShipmentOrder> subshipmentorder = _repository.GetQueryable().Where(x => x.OfficeId == shipmentorder.OfficeId && x.JobId == shipmentorder.JobId
                && x.JobNumber == shipmentorder.JobNumber && x.SubJobNumber > 0).ToList();
            foreach (var item in subshipmentorder)
            {
                ShipmentOrder subshipment = _repository.GetObjectById(item.Id);
                subshipment.ETA = shipmentorder.ETA;
                subshipment.ETD = shipmentorder.ETD;
                shipmentorder = _repository.UpdateObject(subshipment);
            }
            return shipmentorder;
        }

        public ShipmentOrder SoftDeleteObject(ShipmentOrder shipmentorder)
        {
            shipmentorder = _repository.SoftDeleteObject(shipmentorder);
            return shipmentorder;
        }

        public bool isValid(ShipmentOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
