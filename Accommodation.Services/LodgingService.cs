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
    public class LodgingService : ILodgingService
    {
        private ILodgingRepository _repository;
        private IEmailService _emailService;

        public LodgingService(ILodgingRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public Lodging Register(Lodging item)
        {
            string body = "Type:" + item.PublishType.ID + " Name:" + item.Name + " ID:" + item.ID + " Location:" + item.Location.ID;
            _emailService.Send("turismoalojamientos@gmail.com", "New Accommodation", body, true);
            return _repository.Register(item);
        }

        public bool Update(Lodging domain)
        {
            string body = "Type:" + domain.PublishType.ID + " Name:" + domain.Name + " ID:" + domain.ID + " Location:" + domain.Location.ID;
            _emailService.Send("turismoalojamientos@gmail.com", "Update Accommodation", body, true);
            return _repository.Update(domain);
        }

        public IList<Lodging> GetByLocation(int location)
        {
            return _repository.GetByLocation(location);
        }

        public IList<Lodging> GetByUser(string user)
        {
            return _repository.GetByUser(user);
        }

        public Lodging GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Lodging GetByHashTag(string hashtag)
        {
            return _repository.GetByHashTag(hashtag);
        }

        public IList<Lodging> GetAll()
        {
            return _repository.GetAll();
        }

        
    }
}
