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
    public class LodgingRepository : BaseRepository, ILodgingRepository
    {
        public LodgingRepository(IAccommodationContext context, ICurrentUserService userService)
            : base(context, userService)
        {
        }
        private const string EnterDate = "EnterDate";

        public Lodging Register(Lodging domain)
        {
            try
            {
                domain.Created = DateTime.Now;
                domain.Deleted = false;

                domain.PublishType = Context.PublishTypes.Where(x => x.ID == domain.PublishType.ID).Single();
                domain.AccommodationType = Context.AccommodationTypes.Where(x => x.ID == domain.AccommodationType.ID).Single();
                domain.Location = Context.Locations.Where(x => x.ID == domain.Location.ID).Single();

                var emp = Insert<Lodging>(domain);
                return emp;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public bool Update(Lodging domain)
        {
            try
            {
                domain.Updated = DateTime.Now;

                domain.PublishType = Context.PublishTypes.Where(x => x.ID == domain.PublishType.ID).Single();
                domain.AccommodationType = Context.AccommodationTypes.Where(x => x.ID == domain.AccommodationType.ID).Single();
                domain.Location = Context.Locations.Where(x => x.ID == domain.Location.ID).Single();

                var item = Update<Lodging>(domain);
                return item;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public IList<Lodging> GetByLocation(int location)
        {

            try
            {
                var list = (from rec in Context.Lodgings.Include("Location").Include("AccommodationType").Include("PublishType")
                            where
                                 rec.Location.ID == location &&
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

        public IList<Lodging> GetByUser(string user)
        {

            try
            {
                var list = (from rec in Context.Lodgings.Include("Location").Include("AccommodationType").Include("PublishType")
                            where
                            rec.UserID == user &&
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

        public Lodging GetById(int id)
        {

            try
            {
                var item = (from rec in Context.Lodgings.Include("Location").Include("Location.State").Include("AccommodationType").Include("PublishType")
                            where
                            rec.ID == id &&
                            !rec.Deleted
                            select rec).SingleOrDefault();


                return item;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public Lodging GetByHashTag(string hashtag)
        {

            try
            {
                var item = (from rec in Context.Lodgings.Include("Location").Include("Location.State").Include("AccommodationType").Include("PublishType")
                            where
                            rec.Hashtag == hashtag &&
                            !rec.Deleted
                            select rec).SingleOrDefault();


                return item;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public IList<Lodging> GetAll()
        {

            try
            {
                var list = (from rec in Context.Lodgings.Include("Location")
                            where
                            rec.Active
                            select rec).ToList();


                return list;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

    }
}
