using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class CashAdvanceDetail
    {
        public int Id { get; set; }
        public int CashAdvanceId { get; set; }
        public int Sequence { get; set; }
        public int OfficeId { get; set; }
        public int MasterCode { get; set; }
        public string Description { get; set; }
        public decimal AmountUSD { get; set; } 
        public decimal AmountIDR { get; set; }
        public Nullable<int> PayableId { get; set; }
        public Nullable<int> ShipmentOrderId { get; set; }
        public string ShipmentNo { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; } 
        public virtual Payable Payable { get; set; }
        public virtual CashAdvance CashAdvance { get; set; }
        public virtual Office Office { get; set; }
        public Dictionary<String, String> Errors { get; set; }

    }
}
