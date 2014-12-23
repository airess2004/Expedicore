using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class JobMapping : EntityTypeConfiguration<Job>
    {
        public JobMapping()
        {  
            HasKey(u => u.Id);
            HasRequired(u => u.CreatedBy)
               .WithMany()
               .HasForeignKey(u => u.CreatedById)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.UpdatedBy)
               .WithMany()
               .HasForeignKey(u => u.UpdatedById)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.Office)
               .WithMany()
               .HasForeignKey(u => u.OfficeId)
               .WillCascadeOnDelete(false);
            Ignore(u => u.Errors);
        }
    }
}
