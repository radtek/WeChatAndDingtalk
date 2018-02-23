using System;

namespace Beisen.AppConnect.WeChatSDK.Helper
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Unix起始时间
        /// </summary>
        private static DateTime BaseTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// 转换C#时间到微信时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetWeixinDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - BaseTime.Ticks) / 10000000 - 8 * 60 * 60;
        }

        /// <summary>
        /// 转换微信时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(string dateTimeFromXml)
        {
            return BaseTime.AddTicks((long.Parse(dateTimeFromXml) + 8 * 60 * 60) * 10000000);
        }
    }
}
