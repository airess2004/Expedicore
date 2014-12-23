using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel 
{
    public partial class TemporaryPaymentJob
    { 
        public int Id{ get; set; }
        public int TemporaryPaymentId{ get; set; }
        public int OfficeId{ get; set; }
        public int MasterCode { get; set; }
        public Nullable<Decimal> CashUSD { get; set; }
        public Nullable<Decimal> CashIDR { get; set; } 
        public Nullable<Decimal> BankUSD { get; set; }
        public Nullable<Decimal> BankIDR { get; set; }
        public Nullable<bool> PaidUSD { get; set; }
        public Nullable<bool> PaidIDR { get; set; }
        public Nullable<bool> SaveRV { get; set; }
        public Nullable<int> ShipmentOrderId{ get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; } 
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Office Office { get; set; }
        public virtual Contact CreateBy { get; set; }
        public virtual Contact UpdatedBy { get; set; }
        public virtual TemporaryPayment TemporaryPayment { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}
