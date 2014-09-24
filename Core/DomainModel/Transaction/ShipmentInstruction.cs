using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class ShipmentInstruction
    { 
        public int Id { get; set; }

        public int ShipmentOrderId { get; set; }
        public int OfficeId { get; set; }
        public string SIReference { get; set; }
        public string Attention { get; set; }
        public Nullable<int> ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }
        public Nullable<int> ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public Nullable<int> NPartyId { get; set; }
        public string NPartyName { get; set; }
        public string NPartyAddress { get; set; }
        public string GoodDescription { get; set; }
        public Nullable<int> Updated { get; set; }
        public string SpecialInstruction { get; set; }
        public string OriginalBL { get; set; }
        public Nullable<int> CollectAt { get; set; } 
        public string FreightAgreed { get; set; }
        public string CollectName { get; set; }
        public string CollectAddress { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }
         
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public virtual Office Office { get; set; }
        public virtual Contact Shipper { get; set; }
        public virtual Contact Consignee { get; set; }
        public virtual Contact NParty { get; set; } 

    }
}
