using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models.Enumerators;
using Accommodation.Models;

namespace Accommodation.Models
{
    public class Photo : EntityBase
    {

        public override int IdentityID() { return ID; }

        public int ID { get; set; }
        public string Description { get; set; }
        public Lodging Lodging { get; set; }
        public Service Service { get; set; }
        public Tourism Tourism { get; set; }
        public string URL { get; set; }

    }
}
