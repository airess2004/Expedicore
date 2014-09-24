using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class AccessUserMapping : EntityTypeConfiguration<AccessUser>
    {

        public AccessUserMapping()
        { 
            HasKey(u => u.Id);
            HasRequired(u => u.MenuUsers)
                .WithMany(u => u.AccessUsers)
                .HasForeignKey(u => u.UserMenuId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.AccountUsers) 
                .WithMany(u => u.AccessUsers)
                .HasForeignKey(u => u.UserAccountId)
                .WillCascadeOnDelete(false);
            Ignore(u => u.Errors);
        }
    }
}
