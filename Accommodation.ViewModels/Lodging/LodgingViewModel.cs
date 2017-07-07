using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models.Enumerators;
using Accommodation.ViewModels.Location;
using System.Web;

namespace Accommodation.ViewModels
{
   

    public class LodgingViewModel : IBaseViewModel
    {
        public int ID { get; set; }

        public string UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Hashtag { get; set; }
        public string LocationTag { get; set; }
        public string WebPage { get; set; }
        public bool Active { get; set; }
        public AccommodationTypeViewModel AccommodationType { get; set; }

        public PublishTypeViewModel PublishType { get; set; }

        public IList<PhotoViewModel> Photos { get; set; }
        public LocationViewModel Location { get; set; }

        public dynamic File { get; set; }

        public bool PhotoHasChanged { get; set; }
    }

    public class LodgingHashTagViewModel : IBaseViewModel
    {
        public int ID { get; set; }
        public string Hashtag { get; set; }
        public string LocationTag { get; set; }
    }
}
