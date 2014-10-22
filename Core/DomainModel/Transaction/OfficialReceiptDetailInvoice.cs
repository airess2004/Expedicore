using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class OfficialReceiptDetailInvoice
    { 

        public int Id; 
        public string OfficialReceiptId;
        public int OfficeId;
        public int InvoicesId;
        public Nullable<decimal> CashUSD;
        public Nullable<decimal> CashIDR;
        public Nullable<decimal> BankUSD;
        public Nullable<decimal> BankIDR;
        public Nullable<decimal> Rate;
        public Nullable<DateTime> ExRateDate;

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual OfficialReceipt OfficalReceipt { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}
