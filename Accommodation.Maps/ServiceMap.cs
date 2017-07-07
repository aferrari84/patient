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
    public class ServiceMap : IServiceMap
    {
        IServiceService _service;
        IPhotoService _photoService;


        public ServiceMap(IServiceService service, IPhotoService photoService)
        {
            _service = service;
            _photoService = photoService;
        }

        public ServiceViewModel Register(ServiceViewModel viewModel)
        {
            try
            {
                Models.Service domain = ViewModelToDomain(viewModel);
                return DomainToViewModel(_service.Register(domain));
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }


        public bool Update(ServiceViewModel viewModel)
        {
            try
            {
                Models.Service domain = ViewModelToDomain(viewModel);
                return _service.Update(domain);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public IList<ServiceViewModel> GetByLocation(int location)
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

        public IList<ServiceViewModel> GetByUser(string user)
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

        public ServiceViewModel GetById(int id)
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


        public ServiceViewModel GetByHashTag(string hashtag)
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

        private ServiceViewModel DomainToViewModel(Models.Service domain)
        {
            ServiceViewModel rec = new ServiceViewModel();

            rec.Email = domain.Email;
            rec.Hashtag = domain.Hashtag;
            rec.ID = domain.ID;
            rec.Phone = domain.Phone;
            rec.WebPage = domain.WebPage;
            rec.LocationTag = domain.Location.Hashtag;
            rec.Description = domain.Description;
            rec.Name = domain.Name;
            rec.PublishType = new PublishTypeViewModel() { ID = domain.PublishType.ID, Name = domain.PublishType.Name };
            rec.Location = new LocationViewModel() { ID = domain.Location.ID, Name = domain.Location.Name, State = new StateViewModel() { ID = domain.Location.State != null ? domain.Location.State.ID : 0 } };
            rec.ServiceType = new ServiceTypeViewModel() { ID = domain.ServiceType.ID, Name = domain.ServiceType.Name };
            rec.Photos = _photoService.GetByService(rec.ID).Select(x => new PhotoViewModel() { ID = x.ID, Description = x.Description, URL = x.URL }).ToList();

            return rec;
        }

        private Models.Service ViewModelToDomain(ServiceViewModel domain)
        {
            Models.Service rec;

            if (domain.ID == 0)
                rec = new Models.Service();
            else
                rec = _service.GetById(domain.ID);

            rec.Email = domain.Email;
            rec.Hashtag = domain.Hashtag;
            rec.ID = domain.ID;
            rec.Phone = domain.Phone;
            rec.WebPage = domain.WebPage;
            rec.Hashtag = domain.Hashtag;
            rec.Description = domain.Description;
            rec.Name = domain.Name;
            rec.Active = domain.Active;
            rec.UserID = domain.UserID;
            rec.PublishType = new PublishType() { ID = domain.PublishType.ID };
            rec.Location = new Location() { ID = domain.Location.ID };
            rec.ServiceType = new ServiceType() { ID = domain.ServiceType.ID };
            //rec.Photos = _photoService.GetByLodging(rec.ID).Select(x => new PhotoViewModel() { ID = x.ID, Description = x.Description, URL = x.URL }).ToList();

            return rec;
        }

        public HashTagViewModel DomainToHashTagViewModel(Models.Service domain)
        {
            HashTagViewModel rec = new HashTagViewModel();

            rec.Name = domain.Name;
            rec.HashTag = domain.Hashtag;      
            rec.LocationTag = domain.Location.Hashtag;
            rec.ID = domain.ID;
            rec.Type = "Service";

            return rec;
        }


    }
}
