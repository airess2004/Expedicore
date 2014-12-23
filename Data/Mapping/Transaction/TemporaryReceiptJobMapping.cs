using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class TemporaryReceiptJobMapping : EntityTypeConfiguration<TemporaryReceiptJob>
    {
        public TemporaryReceiptJobMapping()
        {  
            HasKey(u => u.Id);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.TemporaryReceipt)
               .WithMany()
               .HasForeignKey(u => u.TemporaryReceiptId)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.ShipmentOrder)
               .WithMany()
               .HasForeignKey(u => u.ShipmentOrderId)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.CreateBy)
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
