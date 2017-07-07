using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Accommodation.Commons;
using Accommodation.Interfaces;

namespace Accommodation.Web.API.Result_Handlers
{
    public class DeleteHandler<T> : BaseHandler<T>
    {
        public DeleteHandler(bool responseData, HttpRequestMessage request)
        {
            try
            {
                IResponseMessage<T> response = new ResponseMessage<T>();
                response.IsValid = true;
                response.ResponseData = (T)((object)Convert.ToBoolean(responseData));
                HttpStatusCode = responseData ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound;
                Content = response;
                Request = request;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}