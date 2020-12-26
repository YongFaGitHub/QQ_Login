using AndroidQQLib.QQ580.AndroidQQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQ_Login
{
    public partial class AndroidQQ : Form
    {
        public AndroidQQ()
        {
            InitializeComponent();
        }

        Android sdk = new Android();
        private int state = 0;
        private string code = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "登录")
            {
                Console.WriteLine("开始登录QQ：" + textBox1.Text);
                sdk.init(textBox1.Text, textBox2.Text);
                state = sdk.Fun_Login();
                this.do_login();
            }
            else if (button1.Text == "再次登录")
            {
                do_login(textBox3.Text);
            }
        }

        private void do_login(string code = "")
        {
            if (state == sdk.login_state_success)
            {
                string test = sdk.QQ_online();   //上线
                //button1.Enabled = false;
                MessageBox.Show("登录成功！" + test);
            }
            else if (state == sdk.login_state_veriy)
            {
                Console.WriteLine("需要验证码");
                button1.Text = "再次登录";
                if (code != "")
                {
                    state = sdk.Fun_SendCode(code);
                    do_login();
                    return;
                }
                try
                {
                    byte[] b = sdk.getViery();
                    FileStream fs = new FileStream("vcode.png", FileMode.Create);
                    fs.Write(b, 0, b.Length);
                    fs.Flush();
                    fs.Close();
                    Bitmap bp = new Bitmap("vcode.png");
                    pictureBox1.Image = bp;
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                MessageBox.Show("登录失败" + sdk.getLastError());
            }
        }
        private void AndroidQQ_Load(object sender, EventArgs e)
        {

        }


    }
}
