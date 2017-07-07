using System;
using System.Collections.Generic;
using System.Linq;
using Accommodation.ViewModels.Location;
using Accommodation.Interfaces.Maps;
using Accommodation.Models;
using Accommodation.Interfaces.Services;

namespace Accommodation.Maps
{
    public class LocationMap : ILocationMap
    {
        ILocationService _service;


        public LocationMap(ILocationService service)
        {
            _service = service;
        }

        public IList<LocationViewModel> GetAll()
        {
            try
            {
                return _service.GetAll().Select(x => DomainToViewModel(x)).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public IList<LocationViewModel> GetByState(int id)
        {
            try
            {
                return _service.GetAll().Where(x=> x.State.ID == id).Select(x => DomainToViewModel(x)).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public LocationViewModel DomainToViewModel(Location domain)
        {
            LocationViewModel location = new LocationViewModel();

            location.ID = domain.ID;
            location.Hashtag = domain.Hashtag;
            location.Name = domain.Name;
            location.State = new StateViewModel() { ID = domain.State.ID, Name = domain.State.Name };
            location.ImageURL = domain.ImageURL;
            
            return location;
        }

       
    }
}
