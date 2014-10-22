using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Cost
    { 
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Quantity { get; set; }
        public bool CBM { get; set; }
        public string Remarks { get; set; }
        public int MasterCode { get; set; }
        public int OfficeId { get; set; }
        public int CostType { get; set; } // 1 : Sea , 2 : Air 
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Office Office { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
    }
}