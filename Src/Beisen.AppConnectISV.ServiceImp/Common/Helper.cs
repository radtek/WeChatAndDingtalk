using Beisen.AppConnectISV.Model.BusinessModel;
using Beisen.MultiTenant.Model;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using Beisen.AppConnectISV.Model;
using Beisen.AppConnectISV.Model.BusinessEnum;

namespace Beisen.AppConnectISV.ServiceImp
{
    public class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double GetTimeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds);
        }
        public static string GetImageUrl(string productId, int messageType = (int)MappingType.DingTalk)
        {
            string imgUrlFormat = "http://stnew03.beisen.com/ux/beisen-common/upaas-static/upaas/{0}.png";
            string imageUrl = string.Empty;
            string imageName = string.Empty;
            switch (messageType)
            {
                case 13:
                case 14:
                    imageName = "italent_massage_03";
                    break;
                case 21:
                    imageName = "italent_massage_04";
                    break;
                default:
                    break;
            }
            switch (productId)
            {
                //审批中心
                case "907": imageUrl = string.Format(imgUrlFormat, imageName); break;
                //核心人力
                case "908": imageUrl = string.Format(imgUrlFormat, imageName); break;
                //假勤
                case "909": imageUrl = string.Format(imgUrlFormat, imageName); break;
                //薪酬
                case "960": imageUrl = string.Format(imgUrlFormat, imageName); break;
                default:
                    imageUrl = string.Format(imgUrlFormat, imageName);
                    break;
            }
            return imageUrl;
        }


        public static void WriteLog(string strMemo)
        {
            string directoryPath = HttpContext.Current.Server.MapPath(@"Logs");
            string fileName = directoryPath + @"\log" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(fileName))
                {
                    sr = File.CreateText(fileName);
                }
                else
                {
                    sr = File.AppendText(fileName);
                }
                sr.WriteLine(DateTime.Now + ": " + strMemo);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string GetCorpExecuteResult(string url, string postdata)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                CookieContainer cc = new CookieContainer();
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded;";
                byte[] postdatabyte = Encoding.UTF8.GetBytes(postdata);
                request.ContentLength = postdatabyte.Length;
                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.KeepAlive = true;

                //提交请求
                Stream stream;
                stream = request.GetRequestStream();
                stream.Write(postdatabyte, 0, postdatabyte.Length);
                stream.Close();

                //接收响应
                response = (HttpWebResponse)request.GetResponse();
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);

                //CookieCollection cook = response.Cookies;
                ////Cookie字符串格式
                //string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);
                Stream strm = response.GetResponseStream();

                StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);

                string line;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line + System.Environment.NewLine);
                }
                sr.Close();
                strm.Close();
                return sb.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string GetCorpExecuteResult(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                string serviceAddress = url;
                request = (HttpWebRequest)WebRequest.Create(serviceAddress);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                response = (HttpWebResponse)request.GetResponse();

                Stream strm = response.GetResponseStream();

                StreamReader sr = new StreamReader(strm, Encoding.UTF8);

                string line;

                StringBuilder sb = new StringBuilder();

                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line + Environment.NewLine);
                }
                sr.Close();
                strm.Close();
                return sb.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
