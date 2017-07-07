using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Interfaces;

namespace Accommodation.Web.API.Result_Handlers
{
    public class ResponseMessage<T> : IResponseMessage<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }

        public bool IsValid { get; set; }

        public T ResponseData { get; set; }

        public Exception Exception { get; set; }

        public ResponseMessage()
        {
            IsValid = true;
            StatusCode = System.Net.HttpStatusCode.OK;
        }
    }

}
