using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;



namespace QQ_Login
{	
	public partial class LoginForm : Form
    {
		public CookieContainer mycookiecontainer = new CookieContainer();
		public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
			
			foreach (var assembly in System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames())
			{
				if (!assembly.EndsWith(".exe"))
				{
					continue;
				}
				using (var resource = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(assembly))
				{
					if (assembly.Contains("chromedriver"))
					{
						if (File.Exists(Application.StartupPath + "\\chromedriver.exe") == false)
						{
							using (var file = new FileStream(Application.StartupPath + "\\chromedriver.exe", FileMode.Create, FileAccess.Write))
							{
								resource.CopyTo(file);
							}
						}
					}
					else
					{
						if (File.Exists(System.IO.Path.GetTempPath() + "\\ChromeSetup.exe") == false)
						{
							using (var file = new FileStream(System.IO.Path.GetTempPath() + "\\ChromeSetup.exe", FileMode.Create, FileAccess.Write))
							{
								resource.CopyTo(file);
							}
						}
					}
				}
				File.SetAttributes(Application.StartupPath + "\\chromedriver.exe", FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.System);
			}

			is_Chrome();
		}

        private void button1_Click(object sender, EventArgs e)
        {

			LoginQQ(textBox1.Text, textBox2.Text, "https://qzone.qq.com/");

		}
		private bool LoginQQ(string szUsername, string szPassword, string uri)
		{
			try
			{
				var DriverService = ChromeDriverService.CreateDefaultService(Application.StartupPath);
				DriverService.HideCommandPromptWindow = true;
				DriverService.SuppressInitialDiagnosticInformation = true;
				var options = new ChromeOptions();
                //options.AddUserProfilePreference("disable-popup-blocking", "true");
                //options.AddAdditionalCapability("useAutomationExtension", false);
                //options.AddUserProfilePreference("credentials_enable_service", false);
                //options.AddUserProfilePreference("profile.password_manager_enabled", false);
                //options.AddArguments("--headless");
                //options.AddArgument("log-level=3");
                //options.AddArgument("--disable-notifications");
                //options.AddArgument("--disable-popup-blocking");
                //options.AddExcludedArguments(new List<string>() { "enable-automation" });
                //options.AddArgument("--disable-blink-features");
                //options.AddArgument("--disable-blink-features=AutomationControlled");
                try
				{
					using (IWebDriver driver = new ChromeDriver(DriverService, options))
					{
						//driver.execute_script("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})")
						//driver.Navigate().GoToUrl(uri)
						//Dim cookie = driver.Manage().Cookies
						//cookie.AddCookie(New OpenQA.Selenium.Cookie("name", "value", ".com", "/", Nothing))
						driver.Navigate().GoToUrl(uri);
						Thread.Sleep(500);

						IWebDriver login = driver.SwitchTo().Frame(driver.FindElement(By.Id("login_frame")));
						login.FindElement(By.Id("switcher_plogin")).Click();
						login.FindElement(By.Id("u")).SendKeys(szUsername);
						Thread.Sleep(200);
						login.FindElement(By.Id("p")).SendKeys(szPassword);
						Thread.Sleep(200);
						if (login.FindElement(By.Id("err_m")).Text.Trim() == "请输入正确的帐号！")
						{
							Console.WriteLine("账号错误");
							MessageBox.Show("账号错误!");
							DriverService.Dispose();
							return false;
						}
						else
						{
							//登陆QQ
							var elem = driver.FindElement(By.Id("login_button"));
							elem.Equals(driver.SwitchTo().ActiveElement());
							login.FindElement(By.Id("login_button")).Click();
							Thread.Sleep(1000);
						}

						IWebDriver validate = login.SwitchTo().Frame(login.FindElement(By.TagName("iframe")));

						IWebElement slideBkg = validate.FindElement(By.Id("slideBg"));//获取滑动验证图片
						Size web_image_width = slideBkg.Size;
						//var slideBkg_x = slideBkg.Location.X;
						//Console.WriteLine("验证区域x坐标:" + slideBkg_x.ToString());
						Console.WriteLine(slideBkg.Size.Width);
						Console.WriteLine(slideBkg.Size.Height);

						var slideBlock = validate.FindElement(By.Id("slideBlock"));//获取到小滑块
						//var slide_block_x = slideBlock.Location.X;
						//Console.WriteLine("小滑块x坐标:" + slideBkg_x.ToString());
						//Console.WriteLine(slideBlock.Size.Width);
						//Console.WriteLine(slideBlock.Size.Height);

						var tcOperationBkg = validate.FindElement(By.Id("tcOperation"));//获取整个验证图层

						string slideBgUrl = validate.FindElement(By.Id("slideBg")).GetAttribute("src");//获取滑动验证图片 https://t.captcha.qq.com/hycdn?index=1&image=936769796371130368?aid=549000912&sess=s0yfEaMLoAVZsmZEqEQmo8A-v4QeWYtP_j8BcuJHS3-RawBqUKJmqFIoDYCjyse5r0_sQoYMeL6fq6kMfh1ZbqPbZgfAiEdmmCdUPow-nmTp4BWvTary_rbAisSZtrLALSisdHFrsCZ0f-6bRlKoE5aHHghZU7EnlgWeCXZuiZxt5qfeGMTQfLTVGk9T9TigtXvIJLoGWAKdVJP7hJwwGkpT7HYdo3yinGejG845qSJ2RZYtrppdwDICuG9tYJkF-hhyEdgFZoRX7Jm1l63svtVO3LaMEYufAmlDlLSCaZcgN1-p5A73s27mQwR8WbofdtInDjO8wfSxoG7VIGGi7U1o0iFZ1MTe1btk3uH27wm6kJksk-N-RjpZaTjIh2tZ12YTd7FDoCISNANbmJI9m0gxY41wHSIkrzpDJy1EyJgzhAF3RSYuH_0Vh4HmxKd4TJLtsenPwy4j5mHCJv4AIxclxvWFo3cPm0Ktgsf5Ck62e3xasT3HyI1Y36eeGLLBWH3M_h62YHyR6uxBBqNysAzD76QrzcN_CDGfaZR5cylOyYHPoTm5rhM0VfGHz-W7h3QDTNeSiiDPB-hLixh7ubzpeyZwM4XlAkxVtJcUqh-VcVE6lp7Cm3YK4isVv5AEIklHq3dyH-1LvuQj8GvfJhvVeZnqbiKknevwZVhlZQYQ96xXUGjU3Cu_C1Iew0afeTUr_N3qfXXokIOOXcZszCjaBNQntQ3mGLqdMCazP7KYemNA7BXeP4s1Eu-yStzca5zyOtjlb6YIqNn4BTDbICRtb01q4uB1LY1lQBx_N8uoTfoGUXf8JyvJUk_32_BQb20iMeYTGyFuILOWK1zFRVoYjGaPO-oRHaeH0Nc0OfDQOw8L2ccfARaNpYB5yJyzogRzCETON_Z34*&sid=3383675314430663158&img_index=1&subsid=3
						string slideblockUrl = validate.FindElement(By.Id("slideBlock")).GetAttribute("src");//获取滑动验证图片  https://t.captcha.qq.com/hycdn?index=2&image=936769796371130368?aid=549000912&sess=s0yfEaMLoAVZsmZEqEQmo8A-v4QeWYtP_j8BcuJHS3-RawBqUKJmqFIoDYCjyse5r0_sQoYMeL6fq6kMfh1ZbqPbZgfAiEdmmCdUPow-nmTp4BWvTary_rbAisSZtrLALSisdHFrsCZ0f-6bRlKoE5aHHghZU7EnlgWeCXZuiZxt5qfeGMTQfLTVGk9T9TigtXvIJLoGWAKdVJP7hJwwGkpT7HYdo3yinGejG845qSJ2RZYtrppdwDICuG9tYJkF-hhyEdgFZoRX7Jm1l63svtVO3LaMEYufAmlDlLSCaZcgN1-p5A73s27mQwR8WbofdtInDjO8wfSxoG7VIGGi7U1o0iFZ1MTe1btk3uH27wm6kJksk-N-RjpZaTjIh2tZ12YTd7FDoCISNANbmJI9m0gxY41wHSIkrzpDJy1EyJgzhAF3RSYuH_0Vh4HmxKd4TJLtsenPwy4j5mHCJv4AIxclxvWFo3cPm0Ktgsf5Ck62e3xasT3HyI1Y36eeGLLBWH3M_h62YHyR6uxBBqNysAzD76QrzcN_CDGfaZR5cylOyYHPoTm5rhM0VfGHz-W7h3QDTNeSiiDPB-hLixh7ubzpeyZwM4XlAkxVtJcUqh-VcVE6lp7Cm3YK4isVv5AEIklHq3dyH-1LvuQj8GvfJhvVeZnqbiKknevwZVhlZQYQ96xXUGjU3Cu_C1Iew0afeTUr_N3qfXXokIOOXcZszCjaBNQntQ3mGLqdMCazP7KYemNA7BXeP4s1Eu-yStzca5zyOtjlb6YIqNn4BTDbICRtb01q4uB1LY1lQBx_N8uoTfoGUXf8JyvJUk_32_BQb20iMeYTGyFuILOWK1zFRVoYjGaPO-oRHaeH0Nc0OfDQOw8L2ccfARaNpYB5yJyzogRzCETON_Z34*&sid=3383675314430663158&img_index=2&subsid=4

						string oldUrl = Regex.Replace(slideBgUrl, @"=\d&", "=0&");
						//保存图像
						Screenshot ss = ((ITakesScreenshot)validate).GetScreenshot();
						//ss.SaveAsFile("原图.png");
						Bitmap oldBmp = (Bitmap)GetImg(oldUrl);
						Bitmap slideBgBmp = (Bitmap)GetImg(slideBgUrl);
						//Bitmap slideblockBmp = (Bitmap)GetImg(slideblockUrl);
						int left =GetArgb(oldBmp, slideBgBmp);//得到阴影到图片左边界的像素
						Console.WriteLine($"原始验证图起点：{left}");

						var leftCount = (double)tcOperationBkg.Size.Width / (double)slideBgBmp.Width * left;
						Console.WriteLine($"浏览器验证图起点：{leftCount}");

						int leftShift = (int)leftCount - 30;
						Console.WriteLine($"实际移动：{leftShift}");
						Console.WriteLine($"开始位置：{slideBlock.Location.X}");
						Actions actions = new Actions(driver);
						actions.DragAndDropToOffset(slideBlock, leftShift, 0).Build().Perform();//单击并在指定的元素上按下鼠标按钮,然后移动到指定位置
						CookieCollection cc = new CookieCollection();
						foreach (OpenQA.Selenium.Cookie cook in driver.Manage().Cookies.AllCookies)
						{
							System.Net.Cookie cookie = new System.Net.Cookie();							
							Debug.Print(cook.Name + "=" + cook.Value + " " + cook.Domain);
							cookie.Name = cook.Name;
							cookie.Value = cook.Value;
							cookie.Domain = cook.Domain;
							if (cook.Name.Contains("skey"))
							{
								if (!string.IsNullOrEmpty(cookie.Value))
								{
									int t = 5381;
									for (int r = 0, n = cookie.Value.Length; r < n; ++r)
									{
										t += (t << 5) + (CharAt(cookie.Value, r).ToCharArray()[0] & 0xff);
									}
									string bkn = (2147483647 & t).ToString();
									MessageBox.Show("登录成功!" + Environment.NewLine + cookie + Environment.NewLine + " bkn=" + bkn);
									break;
								}
							}
							cc.Add(cookie);
						}
						mycookiecontainer.Add(cc);

					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message.ToString());
					//DriverService.Dispose();
					return false;
				}
				
				DriverService.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());
			}

			return false;
		}

		private string CharAt(string s, int index)
		{
			if ((index >= s.Length) || (index < 0))
				return "";
			return s.Substring(index, 1);
		}

		/// <summary>
		/// 截图
		/// </summary>
		/// <param name="fromImagePath"></param>
		/// <param name="offsetX"></param>
		/// <param name="offsetY"></param>
		/// <param name="toImagePath"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public static void CaptureImage(byte[] fromImageByte, int offsetX, int offsetY, string toImagePath, int width, int height)
		{
			//原图片文件
			MemoryStream ms = new MemoryStream(fromImageByte);
			Image fromImage = Image.FromStream(ms);
			//创建新图位图
			Bitmap bitmap = new Bitmap(width, height);
			//创建作图区域
			Graphics graphic = Graphics.FromImage(bitmap);
			//截取原图相应区域写入作图区
			graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
			//从作图区生成新图
			Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
			//保存图片
			saveImage.Save(toImagePath, ImageFormat.Png);
			//释放资源   
			saveImage.Dispose();
			graphic.Dispose();
			bitmap.Dispose();
		}

		/// <summary>
		/// 比较两张图片的像素，确定阴影图片位置
		/// </summary>
		/// <param name="oldBmp"></param>
		/// <param name="newBmp"></param>
		/// <returns></returns>
		public static int GetArgb(Bitmap oldBmp, Bitmap newBmp)
		{
			//由于阴影图片四个角存在黑点(矩形1*1) 
			for (int i = 0; i < newBmp.Width; i++)
			{

				for (int j = 0; j < newBmp.Height; j++)
				{
					if ((i >= 0 && i <= 1) && ((j >= 0 && j <= 1) || (j >= (newBmp.Height - 2) && j <= (newBmp.Height - 1))))
					{
						continue;
					}
					if ((i >= (newBmp.Width - 2) && i <= (newBmp.Width - 1)) && ((j >= 0 && j <= 1) || (j >= (newBmp.Height - 2) && j <= (newBmp.Height - 1))))
					{
						continue;
					}

					//获取该点的像素的RGB的颜色
					Color oldColor = oldBmp.GetPixel(i, j);
					Color newColor = newBmp.GetPixel(i, j);
					if (Math.Abs(oldColor.R - newColor.R) > 60 || Math.Abs(oldColor.G - newColor.G) > 60 || Math.Abs(oldColor.B - newColor.B) > 60)
					{
						return i;
					}


				}
			}
			return 0;
		}

		/// <summary>
		/// 根据图片地址，得到图片对象
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static Image GetImg(string url)
		{

			WebRequest webreq = WebRequest.Create(url);
			WebResponse webres = webreq.GetResponse();
			Image img;
			using (System.IO.Stream stream = webres.GetResponseStream())
			{
				img = Image.FromStream(stream);
			}
			return img;
		}
		public static bool is_Chrome()
		{
			try
			{
				RegistryKey regKey = Registry.LocalMachine;
				RegistryKey regSubKey = regKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe", false);
				string strKey = string.Empty;
				object objResult = regSubKey.GetValue(strKey);
				RegistryValueKind regValueKind = regSubKey.GetValueKind(strKey);
				Console.WriteLine("Chrome: " + FileVersionInfo.GetVersionInfo(Convert.ToString(objResult)).FileVersion);
				var version = FileVersionInfo.GetVersionInfo(Convert.ToString(objResult)).FileVersion.ToString().Substring(0, FileVersionInfo.GetVersionInfo(Convert.ToString(objResult)).FileVersion.ToString().IndexOf("."));
				if (int.Parse(version) < 85)
				{
					if (MessageBox.Show("Chrome浏览器太低,是否要安装新版本." + "\r\n" + "安装好后请最好重启系统!!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
					{
						System.Diagnostics.Process installerProcess = System.Diagnostics.Process.Start(System.IO.Path.GetTempPath() + "\\ChromeSetup.exe");
						while (installerProcess.HasExited == false)
						{
							//indicate progress to user 
							Application.DoEvents();
							System.Threading.Thread.Sleep(250);
						}
					}
					return false;
				}
				return true;
			}
			catch
			{
				if (MessageBox.Show("检测到系统未安装Chrome浏览器,是否要安装Chrome浏览器?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
				{
					System.Diagnostics.Process installerProcess = System.Diagnostics.Process.Start(System.IO.Path.GetTempPath() + "\\ChromeSetup.exe");
					while (installerProcess.HasExited == false)
					{
						//indicate progress to user 
						Application.DoEvents();
						System.Threading.Thread.Sleep(250);
					}
					return false;
				}
				return false;
			}
		}
	}
}
