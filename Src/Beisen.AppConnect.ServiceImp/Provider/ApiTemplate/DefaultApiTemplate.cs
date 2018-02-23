using System;
using System.Collections.Generic;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Template;
using Beisen.AppConnect.ServiceInterface.Model;
using System.Web;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    /// <summary>
    /// 默认请求模板
    /// </summary>
    public class DefaultApiTemplate : ApiTemplate
    {
        /// <summary>
        /// 第三方账户信息
        /// </summary>
        private readonly AppAccountInfo _appAccountInfo;

        /// <summary>
        /// 扩展信息，用于填充模板数据
        /// </summary>
        private readonly Dictionary<string, string> _extend;

        /// <summary>
        /// 初始化默认请求模板
        /// </summary>
        /// <param name="appAccount"></param>
        /// <param name="apiName"></param>
        /// <param name="extend"></param>
        public DefaultApiTemplate(AppAccountInfo appAccount, string apiName, Dictionary<string, string> extend = null)
            :base(appAccount.Type,apiName)
        {
            _appAccountInfo = appAccount;
            _extend = extend;
        }

        /// <summary>
        /// 获取模板填充数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> GetArgs(IDictionary<string, string> args)
        {
            var result = new Dictionary<string, string>(); 
            foreach (var arg in args)
            {
                var key = arg.Key.ToUpperInvariant();
                string value;
                switch (key)
                {
                    case TemplateConst.AppId:
                        value = _appAccountInfo.AppId;
                        break;
                    case TemplateConst.Secret:
                        value = _appAccountInfo.AppSecret;
                        break;
                    case TemplateConst.RedirectUri:
                        var url = AppConnectHostConfig.Cache[0] + "/Authorize/Callback?batch=" + _extend[TemplateConst.ExtendBatch];
                        value = HttpUtility.UrlEncode(url);
                        break;
                    case TemplateConst.ResponseType:
                        value = "code";
                        break;
                    case TemplateConst.Scope:
                        value = "snsapi_base";
                        break;
                    case TemplateConst.State:
                        value = _extend.ContainsKey(TemplateConst.ExtendState) ? _extend[TemplateConst.ExtendState] : "state";
                        break;
                    case TemplateConst.Host:
                        value = AppConnectHostConfig.Cache[_appAccountInfo.Type];
                        break;
                    case TemplateConst.Code:
                        value = _extend[TemplateConst.ExtendCode];
                        break;
                    case TemplateConst.Batch:
                        value = _extend[TemplateConst.ExtendBatch];
                        break;
                    case TemplateConst.Token:
                        value = _extend[TemplateConst.ExtendToken];
                        break;
                    default:
                        throw new ArgumentException("模板Key无效：key=" + key);
                }
                result.Add(arg.Key, value);
            }
            return result;
        }
    }
}