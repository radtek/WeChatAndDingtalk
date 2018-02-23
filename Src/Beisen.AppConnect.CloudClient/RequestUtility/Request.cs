﻿using System;
using System.Net;
using System.Text;
using Beisen.AppConnect.CloudClient.Helper;
using RestSharp;

namespace Beisen.AppConnect.CloudClient.RequestUtility
{
    /// <summary>
    /// 发送http请求
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <param name="method">请求的类型</param>
        /// <param name="data">请求的body</param>
        /// <returns></returns>
        public static string SendRequest(string url, Method method = Method.GET, object data = null)
        {
            var body = string.Empty;
            if (method == Method.POST && data != null)
            {
                body = SerializeHelper.Serialize(data);
            }
            return SendRequestForJson(url, method, body);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T">继承自Result的实体</typeparam>
        /// <param name="url">请求的url</param>
        /// <param name="method">请求的类型</param>
        /// <param name="data">请求的body</param>
        /// <returns></returns>
        public static T SendRequest<T>(string url, Method method = Method.GET, object data = null)
        {
            var body = string.Empty;
            if (method == Method.POST && data != null)
            {
                body = SerializeHelper.Serialize(data);
            }
            return SendRequestForJson<T>(url, method, body);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <param name="method">请求的类型</param>
        /// <param name="data">请求的body的字符串（json序列化）</param>
        /// <returns></returns>
        public static string SendRequestForJson(string url, Method method, string data = null)
        {
            var request = new RestRequest(method);

            if (method == Method.POST && !string.IsNullOrWhiteSpace(data))
            {
                request.AddParameter("application/json", data, ParameterType.RequestBody);
            }

            var client = new RestClient();
            client.BaseUrl = new Uri(url);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content;
                return result;
            }
            throw new Exception("请求失败：" + GetRequetLog(request, response.StatusCode.ToString()));
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T">继承自Result的实体</typeparam>
        /// <param name="url">请求的url</param>
        /// <param name="method">请求的类型</param>
        /// <param name="data">请求的body的字符串（json序列化）</param>
        /// <returns></returns>
        public static T SendRequestForJson<T>(string url, Method method, string data = null)
        {
            var request = new RestRequest(method);

            if (method == Method.POST && !string.IsNullOrWhiteSpace(data))
            {
                request.AddParameter("application/json", data, ParameterType.RequestBody);
            }
            var client = new RestClient();
            client.BaseUrl = new Uri(url);

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content;
                return SerializeHelper.Deserialize<T>(result);
            }
            throw new Exception("请求失败：" + GetRequetLog(request, response.StatusCode.ToString()));
        }

        /// <summary>
        /// 获取请求异常时的message
        /// </summary>
        /// <param name="request">请求体</param>
        /// <param name="msgs">message</param>
        /// <returns></returns>
        private static string GetRequetLog(RestRequest request, params string[] msgs)
        {
            var log = new StringBuilder();
            foreach (var parameter in request.Parameters)
            {
                log.AppendFormat("Name:{0}  value:{1}", parameter.Name, parameter.Value);
            }
            log.AppendFormat("request Method:{0} ", request.Method);
            log.AppendFormat("Resource:{0} ", request.Resource);
            foreach (var msg in msgs)
            {
                log.Append(msg);
            }

            return log.ToString();
        }
    }
}
