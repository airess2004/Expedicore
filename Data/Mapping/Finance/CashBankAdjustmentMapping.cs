﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CashBankAdjustmentMapping : EntityTypeConfiguration<CashBankAdjustment>
    {
        public CashBankAdjustmentMapping()
        {
            HasKey(cba => cba.Id);
            HasRequired(cba => cba.CashBank)
                .WithMany()
                .HasForeignKey(cba => cba.CashBankId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.Office)
               .WithMany()
               .HasForeignKey(u => u.OfficeId)
               .WillCascadeOnDelete(false);
            Ignore(cba => cba.Errors);
        }
    }
}
