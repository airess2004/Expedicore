using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class TruckOrder
    { 
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string NoJob { get; set; }
        public int EmployeeId { get; set; }
        public int ContactId { get; set; }
        public int ContainerYardId { get; set; }
        public int DepoId { get; set; }
        public int TruckId { get; set; }

        public DateTime TanggalSPPB { get; set; }
        public DateTime TanggalOrder { get; set; }
        public string AgendaTruck { get; set; }
        public string PICOrder { get; set; }
        public string AgendaMTP { get; set; }
        public string InvoiceShipper { get; set; }
        public string InvoiceNo { get; set; }
        public string Party { get; set; }
        public string PIC { get; set; }
        public string NoContainer { get; set; }
        public string Tujuan { get; set; }
        public int MasterCode { get; set; }

        public Nullable<DateTime> TglLoloBayar { get; set; }

        public Nullable<DateTime> UJ {get;set;}
        public Nullable<DateTime> CYIN {get;set;}
        public Nullable<DateTime> CYOUT {get;set;}
        public Nullable<DateTime> MILLIN {get;set;}
        public Nullable<DateTime> REGIN {get;set;}
        public Nullable<DateTime> REGOUT {get;set;}
        public Nullable<DateTime> MILLOUT {get;set;}
        public Nullable<DateTime> BOGASARI {get;set;}
        public Nullable<DateTime> DEPOIN {get;set;}
        public Nullable<DateTime> DEPOOUT {get;set;}
        public Nullable<DateTime> GARASI {get;set;}
        public Nullable<DateTime> CYMILL {get;set;}
        public Nullable<DateTime> DIMILL {get;set;}
        public Nullable<DateTime> OTWKEPRIOK {get;set;}
        public Nullable<DateTime> ADATOLCIUJUNG {get;set;}
        public Nullable<DateTime> JORR {get;set;}
        public bool IsFinished { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; } 
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }
        
        public virtual Office Office { get; set; }
        public virtual Truck Truck { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Depo Depo { get; set; }
        public virtual ContainerYard ContainerYard { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
    }
}
