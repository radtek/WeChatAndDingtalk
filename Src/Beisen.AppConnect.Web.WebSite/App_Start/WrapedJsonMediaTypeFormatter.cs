using System;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace Beisen.AppConnect.Web.WebSite
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
            //object model = value;
            //if (value is WebApiResult || value is WebApiResult<ThreadStaticAttribute>)
            //{
            //    var tempValue = value as ResultModel;

            //    if (tempValue.ErrCode == 0)
            //    {
            //        model = tempValue.Data;
            //    }
            //    else
            //    {
            //        model = new WeChatResult { ErrCode = tempValue.Code, ErrMsg = tempValue.Message };
            //    }
            //}

            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }
    }
}