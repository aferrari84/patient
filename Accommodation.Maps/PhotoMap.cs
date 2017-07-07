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
    public class PhotoMap : IPhotoMap
    {
        IPhotoService _service;
        ILodgingService _serviceLodging;
        IServiceService _serviceService;
        ITourismService _serviceTourism;


        public PhotoMap(IPhotoService service, ILodgingService serviceLodging, IServiceService serviceService, ITourismService serviceTourism)
        {
            _service = service;
            _serviceLodging = serviceLodging;
            _serviceService = serviceService;
            _serviceTourism = serviceTourism;
        }

        public IList<PhotoViewModel> GetByHashtag(string hashtag)
        {
            try
            {
                return _service.GetByHashtag(hashtag).Select(x => DomainToViewModel(x)).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }


        public bool Create(PhotoViewModel photo)
        {
            try
            {
                return _service.Create(ViewModelToDomain(photo));
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }


        public bool Remove(string hashtag)
        {
            try
            {
                return _service.Remove(hashtag);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public bool Rename(string newHashtag, string oldHashtag)
        {
            try
            {
                return _service.Rename(newHashtag, oldHashtag);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public PhotoViewModel DomainToViewModel(Photo domain)
        {
            PhotoViewModel rec = new PhotoViewModel();

            rec.ID = domain.ID;
            rec.Description = domain.Description;
            rec.URL = domain.URL;

            return rec;
        }

        public Photo  ViewModelToDomain(PhotoViewModel domain)
        {
            Photo rec = new Photo();

            rec.ID = domain.ID;
            rec.Description = domain.Description;
            rec.URL = domain.URL;

            return rec;
        }

    }
}
