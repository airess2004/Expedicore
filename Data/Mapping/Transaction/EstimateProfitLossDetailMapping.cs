using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class EstimateProfitLossDetailMapping : EntityTypeConfiguration<EstimateProfitLossDetail>
    {
        public EstimateProfitLossDetailMapping()
        {  
            HasKey(u => u.Id);
            HasRequired(u => u.EstimateProfitLoss)
                .WithMany()
                .HasForeignKey(u => u.EstimateProfitLossId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.Cost)
                .WithMany()
                .HasForeignKey(u => u.CostId)
                .WillCascadeOnDelete(false);
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
            HasRequired(u => u.Office)
               .WithMany()
               .HasForeignKey(u => u.OfficeId)
               .WillCascadeOnDelete(false);
            Ignore(u => u.Errors);
        }
    }
}
