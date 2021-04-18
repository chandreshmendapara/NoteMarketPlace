using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class Profile
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        public string EmailID { get; set; }


        public Nullable<System.DateTime> DOB { get; set; }

        [Required]
        public Nullable<int> Gender { get; set; }

        public string PhoneNumber_CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        
        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }
        public string University { get; set; }
        public string College { get; set; }
    }
}