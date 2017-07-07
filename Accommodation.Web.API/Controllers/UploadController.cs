using Accommodation.Interfaces.Maps;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;


namespace Accommodation.Web.API.Controllers
{

    public class UploadController : BaseController
    {
        IPhotoMap _photoMap;

        private readonly string _sRoot;

        public UploadController(IPhotoMap photoMap)
            : base("UploadController")
        {
            _photoMap = photoMap;
            _sRoot = HostingEnvironment.MapPath("~/Uploads");
        }


        //[Route("upload"), AcceptVerbs("GET")]
        //public IHttpActionResult Upload([FromUri] UploadBindingModel model)
        //{
        //    if (IsChunkHere(model.FlowChunkNumber, model.FlowIdentifier)) return Ok();
        //    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Accepted));
        //}

        [Route("upload"), AcceptVerbs("POST")]
        public async Task<IHttpActionResult> Upload(string hashtag, string isNew)
        {
            // ensure that the request contains multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            if (!Directory.Exists(_sRoot)) Directory.CreateDirectory(_sRoot);
            MultipartFormDataStreamProvider provider =
                new MultipartFormDataStreamProvider(_sRoot);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                int nChunkNumber = Convert.ToInt32(provider.FormData["flowChunkNumber"]);
                int nTotalChunks = Convert.ToInt32(provider.FormData["flowTotalChunks"]);
                string sIdentifier = provider.FormData["flowIdentifier"];
                string sFileName = provider.FormData["flowFilename"];

                // rename the generated file
                MultipartFileData chunk = provider.FileData[0]; // Only one file in multipart message
                RenameChunk(chunk, nChunkNumber, sIdentifier);

                // assemble chunks into single file if they're all here
                TryAssembleFile(sIdentifier, nTotalChunks, sFileName, hashtag, isNew);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private string GetChunkFileName(int chunkNumber, string identifier)
        {
            return Path.Combine(_sRoot,
                String.Format(CultureInfo.InvariantCulture, "{0}_{1}",
                    identifier, chunkNumber));
        }

        private void RenameChunk(MultipartFileData chunk, int chunkNumber, string identifier)
        {
            string sGeneratedFileName = chunk.LocalFileName;
            string sChunkFileName = GetChunkFileName(chunkNumber, identifier);
            if (File.Exists(sChunkFileName)) File.Delete(sChunkFileName);
            File.Move(sGeneratedFileName, sChunkFileName);
        }

        private string GetFileName(string identifier)
        {
            return Path.Combine(_sRoot, identifier);
        }

        private bool IsChunkHere(int chunkNumber, string identifier)
        {
            string sFileName = GetChunkFileName(chunkNumber, identifier);
            return File.Exists(sFileName);
        }

        private bool AreAllChunksHere(string identifier, int totalChunks)
        {
            for (int nChunkNumber = 1; nChunkNumber <= totalChunks; nChunkNumber++)
                if (!IsChunkHere(nChunkNumber, identifier)) return false;
            return true;
        }

        private void TryAssembleFile(string identifier, int totalChunks, string filename, string hashtag, string isNew)
        {
            if (!AreAllChunksHere(identifier, totalChunks)) return;

            // create a single file
            string sConsolidatedFileName = GetFileName(identifier);
            using (Stream destStream = File.Create(sConsolidatedFileName, 15000))
            {
                for (int nChunkNumber = 1; nChunkNumber <= totalChunks; nChunkNumber++)
                {
                    string sChunkFileName = GetChunkFileName(nChunkNumber, identifier);
                    using (Stream sourceStream = File.OpenRead(sChunkFileName))
                    {
                        sourceStream.CopyTo(destStream);
                    }
                } //efor
                destStream.Close();
            }

            // rename consolidated with original name of upload
            // strip to filename if directory is specified (avoid cross-directory attack)
            filename = Path.GetFileName(filename);
            Debug.Assert(filename != null);

            string sRealFileName = Path.Combine(_sRoot, filename);
            if (File.Exists(filename)) File.Delete(sRealFileName);

            

            File.Move(sConsolidatedFileName, sRealFileName);

            // delete chunk files
            for (int nChunkNumber = 1; nChunkNumber <= totalChunks; nChunkNumber++)
            {
                string sChunkFileName = GetChunkFileName(nChunkNumber, identifier);
                File.Delete(sChunkFileName);
            } //efor

            _photoMap.Create(new ViewModels.PhotoViewModel() { Description = hashtag, URL = "uploads/" + filename });

        }
    }


    public sealed class UploadBindingModel
    {
        public int FlowChunkNumber { get; set; }
        public long FlowChunkSize { get; set; }
        public long FlowCurrentChunkSize { get; set; }
        public long FlowTotalSize { get; set; }
        public string FlowIdentifier { get; set; }
        public string FlowFileName { get; set; }
        public string FlowRelativePath { get; set; }
        public int FlowTotalChunks { get; set; }
    }
}