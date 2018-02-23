using Beisen.AppConnect.ServiceInterface.Model;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    public interface IQrCodeLoginProvider
    {
        /// <summary>
        /// 增加二维码登录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        void Add(QrCodeLoginInfo info);

        /// <summary>
        /// 根据Code获取二维码登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        QrCodeLoginInfo GetAndUpdateByCode(string code);

        /// <summary>
        /// 更新二维码登录状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        void UpdateState(string code, QrCodeLoginState state);

        /// <summary>
        /// 更新二维码用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        void UpdateIdentity(string code, QrCodeLoginState state, int tenantId, int userId);

        /// <summary>
        /// 生成二维码登录
        /// </summary>
        /// <param name="titaAppId"></param>
        /// <returns></returns>
        string GenerateQrCode(int titaAppId);

        /// <summary>
        /// 获取二维码图片
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="appAccountId"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        byte[] GenerateQrCodePicture(int tenantId, string appAccountId, int type, string code, int size);

        /// <summary>
        /// 扫码二维码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool Scan(string code);

        /// <summary>
        /// 提交扫码结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        bool Submit(string code, QrCodeLoginState state, string appId, string openId);
    }
}
