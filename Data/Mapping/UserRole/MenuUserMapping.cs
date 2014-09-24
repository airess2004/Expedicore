using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class MenuUserMapping : EntityTypeConfiguration<MenuUser>
    { 

        public MenuUserMapping()
        {
            HasKey(u => u.Id);
            Ignore(u => u.Errors);
        }
    }
}
