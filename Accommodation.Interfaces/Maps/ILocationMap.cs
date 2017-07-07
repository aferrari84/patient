using Accommodation.ViewModels.Location;
using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Interfaces.Maps
{
    public interface ILocationMap
    {
        IList<LocationViewModel> GetAll();
        IList<LocationViewModel> GetByState(int id);

        LocationViewModel DomainToViewModel(Location domain);
    }
}
