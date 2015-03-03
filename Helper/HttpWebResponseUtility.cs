using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SyncDataTool.BLL;

namespace SyncDataTool.Helper
{
    /// <summary>   
    /// 有关HTTP请求的辅助类   
    /// </summary>   
    public class HttpWebResponseUtility
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>   
        /// 创建GET方式的HTTP请求   
        /// </summary>   
        /// <param name="url">请求的URL</param>   
        /// <param name="timeout">请求的超时时间</param>   
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>   
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>   
        /// <returns></returns>   
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, string user, string pwd, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            ////如果是发送HTTPS请求   
            //if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            //{
            //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //    request = WebRequest.Create(url) as HttpWebRequest;
            //    request.ProtocolVersion = HttpVersion.Version10;
            //}
            //else
            //{
            //    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //}
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            NetworkCredential credentials = new NetworkCredential(user, pwd);
            request.Credentials = credentials;
            //HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>   
        /// 创建POST方式的HTTP请求   
        /// </summary>   
        /// <param name="url">请求的URL</param>   
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>   
        /// <param name="timeout">请求的超时时间</param>   
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>   
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>   
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>   
        /// <returns></returns>   
        public static HttpWebResponse CreatePostHttpResponse(string url, string postDataStr, string user, string pwd, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            NetworkCredential credentials = new NetworkCredential(user, pwd);
            request.Credentials = credentials;
            //request.PreAuthenticate = true;//2014/08/08 added
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/json"; //"application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.UserAgent = DefaultUserAgent;
            request.Timeout = 60000;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            StreamWriter myStreamWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);
            myStreamWriter.Write(postDataStr);
            //myStreamWriter.Close();

            //byte[] data = Encoding.UTF8.GetBytes(postDataStr);
            //using (Stream stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

        #region 同步通过POST方式发送数据
        /// <summary>
        /// 通过POST方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">Post数据</param>
        /// <param name="cookie">Cookie容器</param>
        /// <returns></returns>
        public static string SendDataByPost(string Url, string postDataStr, string user, string pwd, ref CookieContainer cookie)
        {
            string retString = null;
            HttpWebRequest request = null;
            Stream myRequestStream = null;
            StreamWriter myStreamWriter = null;
            HttpWebResponse response = null;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.KeepAlive = true;
                if (cookie.Count == 0)
                {
                    request.CookieContainer = new CookieContainer();
                    cookie = request.CookieContainer;
                }
                else
                {
                    request.CookieContainer = cookie;
                }
                request.Timeout = 60000;//设置60秒超时

                NetworkCredential credentials = new NetworkCredential(user, pwd);
                request.Credentials = credentials;
                request.PreAuthenticate = true;//2014/08/08 added
                request.Method = "POST";
                request.ContentType = "application/json"; //"application/x-www-form-urlencoded";
                request.ContentLength = postDataStr.Length;
                myRequestStream = request.GetRequestStream();
                myStreamWriter = new StreamWriter(myRequestStream, Encoding.UTF8);
                myStreamWriter.Write(postDataStr);
                //myStreamWriter.Close();

                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Common.WriteLog("数据POST传输失败\r\n" + ex.Message + "\r\nDetails:" + ex.StackTrace);
            }
            finally
            {
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                }
                if (myResponseStream != null)
                {
                    myResponseStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (myStreamWriter != null)
                {
                    myStreamWriter.Close();
                }
                if (myRequestStream != null)
                {
                    myRequestStream.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return retString;
        }
        #endregion

        public static void post(string strUrl, string strPostData, string user, string pwd)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            NetworkCredential credentials = new NetworkCredential(user, pwd);
            httpWebRequest.Credentials = credentials;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = strPostData;

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
        }

    }
}