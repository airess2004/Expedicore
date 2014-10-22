using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class InvoiceDetail
    { 
        public int Id;
        public int InvoiceId;
        public string DebetCredit;
        public int OfficeId;	
        public Nullable<int> Sequence;	
        public Nullable<int> CostId;	
        public string Description;	
        public Nullable<int> Type;	
        public Nullable<bool> CodingQuantity;	
        public Nullable<decimal> Quantity;	
        public Nullable<decimal> PerQty;	
        public Nullable<bool> Sign;	
        public Nullable<int> AmountCrr;
        public Nullable<decimal> Amount;	
        public Nullable<decimal> PercentVat;
        public Nullable<decimal> AmountVat;	
        public Nullable<int> EPLDetailId;	
        public Nullable<int> CreatedById;	
        public Nullable<DateTime> CreatedAt;	
        public Nullable<int> UpdatedById;	
        public Nullable<DateTime> UpdatedAt;
        public Nullable<int> VatId;

        public Dictionary<string, string> Errors { get; set; }
         
        public virtual Office Office { get; set; }
        public virtual Cost Cost { get; set; }
        public virtual EstimateProfitLossDetail EPLDetail { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual Invoice Invoices { get; set; }

    }
}
