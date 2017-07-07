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
    public class LodgingMap : ILodgingMap
    {
        ILodgingService _service;
        IPhotoService _photoService;


        public LodgingMap(ILodgingService service, IPhotoService photoService)
        {
            _service = service;
            _photoService = photoService;
        }

        public LodgingViewModel Register(LodgingViewModel viewModel)
        {
            try
            {
                Lodging domain = ViewModelToDomain(viewModel);
                return DomainToViewModel(_service.Register(domain));
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public bool Update(LodgingViewModel viewModel)
        {
            try
            {
                Lodging domain = ViewModelToDomain(viewModel);
                return _service.Update(domain);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public IList<LodgingViewModel> GetByLocation(int location)
        {
            try
            {
                return _service.GetByLocation(location).Select(x => DomainToViewModel(x)).OrderByDescending(y => y.PublishType.ID).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public IList<LodgingViewModel> GetByUser(string user)
        {
            try
            {
                return _service.GetByUser(user).Select(x => DomainToViewModel(x)).OrderByDescending(y => y.PublishType.ID).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }


        public LodgingViewModel GetById(int id)
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


        public LodgingViewModel GetByHashTag(string hashtag)
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

        private LodgingViewModel DomainToViewModel(Lodging domain)
        {
            LodgingViewModel rec = new LodgingViewModel();

            rec.Email = domain.Email;
            rec.Hashtag = domain.Hashtag;
            rec.ID = domain.ID;
            rec.Phone = domain.Phone;
            rec.Capacity = domain.Capacity;
            rec.WebPage = domain.WebPage;
            rec.LocationTag = domain.Location.Hashtag;
            rec.Description = domain.Description;
            rec.Name = domain.Name;
            rec.PublishType = new PublishTypeViewModel() { ID = domain.PublishType.ID, Name = domain.PublishType.Name };
            rec.Location = new LocationViewModel() { ID = domain.Location.ID, Name = domain.Location.Name, State = new StateViewModel() { ID = domain.Location.State != null? domain.Location.State.ID : 0 } };
            rec.AccommodationType = new AccommodationTypeViewModel() { ID = domain.AccommodationType.ID, Name = domain.AccommodationType.Name };
            rec.Photos = _photoService.GetByLodging(rec.ID).Select(x => new PhotoViewModel() { ID = x.ID, Description = x.Description, URL = x.URL }).ToList();

            return rec;
        }

        private Lodging ViewModelToDomain(LodgingViewModel domain)
        {
            Lodging rec;

            if (domain.ID == 0)
                rec = new Lodging();
            else
                rec = _service.GetById(domain.ID);

            rec.Email = domain.Email;
            rec.Hashtag = domain.Hashtag;
            rec.ID = domain.ID;
            rec.Phone = domain.Phone;
            rec.Capacity = domain.Capacity;
            rec.WebPage = domain.WebPage;
            rec.Description = domain.Description;
            rec.Name = domain.Name;
            rec.Active = domain.Active;
            rec.UserID = domain.UserID;
            rec.PublishType = new PublishType() { ID = domain.PublishType.ID };
            rec.Location = new Location() { ID = domain.Location.ID };
            rec.AccommodationType = new AccommodationType() { ID = domain.AccommodationType.ID };
            //rec.Photos = _photoService.GetByLodging(rec.ID).Select(x => new PhotoViewModel() { ID = x.ID, Description = x.Description, URL = x.URL }).ToList();

            return rec;
        }

        public HashTagViewModel DomainToHashTagViewModel(Lodging domain)
        {
            HashTagViewModel rec = new HashTagViewModel();

            rec.Name = domain.Name;
            rec.HashTag = domain.Hashtag;      
            rec.LocationTag = domain.Location.Hashtag;
            rec.ID = domain.ID;
            rec.Type = "Lodging";

            return rec;
        }


    }
}
