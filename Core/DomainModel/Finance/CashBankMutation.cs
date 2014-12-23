using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CashBankMutation
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public int MasterCode { get; set; }

        public int SourceCashBankId { get; set; }
        public int TargetCashBankId { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }

        public virtual CashBank TargetCashBank { get; set; }
        public virtual CashBank SourceCashBank { get; set; }
        public Dictionary<String, String> Errors { get; set; }
        public virtual Office Office { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
    }
}
