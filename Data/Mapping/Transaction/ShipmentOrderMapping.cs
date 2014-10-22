using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class ShipmentOrderMapping : EntityTypeConfiguration<ShipmentOrder>
    {
        public ShipmentOrderMapping()
        {   
            HasKey(u => u.Id);
            HasRequired(u => u.Office)
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            HasOptional(u => u.Consignee)
               .WithMany()
               .HasForeignKey(u => u.ConsigneeId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.NParty)
                .WithMany()
                .HasForeignKey(u => u.NPartyId)
                .WillCascadeOnDelete(false);
            HasOptional(u => u.Shipper)
                .WithMany()
                .HasForeignKey(u => u.ShipperId)
                .WillCascadeOnDelete(false);
            HasOptional(u => u.Consignee)
                .WithMany()
                .HasForeignKey(u => u.ConsigneeId)
                .WillCascadeOnDelete(false);
            HasOptional(u => u.NParty)
                .WithMany()
                .HasForeignKey(u => u.NPartyId)
                .WillCascadeOnDelete(false);
            HasOptional(u => u.ReceiptPlace)
               .WithMany()
               .HasForeignKey(u => u.ReceiptPlaceId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.LoadingPort)
               .WithMany()
               .HasForeignKey(u => u.LoadingPortId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.DischargePort)
               .WithMany()
               .HasForeignKey(u => u.DischargePortId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.DeliveryPlace)
               .WithMany()
               .HasForeignKey(u => u.DeliveryPlaceId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.DepartureAirPort)
               .WithMany()
               .HasForeignKey(u => u.DepartureAirPortId)
               .WillCascadeOnDelete(false);
            HasOptional(u => u.DestinationAirPort)
               .WithMany()
               .HasForeignKey(u => u.DestinationAirPortId)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.ShipmentAdvice)
               .WithMany()
               .HasForeignKey(u => u.Id)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.ShipmentInstruction)
               .WithMany()
               .HasForeignKey(u => u.Id)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.DeliveryOrder)
               .WithMany()
               .HasForeignKey(u => u.Id)
               .WillCascadeOnDelete(false);
            HasRequired(u => u.NoticeOfArrival)
               .WithMany()
               .HasForeignKey(u => u.Id)
               .WillCascadeOnDelete(false);
            HasMany(u => u.SeaContainers)
               .WithRequired(u => u.ShipmentOrder)
               .HasForeignKey(u => u.ShipmentOrderId);
            HasMany(u => u.ShipmentOrderRoutings)
               .WithRequired(u => u.ShipmentOrder)
               .HasForeignKey(u => u.ShipmentOrderId);
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
