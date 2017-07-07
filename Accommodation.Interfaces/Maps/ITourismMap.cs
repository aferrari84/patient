using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;
using Accommodation.ViewModels;


namespace Accommodation.Interfaces.Maps
{
    public interface ITourismMap
    {
        TourismViewModel Register(TourismViewModel viewModel);

        IList<TourismViewModel> GetByLocation(int location);

        IList<TourismViewModel> GetByUser(string user);

        TourismViewModel GetById(int id);
        TourismViewModel GetByHashTag(string hashtag);

        IList<HashTagViewModel> GetAllHashTag();

        
    }
}
