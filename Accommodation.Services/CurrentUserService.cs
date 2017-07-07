using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Accommodation.Interfaces.Services;

namespace Accommodation.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserName()
        {
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return null;
                return HttpContext.Current.User.Identity.Name;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

    }
}
