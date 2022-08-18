using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VueLogin.Models
{
    public class UserList
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
