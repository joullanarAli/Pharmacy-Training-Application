using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.View
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50,MinimumLength = 5)]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(50,MinimumLength =5)]
        public string Password { get; set; }=null!;
    }
}
