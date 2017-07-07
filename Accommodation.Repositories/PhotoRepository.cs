using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Commons;
using Accommodation.Interfaces;
using Accommodation.Interfaces.Repositories;
using Accommodation.Interfaces.Services;
using Accommodation.Models;
using Accommodation.Models.Context;
using Accommodation.Models.Enumerators;
using Accommodation.Models.Interfaces;
using System.Data.Entity;

namespace Accommodation.Repositories
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public PhotoRepository(IAccommodationContext context, ICurrentUserService userService)
            : base(context, userService)
        {
        }

        public bool Create(Photo photo)
        {
            photo.Created = DateTime.Now;
            photo.Deleted = false;

            photo.Lodging = Context.Lodgings.Where(x => x.Hashtag == photo.Description).SingleOrDefault();
            photo.Service = Context.Services.Where(x => x.Hashtag == photo.Description).SingleOrDefault();
            photo.Tourism = Context.Tourisms.Where(x => x.Hashtag == photo.Description).SingleOrDefault();

            var item = Insert<Photo>(photo);
            return true;
        }

        public IList<Photo> GetByHashtag(string hashtag)
        {
            try
            {
                var list = (from rec in Context.Photos
                            where
                            rec.Description == hashtag &&
                            !rec.Deleted
                            select rec).ToList();


                return list;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public IList<Photo> GetByLodging(int lodgingId)
        {

            try
            {
                var list = (from rec in Context.Photos
                                 where
                                 rec.Lodging.ID == lodgingId &&
                                 !rec.Deleted
                                 select rec).ToList();


                return list;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public IList<Photo> GetByService(int serviceId)
        {

            try
            {
                var list = (from rec in Context.Photos
                            where
                            rec.Service.ID == serviceId &&
                            !rec.Deleted
                            select rec).ToList();


                return list;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public IList<Photo> GetByTourism(int tourismId)
        {

            try
            {
                var list = (from rec in Context.Photos
                            where
                            rec.Tourism.ID == tourismId &&
                            !rec.Deleted
                            select rec).ToList();


                return list;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public bool Remove(string hashtag)
        {
            var photos = GetByHashtag(hashtag);

            foreach(Photo photo in photos )
            {
                photo.Updated = DateTime.Now;
                photo.Deleted = true;

                var item = Delete<Photo>(photo);
            }

           
            return true;
        }

        public bool Rename(string newHashtag, string oldHashtag)
        {
            var photos = GetByHashtag(oldHashtag);

            foreach (Photo photo in photos)
            {
                photo.Updated = DateTime.Now;
                photo.Description = newHashtag;
                photo.URL = photo.URL.Replace(oldHashtag, newHashtag);

                var item = Update<Photo>(photo);
            }


            return true;
        }
    }
}
