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
    public class TourismRepository : BaseRepository, ITourismRepository
    {
        public TourismRepository(IAccommodationContext context, ICurrentUserService userService)
            : base(context, userService)
        {
        }
        private const string EnterDate = "EnterDate";


        public Tourism Register(Tourism domain)
        {
            try
            {
                domain.Created = DateTime.Now;
                domain.Deleted = false;

                var emp = Insert<Tourism>(domain);
                return emp;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public IList<Tourism> GetByLocation(int location)
        {

            try
            {
                var list = (from rec in Context.Tourisms.Include("Location").Include("TourismType")
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

        public IList<Tourism> GetByUser(string user)
        {

            try
            {
                var list = (from rec in Context.Tourisms.Include("Location").Include("TourismType")
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

        public Tourism GetById(int id)
        {

            try
            {
                var item = (from rec in Context.Tourisms.Include("Location").Include("TourismType")
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

        public Tourism GetByHashTag(string hashtag)
        {

            try
            {
                var item = (from rec in Context.Tourisms.Include("Location").Include("TourismType")
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

        public IList<Tourism> GetAll()
        {

            try
            {
                var list = (from rec in Context.Tourisms.Include("Location")
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
