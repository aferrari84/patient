using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Commons
{
    public static class CustomExtensions
    {
        public static StringBuilder Replace(this StringBuilder sb,
            IDictionary<string, string> dict)
        {
            try
            {
                foreach (KeyValuePair<string, string> replacement in dict)
                {
                    sb.Replace(replacement.Key, replacement.Value);
                }

                return sb;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
        public static string Replace(this string str, IDictionary<string, string> dict)
        {
            try
            {
                StringBuilder sb = new StringBuilder(str);

                return sb.Replace(dict).ToString();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }


    }
}
