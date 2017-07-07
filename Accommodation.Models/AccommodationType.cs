using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models
{
    public class AccommodationType : EntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
