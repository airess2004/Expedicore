using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class InvoiceDetail
    { 
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string DebetCredit { get; set; }
        public int OfficeId { get; set; }
        public int MasterCode { get; set; }
        public Nullable<int> Sequence { get; set; }	
        public Nullable<int> CostId { get; set; }	
        public string Description { get; set; }	
        public Nullable<int> Type { get; set; }	
        public Nullable<bool> CodingQuantity { get; set; }	
        public Nullable<decimal> Quantity { get; set; }	
        public Nullable<decimal> PerQty { get; set; }	
        public Nullable<bool> Sign { get; set; }	
        public Nullable<int> AmountCrr { get; set; }
        public Nullable<decimal> Amount { get; set; }	
        public Nullable<decimal> PercentVat { get; set; }
        public Nullable<decimal> AmountVat { get; set; }	
        public Nullable<int> EPLDetailId { get; set; }	
        public int CreatedById { get; set; }	
        public DateTime CreatedAt { get; set; }	
        public Nullable<int> UpdatedById { get; set; }	
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> VatId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public Dictionary<string, string> Errors { get; set; }
         
        public virtual Office Office { get; set; }
        public virtual Cost Cost { get; set; }
        public virtual EstimateProfitLossDetail EPLDetail { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual Invoice Invoices { get; set; }

    }
}
