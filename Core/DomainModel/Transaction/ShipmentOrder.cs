using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class ShipmentOrder
    {
        public int Id { get; set; }
        public int MasterCode { get; set; }  
        public int JobNumber { get; set; }
        public int SubJobNumber { get; set; }
        public int JobId { get; set; }
        public int OfficeId { get; set; } 
        public int TotalSub { get; set; }
        public string ShipmentOrderCode { get; set; }
        public string SIReference { get; set; }// PIB untuk PPJK
        public Nullable<DateTime> SIDate { get; set; }
        public string LoadStatus { get; set; }
        public string ContainerStatus { get; set; }
        public string FreightStatus { get; set; }
        public string ShipmentStatus { get; set; }
        public Nullable<int> ServiceNoID { get; set; }
        public Nullable<int> MarketId { get; set; }
        public Nullable<int> MarketCompanyId { get; set; }
        public string JobStatus { get; set; }
        public Nullable<DateTime> GoodRecDate { get; set; }
        public string Conversion { get; set; }
        public string QuotationNo { get; set; }
        public Nullable<int> AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentAddress { get; set; }
        public Nullable<int> DeliveryId { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<int> TranshipmentId { get; set; }
        public string TranshipmentName { get; set; }
        public string TranshipmentAddress { get; set; }
        public Nullable<int> ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }
        public Nullable<int> ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public Nullable<int> NPartyId { get; set; }
        public string NPartyName { get; set; }
        public string NPartyAddress { get; set; }
        public Nullable<int> ReceiptPlaceId { get; set; }
        public string ReceiptPlaceName { get; set; }
        public Nullable<int> LoadingPortId{ get; set; }
        public string LoadingPortName { get; set; }
        public Nullable<int> DischargePortId { get; set; }
        public string DischargePortName { get; set; }
        public Nullable<int> DeliveryPlaceId { get; set; }
        public string DeliveryPlaceName { get; set; }
        public Nullable<int> DepartureAirPortId { get; set; }
        public string DepartureAirPortName { get; set; }
        public Nullable<int> DestinationAirPortId { get; set; }
        public string DestinationAirPortName { get; set; }
        public string GoodDescription { get; set; }
        public string PiecesRCP { get; set; }
        public Nullable<decimal> GrossWeight { get; set; }
        public string KGLB { get; set; }
        public Nullable<decimal> ChargeWeight { get; set; }
        public Nullable<decimal> ChargeRate { get; set; }
        public string Total { get; set; }
        public string Commodity { get; set; }
        public string PackagingCode { get; set; }
        public string GoodNatureQuantity { get; set; }
        public string Shipmark { get; set; }
        public Nullable<decimal> ChargeableWeight { get; set; }
        public Nullable<decimal> WeightHAWB { get; set; }
        public string CarriageValue { get; set; }
        public string CustomValue { get; set; }
        public Nullable<DateTime> ETD { get; set; }
        public Nullable<DateTime> ETA { get; set; }
        public string OBLStatus { get; set; } 
        public Nullable<int> OBLCollectId { get; set; } 
        public Nullable<int> OBLPayableId { get; set; } 
        public Nullable<decimal> OBLAmount { get; set; } 
        public string OBLCurrency { get; set; } 
        public string HBLStatus { get; set; } 
        public Nullable<int> HBLCollectId { get; set; } 
        public Nullable<int> HBLPayableId { get; set; } 
        public string HBLCurrency { get; set; } 
        public Nullable<decimal> HBLAmount { get; set; } 
        public string MAWBStatus { get; set; } 
        public Nullable<int> MAWBCollectId { get; set; } 
        public Nullable<int> MAWBPayableId { get; set; } 
        public Nullable<decimal> MAWBRate { get; set; } 
        public string MAWBCurrency { get; set; } 
        public string HAWBStatus { get; set; } 
        public Nullable<int> HAWBCollectId { get; set; } 
        public Nullable<int> HAWBPayableId { get; set; } 
        public Nullable<decimal> HAWBRate { get; set; } 
        public string HAWBCurrency { get; set; } 
        public string Currency { get; set; } 
        public string HandlingInfo { get; set; } 
        public string HAWBNo { get; set; } 
        public string MAWBNo { get; set; } 
        public string OceanMSTBLNo { get; set; } 
        public string HouseBLNo { get; set; } 
        public string SecondBLNo { get; set; } 
        public Nullable<decimal> VolumeBL { get; set; } 
        public Nullable<decimal> VolumeInvoice { get; set; } 
        public string WareHouseName { get; set; } 
        public string KINS { get; set; } 
        public string CFName { get; set; } 
        public Nullable<int> SSLineId { get; set; } 
        public Nullable<int> IATAId { get; set; } 
        public Nullable<int> BrokerId { get; set; } 
        public Nullable<int> DepoId { get; set; } 
        public string VesselFlight { get; set; }
        public Nullable<bool> JobClosed { get; set; } 
        public Nullable<DateTime> JobClosedOn { get; set; }

        // PPJK
        public string InvoiceNo { get; set; }
        public string JobOrderPTP { get; set; }
        public string JobOrderCustomer { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }

        public virtual Office Office { get; set; }
        public virtual Contact Agent { get; set; }
        public virtual Contact Delivery { get; set; }
        public virtual Contact Transhipment { get; set; }
        public virtual Contact Shipper { get; set; }
        public virtual Contact Consignee { get; set; }
        public virtual Contact NParty { get; set; }
        public virtual CityLocation ReceiptPlace { get; set; }
        public virtual Port LoadingPort { get; set; }
        public virtual Port DischargePort { get; set; }
        public virtual CityLocation DeliveryPlace { get; set; }
        public virtual Airport DepartureAirPort { get; set; }
        public virtual Airport DestinationAirPort { get; set; }
        public virtual ICollection<ShipmentAdvice> ShipmentAdvice { get; set; }
        public virtual ICollection<ShipmentInstruction> ShipmentInstruction { get; set; }
        public virtual ICollection<DeliveryOrder> DeliveryOrder { get; set; }
        public virtual ICollection<NoticeOfArrival> NoticeOfArrival { get; set; }

        public virtual ICollection<ShipmentOrderRouting> ShipmentOrderRoutings { get; set; }
        public virtual ICollection<SeaContainer> SeaContainers { get; set; }


    }
}