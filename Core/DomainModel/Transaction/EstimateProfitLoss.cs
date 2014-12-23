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
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> EstIDRShipCons { get; set; }
        public Nullable<decimal> EstIDRAgent { get; set; }
        public Nullable<decimal> EstUSDShipCons { get; set; }
        public Nullable<decimal> EstUSDAgent { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public virtual Office Office { get; set; } 
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }  
        public virtual ICollection<EstimateProfitLossDetail> EstimateProfitLossDetails { get; set; }


    }
}