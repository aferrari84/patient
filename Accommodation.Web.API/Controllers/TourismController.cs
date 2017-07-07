using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Accommodation.Api.Providers;
using Accommodation.Commons.ExtensionMethods;
using Accommodation.Interfaces.Maps;
using Accommodation.ViewModels;
using Accommodation.Web.API.Attributes;
using Accommodation.Web.API.Result_Handlers;



namespace Accommodation.Web.API.Controllers
{
    public class TourisimController : BaseController
    {
        ITourismMap _tourismMap;


        public TourisimController(ITourismMap tourisimMap)
            : base("TourismController")
        {
            _tourismMap = tourisimMap;
        }


        // POST: api/tourisms
        [HttpPost]
        [Route("tourisms")]
        //[CustomAuthorize(Rights = "CanCreate", Module = "employees")]
        public IHttpActionResult Post(TourismViewModel data)
        {
            return new PostHandler<TourismViewModel>(_tourismMap.Register(data), Request);
        }


        // GET api/tourisms/{location_id}
        [HttpGet]
        [Route("tourisms/{location_id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByLocation(int location_id)
        {
            return new GetListHandler<IList<TourismViewModel>>(_tourismMap.GetByLocation(location_id), Request);
        }

        // GET api/tourisms-user
        [HttpGet]
        [Route("tourisms-user")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByUser()
        {
            return new GetListHandler<IList<TourismViewModel>>(_tourismMap.GetByUser(User.Identity.Name), Request);
        }


        // GET api/tourisms/{id}
        [HttpGet]
        [Route("tourism/{id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetById(int id)
        {
            return new GetListHandler<TourismViewModel>(_tourismMap.GetById(id), Request);
        }


    }
}
