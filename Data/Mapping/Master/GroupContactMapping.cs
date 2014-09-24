using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class GroupContactMapping : EntityTypeConfiguration<GroupContact>
    {
        public GroupContactMapping()
        { 
            HasKey(u => u.Id);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.CreatedBy)
               .WithMany()
               .HasForeignKey(u => u.Id)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.UpdatedBy)
               .WithMany()
               .HasForeignKey(u => u.Id)
               .WillCascadeOnDelete(false);
            Ignore(u => u.Errors);
        }
    }
}
