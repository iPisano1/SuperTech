namespace Computer_Shop_System
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.loginForm_Dashboard = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.registerSwitchBtn = new System.Windows.Forms.Button();
            this.loginSwitchBtn = new System.Windows.Forms.Button();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.login_LoginBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.login_PasswordText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.login_UsernameText = new System.Windows.Forms.TextBox();
            this.registerPanel = new System.Windows.Forms.Panel();
            this.register_RegisterBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.register_PasswordText = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.register_UsernameText = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.minimizeBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.loginForm_Dashboard.SuspendLayout();
            this.panel2.SuspendLayout();
            this.loginPanel.SuspendLayout();
            this.registerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginForm_Dashboard
            // 
            this.loginForm_Dashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(87)))), ((int)(((byte)(122)))));
            this.loginForm_Dashboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginForm_Dashboard.Controls.Add(this.label9);
            this.loginForm_Dashboard.Controls.Add(this.panel1);
            this.loginForm_Dashboard.Controls.Add(this.minimizeBtn);
            this.loginForm_Dashboard.Controls.Add(this.exitBtn);
            this.loginForm_Dashboard.Location = new System.Drawing.Point(-1, -2);
            this.loginForm_Dashboard.Name = "loginForm_Dashboard";
            this.loginForm_Dashboard.Size = new System.Drawing.Size(1031, 59);
            this.loginForm_Dashboard.TabIndex = 0;
            this.loginForm_Dashboard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.loginForm_Dashboard_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(131, 238);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 65);
            this.label1.TabIndex = 1;
            this.label1.Text = "SuperTech Shop";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(79, 303);
            this.label2.Margin = new System.Windows.Forms.Padding(70, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(489, 48);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.registerSwitchBtn);
            this.panel2.Controls.Add(this.loginSwitchBtn);
            this.panel2.Controls.Add(this.loginPanel);
            this.panel2.Controls.Add(this.registerPanel);
            this.panel2.Location = new System.Drawing.Point(654, 100);
            this.panel2.Margin = new System.Windows.Forms.Padding(40, 40, 70, 40);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(20);
            this.panel2.Size = new System.Drawing.Size(294, 464);
            this.panel2.TabIndex = 3;
            // 
            // registerSwitchBtn
            // 
            this.registerSwitchBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.registerSwitchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.registerSwitchBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.registerSwitchBtn.Location = new System.Drawing.Point(156, 384);
            this.registerSwitchBtn.Name = "registerSwitchBtn";
            this.registerSwitchBtn.Size = new System.Drawing.Size(115, 57);
            this.registerSwitchBtn.TabIndex = 2;
            this.registerSwitchBtn.Text = "Register";
            this.registerSwitchBtn.UseVisualStyleBackColor = true;
            this.registerSwitchBtn.Click += new System.EventHandler(this.registerSwitchBtn_Click);
            // 
            // loginSwitchBtn
            // 
            this.loginSwitchBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginSwitchBtn.Enabled = false;
            this.loginSwitchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginSwitchBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginSwitchBtn.Location = new System.Drawing.Point(23, 384);
            this.loginSwitchBtn.Name = "loginSwitchBtn";
            this.loginSwitchBtn.Size = new System.Drawing.Size(115, 57);
            this.loginSwitchBtn.TabIndex = 1;
            this.loginSwitchBtn.Text = "Log In";
            this.loginSwitchBtn.UseVisualStyleBackColor = true;
            this.loginSwitchBtn.Click += new System.EventHandler(this.loginSwitchBtn_Click);
            // 
            // loginPanel
            // 
            this.loginPanel.Controls.Add(this.login_LoginBtn);
            this.loginPanel.Controls.Add(this.label5);
            this.loginPanel.Controls.Add(this.login_PasswordText);
            this.loginPanel.Controls.Add(this.label4);
            this.loginPanel.Controls.Add(this.label3);
            this.loginPanel.Controls.Add(this.login_UsernameText);
            this.loginPanel.Controls.Add(this.panel3);
            this.loginPanel.Location = new System.Drawing.Point(23, 23);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.loginPanel.Size = new System.Drawing.Size(248, 342);
            this.loginPanel.TabIndex = 0;
            // 
            // login_LoginBtn
            // 
            this.login_LoginBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.login_LoginBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_LoginBtn.Location = new System.Drawing.Point(76, 297);
            this.login_LoginBtn.Name = "login_LoginBtn";
            this.login_LoginBtn.Size = new System.Drawing.Size(96, 32);
            this.login_LoginBtn.TabIndex = 6;
            this.login_LoginBtn.Text = "Enter";
            this.login_LoginBtn.UseVisualStyleBackColor = true;
            this.login_LoginBtn.Click += new System.EventHandler(this.login_LoginBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(19, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 21);
            this.label5.TabIndex = 5;
            this.label5.Text = "Password";
            // 
            // login_PasswordText
            // 
            this.login_PasswordText.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_PasswordText.Location = new System.Drawing.Point(23, 255);
            this.login_PasswordText.Name = "login_PasswordText";
            this.login_PasswordText.PasswordChar = '*';
            this.login_PasswordText.Size = new System.Drawing.Size(202, 27);
            this.login_PasswordText.TabIndex = 4;
            this.login_PasswordText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterKey_Event);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(89, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "Log In";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "Username";
            // 
            // login_UsernameText
            // 
            this.login_UsernameText.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_UsernameText.Location = new System.Drawing.Point(23, 201);
            this.login_UsernameText.Name = "login_UsernameText";
            this.login_UsernameText.Size = new System.Drawing.Size(202, 27);
            this.login_UsernameText.TabIndex = 1;
            this.login_UsernameText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterKey_Event);
            // 
            // registerPanel
            // 
            this.registerPanel.Controls.Add(this.register_RegisterBtn);
            this.registerPanel.Controls.Add(this.label6);
            this.registerPanel.Controls.Add(this.register_PasswordText);
            this.registerPanel.Controls.Add(this.label7);
            this.registerPanel.Controls.Add(this.label8);
            this.registerPanel.Controls.Add(this.register_UsernameText);
            this.registerPanel.Controls.Add(this.panel4);
            this.registerPanel.Enabled = false;
            this.registerPanel.Location = new System.Drawing.Point(23, 23);
            this.registerPanel.Name = "registerPanel";
            this.registerPanel.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.registerPanel.Size = new System.Drawing.Size(248, 342);
            this.registerPanel.TabIndex = 7;
            this.registerPanel.Visible = false;
            // 
            // register_RegisterBtn
            // 
            this.register_RegisterBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.register_RegisterBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.register_RegisterBtn.Location = new System.Drawing.Point(76, 297);
            this.register_RegisterBtn.Name = "register_RegisterBtn";
            this.register_RegisterBtn.Size = new System.Drawing.Size(96, 32);
            this.register_RegisterBtn.TabIndex = 6;
            this.register_RegisterBtn.Text = "Register";
            this.register_RegisterBtn.UseVisualStyleBackColor = true;
            this.register_RegisterBtn.Click += new System.EventHandler(this.register_RegisterBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(19, 234);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "Password";
            // 
            // register_PasswordText
            // 
            this.register_PasswordText.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.register_PasswordText.Location = new System.Drawing.Point(23, 255);
            this.register_PasswordText.Name = "register_PasswordText";
            this.register_PasswordText.PasswordChar = '*';
            this.register_PasswordText.Size = new System.Drawing.Size(202, 27);
            this.register_PasswordText.TabIndex = 4;
            this.register_PasswordText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterKey_Event);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(80, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 30);
            this.label7.TabIndex = 3;
            this.label7.Text = "Register";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(19, 180);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 21);
            this.label8.TabIndex = 2;
            this.label8.Text = "Username";
            // 
            // register_UsernameText
            // 
            this.register_UsernameText.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.register_UsernameText.Location = new System.Drawing.Point(23, 201);
            this.register_UsernameText.Name = "register_UsernameText";
            this.register_UsernameText.Size = new System.Drawing.Size(202, 27);
            this.register_UsernameText.TabIndex = 1;
            this.register_UsernameText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterKey_Event);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(436, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 40);
            this.label9.TabIndex = 5;
            this.label9.Text = "SuperTech";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = global::Computer_Shop_System.Properties.Resources.login;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Location = new System.Drawing.Point(66, 21);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(117, 117);
            this.panel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImage = global::Computer_Shop_System.Properties.Resources.login;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(66, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(117, 117);
            this.panel4.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Computer_Shop_System.Properties.Resources.dashboards;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Location = new System.Drawing.Point(13, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(45, 40);
            this.panel1.TabIndex = 4;
            // 
            // minimizeBtn
            // 
            this.minimizeBtn.BackColor = System.Drawing.Color.Transparent;
            this.minimizeBtn.BackgroundImage = global::Computer_Shop_System.Properties.Resources.minimize;
            this.minimizeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.minimizeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizeBtn.FlatAppearance.BorderSize = 0;
            this.minimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeBtn.ForeColor = System.Drawing.Color.Transparent;
            this.minimizeBtn.Location = new System.Drawing.Point(929, 11);
            this.minimizeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.minimizeBtn.Name = "minimizeBtn";
            this.minimizeBtn.Size = new System.Drawing.Size(45, 40);
            this.minimizeBtn.TabIndex = 5;
            this.minimizeBtn.UseVisualStyleBackColor = false;
            this.minimizeBtn.Click += new System.EventHandler(this.minimizeBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.Color.Transparent;
            this.exitBtn.BackgroundImage = global::Computer_Shop_System.Properties.Resources.exit;
            this.exitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitBtn.FlatAppearance.BorderSize = 0;
            this.exitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitBtn.ForeColor = System.Drawing.Color.Transparent;
            this.exitBtn.Location = new System.Drawing.Point(974, 11);
            this.exitBtn.Margin = new System.Windows.Forms.Padding(0);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(45, 40);
            this.exitBtn.TabIndex = 4;
            this.exitBtn.UseVisualStyleBackColor = false;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(240)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1027, 613);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loginForm_Dashboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.loginForm_Dashboard.ResumeLayout(false);
            this.loginForm_Dashboard.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.loginPanel.ResumeLayout(false);
            this.loginPanel.PerformLayout();
            this.registerPanel.ResumeLayout(false);
            this.registerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel loginForm_Dashboard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Button minimizeBtn;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button registerSwitchBtn;
        private System.Windows.Forms.Button loginSwitchBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox login_UsernameText;
        private System.Windows.Forms.Button login_LoginBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox login_PasswordText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel registerPanel;
        private System.Windows.Forms.Button register_RegisterBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox register_PasswordText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox register_UsernameText;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
    }
}

