using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class BillOfLadingMapping : EntityTypeConfiguration<BillOfLading>
    {
        public BillOfLadingMapping()
        {  
            HasKey(u => u.Id);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.ShipmentOrder)
               .WithMany()
               .HasForeignKey(u => u.ShipmentOrderId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.Agent)
               .WithMany()
               .HasForeignKey(u => u.AgentId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.Consignee)
               .WithMany()
               .HasForeignKey(u => u.ConsigneeId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.NParty)
               .WithMany()
               .HasForeignKey(u => u.NPartyId)
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
