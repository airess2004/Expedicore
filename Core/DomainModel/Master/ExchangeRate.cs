﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class ExchangeRate
    {
        public int Id { get; set; }

        public DateTime ExRateDate { get; set; }
        public decimal ExRate1 { get; set; }
        public decimal ExRate2 { get; set; }
        public decimal ExRate3 { get; set; }

        public int OfficeId { get; set; }
        public int MasterCode { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }
         
        public virtual Office Office { get; set; }
        public virtual AccountUser CreatedBy { get; set; } 
        public virtual AccountUser UpdatedBy { get; set; }



    }
}
