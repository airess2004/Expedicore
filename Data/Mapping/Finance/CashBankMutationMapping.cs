﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CashBankMutationMapping : EntityTypeConfiguration<CashBankMutation>
    {
        public CashBankMutationMapping()
        {
            HasKey(cbm => cbm.Id);
            HasRequired(u => u.Office)
               .WithMany()
               .HasForeignKey(u => u.OfficeId)
               .WillCascadeOnDelete(false);
        }
    }
}
