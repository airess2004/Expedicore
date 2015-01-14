using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ReceiptVoucherDetail
    {
        public int Id { get; set; }
        public int ReceiptVoucherId { get; set; }
        public int ReceivableId { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
        public decimal AmountUSD { get; set; }
        public decimal AmountIDR { get; set; }
        public string Description { get; set; }
        public int OfficeId { get; set; }
        public int MasterCode { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }        
        public DateTime CreatedAt { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual ReceiptVoucher ReceiptVoucher { get; set; }
        public virtual Receivable Receivable { get; set; }
        public virtual Office Office { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}