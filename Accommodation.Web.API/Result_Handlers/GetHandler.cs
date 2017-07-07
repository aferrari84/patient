﻿using System;
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
    public class GetHandler<T> : BaseHandler<T>
    {
        public GetHandler(T responseData, HttpRequestMessage request)
        {
            try
            {
                IResponseMessage<T> response = new ResponseMessage<T>();
                response.ResponseData = responseData;
                response.IsValid = true;
                HttpStatusCode = System.Net.HttpStatusCode.OK;
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