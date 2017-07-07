using Accommodation.Interfaces.Maps;
using Accommodation.ViewModels;
using Accommodation.Web.API.Result_Handlers;
using System.Collections.Generic;
using System.Web.Http;


namespace Accommodation.Web.API.Controllers
{
    public class AdvertisingController : BaseController
    {
        IAdvertisingMap _advertisingMap;


        public AdvertisingController(IAdvertisingMap advertisingMap)
            : base("LodgingController")
        {
            _advertisingMap = advertisingMap;
        }


        // GET api/lodgings/hashtag
        [HttpGet]
        [Route("advertisings/global")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetAllHashTag()
        {
            return new GetListHandler<IList<AdvertisingViewModel>>(_advertisingMap.GetGlobals(), Request);
        }


        // GET api/lodgings/{location_id}
        [HttpGet]
        [Route("advertisings/{location_id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByLocation(int location_id)
        {
            return new GetListHandler<IList<AdvertisingViewModel>>(_advertisingMap.GetByLocation(location_id), Request);
        }
    }
}