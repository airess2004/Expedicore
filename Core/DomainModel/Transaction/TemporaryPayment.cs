using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TemporaryPayment
    { 
        public int Id;
        public string TPNo;
        public int OfficeId;
        public int JobCode;
        public Nullable<int> ContactId;
        public Nullable<Decimal> TotalCashIDR;
        public Nullable<Decimal> TotalBankIDR;
        public Nullable<Decimal> Rate;

        public Nullable<Decimal> ExRateDate;
        public Nullable<int> Printing;
        public Nullable<DateTime> PrintedOn;
        public Nullable<bool> VerifyAcc;
        public Nullable<DateTime> VerifyAccOn;
        public Nullable<bool> PaidPV;
        public Nullable<string> PaymentTo;
        public Nullable<DateTime> TPDueDate;
        public Nullable<int>JobOwnerId;
        public int CreatedById;
        public DateTime CreatedAt; 
        public Nullable<int> IsDeleted;
        public Nullable<DateTime> DeletedAt;
        public Nullable<int> UpdatedById;
        public Nullable<DateTime> UpdatedAt;
        public Nullable<int> CostId;


        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
