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
    public class TourismMap : ITourismMap
    {
        ITourismService _service;
        IPhotoService _photoService;


        public TourismMap(ITourismService service, IPhotoService photoService)
        {
            _service = service;
            _photoService = photoService;
        }

        public TourismViewModel Register(TourismViewModel viewModel)
        {
            try
            {
                Tourism domain = ViewModelToDomain(viewModel);
                return DomainToViewModel(_service.Register(domain));
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public IList<TourismViewModel> GetByLocation(int location)
        {
            try
            {
                return _service.GetByLocation(location).Select(x => DomainToViewModel(x)).OrderByDescending(y => y.Outstanding).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public IList<TourismViewModel> GetByUser(string user)
        {
            try
            {
                return _service.GetByUser(user).Select(x => DomainToViewModel(x)).OrderByDescending(y => y.Outstanding).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public TourismViewModel GetById(int id)
        {
            try
            {
                return DomainToViewModel(_service.GetById(id));
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public TourismViewModel GetByHashTag(string hashtag)
        {
            try
            {
                return DomainToViewModel(_service.GetByHashTag(hashtag));
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public IList<HashTagViewModel> GetAllHashTag()
        {
            try
            {
                return _service.GetAll().Select(x => DomainToHashTagViewModel(x)).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        private TourismViewModel DomainToViewModel(Tourism domain)
        {
            TourismViewModel rec = new TourismViewModel();

            rec.Email = domain.Email;
            rec.Hashtag = domain.Hashtag;
            rec.ID = domain.ID;
            rec.Phone = domain.Phone;
            rec.WebPage = domain.WebPage;
            rec.LocationTag = domain.Location.Hashtag;
            rec.Description = domain.Description;
            rec.Name = domain.Name;
            rec.Location = new LocationViewModel() { ID = domain.Location.ID, Name = domain.Location.Name };
            rec.TourismType = new TourismTypeViewModel() { ID = domain.TourismType.ID, Name = domain.TourismType.Name };
            rec.Photos = _photoService.GetByTourism(rec.ID).Select(x => new PhotoViewModel() { ID = x.ID, Description = x.Description, URL = x.URL }).ToList();

            return rec;
        }

        private Tourism ViewModelToDomain(TourismViewModel domain)
        {
            Tourism rec = new Tourism();

            rec.Email = domain.Email;
            rec.Hashtag = domain.Hashtag;
            rec.ID = domain.ID;
            rec.Phone = domain.Phone;
            rec.WebPage = domain.WebPage;
            //rec.Hashtag = domain.Location.LocationTag;
            rec.Description = domain.Description;
            rec.Name = domain.Name;
            //rec.Location = new LocationViewModel() { ID = domain.Location.ID, Name = domain.Location.Name };
            //rec.TourismType = new TourismTypeViewModel() { ID = domain.TourismType.ID, Name = domain.TourismType.Name };
            //rec.Photos = _photoService.GetByLodging(rec.ID).Select(x => new PhotoViewModel() { ID = x.ID, Description = x.Description, URL = x.URL }).ToList();

            return rec;
        }

        public HashTagViewModel DomainToHashTagViewModel(Tourism domain)
        {
            HashTagViewModel rec = new HashTagViewModel();

            rec.Name = domain.Name;
            rec.HashTag = domain.Hashtag;      
            rec.LocationTag = domain.Location.Hashtag;
            rec.ID = domain.ID;
            rec.Type = "Tourism";

            return rec;
        }


    }
}
