using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class OfficialReceiptDetailInvoice
    { 

        public int Id { get; set; } 
        public string OfficialReceiptId { get; set; }
        public int OfficeId { get; set; }
        public int InvoicesId { get; set; }
        public int MasterCode { get; set; }
        public Nullable<decimal> CashUSD { get; set; }
        public Nullable<decimal> CashIDR { get; set; }
        public Nullable<decimal> BankUSD { get; set; }
        public Nullable<decimal> BankIDR { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<DateTime> ExRateDate { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Office Office { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual OfficialReceipt OfficalReceipt { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}
