using Beisen.AppConnect.WeChatSDK.Enum;
using IndustryResultModel = Beisen.AppConnect.ServiceInterface.Model.WebModel.WeChat.IndustryResultModel;
using ResultModel = Beisen.AppConnect.ServiceInterface.Model.WebModel.ResultModel;
using TemplateForAddResultModel = Beisen.AppConnect.ServiceInterface.Model.WebModel.WeChat.TemplateForAddResultModel;
using TemplateForGetAllPrivateResultModel = Beisen.AppConnect.ServiceInterface.Model.WebModel.WeChat.TemplateForGetAllPrivateResultModel;
using TemplateForSendResultModel = Beisen.AppConnect.ServiceInterface.Model.WebModel.WeChat.TemplateForSendResultModel;
using WeChatTemplateMessageInfo = Beisen.AppConnect.ServiceInterface.Model.WeChatTemplateMessageInfo;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    /// <summary>
    /// 模板消息接口
    /// </summary>
    public interface ITemplateProvider
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="sourceId">原始Id</param>
        /// <param name="openId">OpenId</param>
        /// <param name="message">Json消息</param>
        /// <returns></returns>
        TemplateForSendResultModel SendForJson(int applicationId, int tenantId, string sourceId, string openId, string message);

        /// <summary>
        /// 根据Id获取模板消息
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="id">消息</param>
        /// <returns></returns>
        WeChatTemplateMessageInfo GetById(int applicationId, int tenantId, int id);

        /// <summary>
        /// 更新模板消息状态
        /// </summary>
        /// <param name="sourceId">原始Id</param>
        /// <param name="openId">OpenId</param>
        /// <param name="msgId">微信消息Id</param>
        /// <param name="status">消息状态</param>
        void UpdateStatus(string sourceId, string openId, long msgId, TemplateSendStatus status);

        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="json">行业信息</param>
        /// <returns></returns>
        ResultModel SetIndustryForJson(int applicationId, int tenantId, string json);

        /// <summary>
        /// 获取设置的行业信息
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <returns></returns>
        IndustryResultModel GetIndustry(int applicationId, int tenantId);

        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="json">模板编号信息</param>
        /// <returns></returns>
        TemplateForAddResultModel AddTemplateForJson(int applicationId, int tenantId, string json);

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <returns></returns>
        TemplateForGetAllPrivateResultModel GetAllPrivateTemplate(int applicationId, int tenantId);

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="json">模板Id信息</param>
        /// <returns></returns>
        ResultModel DelPrivateTemplateForJson(int applicationId, int tenantId, string json);
    }
}
