using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Interfaces.Common;

namespace Accommodation.Commons
{
    public class ConfigurationManager : IConfigurationManager
    {
        public virtual dynamic GetAppSettingValue(string keyName)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[keyName];
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}
