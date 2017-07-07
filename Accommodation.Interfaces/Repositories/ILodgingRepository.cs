using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;

namespace Accommodation.Interfaces.Repositories
{
    public interface ILodgingRepository
    {
        Lodging Register(Lodging domain);
        bool Update(Lodging domain);
        IList<Lodging> GetByLocation(int location);
        IList<Lodging> GetByUser(string user);
        Lodging GetById(int id);
        Lodging GetByHashTag(string hashtag);
        IList<Lodging> GetAll();
    }
}
