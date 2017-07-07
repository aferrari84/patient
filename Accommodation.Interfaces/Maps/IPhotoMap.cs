using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;
using Accommodation.ViewModels;


namespace Accommodation.Interfaces.Maps
{
    public interface IPhotoMap
    {
        IList<PhotoViewModel> GetByHashtag(string hashtag);

        bool Create(PhotoViewModel photo);

        bool Remove(string hashtag);
        bool Rename(string newHashtag, string oldHashtag);

    }
}
