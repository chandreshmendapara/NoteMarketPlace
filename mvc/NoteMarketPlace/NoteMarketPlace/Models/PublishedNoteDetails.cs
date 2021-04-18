using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class PublishedNoteDetails
    {

        public int id { get; set; }
        public string Title { get; set; }
        public string categoryName { get; set; }
        public bool IsPaid { get; set; }

        public Nullable<decimal> sellingPrice { get; set; }
        public string sellerName { get; set; }
        public DateTime PublishedDate { get; set; }

        public float AttachmentSize { get; set; }

        public string AttachmentPath { get; set; }

        public int totalDownloads { get; set; }

    }
}