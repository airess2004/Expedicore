using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class EstimateProfitLossMapping : EntityTypeConfiguration<EstimateProfitLoss>
    {
        public EstimateProfitLossMapping()
        {  
            HasKey(u => u.Id);
            HasMany(u => u.EstimateProfitLossDetails) 
                .WithRequired(u => u.EstimateProfitLoss)
                .HasForeignKey(u => u.EstimateProfitLossId);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            //HasRequired(u => u.ExchangeRate)
            //    .WithMany()
            //    .HasForeignKey(u => u.ExchangeRateId)
            //    .WillCascadeOnDelete(false);
            HasRequired(u => u.ShipmentOrder)
               .WithMany()
               .HasForeignKey(u => u.ShipmentOrderId)
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
