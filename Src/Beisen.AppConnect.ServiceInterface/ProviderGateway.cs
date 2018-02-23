using Beisen.AppConnect.Infrastructure;
using Beisen.AppConnect.ServiceInterface.Interface;

namespace Beisen.AppConnect.ServiceInterface
{
    public static class ProviderGateway
    {
        /// <summary>
        /// 获取 IWechatTenantInfoProvider 实现类的实例
        /// </summary>
        public static IAppAccountProvider AppAccountProvider
        {
            get { return ProviderFactory.GetProvider<IAppAccountProvider>(); }
        }

        /// <summary>
        /// 获取 ITokenProvider 实现类的实例
        /// </summary>
        public static ITokenProvider TokenProvider
        {
            get { return ProviderFactory.GetProvider<ITokenProvider>(); }
        }

        /// <summary>
        /// 获取 IJsApiProvider 实现类的实例
        /// </summary>
        public static IJsApiProvider JsApiProvider
        {
            get { return ProviderFactory.GetProvider<IJsApiProvider>(); }
        }

        /// <summary>
        /// 获取 IAuthorizeProvider 实现类的实例
        /// </summary>
        public static IAuthorizeProvider AuthorizeProvider
        {
            get { return ProviderFactory.GetProvider<IAuthorizeProvider>(); }
        }

        /// <summary>
        /// 获取 IAppUserAccountProvider 实现类的实例
        /// </summary>
        public static IAppUserAccountProvider AppUserAccountProvider
        {
            get { return ProviderFactory.GetProvider<IAppUserAccountProvider>(); }
        }

        /// <summary>
        /// 获取 ICallbackContentProvider 实现类的实例
        /// </summary>
        public static ICallbackContentProvider CallbackContentProvider
        {
            get { return ProviderFactory.GetProvider<ICallbackContentProvider>(); }
        }

        /// <summary>
        /// 获取 IQrCodeLoginProvider 实现类的实例
        /// </summary>
        public static IQrCodeLoginProvider QrCodeLoginProvider
        {
            get { return ProviderFactory.GetProvider<IQrCodeLoginProvider>(); }
        }

        /// <summary>
        /// 获取 IBindBatchProvider 实现类的实例
        /// </summary>
        public static IBindBatchProvider BindBatchProvider
        {
            get { return ProviderFactory.GetProvider<IBindBatchProvider>(); }
        }

        /// <summary>
        /// 获取 IMenuProvider 实现类的实例
        /// </summary>
        public static IMenuProvider MenuProvider
        {
            get { return ProviderFactory.GetProvider<IMenuProvider>(); }
        }

        /// <summary>
        /// 获取 IMessageProvider 实现类的实例
        /// </summary>
        public static IMessageProvider MessageProvider
        {
            get { return ProviderFactory.GetProvider<IMessageProvider>(); }
        }

        /// <summary>
        /// 获取 ITemplateMappingProvider 实现类的实例
        /// </summary>
        public static ITemplateMappingProvider TemplateMappingProvider
        {
            get { return ProviderFactory.GetProvider<ITemplateMappingProvider>(); }
        }

        /// <summary>
        /// 获取 IRegisterProvider 实现类的实例
        /// </summary>
        public static IRegisterProvider RegisterProvider
        {
            get { return ProviderFactory.GetProvider<IRegisterProvider>(); }
        }

        /// <summary>
        /// 获取 IMobileVerificationProvider 实现类的实例
        /// </summary>
        public static IMobileVerificationProvider MobileVerificationProvider
        {
            get { return ProviderFactory.GetProvider<IMobileVerificationProvider>(); }
        }

        /// <summary>
        /// 获取 ISMSProvider 实现类的实例
        /// </summary>
        public static ISMSProvider SMSProvider
        {
            get { return ProviderFactory.GetProvider<ISMSProvider>(); }
        }

        /// <summary>
        /// 获取 IStaffProvider 实现类的实例
        /// </summary>
        public static IStaffProvider StaffProvider
        {
            get { return ProviderFactory.GetProvider<IStaffProvider>(); }
        }

        /// <summary>
        /// 获取 ITenantProvider 实现类的实例
        /// </summary>
        public static ITenantProvider TenantProvider
        {
            get { return ProviderFactory.GetProvider<ITenantProvider>(); }
        }
    }
}