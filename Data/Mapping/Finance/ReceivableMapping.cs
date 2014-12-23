using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ReceivableMapping : EntityTypeConfiguration<Receivable>
    {
        public ReceivableMapping()
        {
            HasKey(r => r.Id);
            HasRequired(r => r.Contact)
                .WithMany()
                .HasForeignKey(r => r.ContactId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.Office)
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            Ignore(r => r.Errors);
        }
    }
}
