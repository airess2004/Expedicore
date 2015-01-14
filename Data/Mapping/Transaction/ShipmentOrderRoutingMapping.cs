using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class ShipmentOrderRoutingMapping : EntityTypeConfiguration<ShipmentOrderRouting>
    {
        public ShipmentOrderRoutingMapping()
        {   
            HasKey(u => u.Id);
            HasRequired(u => u.ShipmentOrder)
               .WithMany()
               .HasForeignKey(u => u.ShipmentOrderId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.AirPortForm)
               .WithMany()
               .HasForeignKey(u => u.AirportFromId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.AirPortTo)
               .WithMany()
               .HasForeignKey(u => u.AirportToId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.City)
              .WithMany()
              .HasForeignKey(u => u.CityId)
              .WillCascadeOnDelete(false);
            HasOptional(u => u.Flight)
              .WithMany()
              .HasForeignKey(u => u.FlightId)
              .WillCascadeOnDelete(false);
            HasOptional(u => u.Port)
              .WithMany()
              .HasForeignKey(u => u.PortId)
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
