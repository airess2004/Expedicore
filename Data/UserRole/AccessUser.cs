﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class AccessUser
    {
        public int Id { get; set; }
        public int UserMenuId { get; set; }
        public int UserAccountId { get; set; }

        public bool AllowView { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowConfirm { get; set; }
        public bool AllowUnconfirm { get; set; }
        public bool AllowPaid { get; set; }
        public bool AllowUnpaid { get; set; }
        public bool AllowReconcile { get; set; }
        public bool AllowUnreconcile { get; set; }
        public bool AllowPrint { get; set; }
        public bool AllowUndelete { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
         
        public virtual AccountUser AccountUsers { get; set; }
        public virtual MenuUser MenuUsers { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}
