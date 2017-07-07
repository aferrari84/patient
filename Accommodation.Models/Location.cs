using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models
{
    public class Location : EntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Hashtag { get; set; }
        public State State { get; set; }
        public string ImageURL { get; set; }
    }
}
