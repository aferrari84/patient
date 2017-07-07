using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Accommodation.Interfaces;

namespace Accommodation.Web.API.Result_Handlers
{
    public class CsvExportHandler<T>
    {
        public HttpResponseMessage Export(List<T> responseData, string fileName)
        {

            try
            {
                string result = ConvertListToCsv<T>(responseData);

                HttpResponseMessage message = new HttpResponseMessage();

                if (!string.IsNullOrEmpty(result))
                {
                    message.StatusCode = HttpStatusCode.OK;
                    message.Content = new StringContent(result, Encoding.UTF8);
                    message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    message.Content.Headers.ContentDisposition.FileName = fileName;
                }
                else
                {
                    message.StatusCode = HttpStatusCode.NotFound;
                }

                return message;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        private string ConvertListToCsv<T>(List<T> list)
        {
            try
            {
                string response = string.Empty;

                if (list != null && list.Count != 0)
                {
                    Type t = typeof(T);
                    object obj = Activator.CreateInstance(t);
                    PropertyInfo[] props = obj.GetType().GetProperties();

                    string body = string.Empty;
                    StringBuilder sb = new StringBuilder();

                    // Header
                    string header = string.Join(",", props.Where(x => x.IsDefined(typeof(DisplayNameAttribute), false))
                        .Select(x => new { Name = x.GetCustomAttributes(typeof(DisplayNameAttribute), false).Cast<DisplayNameAttribute>().FirstOrDefault().DisplayName.ToString() })
                        .Select(x => x.Name));

                    sb.AppendLine(header);

                    // Body
                    foreach (T item in list)
                    {
                        foreach (PropertyInfo pi in props.Where(x => x.IsDefined(typeof(DisplayNameAttribute), false)))
                        {
                            body += string.Concat(Convert.ToString(item.GetType().GetProperty(pi.Name).GetValue(item, null)).Replace(',', ' '), ',');
                        }

                        sb.AppendLine(body);
                        body = string.Empty;
                    }

                    response = sb.ToString();

                }

                return response;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}
