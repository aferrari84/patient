using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Interfaces.Common
{
    public interface IConfigurationManager
    {
        dynamic GetAppSettingValue(string keyName);
    }
}
