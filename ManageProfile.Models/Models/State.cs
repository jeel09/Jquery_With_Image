using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManageProfile.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Required]
        public string StateName { get; set; }

        [Required]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        [ValidateNever]
        public Country Country { get; set; }

    }
}
