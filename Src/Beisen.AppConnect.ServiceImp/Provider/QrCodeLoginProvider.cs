using System;
using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.Infrastructure.Configuration;
using Beisen.AppConnect.Infrastructure.Helper;
using Beisen.AppConnect.ServiceImp.Persistance;
using Beisen.AppConnect.ServiceInterface;
using Beisen.AppConnect.ServiceInterface.Interface;
using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using Beisen.Common.HelperObjects;

namespace Beisen.AppConnect.ServiceImp.Provider
{
    public class QrCodeLoginProvider: IQrCodeLoginProvider
    {
        #region 单例

        private static readonly IQrCodeLoginProvider _instance = new QrCodeLoginProvider();
        public static IQrCodeLoginProvider Instance
        {
            get { return _instance; }
        }

        private QrCodeLoginProvider()
        {
        }

        #endregion

        /// <summary>
        /// 增加二维码登录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void Add(QrCodeLoginInfo info)
        {
            ArgumentHelper.AssertIsTrue(info != null, "QrCodeLoginInfo is null");
            ArgumentHelper.AssertNotNullOrEmpty(info.Code, "QrCodeLoginInfo.Code is null or empty");
            ArgumentHelper.AssertIsTrue(info.TitaAppId > 0, "info.TitaAppId is less than 0");

            QrCodeLoginDao.Insert(info);
        }

        /// <summary>
        /// 根据Code获取二维码登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public QrCodeLoginInfo GetAndUpdateByCode(string code)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "Code is null or empty");
            
            var qrcode = GetByCode(code);
            if (qrcode != null && qrcode.State == QrCodeLoginState.Login)
            {
                qrcode = qrcode.Clone();
                UpdateState(code, QrCodeLoginState.Invalid);
            }

            return qrcode;
        }

        /// <summary>
        /// 根据Code获取二维码登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private QrCodeLoginInfo GetByCode(string code)
        {
            var qrcode = QrCodeLoginDao.GetByCode(code);
            return qrcode;
        }

        /// <summary>
        /// 更新二维码登录状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        public void UpdateState(string code, QrCodeLoginState state)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "Code is null or empty");

            var info = GetByCode(code);
            if (info != null)
            {
                info.State = state;
                QrCodeLoginDao.UpdateState(info);
            }            
        }

        /// <summary>
        /// 更新二维码用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        public void UpdateIdentity(string code, QrCodeLoginState state, int tenantId, int userId)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "Code is null or empty");
            ArgumentHelper.AssertIsTrue(tenantId > 0, "TenantId is less than 0");
            ArgumentHelper.AssertIsTrue(userId > 0, "TenantId is less than 0");

            var info = GetByCode(code);
            if (info != null)
            {
                info.State = state;
                info.TenantId = tenantId;
                info.UserId = userId;
                QrCodeLoginDao.UpdateIdentity(info);
            }
        }

        /// <summary>
        /// 生成二维码登录
        /// </summary>
        /// <returns></returns>
        public string GenerateQrCode(int titaAppId)
        {
            var checkResult = ProviderGateway.AuthorizeProvider.CheckApp(titaAppId, "");
            if (!checkResult)
            {
                AppConnectLogHelper.Error("GenerateQrCode--检查app失败");
                return null;
            }

            var qrCodeLoginInfo = new QrCodeLoginInfo
            {
                Code = Guid.NewGuid().ToString(),
                TitaAppId= titaAppId
            };
            Add(qrCodeLoginInfo);

            return qrCodeLoginInfo.Code;
        }

        /// <summary>
        /// 获取二维码图片
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appAccountId"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public byte[] GenerateQrCodePicture(int tenantId, string appAccountId, int type, string code, int size)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "code is null or empty");
            AppConnectLogHelper.DebugFormat("调用GenerateQrCodePicture方法:tenantId:{0},code:{1}", tenantId, code);
            var scanUrl = UrlHelper.AddParameter(AppConnectHostConfig.Cache[0] + HostConst.QrCodeScanLogin, "code", code);
            AppConnectLogHelper.DebugFormat("scanUrl:{0}", scanUrl);
            return QrCodeHelper.Generate(scanUrl, size);
        }

        /// <summary>
        /// 扫码二维码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool Scan(string code)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "code is null or empty");

            var qrCodeLoginInfo = GetByCode(code);
            if (qrCodeLoginInfo == null || (qrCodeLoginInfo.State!= QrCodeLoginState.UnLogin && qrCodeLoginInfo.State != QrCodeLoginState.Cancel))
            {
                return false;
            }
            UpdateState(code, QrCodeLoginState.Scanned);
            return true;
        }

        /// <summary>
        /// 提交扫码结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool Submit(string code, QrCodeLoginState state,string appId,string openId)
        {
            ArgumentHelper.AssertNotNullOrEmpty(code, "code is null or empty");

            if (state == QrCodeLoginState.Cancel)
            {
                UpdateState(code, QrCodeLoginState.Cancel);
            }
            else if (state == QrCodeLoginState.Login)
            {
                var appUserAccount = ProviderGateway.AppUserAccountProvider.GetByOpenId(appId, openId);
                UpdateIdentity(code, QrCodeLoginState.Login, appUserAccount.TenantId, appUserAccount.UserId);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
