using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class OfficialReceipt
    {
        public int Id { get; set; }
        public int ORNo { get; set; }
        public int OfficeId { get; set; }
        public string PaymentFor { get; set; }
        public string PaymentBy { get; set; }
        public int ContactId { get; set; }
        public Nullable<Decimal> Rate { get; set; }
        public Nullable<DateTime> ExRateDate { get; set; }
        public Nullable<Decimal> TotalCashUSD { get; set; }
        public Nullable<Decimal> TotalCashIDR { get; set; }
        public Nullable<Decimal> TotalBankUSD { get; set; }
        public Nullable<Decimal> TotalBankIDR { get; set; }
        public int Printing { get; set; }
        public Nullable<DateTime> PrintedOn { get; set; }
        public Nullable<bool> VerifyAcc { get; set; }
        public Nullable<DateTime> VerifyAccDate { get; set; }
        public Nullable<Decimal> Difference { get; set; }
        public Nullable<bool> SaveOR { get; set; }
        public Nullable<int> JobOwnerId { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<bool> RePrintApproved { get; set; }
        public Nullable<DateTime> RePrintApprovedOn { get; set; }
        public Nullable<DateTime> RePrintApprovedById { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<OfficialReceiptDetailInvoice> OfficialReceiptDetailInvoices { get; set; }
        
        public Dictionary<String, String> Errors { get; set; }
    }
}
