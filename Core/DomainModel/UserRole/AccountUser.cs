using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class AccountUser
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAdmin { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
         
        public Dictionary<String, String> Errors { get; set; }
        public ICollection<AccessUser> AccessUsers { get; set; }
        public virtual Office Office { get; set; }
    }
}
