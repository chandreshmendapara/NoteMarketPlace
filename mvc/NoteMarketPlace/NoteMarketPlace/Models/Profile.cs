using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class Profile
    {
        public int Id { get; set; }

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

        [Required]
        public string PhoneNumber_CountryCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        [Required]
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

        public string joinDate { get; set; }

        public int userBooksUnderReview { get; set; }

        public int userPublishedBook { get; set; }

        public int userDownloadedBook { get; set; }

        public int userTotalExpense { get; set; }

        public int userTotalEarning { get; set; }

    }
}