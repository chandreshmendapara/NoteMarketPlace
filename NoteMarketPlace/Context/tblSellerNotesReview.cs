//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoteMarketPlace.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSellerNotesReview
    {
        public int ID { get; set; }
        public int NoteID { get; set; }
        public int ReviewedByID { get; set; }
        public int AgainstDownloadsID { get; set; }
        public decimal Ratings { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual tblDownload tblDownload { get; set; }
        public virtual tblSellerNote tblSellerNote { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}
