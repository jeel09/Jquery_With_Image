using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public int StateId { get; set; }
        [ValidateNever]
        [ForeignKey("StateId")]
        public State State { get; set; }

        [Required]
        public int CountryId { get; set; }
        [ValidateNever]
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
