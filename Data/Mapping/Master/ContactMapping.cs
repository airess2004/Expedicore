using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping

{
    public class ContactMapping : EntityTypeConfiguration<Contact>
    {
        public ContactMapping()
        { 
            HasKey(u => u.Id);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasOptional(u => u.Airport)
                        .WithMany()
                        .HasForeignKey(u => u.AirportId)
                        .WillCascadeOnDelete(false);
            HasOptional(u => u.CityLocation)
                        .WithMany()
                        .HasForeignKey(u => u.CityId)
                        .WillCascadeOnDelete(false);
            HasOptional(u => u.Port)
                       .WithMany()
                       .HasForeignKey(u => u.PortId)
                       .WillCascadeOnDelete(false);
            HasOptional(u => u.UpdatedBy)
               .WithMany()
               .HasForeignKey(u => u.UpdatedById)
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
