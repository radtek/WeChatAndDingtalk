using System.Collections.Generic;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class CallbackPrivateAndPublicState : CallbackStateBase
    {
        #region 单例

        protected static readonly CallbackStateBase _instance = new CallbackPrivateAndPublicState();

        public static CallbackStateBase Intance
        {
            get { return _instance; }
        }

        private CallbackPrivateAndPublicState()
        {
        }

        #endregion

        internal override string GetRedirectUrl(CallbackContentInfo contentInfo, string code, string state)
        {
            var appAccountPublic = ProviderGateway.AppAccountProvider.Get(contentInfo.AppAccountPublic);

            GetOpenId(appAccountPublic, code);
            
            var appAccountPrivate = ProviderGateway.AppAccountProvider.Get(contentInfo.AppAccountPrivate);
            var extend = new Dictionary<string, string>
            {
                {TemplateConst.ExtendState, state},
                {TemplateConst.ExtendBatch, contentInfo.BatchId}
            };
            var requestTemplate = new DefaultApiTemplate(appAccountPrivate, TemplateConst.AuthorizeUrl, extend);

            SetNextState(contentInfo.BatchId, CallbackContentState.Private);

            //跳转获取私有身份
            return requestTemplate.GetRequestUrl();
        }
    }
}
