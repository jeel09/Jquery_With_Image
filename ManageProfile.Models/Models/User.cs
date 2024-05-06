using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int GenderId { get; set; }

        [Required]
        public string HobbyId { get; set; }


        [Required]
        public int CountryId { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        public int CityId { get; set; }

        public string? ImageUrl { get; set; }

        [ValidateNever]
        public virtual Gender Gender { get; set; }

        [ValidateNever]
        public virtual Hobby Hobby { get; set; }

        [ValidateNever]
        public virtual Country Country { get; set; }

        [ValidateNever]
        public virtual State State { get; set; }

        [ValidateNever]
        public virtual City City { get; set; }
    }
}
