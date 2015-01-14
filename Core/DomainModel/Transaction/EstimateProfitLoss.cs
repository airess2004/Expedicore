using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EstimateProfitLoss 
    {
        public int Id { get; set; }
        public Nullable<int> EPLNo { get; set; }
        public int ShipmentOrderId { get; set; }
        public int OfficeId { get; set; }
        public int MasterCode { get; set; }
        public bool CloseEPL { get; set; }
        public Nullable<DateTime> DateClose { get; set; } 
        public Nullable<int> TimeCLoseEPL { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> TotalIncomeIDR { get; set; }
        public Nullable<decimal> TotalCostIDR { get; set; }
        public Nullable<decimal> TotalIncomeUSD { get; set; }
        public Nullable<decimal> TotalCostUSD { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public int Printing { get; set; } 
        public Nullable<DateTime> PrintDate { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public Nullable<int> ExchangeRateId { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual ShipmentOrder ShipmentOrder { get; set; }
       // public virtual ExchangeRate ExchangeRate { get; set; }

        public virtual Office Office { get; set; } 
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }  
        public virtual ICollection<EstimateProfitLossDetail> EstimateProfitLossDetails { get; set; }


    }
}