using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ReceiptVoucherDetailMapping : EntityTypeConfiguration<ReceiptVoucherDetail>
    {
        public ReceiptVoucherDetailMapping()
        {
            HasKey(rvd => rvd.Id);
            HasRequired(rvd => rvd.ReceiptVoucher)
                .WithMany(pv => pv.ReceiptVoucherDetails)
                .HasForeignKey(rvd => rvd.ReceiptVoucherId);
            HasRequired(rvd => rvd.Receivable)
                .WithMany()
                .HasForeignKey(rvd => rvd.ReceivableId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.Office)
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .WillCascadeOnDelete(false);
            Ignore(rvd => rvd.Errors);
        }
    }
}
