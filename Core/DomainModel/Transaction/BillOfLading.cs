using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class BillOfLading
    {
        public int Id { get; set; } 
        public int ShipmentOrderId { get; set; }
        public Nullable<int> MasterBLId { get; set; }
        public int OfficeId { get; set; }
        public string BLNumber { get; set; }
        public string NoOfBL { get; set; }
        public Nullable<DateTime>  PlaceDateOfIssue { get; set; }
        public Nullable<int>  AgentId { get; set; }
        public string  AgentName { get; set; }
        public string  AgentAddress { get; set; }
        public Nullable<int>  ShipperId { get; set; }
        public string  ShipperName { get; set; }
        public string  ShipperAddress { get; set; }
        public Nullable<int>  ConsigneeId { get; set; }
        public string  ConsigneeName { get; set; }
        public string  ConsigneeAddress { get; set; }
        public Nullable<int>  NPartyId { get; set; }
        public string  NPartyName { get; set; }
        public string  NPartyAddress { get; set; }
        public Nullable<DateTime>  ShipmentOnBoard { get; set; } 
        public string  TotalNoOfContainer { get; set; } 
        public Nullable<decimal>  HAWBFee { get; set; } 
        public string  CargoInsurance { get; set; } 
        public string  AmountInsurance { get; set; } 
        public string  FreightAmount { get; set; } 
        public string  FreightPayableAt { get; set; } 
        public string  Descriptions { get; set; } 
        public Nullable<int>  PrintDraft { get; set; } 
        public Nullable<int>  PrintFixed { get; set; } 
        public Nullable<DateTime>  PrintFixedOn { get; set; } 
        public Nullable<DateTime>  FirstPrintFixedOn { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; } 
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }
         
        public virtual Office Office { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public virtual Contact Agent { get; set; }
        public virtual Contact Shipper { get; set; } 
        public virtual Contact Consignee { get; set; }
        public virtual Contact NParty { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
    }
}
