using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;
using Accommodation.Interfaces;
using Accommodation.ViewModels;

namespace Accommodation.Web.API.Controllers
{
    public class BaseController : ApiController
    {
        private Logger _logger;
        public Logger Log
        {
            get
            {
                if (_logger == null)
                {
                    string childClassName = this.GetType().Name;
                    _logger = LogManager.GetLogger(childClassName);
                }
                return _logger;
            }
        }

        public BaseController(string controllerName = null)
        {
            _logger = LogManager.GetLogger(controllerName);
        }

    }
}
