using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Invoice
    {
        public int Id;
        public string InvoicesNo;
        public string DebetCredit;
        public int ShipmentOrderId;
        public int OfficeId;

        public int CustomerTypeId;
        public int ContactId;
        public string CustomerName;
        public string CustomerAddress;
        public Nullable<int>BillId;
        public string BillName;
        public string BillAddress;
        public string InvoicesTo;
        public Nullable<int> InvoiceStatus;
        public Nullable<decimal> PaymentUSD;
        public Nullable<decimal> PaymentIDR;
        public Nullable<decimal> TotalVatUSD;
        public Nullable<decimal> TotalVatIDR;
        public Nullable<decimal> Rate;
        public Nullable<DateTime>ExRateDate;
        public Nullable<int> Period;
        public Nullable<int> YearPeriod;
        public string InvoicesAgent;
        public string InvoicesEdit;
        public string JenisInvoices;
        public string LinkTo;
        public Nullable<int> DueDate;
        public Nullable<bool> Paid;
        public Nullable<DateTime> PaidOn;
        public Nullable<bool> SaveOR;
        public Nullable<bool> BadDebt;
        public Nullable<DateTime> BadDebtOn;
        public Nullable<bool> ReBadDebt;
        public Nullable<DateTime> DateReBadDebt;
        public Nullable<int> Printing;
        public Nullable<DateTime> PrintedAt;
        public Nullable<bool>IsDeleted;
        public Nullable<DateTime> DeletedAt;
        public int CreatedById;
        public Nullable<DateTime> CreatedAt;
        public Nullable<decimal> UpdatedById;
        public Nullable<DateTime> UpdatedAt;
        public String InvoiceNo2;
        public Nullable<int> InvHeader;
        public Nullable<int> ExRateId;
        public Nullable<int> RePrintApproved;
        public Nullable<DateTime> RePrintApprovedOn;
        public Nullable<int> RePrintApprovedBy;

        public Dictionary<string, string> Errors { get; set; }
        
        public virtual Office Office { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
