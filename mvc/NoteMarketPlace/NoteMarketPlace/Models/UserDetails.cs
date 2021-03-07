using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class UserDetails
    {

        [Required]
        public string FirstName { get; set; }



        public string LastName { get; set; }

        [Required]
        public string EmailID { get; set; }

        [Required]
        public string CountryCode { get; set; }


        [RegularExpression("[1-9]{1}[0-9]{9}", ErrorMessage = "You must provide a phone number")]
        public string PhnNo { get; set; }
    }
}