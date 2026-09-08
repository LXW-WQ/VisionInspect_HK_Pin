using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vison_Inspect_System
{
    public delegate void SendLogin(Users user);
    public partial class UserLoginFrm : Form
    {
        public SendLogin sendlogin;
        public UserLoginFrm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           userHelper helper = new userHelper();
            var res = helper.CheckUserLogin("user.SRX", txbUserName.Text, txbPassword.Text);
            if (res == null)
            {
                MessageBox.Show("用户名或密码不正确");
                return;
            }
            sendlogin(res);
            this.Close();
        }

      
    }
}
