using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class systemConfig
    {
        [Required]
        public HttpPostedFileBase defaultProfilePic { get; set; }


        [Required]
        public string supportEmialID { get; set; }

        [Required]
        public string supportPhnNo { get; set; }


        [Required]
        public string notificationEmailID { get; set; }



        [Required]
        public string TwitterURL { get; set; }


        [Required]
        public string FacebookURL { get; set; }


        [Required]
        public string LinkedinURL { get; set; }


        [Required]
        public HttpPostedFileBase defaultNoteImg { get; set; }
    }
}