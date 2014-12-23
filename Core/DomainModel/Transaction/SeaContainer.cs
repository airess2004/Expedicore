using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SeaContainer
    {
        public int Id { get; set; }
        public int ShipmentOrderId { get; set; }
        public int OfficeId { get; set; }
        public int TotalSub { get; set; }
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public Nullable<int> Size { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<decimal> GrossWeight { get; set; }
        public Nullable<decimal> NetWeight { get; set; }
        public Nullable<decimal> CBM { get; set; }
        public Nullable<bool> PartOf { get; set; }
        public string Commodity { get; set; }
        public Nullable<decimal> NoOfPieces { get; set; }
        public string PackagingCode { get; set; }
        public int MasterCode { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public virtual Office Office { get; set; }  


    }
}