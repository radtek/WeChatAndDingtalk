using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.Infrastructure.RequestUtility;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Beisen.AppConnect.Infrastructure.Template
{
    /// <summary>
    /// 接口模板
    /// </summary>
    public abstract class ApiTemplate
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        protected readonly int ApiType;

        /// <summary>
        /// 接口名称
        /// </summary>
        protected readonly string ApiName;

        /// <summary>
        /// 请求URL
        /// </summary>
        protected string RequestUrl;

        /// <summary>
        /// 请求Body
        /// </summary>
        protected string RequestBody;

        /// <summary>
        /// 占位符正则表达式
        /// </summary>
        private const string Pattern = "<%.*?%>";

        /// <summary>
        /// 初始化接口模板
        /// </summary>
        /// <param name="apiType">接口类型</param>
        /// <param name="apiName">接口名称</param>
        protected ApiTemplate(int apiType, string apiName)
        {
            ApiType = apiType;
            ApiName = apiName;
        }

        /// <summary>
        /// 获取请求URL
        /// </summary>
        /// <returns></returns>
        public string GetRequestUrl()
        {
            if (!string.IsNullOrWhiteSpace(RequestUrl))
            {
                return RequestUrl;
            }
            //获取配置文件
            var apiConfig = AppConnectRequestTemplateConfig.Cache[ApiType][ApiName];
            //处理Url
            RequestUrl = FillTemplate(apiConfig.Url);
            return RequestUrl;
        }

        /// <summary>
        /// 获取请求Body
        /// </summary>
        /// <returns></returns>
        public string GetRequestBody()
        {
            if (!string.IsNullOrWhiteSpace(RequestBody))
            {
                return RequestBody;
            }

            var bodyTemplate = AppConnectRequestTemplateConfig.Cache[ApiType][ApiName].RequestBody;
            var body = FillTemplate(bodyTemplate);
            RequestBody = body;
            return body;
        }

        /// <summary>
        /// 获取请求Method
        /// </summary>
        /// <returns></returns>
        public Method GetRequestMethod()
        {
            var method = AppConnectRequestTemplateConfig.Cache[ApiType][ApiName].Method;
            return MethodHelper.GetMethod(method);
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetResponse()
        {
            var result = Request.SendRequest(GetRequestUrl(), GetRequestMethod(), GetRequestBody());
            if (string.IsNullOrWhiteSpace(result))
            {
                return null;
            }
            var json = SerializeHelper.Deserialize<JObject>(result);
            var responseMappings = AppConnectRequestTemplateConfig.Cache[ApiType][ApiName].ResponseMapping;
            var responseResult = new Dictionary<string, string>(responseMappings.Count);
            foreach (var mapping in responseMappings)
            {
                var mappingArr = mapping.Value.Split('.');
                var jToken = json[mappingArr[0]];
                for (var i = 1; i < mappingArr.Length; i++)
                {
                    jToken = jToken[mappingArr[i]];
                }
                responseResult.Add(mapping.Key, jToken.ToString());
            }
            return responseResult;
        }

        /// <summary>
        /// 填充模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private string FillTemplate(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                return null;
            }

            var matches = Regex.Matches(template, Pattern);
            var args = matches.Cast<object>().ToDictionary<object, string, string>(item => item.ToString(), item => null);
            var argsAfterFill = GetArgs(args);
            var result = template;
            foreach (var arg in argsAfterFill)
            {
                result = result.Replace(arg.Key, arg.Value);
            }
            return result;
        }

        /// <summary>
        /// 获取模板填充数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected abstract Dictionary<string, string> GetArgs(IDictionary<string, string> args);
    }
}