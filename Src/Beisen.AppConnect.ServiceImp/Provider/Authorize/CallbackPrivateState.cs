using System;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    internal class CallbackPrivateState : CallbackStateBase
    {
        #region 单例

        protected static readonly CallbackStateBase _instance = new CallbackPrivateState();

        public static CallbackStateBase Intance
        {
            get { return _instance; }
        }

        private CallbackPrivateState()
        {
        }

        #endregion

        internal override string GetRedirectUrl(CallbackContentInfo contentInfo, string code, string state)
        {
            var appAccountPvivate = ProviderGateway.AppAccountProvider.Get(contentInfo.AppAccountPrivate);

            GetOpenId(appAccountPvivate, code);
            SetNextState(contentInfo.BatchId, CallbackContentState.Finish);
            //跳转获取私有身份
            var rd = new Random();
            var r = rd.Next();
            //返回身份
            return UrlHelper.AddParameter(contentInfo.Content, "r", r.ToString());
        }
    }
}
