using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int MasterCode { get; set; }
        public string ContactStatus { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPerson { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string NPWP { get; set; }
        public string NPPKP { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> PortId { get; set; }
        public Nullable<int> AirportId { get; set; }
        public Nullable<int> MarketId { get; set; }
        public Nullable<int> CreditTermInDays { get; set; }
        public Nullable<decimal> CreditTermInIDR { get; set; }
        public string ShipmentStatus { get; set; }
        public Nullable<DateTime> LastShipmentDate { get; set; }
        public bool IsAgent { get; set; }
        public Nullable<int> AgentCode { get; set; }
        public bool IsShipper { get; set; }
        public Nullable<int> ShipperCode { get; set; }
        public bool IsConsignee { get; set; }
        public Nullable<int> ConsigneeCode { get; set; }
        public bool IsIATA { get; set; }
        public Nullable<int> IATACode { get; set; }
        public bool IsSSLine { get; set; }
        public Nullable<int> SSLineCode { get; set; }
        public bool IsDepo { get; set; }
        public Nullable<int> DepoCode { get; set; }
        public bool IsEMKL { get; set; }
        public Nullable<int> EMKLCode { get; set; }
        public int OfficeId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Office Office { get; set; } 
        public virtual CityLocation CityLocation { get; set; }
        public virtual Airport Airport { get; set; }
        public virtual Port Port { get; set; }

        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
    }
}
