using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CashBank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OfficeId { get; set; }
        public decimal Amount { get; set; }
        public int MasterCode { get; set; }
        public int CurrencyId { get; set; }         
        public bool IsBank { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }

        public Dictionary<String, String> Errors { get; set; }
        public ICollection<CashMutation> CashMutations { get; set; }
        public virtual Office Office { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
    }
}
