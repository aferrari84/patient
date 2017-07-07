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
    public class AdvertisingRepository : BaseRepository, IAdvertisingRepository
    {
        public AdvertisingRepository(IAccommodationContext context, ICurrentUserService userService)
            : base(context, userService)
        {
        }
        private const string EnterDate = "EnterDate";

        public IList<Advertising> GetByLocation(int location)
        {

            try
            {
                var list = (from rec in Context.Advertisings.Include("Location").Include("Lodging")
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


        public IList<Advertising> GetGlobals()
        {

            try
            {
                var list = (from rec in Context.Advertisings.Include("Location")
                            where
                                 rec.Location == null &&
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

    }
}
