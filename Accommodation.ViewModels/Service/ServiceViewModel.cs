﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models.Enumerators;
using Accommodation.ViewModels.Location;

namespace Accommodation.ViewModels
{
   

    public class ServiceViewModel : IBaseViewModel
    {
        public int ID { get; set; }

        public string UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Hashtag { get; set; }
        public string LocationTag { get; set; }
        public bool Outstanding { get; set; }
        public string WebPage { get; set; }
        public bool Active { get; set; }
        public ServiceTypeViewModel ServiceType { get; set; }

        public PublishTypeViewModel PublishType { get; set; }

        public IList<PhotoViewModel> Photos { get; set; }
        public LocationViewModel Location { get; set; }

        public bool PhotoHasChanged { get; set; }
    }

    public class ServiceHashTagViewModel : IBaseViewModel
    {
        public int ID { get; set; }
        public string Hashtag { get; set; }
        public string LocationTag { get; set; }
    }
}