using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;

namespace Accommodation.Interfaces.Services
{
    public interface IAdvertisingService
    {
        IList<Advertising> GetByLocation(int location);

        IList<Advertising> GetGlobals();

    }
}
