using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accommodation.Interfaces
{
    public interface IResponseMessage<T>
    {
        bool IsValid { get; set; }
        HttpStatusCode StatusCode { get; set; }
        T ResponseData { get; set; }
        Exception Exception { get; set; }
    }
}
