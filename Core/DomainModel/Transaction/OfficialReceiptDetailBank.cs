using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class OfficialReceiptDetailBank
    { 
        public int Id { get; set; } 
        public Nullable<int> OfficialReceiptId { get; set; } 
        public int InvoicesId { get; set; }
        public int MasterCode { get; set; }
        public string BankCode { get; set; }
        public Nullable<int> AmountCrr { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public Nullable<bool> StatusDueDate { get; set; }
        public Nullable<int> AccountId { get; set; }
   
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual OfficialReceipt OfficialReceipt { get; set; }
        public virtual Invoice Invoice { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
