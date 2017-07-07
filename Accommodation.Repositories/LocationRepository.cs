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
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(IAccommodationContext context, ICurrentUserService userService)
            : base(context, userService)
        {
        }
        private const string EnterDate = "EnterDate";

        public IList<Location> GetAll()
        {

            try
            {
                var list = (from rec in Context.Locations.Include("State")
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
