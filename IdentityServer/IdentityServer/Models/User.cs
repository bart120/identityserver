using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class User
    { 
        public int ID { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
