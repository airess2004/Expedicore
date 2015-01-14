using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ReceiptVoucher
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int CashBankId { get; set; }
        public string Code { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal RateToIDR { get; set; }
        public bool IsGBCH { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public bool IsReconciled { get; set; }
        public Nullable<DateTime> ReconciliationDate { get; set; }
        public int OfficeId { get; set; }
        public int MasterCode { get; set; }
        public decimal Rate { get; set; }
        public Nullable<DateTime> ExRateDate { get; set; }
        public Nullable<int> ExRateId { get; set; }

        public decimal TotalAmountIDR { get; set; }
        public decimal TotalAmountUSD { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<ReceiptVoucherDetail> ReceiptVoucherDetails { get; set; }
        public virtual Office Office { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public Dictionary<String, String> Errors { get; set; }

    }
}
