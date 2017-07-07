using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;
using Accommodation.ViewModels;


namespace Accommodation.Interfaces.Maps
{
    public interface IServiceMap
    {
        ServiceViewModel Register(ServiceViewModel viewModel);
        bool Update(ServiceViewModel viewModel);

        IList<ServiceViewModel> GetByLocation(int location);

        IList<ServiceViewModel> GetByUser(string user);

        ServiceViewModel GetById(int id);
        ServiceViewModel GetByHashTag(string hashtag);

        IList<HashTagViewModel> GetAllHashTag();

        
    }
}
