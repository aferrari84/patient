using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models
{
    public class EntityBase
    {
        
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool Deleted { get; set; }

        public EntityBase()
        {
            Created = DateTime.Now;
            Deleted = false;
        }

        public virtual int IdentityID()
        {
            return 0;
        }

        public virtual object[] IdentityID(bool dummy = true)
        {
            return new List<object>().ToArray();
        }
    }

}
