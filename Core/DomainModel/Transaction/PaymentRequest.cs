using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{ 
    public partial class PaymentRequest
    {
       public int Id { get; set; }
       public int PRNo { get; set; }
       public int MasterCode { get; set; }  
       public string DebetCredit	{ get; set; }
       public int ShipmentOrderID { get; set; }
       public int OfficeId { get; set; }
       public int CurrencyId { get; set; }  
       public string Reference { get; set; }
       public int PRStatus { get; set; }
       public DateTime PaymentRequestDate { get; set; }
       public Nullable<bool> IsGeneralPR { get; set; } 
       public Nullable<int> ContactId { get; set; }
       public Nullable<int> ContactTypeId { get; set; }
       public Nullable<int> PersonalId { get; set; }
       public Nullable<decimal> Payment { get; set; }
       public string PRContraStatus { get; set; }
       public Nullable<int> PRContraNo { get; set; }
       public Nullable<bool> Paid { get; set; }
       public Nullable<DateTime> DatePaid { get; set; }
       public bool ApproveOpr { get; set; }
       public Nullable<DateTime> ApproveOprOn { get; set; }
       public bool ApproveAcc { get; set; }
       public Nullable<DateTime> ApproveAccOn { get; set; }
       public Nullable<decimal> Rate { get; set; }
       public Nullable<DateTime> ExRateDate { get; set; }
       public Nullable<int> ExRateId { get; set; }
       public int Printing { get; set; }
       public Nullable<DateTime> PrintedOn { get; set; }

       public bool IsDeleted { get; set; }
       public DateTime CreatedAt { get; set; }
       public int CreatedById { get; set; }
       public Nullable<DateTime> UpdatedAt { get; set; }
       public Nullable<int> UpdatedById { get; set; }
       public Nullable<DateTime> DeletedAt { get; set; }

       public bool IsConfirmed { get; set; }
       public Nullable<DateTime> ConfirmationDate { get; set; }

       public Dictionary<string, string> Errors { get; set; }

       public virtual Office Office { get; set; }
       public virtual ShipmentOrder ShipmentOrder { get; set; }
       public virtual Contact Contact { get; set; }
       public virtual AccountUser CreatedBy { get; set; }
       public virtual AccountUser UpdatedBy { get; set; } 
       public virtual ICollection<PaymentRequestDetail> PaymentRequestDetail { get; set; }

    }
}
