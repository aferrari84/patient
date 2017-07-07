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
    public class Lodging : EntityBase
    {
        public Lodging()
        {
            Active = true;
        }

        public override int IdentityID() { return ID; }

        public int ID { get; set; }
        public string UserID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public AccommodationType AccommodationType { get; set; }
        public int Capacity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Hashtag { get; set; }

        public string WebPage { get; set; }
        
        public bool Active { get; set; }
        public PublishType PublishType { get; set; }

    }
}
