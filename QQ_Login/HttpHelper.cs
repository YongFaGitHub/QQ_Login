using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QQ_Login
{
    public class HttpHelper
      {

		private static string contentType = "application/x-www-form-urlencoded";
		private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
		private static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; Zune 4.7; BOIE9;ZHCN)";
		public static string referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?appid=1006102&s_url=http://id.qq.com/index.html";

		/// <summary>
		/// 获取字符流
		/// </summary>
		/// <param name="url"></param>
		/// <param name="cookieContainer"></param>
		/// <returns></returns>
		public static Stream GetStream(string url, CookieContainer cookieContainer)
		{
			HttpWebRequest httpWebRequest = null;
			HttpWebResponse httpWebResponse = null;

			try
			{
				httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
				httpWebRequest.CookieContainer = cookieContainer;
				httpWebRequest.ContentType = contentType;
				httpWebRequest.Referer = referer;
				httpWebRequest.Accept = accept;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.Method = "GET";
				httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;

				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();

				return responseStream;
			}
			catch (Exception)
			{
				return null;
			}

		}

		/// <summary>
		/// 获取HTML
		/// </summary>
		/// <param name="url"></param>
		/// <param name="cookieContainer"></param>
		/// <returns></returns>
		public static string GetHtml(string url, CookieContainer cookieContainer)
		{
			HttpWebRequest httpWebRequest = null;
			HttpWebResponse httpWebResponse = null;
			try
			{
				httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
				httpWebRequest.CookieContainer = cookieContainer;
				httpWebRequest.ContentType = contentType;
				httpWebRequest.Referer = referer;
				httpWebRequest.Accept = accept;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.Method = "GET";
				httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;

				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
				string html = streamReader.ReadToEnd();

				streamReader.Close();
				responseStream.Close();

				httpWebRequest.Abort();
				httpWebResponse.Close();

				return html;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
			/// <summary>
			/// Http请求
			/// </summary>
			/// <param name="url">请求网址</param>
			/// <param name="Headerdics">头文件固定KEY值字典类型泛型集合</param>
			/// <param name="heard">头文件集合</param>
			/// <param name="cookieContainers">cookie容器</param>
			/// <param name="redirecturl">头文件中的跳转链接</param>
			/// <returns>返回请求字符串结果</returns>
			public static string RequestGet(string url, Dictionary<string, string> Headerdics, WebHeaderCollection heard, CookieContainer cookieContainers, ref string redirecturl)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;
			ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
			//Dim domain = CStr(Regex.Match(url, "^(?:\w+://)?([^/?]*)").Groups(1).Value)
			//If domain.Contains("www.") = True Then
			//    domain = domain.Replace("www.", "")
			//Else
			//    domain = "." & domain
			//End If
			if (string.IsNullOrEmpty(url))
			{
				return "";
			}
			HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
			myRequest.Headers = heard;
			myRequest.Method = "GET";
			foreach (var pair in Headerdics)
			{
				typeof(HttpWebRequest).GetProperty(pair.Key).SetValue(myRequest, pair.Value, null);
			}
			myRequest.CookieContainer = cookieContainers;
			string results = "";

			try
			{
				using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
				{
					if (myResponse.ContentEncoding.ToLower().Contains("gzip"))
					{
						using (Stream stream = new System.IO.Compression.GZipStream(myResponse.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress))
						{
							using (var reader = new StreamReader(stream))
							{
								results = reader.ReadToEnd();
							}
						}
					}
					else if (myResponse.ContentEncoding.ToLower().Contains("deflate"))
					{
						using (Stream stream = new System.IO.Compression.DeflateStream(myResponse.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress))
						{
							using (var reader = new StreamReader(stream))
							{
								results = reader.ReadToEnd();
							}
						}
					}
					else
					{
						using (Stream stream = myResponse.GetResponseStream())
						{
							using (var reader = new StreamReader(stream, Encoding.UTF8))
							{
								results = reader.ReadToEnd();
							}
						}
					}
					if (myResponse.Headers["Location"] != null)
					{
						redirecturl = myResponse.Headers["Location"];
					}
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message.ToString());
			}
			return results;
		}
		/// <summary>
		/// Http响应
		/// </summary>
		/// <param name="url">请求网址</param>
		/// <param name="Headerdics">头文件固定KEY值字典类型泛型集合</param>
		/// <param name="heard">头文件集合</param>
		/// <param name="postdata">提交的字符串型数据</param>
		/// <param name="cookieContainers">cookie容器</param>
		/// <param name="redirecturl">头文件中的跳转链接</param>
		/// <returns>返回响应字符串结果</returns>
		public static string RequestPost(string url, Dictionary<string, string> Headerdics, WebHeaderCollection heard, string postdata, CookieContainer cookieContainers, ref WebHeaderCollection ResponseHeaders, ref string redirecturl)
		{
			if (string.IsNullOrEmpty(url))
			{
				return "";
			}
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;
			ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
			//Dim domain = CStr(Regex.Match(url, "^(?:\w+://)?([^/?]*)").Groups(1).Value)
			//If domain.Contains("www.") = True Then
			//    domain = domain.Replace("www.", "")
			//Else
			//    domain = domain
			//End If
			string results = "";
			try
			{
				var myRequest = (HttpWebRequest)WebRequest.Create(url);
				var data = Encoding.UTF8.GetBytes(postdata);
				myRequest.Headers = heard;
				myRequest.Method = "POST";
				foreach (var pair in Headerdics)
				{
					typeof(HttpWebRequest).GetProperty(pair.Key).SetValue(myRequest, pair.Value, null);
				}
				myRequest.CookieContainer = cookieContainers;
				myRequest.ContentLength = data.Length;
				using (var stream = myRequest.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}

				using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
				{
					if (myResponse.ContentEncoding.ToLower().Contains("gzip"))
					{
						using (var stream = myResponse.GetResponseStream())
						{
							using (StreamReader reader = new StreamReader(new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress), Encoding.UTF8))
							{
								results = reader.ReadToEnd();
							}
						}
					}
					else if (myResponse.ContentEncoding.ToLower().Contains("deflate"))
					{
						using (var stream = myResponse.GetResponseStream())
						{
							using (StreamReader reader = new StreamReader(new System.IO.Compression.DeflateStream(stream, System.IO.Compression.CompressionMode.Decompress), Encoding.UTF8))
							{
								results = reader.ReadToEnd();
							}
						}
					}
					else
					{
						using (Stream stream = myResponse.GetResponseStream())
						{
							using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
							{
								results = reader.ReadToEnd();
							}
						}
					}
					if (myResponse.Headers["Location"] != null)
					{
						redirecturl = myResponse.Headers["Location"];
					}
					ResponseHeaders = myResponse.Headers;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message.ToString());
			}
			return results;
		}

	}
}
