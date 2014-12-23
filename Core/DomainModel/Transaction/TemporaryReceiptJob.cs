using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel 
{
    public partial class TemporaryReceiptJob
    { 
        public int Id { get; set; } 
        public int TemporaryReceiptId { get; set; }
        public int MasterCode { get; set; }
        public Nullable<Decimal> CashUSD { get; set; }
        public Nullable<Decimal> CashIDR { get; set; } 
        public Nullable<Decimal> BankUSD { get; set; }
        public Nullable<Decimal> BankIDR { get; set; }
        public Nullable<bool> PaidUSD { get; set; }
        public Nullable<bool> PaidIDR { get; set; }
        public Nullable<bool> SaveOR { get; set; } 
        public Nullable<int> ShipmentOrderId{ get; set; } 
        public string ShipmentNo { get; set; }
        public int OfficeId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; } 
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Office Office { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public virtual AccountUser CreateBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual TemporaryReceipt TemporaryReceipt { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
