using Accommodation.Interfaces.Maps;
using Accommodation.ViewModels.Location;
using Accommodation.Web.API.Attributes;
using System.Collections.Generic;
using System.Web.Http;
using Accommodation.Web.API.Result_Handlers;

namespace Accommodation.Web.API.Controllers
{
    public class LocationController : BaseController
    {
        ILocationMap _locationMap;


        public LocationController(ILocationMap locationMap)
            : base("LocationController")
        {
            _locationMap = locationMap;
        }

        // GET api/cache
        [HttpGet]
        [Route("locations/cache")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")] 
        public IHttpActionResult UpdateCahceHashTag()
        {
            Cache.Cache.ReloadLocationCache(_locationMap);
            return new GetListHandler<bool>(true, Request);
        }


        // GET api/locations
        [HttpGet]
        [Route("locations")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetAll()
        {
            var result = Cache.Cache.GetLocationHashTag(_locationMap);
            return new GetListHandler<IList<LocationViewModel>>(result, Request);
        }


        // GET api/locations-state/{id}
        [HttpGet]
        [Route("locations-state/{id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByState(int id)
        {
            var result = _locationMap.GetByState(id);
            return new GetListHandler<IList<LocationViewModel>>(result, Request);
        }


    }
}
