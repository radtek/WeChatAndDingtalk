using System;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http;
using Beisen.AppConnect.Api.Controller.Models;
using Beisen.AppConnect.Infrastructure.Helper;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller
{
    public class WrapedJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public WrapedJsonMediaTypeFormatter() : base()
        {
            SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
            this.UseDataContractJsonSerializer = false;
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value,
                                                                       System.IO.Stream writeStream,
                                                                       System.Net.Http.HttpContent content,
                                                                       System.Net.TransportContext transportContext)
        {
            object model = value;
            if (!(model is ApiResult))
            {
                var restApiRest = new ApiResult();

                model = restApiRest;
                var httpError = value as HttpError;

                if (httpError != null)
                {
                    var errorMessage = GetErrorMessage(httpError);

                    AppConnectLogHelper.Error(errorMessage);

                    var exception = HttpContext.Current.Error as HttpException;
                    if (exception != null)
                    {
                        restApiRest.ErrCode = exception.GetHttpCode();
                        restApiRest.ErrMsg = errorMessage;
                        HttpContext.Current.ClearError();
                        HttpContext.Current.Response.StatusCode = 200;
                    }
                    else
                    {

                        restApiRest.ErrCode = httpError.Message.StartsWith("403:") ? 403 : 500;
                        restApiRest.ErrMsg = errorMessage;
                        HttpContext.Current.Response.StatusCode = 200;
                    }
                }
            }

            return base.WriteToStreamAsync(type, model, writeStream, content, transportContext);
        }

        private string GetErrorMessage(HttpError httpError)
        {
            var message = new StringBuilder();
            foreach (var error in httpError)
            {
                message.Append(error.Key);
                message.Append(":");
                message.Append(error.Value);
                message.Append("。");
            }

            return message.ToString();
        }
    }
}
