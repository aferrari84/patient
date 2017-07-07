using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accommodation.Models
{
    [NotMapped]
    public class PagedEntity<T>
    {
        public int Total { get; set; }
        public List<T> Results {get; set;}
    }
}
