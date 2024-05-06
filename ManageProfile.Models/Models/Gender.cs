using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.Models
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }

        [Required]
        public string GenderName { get; set; }
    }
}
