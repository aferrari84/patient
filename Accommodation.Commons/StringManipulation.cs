using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Commons
{
    public static class StringManipulation
    {
        public static string StringToUriEncode(string value)
        {
            try
            {
                return System.Web.HttpUtility.UrlPathEncode(value);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public static string HtmlToString(string value)
        {
            try
            {
                return System.Web.HttpUtility.HtmlDecode(value);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}
