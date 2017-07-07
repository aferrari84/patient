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
    public class AdvertisingService : IAdvertisingService
    {
        private IAdvertisingRepository _repository;

        public AdvertisingService(IAdvertisingRepository repository)
        {
            _repository = repository;
        }
        public IList<Advertising> GetByLocation(int location)
        {
            return _repository.GetByLocation(location);
        }

        public IList<Advertising> GetGlobals()
        {
            return _repository.GetGlobals();
        }

    }
}
