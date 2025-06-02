using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Computer_Shop_System
{
    public partial class LoginForm: Form
    {   
        
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system");

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            ShowOnlyPanel(loginPanel, registerSwitchBtn, true);
            login_UsernameText.Focus();
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            login_UsernameText.Focus();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public static extern void SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void loginForm_Dashboard_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void EnterKey_Event(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                if (logSession == true)
                {
                    login_LoginBtn.PerformClick();
                }
                else{ 
                    register_RegisterBtn.PerformClick();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        bool logSession = true;
        public void ShowOnlyPanel(Panel panel, Button button, bool status) 
        {
            logSession = status;
            loginPanel.Visible = false;
            loginPanel.Enabled = false;
            registerPanel.Visible = false;
            registerPanel.Enabled = false;

            registerSwitchBtn.Enabled = false;
            loginSwitchBtn.Enabled = false;

            panel.Visible = true;
            panel.Enabled = true;
            button.Enabled = true;
        }

        private void registerSwitchBtn_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(registerPanel, loginSwitchBtn, false);
            register_UsernameText.Focus();
            login_UsernameText.Clear();
            login_PasswordText.Clear();
        }

        private void loginSwitchBtn_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(loginPanel, registerSwitchBtn, true);
            login_UsernameText.Focus();
            register_UsernameText.Clear();
            register_PasswordText.Clear();
        }

        public bool CheckIfAccountExist() { 
            MySqlCommand searchCommand = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE `Username` = @username AND Password = @password", connection);
            if (logSession == true)
            {
                searchCommand.Parameters.AddWithValue("@username", login_UsernameText.Text);
                searchCommand.Parameters.AddWithValue("@password", login_PasswordText.Text);
            }
            else {
                searchCommand.Parameters.AddWithValue("@username", register_UsernameText.Text);
                searchCommand.Parameters.AddWithValue("@password", register_PasswordText.Text);
            }
            try
            {
                connection.Open();
                int count = Convert.ToInt32(searchCommand.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally {
                connection.Close();
            }
        }

        public string CheckUserPermission(){
            try
            {
                connection.Open();
                // Check if Admin
                MySqlCommand searchAdminCommand = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE `Username` = @username AND `Permission` = 'Admin'", connection);
                searchAdminCommand.Parameters.AddWithValue("@username", login_UsernameText.Text);
                try
                {
                    int adminCount = Convert.ToInt32(searchAdminCommand.ExecuteScalar());
                    if (adminCount > 0)
                    {
                        return "admin";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // Check if Staff
                MySqlCommand searchStaffCommand = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE `Username` = @username AND `Permission` = 'Staff'", connection);
                searchStaffCommand.Parameters.AddWithValue("@username", login_UsernameText.Text);
                try
                {
                    int staffCount = Convert.ToInt32(searchStaffCommand.ExecuteScalar());
                    if (staffCount > 0)
                    {
                        return "staff";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
            return "customer";
        }

        public void SaveUserData() {
            MySqlCommand searchCommand = new MySqlCommand("SELECT * FROM accounts WHERE Username = @username AND Password = @password", connection);
            searchCommand.Parameters.AddWithValue("@username", login_UsernameText.Text);
            searchCommand.Parameters.AddWithValue("@password", login_PasswordText.Text);
            try
            {
                connection.Open();
                MySqlDataReader reader = searchCommand.ExecuteReader();
                if (reader.Read())
                {
                    Session.UserId = reader.GetInt32("User ID");
                    Session.Username = reader.GetString("Username");
                    Session.Password = reader.GetString("Password");
                    if (!string.IsNullOrWhiteSpace(reader["Phone Number"]?.ToString()))
                    {
                        Session.PhoneNumber = reader["Phone Number"].ToString();
                    }
                    Session.Role = reader.GetString("Permission");
                    if (!string.IsNullOrWhiteSpace(reader["Email"]?.ToString()))
                    {
                        Session.Email = reader["Email"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void login_LoginBtn_Click(object sender, EventArgs e)
        {
            if (CheckIfAccountExist())
            {
                SaveUserData();
                string role = CheckUserPermission();
                MessageBox.Show("Successfully Logged In.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (role == "admin")
                {
                    MessageBox.Show("admin ka");
                }
                else if (role == "staff")
                {
                    StaffForm staffForm = new StaffForm();
                    staffForm.Show();
                    this.Hide();
                }
                else {
                    CustomerForm customerForm = new CustomerForm();
                    customerForm.Show();
                    this.Hide();
                }
                login_UsernameText.Clear();
                login_PasswordText.Clear();
                login_UsernameText.Focus();
            }
            else {
                MessageBox.Show("Invalid Credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                login_UsernameText.Clear();
                login_PasswordText.Clear();
                login_UsernameText.Focus();
            }
        }

        private void register_RegisterBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(register_UsernameText.Text) || string.IsNullOrWhiteSpace(register_PasswordText.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (register_UsernameText.Text.Length < 3 || register_PasswordText.Text.Length < 5)
            {
                MessageBox.Show("Username must be at least 3 characters and password must be at least 5 characters long.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckIfAccountExist())
            {
                MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                register_UsernameText.Clear();
                register_PasswordText.Clear();
                return;
            }
            MySqlCommand insertCommand = new MySqlCommand("INSERT INTO accounts(`Username`, `Password`) VALUES(@username, @password)", connection);
            insertCommand.Parameters.AddWithValue("@username", register_UsernameText.Text);
            insertCommand.Parameters.AddWithValue("@password", register_PasswordText.Text);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                register_UsernameText.Clear();
                register_PasswordText.Clear();
                ShowOnlyPanel(loginPanel, registerSwitchBtn, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
