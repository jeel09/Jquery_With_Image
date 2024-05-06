using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.Models
{
    public class Hobby
    {
        [Key]
        public string HobbyId { get; set; }

        public string? HobbyName { get; set; }
    }
}
