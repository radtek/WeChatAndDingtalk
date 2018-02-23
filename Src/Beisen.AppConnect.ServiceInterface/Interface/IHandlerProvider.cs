using System.IO;

namespace Beisen.AppConnect.ServiceInterface.Interface
{
    /// <summary>
    /// 消息处理接口
    /// </summary>
    public interface IHandlerProvider
    {
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <param name="appId">AppId</param>
        /// <param name="input">请求数据流</param>
        /// <returns></returns>
        string Execute(string signature, string timestamp, string nonce, string echostr, string appId, Stream input);

        /// <summary>
        /// 获取返回XML
        /// </summary>
        /// <param name="input">请求数据流</param>
        /// <returns></returns>
        string GetResponseXml(Stream input);

        /// <summary>
        /// 获取返回XML
        /// </summary>
        /// <param name="input">请求数据流</param>
        /// <returns></returns>
        string GetResponseXml(string input);
    }
}
