using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class TemporaryPaymentMapping : EntityTypeConfiguration<TemporaryPayment>
    {
        public TemporaryPaymentMapping()
        {  
            HasKey(u => u.Id);
            HasMany(u => u.TemporaryPaymentJob)
                .WithRequired(u => u.TemporaryPayment)
                .HasForeignKey(u => u.TemporaryPaymentId);
            HasRequired(u => u.Contact)
               .WithMany()
               .HasForeignKey(u => u.ContactId)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.CreatedBy)
               .WithMany()
               .HasForeignKey(u => u.CreatedById)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.UpdatedBy)
               .WithMany()
               .HasForeignKey(u => u.UpdatedById)
               .WillCascadeOnDelete(false);
            Ignore(u => u.Errors);
        }
    }
}
