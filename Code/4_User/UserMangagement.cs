using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vison_Inspect_System._2_ComPart.Normal_Form;
using Vison_Inspect_System._2_ComPart;

namespace Vison_Inspect_System
{
    public partial class UserMangagement : Form
    {
        userHelper userhelp = new userHelper();
        List<Users> listuser = new List<Users>();
        public UserMangagement()
        {
            InitializeComponent();
            if (File.Exists("user.SRX"))
            {
                listuser = userhelp.DeSerializeUser("user.SRX");
                ShowUserList(listuser);
            }
        }

        public void ShowUserList(List<Users> userlist)
        {
            List<Users> Uplist = new List<Users>();
            for (int i = 0; i < userlist.Count; i++)
            {
                if (i > 0)
                {
                    Uplist.Add(userlist[i]);
                }
            }
            dgvManager.DataSource = null;
            dgvManager.DataSource = Uplist;
        }
        private Users Getuser(List<Users> listuser)
        {
            if (txbUserName.Text.Trim() == string.Empty | txbPassword.Text == string.Empty)
            {
                return null;
            }
            Users user = new Users()
            { SN = listuser[listuser.Count - 1].SN + 1, Username = txbUserName.Text.Trim(), Password = txbPassword.Text.Trim() };
            switch (comboxauthority.SelectedIndex)
            {
                case 0:
                    user.Level = Authroity.Administrator;
                    break;
                case 1:
                    user.Level = Authroity.Engineer;
                    break;
                case 2:
                    user.Level = Authroity.Operator;
                    break;
                default:
                    break;
            }
            return user;
        }
        private void Setuser()
        {
            DataGridViewSelectedRowCollection RowCollection = dgvManager.SelectedRows;
            if (RowCollection.Count == 0) return;
            DataGridViewRow row = RowCollection[0];
            txbUserName.Text = row.Cells[1].Value.ToString();
            txbPassword.Text = row.Cells[2].Value.ToString();
            switch (row.Cells[3].Value.ToString())
            {
                case "Administrator":
                    comboxauthority.SelectedIndex = 0;
                    break;
                case "Engineer":
                    comboxauthority.SelectedIndex = 1;
                    break;
                case "Operator":
                    comboxauthority.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            Users user = Getuser(listuser);
            if (user == null)
            {
                MessageBox.Show("添加的用户名不正确","增加的用户名错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            var res = userhelp.Adduser("user.SRX", listuser, user);
            if (res)
            {
                MessageBox.Show("添加用户成功");
                ShowUserList(listuser);
                Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:添加新用户【{user.Username}】成功");
            }
            else
            {
                MessageBox.Show("添加用户失败");
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (txbUserName.Text == string.Empty)
            {
                MessageBox.Show("删除用户名未输入");
                return;
            }
            var res = userhelp.DeleteUser("user.SRX", listuser, txbUserName.Text);
            if (res)
            {
                MessageBox.Show("成功删除用户名：" + txbUserName.Text);
                ShowUserList(listuser);
                Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:删除用户【{txbUserName.Text}】成功");
            }
            else
            {
                MessageBox.Show("删除用户名：" + txbUserName.Text + "失败");
            }
        }

        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            if (txbUserName.Text == string.Empty)
            {
                MessageBox.Show("修改用户名未输入");
                return;
            }
            Users user = Getuser(listuser);
            var res = userhelp.EditUser("user.SRX", listuser, user);

            if (res)
            {
                MessageBox.Show("成功修改用户名：" + txbUserName.Text);
                ShowUserList(listuser);
                Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:修改用户名【{txbUserName.Text}】成功");
            }
            else
            {
                MessageBox.Show("修改用户名：" + txbUserName.Text + "失败");
            }
        }

        private void dgvManager_SelectionChanged(object sender, EventArgs e)
        {
            Setuser();
        }

        private void UserMangagement_Load(object sender, EventArgs e)
        {
            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:进入用户管理界面");
        }
    }
}
