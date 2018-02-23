using Beisen.Common.HelperObjects;
using Beisen.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public class TitaMessageHelper
    {
        private static string TitaApiUrl = ConfigManager.AppSettings["TitaApiUrl"];
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public static void SendMessage(int tenantId, int userId, string message, string url = "")
        {
            var keyModels = new List<MessageKey>();
            keyModels.Add(new MessageKey { Key = "content", Value = message });
            if (!string.IsNullOrWhiteSpace(url))
                keyModels.Add(new MessageKey { Key = "link", Value = url });
            AddMessageModel messageModel = new AddMessageModel()
            {
                AppId = 926,
                AppType = 926,
                FromUserId = userId,
                IsSign = false,
                TemplateId = "32",
                TemplateTitle = "AppConnect通知",
                TenantId = tenantId,
                ToUserIds = userId.ToString(),
                Values = keyModels
            };
            SendMessage(tenantId, messageModel);
        }
        private static void SendMessage(int tenantId, AddMessageModel model)
        {
            ArgumentHelper.AssertNotNull(model);
            ArgumentHelper.AssertPositive(tenantId);
            ArgumentHelper.AssertPositive(model.FromUserId);

            string url = string.Format(TitaApiUrl + "/api/v1/{0}/{1}/message", tenantId, model.FromUserId);
            try
            {
                var resultStr = HttpClientHelper.HttpPost(url, model);
                var result = JsonConvert.DeserializeObject<ApiResult<object>>(resultStr);
                if (result.code != 0)
                    AppConnectLogHelper.ErrorFormat("Tita发送消息接口出错！租户:{0},发送人:{1}", tenantId, model.FromUserId);
            }
            catch (Exception ex)
            {
                AppConnectLogHelper.ErrorFormat("发送消息出错！接口Url:{0},租户:{1},发送人:{2},异常信息:{3}", url, tenantId, model.FromUserId, JsonConvert.SerializeObject(ex));
            }
        }
        
    }
    public class AddMessageModel
    {
        /// <summary>
        /// App标志
        /// </summary>
        [JsonProperty("app_id")]
        public int AppId { get; set; }

        /// <summary>
        /// App标志
        /// </summary>
        [JsonProperty("app_type")]
        public int AppType { get; set; }

        [JsonProperty("tenant_id")]
        public int TenantId { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        [JsonProperty("from_user_id")]
        public int FromUserId { get; set; }

        /// <summary>
        /// 接收人（逗号隔开）
        /// </summary>
        [JsonProperty("to_user_ids")]
        public string ToUserIds { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 跳转链接是否需要签名
        /// </summary>
        [JsonProperty("is_sign")]
        public bool IsSign { get; set; }

        /// <summary>
        /// 模板标题
        /// </summary>
        [JsonProperty("template_title")]
        public string TemplateTitle { get; set; }

        /// <summary>
        /// 模板的值
        /// </summary>
        [JsonProperty("values")]
        public List<MessageKey> Values { get; set; }
    }
    public class MessageKey
    {
        /// <summary>
        /// 键
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// 键对应的值
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
