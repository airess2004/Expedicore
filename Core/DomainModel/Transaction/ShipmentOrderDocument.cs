using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{ 
    public partial class ShipmentOrderDocument
    {
        public int Id { get; set; }
        public int ShipmentOrderId { get; set; }
        public int OfficeId { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } 
        public int CreatedById { get; set; }
        public Nullable<DateTime> SubmitDate { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsLegacy { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Office Office { get; set; }
         
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }


    }
}