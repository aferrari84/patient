using Accommodation.Interfaces.Maps;
using Accommodation.ViewModels;
using Accommodation.ViewModels.Location;
using Accommodation.Web.API.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;


namespace Accommodation.Web.API.Cache
{
    public class Cache 
    {
        private static IList<HashTagViewModel> HashTags;
        private static IList<LocationViewModel> Locations;

        private static DateTime LastUpdate;



        public static void ReloadCache(ILodgingMap _lodgingMap, ITourismMap _tourismMap, IServiceMap _serviceMap)
        {
            if (LastUpdate == null || (DateTime.Now - LastUpdate).Hours > 6)
            {
                HashTags = _lodgingMap.GetAllHashTag().Union(_tourismMap.GetAllHashTag().Union(_serviceMap.GetAllHashTag())).ToList();
                LastUpdate = DateTime.Now;
            }
        }

        public static IList<HashTagViewModel> GetHashTag(ILodgingMap _lodgingMap, ITourismMap _tourismMap, IServiceMap _serviceMap)
        {
            if (HashTags == null)
            {
                HashTags = _lodgingMap.GetAllHashTag();
                LastUpdate = DateTime.Now;
            }


            return HashTags;
        }






        public static void ReloadLocationCache(ILocationMap _locationMap)
        {
            if (LastUpdate == null || (DateTime.Now - LastUpdate).Hours > 6)
            {
                Locations = _locationMap.GetAll();
                LastUpdate = DateTime.Now;
            }
        }

        public static IList<LocationViewModel> GetLocationHashTag(ILocationMap _locationMap)
        {
            if (Locations == null)
            {
                Locations = _locationMap.GetAll();
                LastUpdate = DateTime.Now;
            }


            return Locations;
        }
    }
}
