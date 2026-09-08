
namespace Vison_Inspect_System
{
    partial class UserMangagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvManager = new System.Windows.Forms.DataGridView();
            this.labUserName = new System.Windows.Forms.Label();
            this.labPassword = new System.Windows.Forms.Label();
            this.txbUserName = new System.Windows.Forms.TextBox();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.BtnAddUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnChangeUser = new System.Windows.Forms.Button();
            this.comboxauthority = new System.Windows.Forms.ComboBox();
            this.labuserLevel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManager)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvManager
            // 
            this.dgvManager.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManager.Location = new System.Drawing.Point(15, 57);
            this.dgvManager.Margin = new System.Windows.Forms.Padding(2);
            this.dgvManager.Name = "dgvManager";
            this.dgvManager.RowHeadersWidth = 62;
            this.dgvManager.RowTemplate.Height = 30;
            this.dgvManager.Size = new System.Drawing.Size(481, 243);
            this.dgvManager.TabIndex = 0;
            this.dgvManager.SelectionChanged += new System.EventHandler(this.dgvManager_SelectionChanged);
            // 
            // labUserName
            // 
            this.labUserName.AutoSize = true;
            this.labUserName.Location = new System.Drawing.Point(35, 321);
            this.labUserName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labUserName.Name = "labUserName";
            this.labUserName.Size = new System.Drawing.Size(53, 12);
            this.labUserName.TabIndex = 1;
            this.labUserName.Text = "UserName";
            // 
            // labPassword
            // 
            this.labPassword.AutoSize = true;
            this.labPassword.Location = new System.Drawing.Point(35, 361);
            this.labPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labPassword.Name = "labPassword";
            this.labPassword.Size = new System.Drawing.Size(53, 12);
            this.labPassword.TabIndex = 1;
            this.labPassword.Text = "Password";
            // 
            // txbUserName
            // 
            this.txbUserName.Location = new System.Drawing.Point(117, 314);
            this.txbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.txbUserName.Name = "txbUserName";
            this.txbUserName.Size = new System.Drawing.Size(179, 21);
            this.txbUserName.TabIndex = 2;
            // 
            // txbPassword
            // 
            this.txbPassword.Location = new System.Drawing.Point(117, 354);
            this.txbPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.Size = new System.Drawing.Size(179, 21);
            this.txbPassword.TabIndex = 2;
            // 
            // BtnAddUser
            // 
            this.BtnAddUser.Location = new System.Drawing.Point(346, 309);
            this.BtnAddUser.Margin = new System.Windows.Forms.Padding(2);
            this.BtnAddUser.Name = "BtnAddUser";
            this.BtnAddUser.Size = new System.Drawing.Size(121, 24);
            this.BtnAddUser.TabIndex = 3;
            this.BtnAddUser.Text = "AddUser";
            this.BtnAddUser.Click += new System.EventHandler(this.BtnAddUser_Click);
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Location = new System.Drawing.Point(346, 349);
            this.btnDeleteUser.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(121, 24);
            this.btnDeleteUser.TabIndex = 3;
            this.btnDeleteUser.Text = "DeleteUser";
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // btnChangeUser
            // 
            this.btnChangeUser.Location = new System.Drawing.Point(346, 387);
            this.btnChangeUser.Margin = new System.Windows.Forms.Padding(2);
            this.btnChangeUser.Name = "btnChangeUser";
            this.btnChangeUser.Size = new System.Drawing.Size(121, 24);
            this.btnChangeUser.TabIndex = 3;
            this.btnChangeUser.Text = "UserMod";
            this.btnChangeUser.Click += new System.EventHandler(this.btnChangeUser_Click);
            // 
            // comboxauthority
            // 
            this.comboxauthority.FormattingEnabled = true;
            this.comboxauthority.ItemHeight = 12;
            this.comboxauthority.Items.AddRange(new object[] {
            "Administrator",
            "Engineer",
            "Operator"});
            this.comboxauthority.Location = new System.Drawing.Point(117, 391);
            this.comboxauthority.Name = "comboxauthority";
            this.comboxauthority.Size = new System.Drawing.Size(179, 20);
            this.comboxauthority.TabIndex = 4;
            // 
            // labuserLevel
            // 
            this.labuserLevel.AutoSize = true;
            this.labuserLevel.Location = new System.Drawing.Point(35, 399);
            this.labuserLevel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labuserLevel.Name = "labuserLevel";
            this.labuserLevel.Size = new System.Drawing.Size(59, 12);
            this.labuserLevel.TabIndex = 1;
            this.labuserLevel.Text = "UserLevel";
            // 
            // UserMangagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 424);
            this.Controls.Add(this.comboxauthority);
            this.Controls.Add(this.btnChangeUser);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.BtnAddUser);
            this.Controls.Add(this.txbPassword);
            this.Controls.Add(this.txbUserName);
            this.Controls.Add(this.labuserLevel);
            this.Controls.Add(this.labPassword);
            this.Controls.Add(this.labUserName);
            this.Controls.Add(this.dgvManager);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserMangagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserMangagement";
            this.Load += new System.EventHandler(this.UserMangagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvManager;
        private System.Windows.Forms.Label labUserName;
        private System.Windows.Forms.Label labPassword;
        private System.Windows.Forms.TextBox txbUserName;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.Button BtnAddUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnChangeUser;
        private System.Windows.Forms.ComboBox comboxauthority;
        private System.Windows.Forms.Label labuserLevel;
    }
}