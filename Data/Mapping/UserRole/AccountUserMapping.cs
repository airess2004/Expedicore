using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class AccountUserMapping : EntityTypeConfiguration<AccountUser>
    {

        public AccountUserMapping()
        {
            HasKey(u => u.Id);
            HasRequired(u => u.Office)
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            Ignore(u => u.Errors);
        }
    }
}
