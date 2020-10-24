
using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QQ_Login
{
    public partial class Form1 : Form
    {
        public static string VerificationCode="";
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);

            //WebBrowser browser =new WebBrowser();
            //browser.ScriptErrorsSuppressed = false;
            //browser.Navigate(new Uri("https://id.qq.com/login/ptlogin.html"));
            //browser.DocumentCompleted += browserOnLoaded;

           
            webBrowser2.Navigate(new Uri("https://xui.ptlogin2.qq.com/cgi-bin/xlogin?appid=501038301&target=self&s_url=https://im.qq.com/loginSuccess.html"));
            //不知道上面这个地址是不是长期固定,保险起见从主页入手
            WebBrowser mbrowser = new WebBrowser();
            mbrowser.ScriptErrorsSuppressed = false;
            mbrowser.Navigate(new Uri("https://im.qq.com/mobileqq/"));
            mbrowser.DocumentCompleted += mbrowserOnLoaded;

        }

        //public static bool IsLogin(string qqnum, string code, string passwords, CookieContainer mycookiecontainer)
        //{
        //    WebHeaderCollection myWebHeaderCollection = new WebHeaderCollection();
        //    var redirect_geturl = string.Empty;
        //    var head1 = new WebHeaderCollection();
        //    Dictionary<string, string> Headerdics = new Dictionary<string, string>()
        //    {
        //        {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3"},
        //        {"ContentType", "application/x-www-form-urlencoded"},
        //        {"Referer", "http://ui.ptlogin2.qq.com/cgi-bin/login?appid=1006102&s_url=http://id.qq.com/index.html"},
        //        {"UserAgent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4098.3 Safari/537.36"}
        //    };
        //    string password = PasswordHelper.GetPassword(qqnum, passwords, code);
        //    string loginUrlstring = @"http://ptlogin2.qq.com/login?u=" + qqnum + "&p=" + password + "&verifycode=" + code + "&aid=1006102&u1=http%3A%2F%2Fid.qq.com%2Findex.html&h=1&ptredirect=1&ptlang=2052&from_ui=1&dumy=&fp=loginerroralert&action=8-29-82478035&mibao_css=&t=1&g=1";
        //    string retString = HttpHelper.RequestGet(loginUrlstring, Headerdics, head1, mycookiecontainer, ref redirect_geturl);
        //    Debug.WriteLine(retString);
        //    return retString.Contains("ptuiCB('0',") ? true : false;
        //}
        //public static string GetVerfiyCode(string qqnum, CookieContainer mycookiecontainer)
        //{
        //    WebHeaderCollection myWebHeaderCollection = new WebHeaderCollection();
        //    var redirect_geturl = string.Empty;
        //    var head1 = new WebHeaderCollection();
        //    Dictionary<string, string> Headerdics = new Dictionary<string, string>()
        //    {
        //        {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3"},
        //        {"ContentType", "application/x-www-form-urlencoded"},
        //        {"Referer", "http://ui.ptlogin2.qq.com/cgi-bin/login?appid=1006102&s_url=http://id.qq.com/index.html"},
        //        {"UserAgent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4098.3 Safari/537.36"}
        //    };
        //    Random rand = new Random();
        //    double r = rand.NextDouble();
        //    string checkcodeurl = string.Format(@"http://check.ptlogin2.qq.com/check?uin={0}&appid=1006102&r={1}", qqnum, r);
        //    string retString = HttpHelper.RequestGet(checkcodeurl, Headerdics, head1, mycookiecontainer, ref redirect_geturl);
        //    return retString;
        //}
    
        //public static Stream GetVerfycodeImage(CookieContainer mycookiecontainer, string qqnum)
        //{
        //    Random rand = new Random();
        //    double r = rand.NextDouble();
        //    string codeimageurl = string.Format("http://captcha.qq.com/getimage?aid=1006102&r={0}&uin={1}", r, qqnum);
        //    Stream retStream = HttpHelper.GetStream(codeimageurl, mycookiecontainer);
        //    return retStream;
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    CookieContainer mycookiecontainer = new CookieContainer();
        //    //bool success = IsLogin(this.txtUseraccount.Text, code, this.txtPassword.Text, cookie);
        //    string retString = GetVerfiyCode(txtUseraccount.Text, mycookiecontainer);
        //    if (retString.Contains("ptui_checkVC('0','"))
        //    {
        //        //不需要手动输入
        //        Form frm = new Form2();
        //        Form2.MyInstance.txtverfiycode.Text = retString.Replace("ptui_checkVC('0','", "").Replace("'", "").Replace(")", "").Replace(";", "").Substring(0, 4);
        //        frm.ShowDialog();

        //    }
        //    else if (retString.Contains("ptui_checkVC('1',"))
        //    {
        //        //需要手动输入，将验证码输出在image中
        //        Form frm = new Form2();
        //        Form2.MyInstance.vefycodpicbox.Image = Image.FromStream(GetVerfycodeImage(mycookiecontainer, this.txtUseraccount.Text));
        //        frm.ShowDialog();                
        //    }
        //    bool success = IsLogin(txtUseraccount.Text,VerificationCode,txtPassword.Text,mycookiecontainer);
        //    if (success)
        //    {
        //        MessageBox.Show("登录成功!");
        //    }
        //    else
        //    {
        //        MessageBox.Show("登录失败!");
        //    }
        //}

        //public void browserOnLoaded(object sender, EventArgs e)
        //{
        //    var browser = (WebBrowser)sender;
        //    if (browser != null && browser.Document != null && browser.Document.Body != null)
        //    {
        //        IHTMLDocument2 ihtmldocument = (IHTMLDocument2)browser.Document.DomDocument;
        //        HtmlElementCollection elementsByTagName = browser.Document.GetElementsByTagName("IFRAME");
        //        if (elementsByTagName.Count > 0)
        //        {
        //            foreach (HtmlElement htmlElement in elementsByTagName)
        //            {
        //                if (htmlElement.OuterHtml.Contains("xui.ptlogin2.qq.com/cgi-bin/xlogin?"))
        //                {
        //                    var source = "https:" + Regex.Match(htmlElement.OuterHtml, "<iframe.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
        //                    string _embeddedpage = @"<html> <body><iframe class='login_frame' type='text/html' width='" + (webBrowser1.Width-20).ToString() + "' height='305' src='"+ source + "' frameborder='0'></iframe></body><html>";
        //                    webBrowser1.DocumentText = _embeddedpage;
        //                    webBrowser1.ScriptErrorsSuppressed = true;
        //                    webBrowser1.DocumentCompleted += webBrowser1Loaded;

        //                }
                       
        //            }
        //        }
         
        //    }
        //}

        public void mbrowserOnLoaded(object sender, EventArgs e)
        {
            
            var browser = (WebBrowser)sender;
            if (browser != null && browser.Document != null && browser.Document.Body != null)
            {
                if (browser.Document.Body.InnerHtml.IndexOf("login_frame", StringComparison.OrdinalIgnoreCase) + 1 > 0)
                {
                    IHTMLDocument2 ihtmldocument = (IHTMLDocument2)browser.Document.DomDocument;
                    HtmlElementCollection elementsByTagName = browser.Document.GetElementsByTagName("iframe");
                    if (elementsByTagName.Count > 0)
                    {
                        foreach (HtmlElement htmlElement in elementsByTagName)
                        {
                            if (htmlElement.OuterHtml.Contains("https://xui.ptlogin2.qq.com/cgi-bin/xlogin?appid="))
                            {
                                var source =  Regex.Match(htmlElement.OuterHtml, "<iframe.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value.Replace("target=self&amp;", "target=_new&amp;");
                                string _embeddedpage = @"<html> <body><iframe class='login_frame' type='text/html' width='" + (webBrowser2.Width - 20).ToString() + "' height='" + (webBrowser2.Height - 20).ToString() + "' src='" + source + "'></iframe></body><html>";
                                webBrowser2.DocumentText = _embeddedpage;
                                webBrowser2.ScriptErrorsSuppressed = true;
                                webBrowser2.DocumentCompleted += webBrowser2Loaded;
                                break;
                            }

                        }
                    }
                }
                else if (browser.Document.Body.InnerHtml.IndexOf("登录", StringComparison.OrdinalIgnoreCase) + 1 > 0)
                {
                    browser.Document.GetElementById("login").Focus();
                    browser.Document.GetElementById("login").InvokeMember("Click");
                    //SendKeys.Send("{ENTER}");
                }  

            }
        }
        public void webBrowser2Loaded(object sender, EventArgs e)
        {
            var browser = (WebBrowser)sender;
            if (browser != null && browser.Document != null && browser.Document.Body != null)
            {
                string cookieStr = browser.Document.Cookie;
                if (string.IsNullOrEmpty(cookieStr)==false)
                {
                    string[] cookstr = cookieStr.Split(';');
                    foreach (string cookie in cookstr)
                    {
                        Console.WriteLine(cookie);
                        if (cookie.Trim().IndexOf("skey") == 0)
                        {
                            string[] cookieNameValue = cookie.Split('=');
                            if (!string.IsNullOrEmpty(cookieNameValue[1]))
                            {
                                int t = 5381;
                                for (int r = 0, n = cookieNameValue[1].Length; r < n; ++r)
                                {
                                    t += (t << 5) + (CharAt(cookieNameValue[1], r).ToCharArray()[0] & 0xff);
                                }
                                string bkn = (2147483647 & t).ToString();
                                MessageBox.Show("登录成功!" + Environment.NewLine + cookie + Environment.NewLine + " bkn=" +bkn );
                                break;
                            }
                        }
                    }
                }
              
            }
        }
        private string CharAt(string s, int index)
        {
            if ((index >= s.Length) || (index < 0))
                return "";
            return s.Substring(index, 1);
        }
    }
}
