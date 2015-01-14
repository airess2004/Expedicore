﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
     
{
    public class CashAdvanceMapping : EntityTypeConfiguration<CashAdvance>
    {
        public CashAdvanceMapping()
        {  
            HasKey(u => u.Id);
            HasMany(u => u.CashAdvanceDetails)
                .WithRequired(u => u.CashAdvance)
                .HasForeignKey(u => u.CashAdvanceId);
            HasRequired(u => u.Office) 
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
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
