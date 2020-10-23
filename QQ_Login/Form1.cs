using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

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
            //Random rand = new Random();
            //double r = rand.NextDouble();
            //string codeimageurl = string.Format(@"https://ssl.ptlogin2.qq.com/ptqrshow?appid=1006102&amp;e=2&amp;l=M&amp;s=3&amp;d=72&amp;v=4&amp;t={0}&amp;daid=1&amp;pt_3rd_aid=0", r);
            //var request = WebRequest.Create(codeimageurl);
            //using (var response = request.GetResponse())
            //using (var stream = response.GetResponseStream())
            //{
            //    pictureBox1.Image = Bitmap.FromStream(stream);
            //}

            WebBrowser browser =new WebBrowser();
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            browser.ScriptErrorsSuppressed = false;
            browser.Navigate(new Uri("https://id.qq.com/login/ptlogin.html"));
            browser.DocumentCompleted += OnDomContentLoaded;

        }

        public static bool IsLogin(string qqnum, string code, string passwords, CookieContainer mycookiecontainer)
        {
            WebHeaderCollection myWebHeaderCollection = new WebHeaderCollection();
            var redirect_geturl = string.Empty;
            var head1 = new WebHeaderCollection();
            Dictionary<string, string> Headerdics = new Dictionary<string, string>()
            {
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3"},
                {"ContentType", "application/x-www-form-urlencoded"},
                {"Referer", "http://ui.ptlogin2.qq.com/cgi-bin/login?appid=1006102&s_url=http://id.qq.com/index.html"},
                {"UserAgent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4098.3 Safari/537.36"}
            };
            string password = PasswordHelper.GetPassword(qqnum, passwords, code);
            string loginUrlstring = @"http://ptlogin2.qq.com/login?u=" + qqnum + "&p=" + password + "&verifycode=" + code + "&aid=1006102&u1=http%3A%2F%2Fid.qq.com%2Findex.html&h=1&ptredirect=1&ptlang=2052&from_ui=1&dumy=&fp=loginerroralert&action=8-29-82478035&mibao_css=&t=1&g=1";
            string retString = HttpHelper.RequestGet(loginUrlstring, Headerdics, head1, mycookiecontainer, ref redirect_geturl);
            Debug.WriteLine(retString);
            return retString.Contains("ptuiCB('0',") ? true : false;
        }
        public static string GetVerfiyCode(string qqnum, CookieContainer mycookiecontainer)
        {
            WebHeaderCollection myWebHeaderCollection = new WebHeaderCollection();
            var redirect_geturl = string.Empty;
            var head1 = new WebHeaderCollection();
            Dictionary<string, string> Headerdics = new Dictionary<string, string>()
            {
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3"},
                {"ContentType", "application/x-www-form-urlencoded"},
                {"Referer", "http://ui.ptlogin2.qq.com/cgi-bin/login?appid=1006102&s_url=http://id.qq.com/index.html"},
                {"UserAgent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4098.3 Safari/537.36"}
            };
            Random rand = new Random();
            double r = rand.NextDouble();
            string checkcodeurl = string.Format(@"http://check.ptlogin2.qq.com/check?uin={0}&appid=1006102&r={1}", qqnum, r);
            string retString = HttpHelper.RequestGet(checkcodeurl, Headerdics, head1, mycookiecontainer, ref redirect_geturl);
            return retString;
        }
    
        public static Stream GetVerfycodeImage(CookieContainer mycookiecontainer, string qqnum)
        {
            Random rand = new Random();
            double r = rand.NextDouble();
            string codeimageurl = string.Format("http://captcha.qq.com/getimage?aid=1006102&r={0}&uin={1}", r, qqnum);
            Stream retStream = HttpHelper.GetStream(codeimageurl, mycookiecontainer);
            return retStream;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CookieContainer mycookiecontainer = new CookieContainer();
            //bool success = IsLogin(this.txtUseraccount.Text, code, this.txtPassword.Text, cookie);
            string retString = GetVerfiyCode(txtUseraccount.Text, mycookiecontainer);
            if (retString.Contains("ptui_checkVC('0','"))
            {
                //不需要手动输入
                Form frm = new Form2();
                Form2.MyInstance.txtverfiycode.Text = retString.Replace("ptui_checkVC('0','", "").Replace("'", "").Replace(")", "").Replace(";", "").Substring(0, 4);
                frm.ShowDialog();

            }
            else if (retString.Contains("ptui_checkVC('1',"))
            {
                //需要手动输入，将验证码输出在image中
                Form frm = new Form2();
                Form2.MyInstance.vefycodpicbox.Image = Image.FromStream(GetVerfycodeImage(mycookiecontainer, this.txtUseraccount.Text));
                frm.ShowDialog();                
            }
            bool success = IsLogin(txtUseraccount.Text,VerificationCode,txtPassword.Text,mycookiecontainer);
            if (success)
            {
                MessageBox.Show("登录成功!");
            }
            else
            {
                MessageBox.Show("登录失败!");
            }
        }

        public void OnDomContentLoaded(object sender, EventArgs e)
        {
            var browser = (WebBrowser)sender;
            if (browser != null && browser.Document != null && browser.Document.Body != null)
            {
   
                //HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                //doc.LoadHtml(browser.DocumentText);
                //var node = doc.DocumentNode.SelectSingleNode("//span[@id='qlogin_list']");

                System.Windows.Forms.HtmlDocument htmlDocument = browser.Document;

                HtmlElementCollection htmlElementCollection = htmlDocument.Images;
                foreach (HtmlElement htmlElement in htmlElementCollection)
                {
                    string imgUrl = htmlElement.GetAttribute("src");
                    if (imgUrl.StartsWith("https://ssl.ptlogin2.qq.com/ptqrshow?appid=1006102&amp;e=2&amp;l=M&amp;s=3&amp;d=72&amp;v=4&amp;t="))
                    {
                        this.pictureBox1.ImageLocation = imgUrl;
                    }

                }
            }
        }
     
    }
}
