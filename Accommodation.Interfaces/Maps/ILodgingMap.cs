using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;
using Accommodation.ViewModels;


namespace Accommodation.Interfaces.Maps
{
    public interface ILodgingMap
    {
        LodgingViewModel Register(LodgingViewModel viewModel);
        bool Update(LodgingViewModel viewModel);

        IList<LodgingViewModel> GetByLocation(int location);

        IList<LodgingViewModel> GetByUser(string user);

        LodgingViewModel GetById(int id);
        LodgingViewModel GetByHashTag(string hashtag);

        IList<HashTagViewModel> GetAllHashTag();
        
    }
}
