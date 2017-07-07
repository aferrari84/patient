using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Accommodation.Models
{
    public class User : IdentityUser
    {
        public override string Email { get; set; }
        public string UserShowName { get; set; }
        public bool? Deleted { get; set; }

        public User()
        {
            Deleted = false;
        }
    }
}
