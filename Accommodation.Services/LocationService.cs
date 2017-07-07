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
    public class LocationService : ILocationService
    {
        private ILocationRepository _repository;

        public LocationService(ILocationRepository repository)
        {
            _repository = repository;
        }
        public IList<Location> GetAll()
        {
            return _repository.GetAll();
        }

        

    }
}
