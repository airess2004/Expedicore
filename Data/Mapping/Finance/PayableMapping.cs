using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PayableMapping : EntityTypeConfiguration<Payable>
    {
        public PayableMapping()
        {
            HasKey(p => p.Id);
            HasRequired(p => p.Contact)
                .WithMany()
                .HasForeignKey(p => p.ContactId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.Office)
               .WithMany()
               .HasForeignKey(u => u.OfficeId)
               .WillCascadeOnDelete(false);
            Ignore(p => p.Errors);
        }
    }
}
