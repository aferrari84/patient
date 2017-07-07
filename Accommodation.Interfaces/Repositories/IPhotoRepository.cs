using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models;

namespace Accommodation.Interfaces.Repositories
{
    public interface IPhotoRepository
    {
        IList<Photo> GetByLodging(int lodgingId);
        IList<Photo> GetByService(int serviceId);
        IList<Photo> GetByTourism(int tourismId);

        IList<Photo> GetByHashtag(string hashtag);

        bool Create(Photo photo);

        bool Remove(string hashtag);
        bool Rename(string newHashtag, string oldHashtag);
    }
}
