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
using System.Web.Hosting;

namespace Accommodation.Web.API.Controllers
{
    public class ServiceController : BaseController
    {
        IServiceMap _serviceMap;
        IPhotoMap _photoMap;

        public ServiceController(IServiceMap serviceMap, IPhotoMap photoMap)
            : base("ServiceController")
        {
            _serviceMap = serviceMap;
            _photoMap = photoMap;
        }


        // POST: api/services
        [HttpPost]
        [Route("services")]
        //[CustomAuthorize(Rights = "CanCreate", Module = "employees")]
        public IHttpActionResult Post(ServiceViewModel data)
        {
            data.UserID = User.Identity.Name;
            data.Hashtag = data.Name.Replace(" ", "").ToLower();
            data.Active = true;
            return new PostHandler<ServiceViewModel>(_serviceMap.Register(data), Request);
        }

        // POST: api/services
        [HttpPut]
        [Route("services/{id}")]
        //[CustomAuthorize(Rights = "CanCreate", Module = "employees")]
        public IHttpActionResult Put(ServiceViewModel data, int id)
        {
            var oldEntity = _serviceMap.GetById(id);

            data.UserID = User.Identity.Name;
            data.Hashtag = data.Name.Replace(" ", "").ToLower();
            data.Active = true;

            var result = new PostHandler<bool>(_serviceMap.Update(data), Request);

            if (data.PhotoHasChanged)
            {
                _photoMap.Remove(data.Hashtag);

                var _sRoot = HostingEnvironment.MapPath("~/Uploads");
                string[] files = System.IO.Directory.GetFiles(_sRoot, data.Hashtag + "*.*", System.IO.SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                if (oldEntity.Hashtag != data.Hashtag)
                {
                    var _sRoot2 = HostingEnvironment.MapPath("~/Uploads");
                    string[] files2 = System.IO.Directory.GetFiles(_sRoot2, oldEntity.Hashtag + "*.*", System.IO.SearchOption.TopDirectoryOnly);
                    foreach (string file in files2)
                    {
                        File.Delete(file);
                    }

                    _photoMap.Remove(oldEntity.Hashtag);
                }
            }
            else
            {
                if (oldEntity.Hashtag != data.Hashtag)
                {
                    var _sRoot = HostingEnvironment.MapPath("~/Uploads");
                    string[] files = System.IO.Directory.GetFiles(_sRoot, oldEntity.Hashtag + "*.*", System.IO.SearchOption.TopDirectoryOnly);
                    foreach (string file in files)
                    {
                        File.Move(file, file.Replace(oldEntity.Hashtag, data.Hashtag));
                    }

                    _photoMap.Rename(data.Hashtag, oldEntity.Hashtag);
                }
            }

            return result;

             
        }


        // GET api/Services/{location_id}
        [HttpGet]
        [Route("services-location/{location_id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByLocation(int location_id)
        {
            return new GetListHandler<IList<ServiceViewModel>>(_serviceMap.GetByLocation(location_id), Request);
        }


        // GET api/Services-user
        [HttpGet]
        [Route("services-user")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByUser()
        {
            return new GetListHandler<IList<ServiceViewModel>>(_serviceMap.GetByUser(User.Identity.Name), Request);
        }

        // GET api/Services/{id}
        [HttpGet]
        [Route("service/{id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetById(int id)
        {
            return new GetListHandler<ServiceViewModel>(_serviceMap.GetById(id), Request);
        }


    }
}
