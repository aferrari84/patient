using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Interfaces;
using Accommodation.Interfaces.Repositories;
using Accommodation.Interfaces.Services;
using Accommodation.Models;
using Accommodation.Models.Enumerators;

namespace Accommodation.Service
{
    public class ServiceService : IServiceService
    {
        private IServiceRepository _repository;
        private IEmailService _emailService;

        public ServiceService(IServiceRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public Models.Service Register(Models.Service item)
        {
            string body = "Type:" + item.PublishType.ID + " Name:" + item.Name + " ID:" + item.ID + " Location:" + item.Location.ID;
            _emailService.Send("turismoalojamientos@gmail.com", "New Service", body, true);
            return _repository.Register(item);
        }

        public bool Update(Models.Service item)
        {
            string body = "Type:" + item.PublishType.ID + " Name:" + item.Name + " ID:" + item.ID + " Location:" + item.Location.ID;
            _emailService.Send("turismoalojamientos@gmail.com", "Update Service", body, true);
            return _repository.Update(item);
        }

        public IList<Models.Service> GetByLocation(int location)
        {
            return _repository.GetByLocation(location);
        }

        public IList<Models.Service> GetByUser(string user)
        {
            return _repository.GetByUser(user);
        }

        public Models.Service GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Models.Service GetByHashTag(string hashtag)
        {
            return _repository.GetByHashTag(hashtag);
        }

        public IList<Models.Service> GetAll()
        {
            return _repository.GetAll();
        }

    }
}
