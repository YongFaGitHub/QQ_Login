using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQ_Login
{
    public partial class Form2 : Form
    {
        public static Form2 MyInstance;
        public Form2()
        {
            InitializeComponent();
            MyInstance = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.VerificationCode = txtverfiycode.Text;
            this.Close();
        }
    }
}
