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
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Runtime.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Hosting;
using System.Globalization;

namespace Accommodation.Web.API.Controllers
{
    public class LodgingController : BaseController
    {
        ILodgingMap _lodgingMap;
        IPhotoMap _photoMap;


        public LodgingController(ILodgingMap lodgingMap, IPhotoMap photoMap)
            : base("LodgingController")
        {
            _lodgingMap = lodgingMap;
            _photoMap = photoMap;
        }


        // POST: api/lodgings
        [HttpPost]
        [Route("lodgings")]
        //[CustomAuthorize(Rights = "CanCreate", Module = "employees")]
        public IHttpActionResult Post(LodgingViewModel data)
        {

            data.UserID = User.Identity.Name;
            data.Hashtag = data.Name.Replace(" ", "").ToLower();
            data.Active = true;
            return new PostHandler<LodgingViewModel>(_lodgingMap.Register(data), Request);
        }



        [HttpPut]
        [Route("lodgings/{id}")]
        //[CustomAuthorize(Rights = "CanCreate", Module = "employees")]
        public IHttpActionResult Put(LodgingViewModel data, int id)
        {
            var oldEntity = _lodgingMap.GetById(id);

            data.UserID = User.Identity.Name;
            data.Hashtag = data.Name.Replace(" ", "").ToLower();
            data.Active = true;

            var result =  new PostHandler<bool>(_lodgingMap.Update(data), Request);

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


        // GET api/lodgings/{location_id}
        [HttpGet]
        [Route("lodgings/{location_id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByLocation(int location_id)
        {
            return new GetListHandler<IList<LodgingViewModel>>(_lodgingMap.GetByLocation(location_id), Request);
        }

        // GET api/lodgings-user
        [HttpGet]
        [Route("lodgings-byuser")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetByUser()
        {
            return new GetListHandler<IList<LodgingViewModel>>(_lodgingMap.GetByUser(User.Identity.Name), Request);
        }

        // GET api/lodgings/{id}
        [HttpGet]
        [Route("lodging/{id}")]
        //[CustomAuthorize(Rights = "CanView", Module = "locations")]
        public IHttpActionResult GetById(int id)
        {
            return new GetListHandler<LodgingViewModel>(_lodgingMap.GetById(id), Request);
        }


    }
}
