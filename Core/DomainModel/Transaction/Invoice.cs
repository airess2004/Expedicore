using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Invoice
    {
        public int Id{ get; set; }
        public int InvoicesNo{ get; set; }
        public string DebetCredit{ get; set; }
        public int ShipmentOrderId{ get; set; }
        public int OfficeId{ get; set; }
        public int MasterCode { get; set; }

        public int CustomerTypeId{ get; set; }
        public int ContactId{ get; set; }
        public string CustomerName{ get; set; }
        public string CustomerAddress{ get; set; }
        public Nullable<int>BillId{ get; set; }
        public string BillName{ get; set; }
        public string BillAddress{ get; set; }
        public string InvoicesTo{ get; set; }
        public Nullable<int> InvoiceStatus { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CurrencyId { get; set; }
        public Nullable<decimal> PaymentUSD { get; set; }
        public Nullable<decimal> PaymentIDR { get; set; }
        public Nullable<decimal> TotalVatUSD { get; set; }
        public Nullable<decimal> TotalVatIDR { get; set; }
        public Nullable<decimal> Rate{ get; set; }
        public Nullable<DateTime> ExRateDate{ get; set; }
        public Nullable<int> Period{ get; set; }
        public Nullable<int> YearPeriod{ get; set; }
        public string InvoicesAgent{ get; set; }
        public string InvoicesEdit{ get; set; }
        public string JenisInvoices{ get; set; }
        public string LinkTo{ get; set; }
        public Nullable<int> DueDate{ get; set; }
        public Nullable<bool> Paid{ get; set; }
        public Nullable<DateTime> PaidOn{ get; set; }
        public Nullable<bool> SaveOR{ get; set; }
        public Nullable<bool> BadDebt{ get; set; }
        public Nullable<DateTime> BadDebtOn{ get; set; }
        public Nullable<bool> ReBadDebt{ get; set; }
        public Nullable<DateTime> DateReBadDebt{ get; set; }
        public Nullable<int> Printing{ get; set; }
        public Nullable<DateTime> PrintedAt{ get; set; }
        public bool IsDeleted{ get; set; }
        public Nullable<DateTime> DeletedAt{ get; set; }
        public int CreatedById{ get; set; }
        public DateTime CreatedAt{ get; set; }
        public Nullable<int> UpdatedById{ get; set; }
        public Nullable<DateTime> UpdatedAt{ get; set; }
        public String InvoiceNo2{ get; set; }
        public Nullable<int> InvHeader{ get; set; }
        public Nullable<int> ExRateId{ get; set; }
        public Nullable<int> RePrintApproved{ get; set; }
        public Nullable<DateTime> RePrintApprovedOn{ get; set; }
        public Nullable<int> RePrintApprovedBy{ get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public virtual Office Office { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
