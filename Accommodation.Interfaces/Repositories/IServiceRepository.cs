using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;

namespace Accommodation.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Service Register(Service domain);
        bool Update(Service domain);
        IList<Service> GetByLocation(int location);
        IList<Service> GetByUser(string user);
        Service GetById(int id);
        Service GetByHashTag(string hashtag);
        IList<Service> GetAll();
    }
}
