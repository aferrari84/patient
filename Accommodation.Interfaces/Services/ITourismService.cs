using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;

namespace Accommodation.Interfaces.Services
{
    public interface ITourismService
    {
        Tourism Register(Tourism domain);

        IList<Tourism> GetByLocation(int location);

        IList<Tourism> GetByUser(string user);

        Tourism GetById(int id);
        Tourism GetByHashTag(string hashtag);

        IList<Tourism> GetAll();

    }
}
