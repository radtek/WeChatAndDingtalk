using Beisen.AppConnect.ServiceInterface.Model;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface ITemplateMappingProvider
    {
        string GetTemplateId(string appId, string templateIdShort);

        int Add(TemplateMappingInfo info);

        void Delete(int userId, int id);
    }
}
