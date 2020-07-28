using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class AuthenticationLoginModel
    {
        [Display(Name ="Login")]
        [Required(ErrorMessage = "Le champ {0} est obligatoire")]
        public string UserName { get; set; }

        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Le champ {0} est obligatoire")]
        [DataType(DataType.Password)]
        public string  Password { get; set; }
    }
}
