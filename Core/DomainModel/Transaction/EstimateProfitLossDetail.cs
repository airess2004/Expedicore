using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EstimateProfitLossDetail 
    {
        public int Id { get; set; } 
        public int EstimateProfitLossId { get; set; }
        public int OfficeId { get; set; } 
        public int Sequence { get; set; } 
        public Nullable<bool> IsIncome { get; set; } 
        public Nullable<int> CustomerId { get; set; } 
        public Nullable<int> CustumerTypeId { get; set; } 
        public Nullable<int> CostId { get; set; }  
        public string Description { get; set; } 
        public Nullable<bool> CodingQuantity { get; set; } 
        public Nullable<int> Type { get; set; } 
        public Nullable<decimal> Quantity { get; set; } 
        public Nullable<decimal> PerQty { get; set; } 
        public Nullable<int> AmountCrr { get; set; } 
        public Nullable<decimal> Amount { get; set; } 
        public Nullable<bool> Sign { get; set; } 
        public Nullable<bool> DataFrom { get; set; } 
        public Nullable<bool> StatusMemo { get; set; } 
        public Nullable<DateTime> CreatedMemoOn { get; set; } 
        public Nullable<bool> PaidPR { get; set; } 
        public bool IsSplitIncCost { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual Office Office { get; set; }
        public virtual Cost Cost { get; set; }
        public virtual EstimateProfitLoss EstimateProfitLoss { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }

    }
}