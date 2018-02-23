using Beisen.AppConnect.Infrastructure.RequestUtility;
using Beisen.AppConnect.WeChatSDK.Helper;
using Beisen.AppConnect.WeChatSDK.Model;
using RestSharp;

namespace Beisen.AppConnect.WeChatSDK.MP
{
    /// <summary>
    /// 模板消息
    /// </summary>
    public class Template
    {
        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <param name="industryId1">公众号模板消息所属行业编号</param>
        /// <param name="industryId2">公众号模板消息所属行业编号</param>
        /// <returns></returns>
        public static WeChatResult SetIndustry(string accessToken, int industryId1, int industryId2)
        {
            var json = "{\"industry_id1\":\"" + industryId1 + "\",\"industry_id2\":\"" + industryId2 + "\"}";

            return SetIndustryForJson(accessToken, json);
        }

        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <param name="json">行业信息</param>
        /// <returns></returns>
        public static WeChatResult SetIndustryForJson(string accessToken, string json)
        {
            var url = string.Format("{0}/cgi-bin/template/api_set_industry?access_token={1}", HostHelper.Host, accessToken);

            return Request.SendRequestForJson<WeChatResult>(url, Method.POST, json);
        }

        /// <summary>
        /// 获取设置的行业信息
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <returns></returns>
        public static IndustryResult GetIndustry(string accessToken)
        {
            var url = string.Format("{0}/cgi-bin/template/get_industry?access_token={1}", HostHelper.Host, accessToken);

            return Request.SendRequest<IndustryResult>(url);
        }

        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <param name="templateidshort">模板库中模板的编号，有“TM**”和“OPENTMTM**”等形式</param>
        /// <returns></returns>
        public static TemplateForAddResult AddTemplate(string accessToken, string templateidshort)
        {
            var json = "{\"template_id_short\":\"" + templateidshort + "\"}";

            return AddTemplateForJson(accessToken, json);
        }

        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <param name="json">模板编号信息</param>
        /// <returns></returns>
        public static TemplateForAddResult AddTemplateForJson(string accessToken, string json)
        {
            var url = string.Format("{0}/cgi-bin/template/api_add_template?access_token={1}", HostHelper.Host, accessToken);

            return Request.SendRequestForJson<TemplateForAddResult>(url, Method.POST, json);
        }

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <returns></returns>
        public static TemplateForGetAllPrivateResult GetAllPrivateTemplate(string accessToken)
        {
            var url = string.Format("{0}/cgi-bin/template/get_all_private_template?access_token={1}", HostHelper.Host, accessToken);

            return Request.SendRequest<TemplateForGetAllPrivateResult>(url);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <param name="templateId">模板Id</param>
        /// <returns></returns>
        public static WeChatResult DelPrivateTemplate(string accessToken, string templateId)
        {
            var json = "{\"template_id\":\"" + templateId + "\"}";

            return DelPrivateTemplateForJson(accessToken, json);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="accessToken">公众号凭证</param>
        /// <param name="json">模板Id信息</param>
        /// <returns></returns>
        public static WeChatResult DelPrivateTemplateForJson(string accessToken, string json)
        {
            var url = string.Format("{0}/cgi-bin/template/del_private_template?access_token={1}", HostHelper.Host, accessToken);

            return Request.SendRequestForJson<WeChatResult>(url, Method.POST, json);
        }

        ///// <summary>
        ///// 发送模板消息
        ///// </summary>
        ///// <param name="accessToken">公众号凭证</param>
        ///// <param name="jsonData">消息内容</param>
        ///// <returns></returns>
        //public static TemplateForSendResult SendTemplateMessage(string accessToken, string jsonData)
        //{
        //    var url = $"{HostHelper.Host}/cgi-bin/message/template/send?access_token={accessToken}";

        //    return Request.SendRequestForJson<TemplateForSendResult>(url, Method.POST, jsonData);
        //}

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public static TemplateForSendResult SendTemplateMessageForJson(string accessToken, string jsondata)
        {
            var url = string.Format("{0}/cgi-bin/message/template/send?access_token={1}", HostHelper.Host, accessToken);

            return Request.SendRequestForJson<TemplateForSendResult>(url, Method.POST, jsondata);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="templateId"></param>
        /// <param name="topcolor"></param>
        /// <param name="url"></param>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public static TemplateForSendResult SendTemplateMessageForJson(string accessToken, string openId, string templateId, string topcolor, string url, string jsondata)
        {
            var urlResult = string.Format("{0}/cgi-bin/message/template/send?access_token={1}", HostHelper.Host, accessToken);
            var json = "{\"template_id\":\"" + templateId + "\",\"topcolor\":\"" + topcolor + "\",\"touser\":\"" + openId + "\",\"url\":\"" + url + "\",\"data\":" + jsondata + "}";

            return Request.SendRequestForJson<TemplateForSendResult>(urlResult, Method.POST, json);
        }
    }
}
