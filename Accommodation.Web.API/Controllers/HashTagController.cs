
using System.Collections.Generic;
using System.Web.Http;
using Accommodation.Interfaces.Maps;
using Accommodation.ViewModels;
using Accommodation.Web.API.Result_Handlers;



namespace Accommodation.Web.API.Controllers
{
    public class HashTagController : BaseController
    {
        ILodgingMap _lodgingMap;
        IServiceMap _serviceMap;
        ITourismMap _tourismMap;


        public HashTagController(ILodgingMap lodgingMap, IServiceMap serviceMap, ITourismMap tourismMap)
            : base("HashTagController")
        {
            _lodgingMap = lodgingMap;
            _serviceMap = serviceMap;
            _tourismMap = tourismMap;
        }


        // GET api/cache
        [HttpGet]
        [Route("hashtags/cache")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")] 
        public IHttpActionResult UpdateCahceHashTag()
        {
            Cache.Cache.ReloadCache(_lodgingMap, _tourismMap, _serviceMap);
            return new GetListHandler<bool>(true, Request);
        }

        // GET api/lodgings/hashtag
        [HttpGet]
        [Route("hashtags")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")] 
        public IHttpActionResult GetAllHashTag()
        {
            var result = Cache.Cache.GetHashTag(_lodgingMap, _tourismMap, _serviceMap);
            return new GetListHandler<IList<HashTagViewModel>>(result, Request);
        }
    
    }
}
