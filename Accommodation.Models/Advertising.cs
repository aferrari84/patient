using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models
{
    public class Advertising : EntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Link { get; set; }
        public bool ExternalLink { get; set; }
        public Location Location { get; set; }
        public Lodging Lodging { get; set; }
        public Service Service { get; set; }
    }
}
