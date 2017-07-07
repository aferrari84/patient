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
    public class ServiceRepository : BaseRepository, IServiceRepository
    {
        public ServiceRepository(IAccommodationContext context, ICurrentUserService userService)
            : base(context, userService)
        {
        }
        private const string EnterDate = "EnterDate";

        public Service Register(Service domain)
        {
            try
            {
                domain.Created = DateTime.Now;
                domain.Deleted = false;

                domain.PublishType = Context.PublishTypes.Where(x => x.ID == domain.PublishType.ID).Single();
                domain.ServiceType = Context.ServiceTypes.Where(x => x.ID == domain.ServiceType.ID).Single();
                domain.Location = Context.Locations.Where(x => x.ID == domain.Location.ID).Single();

                var emp = Insert<Service>(domain);
                return emp;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public bool Update(Service domain)
        {
            try
            {
                domain.Created = DateTime.Now;
                domain.Deleted = false;

                domain.PublishType = Context.PublishTypes.Where(x => x.ID == domain.PublishType.ID).Single();
                domain.ServiceType = Context.ServiceTypes.Where(x => x.ID == domain.ServiceType.ID).Single();
                domain.Location = Context.Locations.Where(x => x.ID == domain.Location.ID).Single();

                var item = Update<Service>(domain);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public IList<Service> GetByLocation(int location)
        {

            try
            {
                var list = (from rec in Context.Services.Include("Location").Include("ServiceType").Include("PublishType")
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

        public IList<Service> GetByUser(string user)
        {

            try
            {
                var list = (from rec in Context.Services.Include("Location").Include("ServiceType").Include("PublishType")
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

        public Service GetById(int id)
        {

            try
            {
                var item = (from rec in Context.Services.Include("Location").Include("Location.State").Include("ServiceType").Include("PublishType")
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

        public Service GetByHashTag(string hashtag)
        {

            try
            {
                var item = (from rec in Context.Services.Include("Location").Include("ServiceType").Include("PublishType")
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

        public IList<Service> GetAll()
        {

            try
            {
                var list = (from rec in Context.Services.Include("Location")
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
