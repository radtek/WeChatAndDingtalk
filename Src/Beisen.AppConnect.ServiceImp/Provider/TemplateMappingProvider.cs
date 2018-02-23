using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.Common.HelperObjects;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class TemplateMappingProvider:ITemplateMappingProvider
    {
        #region 单例

        private static readonly ITemplateMappingProvider _instance = new TemplateMappingProvider();
        public static ITemplateMappingProvider Instance
        {
            get { return _instance; }
        }

        private TemplateMappingProvider()
        {
        }
        #endregion

        public string GetTemplateId(string appId, string templateIdShort)
        {
            ArgumentHelper.AssertNotNullOrEmpty(appId, "appId is null");
            ArgumentHelper.AssertNotNullOrEmpty(templateIdShort, "appId is null");

            var templateMapping = TemplateMappingDao.Get(appId, templateIdShort);
            if (templateMapping == null)
            {
                return string.Empty;
            }
            return templateMapping.TemplateId;
        }

        public int Add(TemplateMappingInfo info)
        {
            ArgumentHelper.AssertNotNull(info, "TemplateMappingInfo is null");
            ArgumentHelper.AssertNotNullOrEmpty(info.AppId, "TemplateMappingInfo.appId is null");
            ArgumentHelper.AssertNotNullOrEmpty(info.TemplateIdShort, "TemplateMappingInfo.TemplateIdShort is null");
            ArgumentHelper.AssertNotNullOrEmpty(info.TemplateId, "TemplateMappingInfo.TemplateId is null");

            return TemplateMappingDao.Insert(info);
        }

        public void Delete(int userId,int id)
        {
            ArgumentHelper.AssertIsTrue(id > 0, "id is 0");

            TemplateMappingDao.Delete(userId, id);
        }
    }
}
