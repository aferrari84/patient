using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Accommodation.Api.Providers
{
    public class MyMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public MyMultipartFormDataStreamProvider(string path)
            : base(path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            try
            {
                string fileName;
                if (!string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))
                {
                    fileName = headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    int i = 1;

                    while (File.Exists(string.Format("{0}\\{1}", RootPath, fileName)))
                    {
                        var info = new FileInfo(string.Format("{0}\\{1}", RootPath, headers.ContentDisposition.FileName.Replace("\"", string.Empty)));
                        fileName = string.Format("{0}({1}){2}", info.Name.Remove(info.Name.IndexOf(info.Extension)), i, info.Extension);

                        i++;
                    }
                }
                else
                {
                    fileName = Guid.NewGuid().ToString() + ".data";
                }

                return fileName.Replace("\"", string.Empty);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}