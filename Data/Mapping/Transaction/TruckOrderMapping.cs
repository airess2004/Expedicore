using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class TruckOrderMapping : EntityTypeConfiguration<TruckOrder>
    {
        public TruckOrderMapping()
        {  
            HasKey(u => u.Id);
            HasRequired(u => u.Contact)
            .WithMany()
            .HasForeignKey(u => u.ContactId)
            .WillCascadeOnDelete(false);
            HasRequired(u => u.Truck)
               .WithMany()
               .HasForeignKey(u => u.TruckId)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.Depo)
               .WithMany()
               .HasForeignKey(u => u.DepoId)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.ContainerYard)
               .WithMany()
               .HasForeignKey(u => u.ContainerYardId)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.Employee)
               .WithMany()
               .HasForeignKey(u => u.EmployeeId)
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
