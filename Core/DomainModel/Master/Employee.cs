﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Employee 
    { 
        public int Id { get; set; }

        public string Name { get; set; }
        public int OfficeId { get; set; }
        public int GroupEmployeeId { get; set; }
        public int MasterCode { get; set; }
        

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }
         
        public virtual Office Office { get; set; }
        public virtual GroupEmployee GroupEmployee { get; set; }
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }

    }
}
