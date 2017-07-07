using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Accommodation.Web.API.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                actionExecutedContext.Request.Content = new StringContent(actionExecutedContext.Exception.Message);

                if (actionExecutedContext.Exception is NotImplementedException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                    return;
                }

                if (actionExecutedContext.Exception is UnsupportedMediaTypeException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
                    return;
                }

                if (actionExecutedContext.Exception is InvalidOperationException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    return;
                }
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

                ErrorManager.ErrorHandler.HandleError(actionExecutedContext.Exception);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
    }
}