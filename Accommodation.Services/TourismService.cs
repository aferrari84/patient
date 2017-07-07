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
    public class TourismService : ITourismService
    {
        private ITourismRepository _repository;

        public TourismService(ITourismRepository repository)
        {
            _repository = repository;
        }

        public Tourism Register(Tourism item)
        {
            return _repository.Register(item);
        }

        public IList<Tourism> GetByLocation(int location)
        {
            return _repository.GetByLocation(location);
        }

        public IList<Tourism> GetByUser(string user)
        {
            return _repository.GetByUser(user);
        }

        public Tourism GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Tourism GetByHashTag(string hashtag)
        {
            return _repository.GetByHashTag(hashtag);
        }

        public IList<Tourism> GetAll()
        {
            return _repository.GetAll();
        }

    }
}
