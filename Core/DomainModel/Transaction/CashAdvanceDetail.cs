using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class CashAdvanceDetail
    {
        public int Id { get; set; }
        public int CashAdvanceId { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> AmountUSD { get; set; }
        public Nullable<decimal> AmountIDR { get; set; }
        public Nullable<int> RefId { get; set; }
        public Nullable<int> ShipmentOrderId { get; set; }
        public string ShipmentNo { get; set; }
           
        public virtual CashAdvance CashAdvance { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
    }
}
