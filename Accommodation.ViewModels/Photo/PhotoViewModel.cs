using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models.Enumerators;
using Accommodation.ViewModels.Location;

namespace Accommodation.ViewModels
{
   

    public class PhotoViewModel : IBaseViewModel
    {
        public int ID { get; set; }

        public string URL { get; set; }
        public string Description { get; set; }

    }
    
}
