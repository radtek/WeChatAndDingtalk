using System;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class CallbackPublicState : CallbackStateBase
    {
        #region 单例

        protected static readonly CallbackStateBase _instance = new CallbackPublicState();

        public static CallbackStateBase Intance
        {
            get { return _instance; }
        }

        private CallbackPublicState()
        {
        }

        #endregion

        internal override string GetRedirectUrl(CallbackContentInfo contentInfo, string code, string state)
        { 
            var appAccount = ProviderGateway.AppAccountProvider.Get(contentInfo.AppAccountPublic);
            GetOpenId(appAccount, code);

            SetNextState(contentInfo.BatchId, CallbackContentState.Finish);

            var rd = new Random();
            var r = rd.Next();
            //返回身份
            return UrlHelper.AddParameter(contentInfo.Content, "r", r.ToString());
        }
    }
}
