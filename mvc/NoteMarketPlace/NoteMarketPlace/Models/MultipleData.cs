using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class MultipleData
    {
        public Context.tblNoteCategory NoteCategory { get; set; }
        public Context.tblUser User { get; set; }

        public Context.tblNoteType NoteType { get; set; }


        public Context.tblSellerNote sellerNote { get; set; }

        public Context.tblCountry Country { get; set; }


        public Context.tblDownload download { get; set; }

        public Context.tblUserProfile userProfile { get; set; }
        public Context.tblSellerNotesReportedIssue reportedIssue { get; set; }


        public Context.tblReferenceData referenceData { get; set; }

        public Context.tblUser actionBy { get; set; }


        public Context.tblUser buyer { get; set; }

        public Context.tblSellerNotesReview notesReview { get; set; }

    }
}