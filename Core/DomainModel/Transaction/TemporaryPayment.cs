using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TemporaryPayment
    { 
        public int Id { get; set; }
        public int TPNo { get; set; }
        public int OfficeId { get; set; }
        public int MasterCode { get; set; }
        public int JobCode { get; set; }
        public Nullable<int> ContactId { get; set; }
        public Nullable<Decimal> TotalCashIDR { get; set; }
        public Nullable<Decimal> TotalCashUSD { get; set; }
        public Nullable<Decimal> TotalBankIDR { get; set; }
        public Nullable<Decimal> TotalBankUSD { get; set; }
        public Nullable<Decimal> Rate { get; set; }
        public Nullable<Decimal> ExRateId { get; set; }
        public Nullable<DateTime> ExRateDate { get; set; }
        public Nullable<int> Printing { get; set; }
        public Nullable<DateTime> PrintedOn { get; set; }
        public Nullable<bool> VerifyAcc { get; set; }
        public Nullable<DateTime> VerifyAccOn { get; set; }
        public Nullable<bool> PaidPV { get; set; }
        public string PaymentTo { get; set; }
        public Nullable<DateTime> TPDueDate { get; set; }
        public Nullable<int>JobOwnerId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } 
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> CostId { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Office Office { get; set; }
        public virtual Contact Contact { get; set; } 
        public virtual ICollection<TemporaryPaymentJob> TemporaryPaymentJob { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}
