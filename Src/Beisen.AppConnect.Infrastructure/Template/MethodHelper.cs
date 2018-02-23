using System;
using RestSharp;

namespace Beisen.AppConnect.Infrastructure.Template
{
    /// <summary>
    /// RestSharp方法帮助
    /// </summary>
    public static class MethodHelper
    {
        /// <summary>
        /// 根据字符串方法名获取方法枚举
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Method GetMethod(string method)
        {
            method = method.ToUpperInvariant();
            switch (method)
            {
                case "GET":
                    return Method.GET;
                case "POST":
                    return Method.POST;
                case "PUT":
                    return Method.PUT;
                case "DELETE":
                    return Method.DELETE;
                case "HEAD":
                    return Method.HEAD;
                case "OPTIONS":
                    return Method.OPTIONS;
                case "PATCH":
                    return Method.PATCH;
                case "MERGE":
                    return Method.MERGE;
                default:
                    throw new ArgumentException("方法名无效：method=" + method);
            }
        }
    }
}
