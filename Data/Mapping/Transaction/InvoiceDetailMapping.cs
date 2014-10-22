using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class InvoiceDetailMapping : EntityTypeConfiguration<InvoiceDetail>
    {
        public InvoiceDetailMapping()
        {  
            HasKey(u => u.Id);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasOptional(u => u.Cost)
               .WithMany()
               .HasForeignKey(u => u.CostId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.EPLDetail)
               .WithMany()
               .HasForeignKey(u => u.EPLDetailId)
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
