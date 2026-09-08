using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vison_Inspect_System._1_Web.WebClient
{
    public class HttpClient
    {
        /// <summary>
        /// 请求Get，Delete， WebAPI
        /// </summary>
        /// <param name="URL">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="Token">秘钥</param>
        /// <returns>服务器返回的数据字符串</returns>
        public static string HttpGet(string URL, string method, string Token)
        {
            //创建HttpWeb请求对象
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Method = method;
            request.Headers.Add("Token", Token);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return $"请求数据异常；异常信息：{e.Message}";
            }

        }
        /// <summary>
        /// 请求Get，Delete， WebAPI(无表头内容)
        /// </summary>
        /// <param name="URL">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <returns>服务器返回的数据字符串</returns>
        public static string HttpGet(string URL, string method)
        {
            //创建HttpWeb请求对象
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Method = method;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return $"请求数据异常；异常信息：{e.Message}";
            }
        }
        /// <summary>
        /// 请求POST（无密钥的请求方法）
        /// </summary>
        /// <param name="URL">请求地址</param>
        /// <param name="method">请求方法</param>
        /// <param name="JsonParas">BodyJson字符串</param>
        /// <returns></returns>
        public static string HttpPost(string URL, string method, string JsonParas)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                byte[] Buffer = Encoding.UTF8.GetBytes(JsonParas);
                //创建HttpWeb请求对象
                request.Method = method;
                request.ContentType = "application/json";
                request.ContentLength = Buffer.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(Buffer, 0, Buffer.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                return $"请求数据异常；异常信息：{e.Message}";
            }
        }
        /// <summary>
        ///  请求POST（有密钥的请求方法）
        /// </summary>
        /// <param name="URL">请求地址</param>
        /// <param name="method">请求方法</param>
        /// <param name="JsonParas">BodyJson字符串</param>
        /// <param name="Token">密钥</param>
        /// <returns></returns>
        public static string HttpPost(string URL, string method, string JsonParas, string Token)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                byte[] Buffer = Encoding.UTF8.GetBytes(JsonParas);
                //创建HttpWeb请求对象
                request.Method = method;
                request.ContentType = "application/json";
                request.ContentLength = Buffer.Length;
                request.Headers.Add("Token", Token);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(Buffer, 0, Buffer.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return $"请求数据异常；异常信息：{e.Message}";
            }
        }
    }
}
