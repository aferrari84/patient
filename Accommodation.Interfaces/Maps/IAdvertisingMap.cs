using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;
using Accommodation.ViewModels;


namespace Accommodation.Interfaces.Maps
{
    public interface IAdvertisingMap
    {
        IList<AdvertisingViewModel> GetByLocation(int location);


        IList<AdvertisingViewModel> GetGlobals();

        AdvertisingViewModel DomainToViewModel(Advertising domain);

        
    }
}
