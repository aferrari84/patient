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
    public class PhotoService : IPhotoService
    {
        private IPhotoRepository _repository;

        public PhotoService(IPhotoRepository repository)
        {
            _repository = repository;
        }

        public bool Create(Photo photo)
        {
            return _repository.Create(photo);
        }

        public IList<Photo> GetByHashtag(string hashtag)
        {
            return _repository.GetByHashtag(hashtag);
        }

        public IList<Photo> GetByLodging(int lodgingId)
        {
            return _repository.GetByLodging(lodgingId);
        }

        public IList<Photo> GetByService(int serviceId)
        {
            return _repository.GetByService(serviceId);
        }

        public IList<Photo> GetByTourism(int tourismId)
        {
            return _repository.GetByTourism(tourismId);
        }

        public bool Remove(string hashtag)
        {
            return _repository.Remove(hashtag);
        }

        public bool Rename(string newHashtag, string oldHashTag)
        {
            return _repository.Rename(newHashtag, oldHashTag);
        }
    }
}
