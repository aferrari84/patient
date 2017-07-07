using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Accommodation.Interfaces;

namespace Accommodation.Web.API.Result_Handlers
{
    public class BaseHandler<T> : IHttpActionResult
    {
        public IResponseMessage<T> Content { get; set; }
        public HttpRequestMessage Request { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(Request.CreateResponse<IResponseMessage<T>>(HttpStatusCode, Content));
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}