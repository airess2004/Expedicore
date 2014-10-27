using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Office
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string InitialCompany { get; set; }
        public string Abbrevation { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
       // public int CityId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string NPWP { get; set; }
        public string NPPKP { get; set; }
        public string ApprovalWP { get; set; }
        public string NamaWP { get; set; }
        public string AlamatWP { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }  
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
    }
}
