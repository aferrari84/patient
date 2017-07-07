using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Accommodation.Interfaces;
using Accommodation.Interfaces.Maps;
using Accommodation.Interfaces.Services;
using Accommodation.Models;
using Accommodation.ViewModels;
using Accommodation.Models.Enumerators;
using Accommodation.ViewModels.Location;

namespace Accommodation.Maps
{
    public class AdvertisingMap : IAdvertisingMap
    {
        IAdvertisingService _service;


        public AdvertisingMap(IAdvertisingService service)
        {
            _service = service;
        }

        public IList<AdvertisingViewModel> GetByLocation(int location)
        {
            try
            {
                return _service.GetByLocation(location).Select(x => DomainToViewModel(x)).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }



        public IList<AdvertisingViewModel> GetGlobals()
        {
            try
            {
                return _service.GetGlobals().Select(x => DomainToViewModel(x)).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public AdvertisingViewModel DomainToViewModel(Advertising domain)
        {
            AdvertisingViewModel rec = new AdvertisingViewModel();

            rec.ID = domain.ID;
            rec.ImageURL = domain.ImageURL;
            rec.Location = new LocationViewModel() { ID = domain.Location.ID, Name = domain.Location.Name };
            rec.Link = domain.Link;
            rec.ExternalLink = domain.ExternalLink;
            rec.IsAccommodation = domain.Lodging != null;

            return rec;
        }

    }
}
