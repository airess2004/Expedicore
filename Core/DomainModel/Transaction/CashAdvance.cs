using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class CashAdvance
    {
        public int Id { get; set; }
        public string CashBondNo { get; set; }
        public int OfficeId { get; set; }
         public string Reference { get; set; }
         public Nullable<bool> Approved { get; set; }
         public Nullable<DateTime> ApprovedAt { get; set; }
         public Nullable<int> EmployeeId { get; set; }
         public string CashBondTo { get; set; }
         public Nullable<decimal> CashBondIDR { get; set; }
         public Nullable<bool> Paid { get; set; }
         public Nullable<DateTime> PaidOn { get; set; }
         public Nullable<Decimal> Rate { get; set; }
         public Nullable<DateTime> ExRateDate { get; set; }
         public Nullable<int> ExRateId { get; set; }
         public Nullable<int> Printing { get; set; }
         public Nullable<DateTime> PrintedAt { get; set; }
         public DateTime CreatedAt { get; set; }
         public int CreatedById { get; set; }
         public Nullable<int> UpdatedById { get; set; }
         public Nullable<DateTime> UpdatedAt { get; set; }
         public Nullable<bool> IsDeleted { get; set; }
         public Nullable<DateTime> DeletedAt { get; set; }
         public bool IsConfirmed { get; set; }
         public Nullable<DateTime> ConfirmationDate { get; set; }
         
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; } 
        public virtual ICollection<CashAdvance> CashAdvanceDetails { get; set; }
    }
}
