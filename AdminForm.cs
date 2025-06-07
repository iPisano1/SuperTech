using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Computer_Shop_System
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            dashboardBtn.PerformClick();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public static extern void SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void adminForm_Dashboard_MouseDown(object sender, MouseEventArgs e)
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

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Session.UserId = 0;
            Session.Username = null;
            Session.Permission = null;
            Session.Password = null;
            Session.Email = null;
            Session.PhoneNumber = null;
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        public void ShowOnlyPanel(Panel panel)
        {
            dashboardPanel.Visible = false;
            manageAccountsPanel.Visible = false;
            otherSettingsPanel.Visible = false;

            panel.Visible = true;
        }

        public void ShowButtonPanel(Button button)
        {
            dashboardBtn.BackColor = Color.FromArgb(137, 214, 251);
            manageAccountsBtn.BackColor = Color.FromArgb(137, 214, 251);
            otherSettingsBtn.BackColor = Color.FromArgb(137, 214, 251);

            button.BackColor = Color.Silver;
        }

        // Side Buttons

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(dashboardPanel);
            ShowButtonPanel(dashboardBtn);
            UpdateDashboardCounter();
        }

        private void manageAccountsBtn_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(manageAccountsPanel);
            ShowButtonPanel(manageAccountsBtn);
            DisplayAccounts();
            ClearAccountFields();
            RefreshAccountGrid();
        }

        private void otherSettingsBtn_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(otherSettingsPanel);
            ShowButtonPanel(otherSettingsBtn);
        }

        // Updater

        public void UpdateDashboardCounter()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand customerCounter = new MySqlCommand("SELECT COUNT(*) FROM accounts", connection);
                    totalAccountsCounter.Text = customerCounter.ExecuteScalar().ToString();

                    MySqlCommand productsCounter = new MySqlCommand("SELECT COUNT(*) FROM products", connection);
                    availableProductsCounter.Text = productsCounter.ExecuteScalar().ToString();

                    MySqlCommand salesCounter = new MySqlCommand("SELECT `Total Amount` FROM orders WHERE `Status` = 'Approved'", connection);
                    MySqlDataAdapter salesAdapter = new MySqlDataAdapter(salesCounter);
                    DataTable salesTable = new DataTable();
                    salesAdapter.Fill(salesTable);
                    decimal grandTotal = 0;
                    foreach (DataRow row in salesTable.Rows)
                    {
                        if (decimal.TryParse(row["Total Amount"].ToString(), out decimal amount))
                        {
                            grandTotal += amount;
                        }
                    }
                    currentSalesCounter.Text = grandTotal.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));

                    MySqlCommand ordersCounter = new MySqlCommand("SELECT COUNT(*) FROM orders", connection);
                    OrdersCounter.Text = ordersCounter.ExecuteScalar().ToString();

                    MySqlCommand productSoldCounter = new MySqlCommand("SELECT SUM(`Quantity`) FROM receipts", connection);
                    object result = productSoldCounter.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        int total = Convert.ToInt32(result);
                        totalProductSold.Text = total.ToString();
                    }
                    else
                    {
                        totalProductSold.Text = "0";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // End of Updater

        // Display Grid

        public void DisplayAccounts()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand displayCommand = new MySqlCommand("SELECT `User ID`, `Username`, `Password`, `First Name`, `Last Name` , `Permission` FROM accounts", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(displayCommand);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                try
                {
                    manageAccounts_DataGrid.Rows.Clear(); // clear old data

                    if (manageAccounts_DataGrid.Columns.Count == 0)
                    {
                        manageAccounts_DataGrid.Columns.Add("UserID", "User ID");
                        manageAccounts_DataGrid.Columns["UserID"].Visible = false;

                        manageAccounts_DataGrid.Columns.Add("Username", "Username");
                        manageAccounts_DataGrid.Columns.Add("Password", "Password");
                        manageAccounts_DataGrid.Columns.Add("FullName", "Full Name");

                        manageAccounts_DataGrid.Columns.Add("Permission", "Permission");
                        manageAccounts_DataGrid.Columns["Permission"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        int userID = Convert.ToInt32(row["User ID"]);
                        string username = row["Username"].ToString();
                        string password = row["Password"].ToString();
                        string fullName = row["First Name"].ToString() + " " + row["Last Name"].ToString();
                        string permission = row["Permission"].ToString();
                        manageAccounts_DataGrid.Rows.Add(userID, username, password, fullName, permission);
                    }

                    manageAccounts_DataGrid.Sort(manageAccounts_DataGrid.Columns["Permission"], ListSortDirection.Ascending);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void manageAccounts_DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = manageAccounts_DataGrid.Rows[e.RowIndex];
                manageAccounts_UsernameText.Text = row.Cells["Username"].Value.ToString();
                manageAccounts_PasswordText.Text = row.Cells["Password"].Value.ToString();
                manageAccounts_PermissionBox.SelectedItem = row.Cells["Permission"].Value.ToString();
            }
        }

        // End of Display Grid

        // Manage Accounts Panel

        public void ClearAccountFields()
        {
            manageAccounts_UsernameText.Clear();
            manageAccounts_PasswordText.Clear();
            manageAccounts_PermissionBox.SelectedIndex = -1;
        }

        public void RefreshAccountGrid()
        {
            DisplayAccounts();
            manageAccounts_DataGrid.ClearSelection();
        }

        private void manageAccounts_RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshAccountGrid();
            ClearAccountFields();
        }

        public bool CheckIfAccountExist()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand searchCommand = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE `Username` = @username", connection);
                searchCommand.Parameters.AddWithValue("@username", manageAccounts_UsernameText.Text);

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
                finally
                {
                    connection.Close();
                }
            }
        }

        private void manageAccounts_AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(manageAccounts_UsernameText.Text) || string.IsNullOrWhiteSpace(manageAccounts_PasswordText.Text) || manageAccounts_PermissionBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(CheckIfAccountExist())
            {
                MessageBox.Show("Username already exists. Please choose a different username.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearAccountFields();
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand insertCommand = new MySqlCommand("INSERT INTO accounts (Username, Password, Permission) VALUES (@Username, @Password, @Permission)", connection);
                insertCommand.Parameters.AddWithValue("@Username", manageAccounts_UsernameText.Text);
                insertCommand.Parameters.AddWithValue("@Password", manageAccounts_PasswordText.Text);
                insertCommand.Parameters.AddWithValue("@Permission", manageAccounts_PermissionBox.SelectedItem.ToString());
                try
                {
                    insertCommand.ExecuteNonQuery();
                    MessageBox.Show("Account added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayAccounts();
                    RefreshAccountGrid();
                    ClearAccountFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void manageAccounts_UpdateBtn_Click(object sender, EventArgs e)
        {
            if (manageAccounts_DataGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an account to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(manageAccounts_UsernameText.Text) || string.IsNullOrWhiteSpace(manageAccounts_PasswordText.Text) || manageAccounts_PermissionBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand updateCommand = new MySqlCommand("UPDATE accounts SET Username = @Username, Password = @Password, Permission = @Permission WHERE `User ID` = @UserID", connection);
                updateCommand.Parameters.AddWithValue("@Username", manageAccounts_UsernameText.Text);
                updateCommand.Parameters.AddWithValue("@Password", manageAccounts_PasswordText.Text);
                updateCommand.Parameters.AddWithValue("@Permission", manageAccounts_PermissionBox.SelectedItem.ToString());
                updateCommand.Parameters.AddWithValue("@UserID", manageAccounts_DataGrid.SelectedRows[0].Cells["UserID"].Value);
                try
                {
                    updateCommand.ExecuteNonQuery();
                    MessageBox.Show("Account updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayAccounts();
                    RefreshAccountGrid();
                    ClearAccountFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void manageAccounts_DeleteBtn_Click(object sender, EventArgs e)
        {
            if (manageAccounts_DataGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an account to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
                {
                    int userId = Convert.ToInt32(manageAccounts_DataGrid.SelectedRows[0].Cells["UserID"].Value);

                    if (userId == Session.UserId)
                    {
                        DialogResult resultLast = MessageBox.Show("Are you sure you want to delete your account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resultLast != DialogResult.Yes)
                        {
                            return;
                        }
                    }

                    // Delete Account
                    MySqlCommand deleteAccountCommand = new MySqlCommand("DELETE FROM accounts WHERE `User ID` = @UserID", connection);
                    deleteAccountCommand.Parameters.AddWithValue("@UserID", userId);

                    // Delete Related Data
                    MySqlCommand deleteShoppingCartCommand = new MySqlCommand("DELETE FROM shopping_cart WHERE `User ID` = @UserID", connection);
                    deleteShoppingCartCommand.Parameters.AddWithValue("@UserID", userId);

                    MySqlCommand deleteOrdersCommand = new MySqlCommand("DELETE FROM orders WHERE `User ID` = @UserID", connection);
                    deleteOrdersCommand.Parameters.AddWithValue("@UserID", userId);

                    MySqlCommand deleteReceiptsCommand = new MySqlCommand("DELETE FROM receipts WHERE `User ID` = @UserID", connection);
                    deleteReceiptsCommand.Parameters.AddWithValue("@UserID", userId);

                    try
                    {
                        connection.Open();
                        deleteShoppingCartCommand.ExecuteNonQuery();
                        deleteOrdersCommand.ExecuteNonQuery();
                        deleteReceiptsCommand.ExecuteNonQuery();
                        deleteAccountCommand.ExecuteNonQuery();
                        MessageBox.Show("Account deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayAccounts();
                        RefreshAccountGrid();
                        ClearAccountFields();
                        if (userId == Session.UserId) 
                        {
                            logoutBtn.PerformClick();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        // End of Manage Accounts Panel

        // Other Settings Panel

        private void otherSettings_ClearShoppingCartBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the shopping cart?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand clearCartCommand = new MySqlCommand("DELETE FROM shopping_cart", connection);
                try
                {
                    clearCartCommand.ExecuteNonQuery();
                    MessageBox.Show("Shopping cart cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_RemoveAllProductsBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove all products?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand removeProductsCommand = new MySqlCommand("DELETE FROM products", connection);
                try
                {
                    removeProductsCommand.ExecuteNonQuery();
                    MessageBox.Show("All products removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_RemoveNoStocksBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove products with no stocks?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand removeNoStocksCommand = new MySqlCommand("DELETE FROM products WHERE `Stocks` <= 0", connection);
                try
                {
                    removeNoStocksCommand.ExecuteNonQuery();
                    MessageBox.Show("Products with no stocks removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_RemoveOrdersBtn_Click(object sender, EventArgs e)
        {
            DialogResult removeOrderResult = MessageBox.Show("Are you sure you want to remove all orders?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (removeOrderResult != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand removeOrdersCommand = new MySqlCommand("DELETE FROM orders", connection);
                try
                {
                    removeOrdersCommand.ExecuteNonQuery();
                    MessageBox.Show("All orders removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            DialogResult removeReceiptResult = MessageBox.Show("Do you also want to delete receipts from all the deleted orders?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (removeReceiptResult != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand removeReceiptsCommand = new MySqlCommand("DELETE FROM receipts", connection);
                try
                {
                    removeReceiptsCommand.ExecuteNonQuery();
                    MessageBox.Show("All receipts removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_ClearPendingBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all pending orders?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand clearPendingCommand = new MySqlCommand("DELETE FROM orders WHERE `Status` = 'Pending'", connection);
                try
                {
                    clearPendingCommand.ExecuteNonQuery();
                    MessageBox.Show("All pending orders cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_ClearRejectedBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all rejected orders?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand clearRejectedCommand = new MySqlCommand("DELETE FROM orders WHERE `Status` = 'Rejected'", connection);
                try
                {
                    clearRejectedCommand.ExecuteNonQuery();
                    MessageBox.Show("All rejected orders cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_ClearApprovedBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all approved orders?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand clearApprovedCommand = new MySqlCommand("DELETE FROM orders WHERE `Status` = 'Approved'", connection);
                try
                {
                    clearApprovedCommand.ExecuteNonQuery();
                    MessageBox.Show("All approved orders cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_ClearReceiptsBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all receipts?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand clearReceiptsCommand = new MySqlCommand("DELETE FROM receipts", connection);
                try
                {
                    clearReceiptsCommand.ExecuteNonQuery();
                    MessageBox.Show("All receipts cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void otherSettings_RemoveAccountsBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove all non-admin accounts?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand removeAccountsCommand = new MySqlCommand("DELETE FROM accounts WHERE `Permission` != 'Admin'", connection);
                MySqlCommand removeShoppingCartCommand = new MySqlCommand("DELETE FROM shopping_cart", connection);
                MySqlCommand removeOrdersCommand = new MySqlCommand("DELETE FROM orders", connection);
                MySqlCommand removeReceiptsCommand = new MySqlCommand("DELETE FROM receipts", connection);
                try
                {
                    removeShoppingCartCommand.ExecuteNonQuery();
                    removeOrdersCommand.ExecuteNonQuery();
                    removeReceiptsCommand.ExecuteNonQuery();
                    removeAccountsCommand.ExecuteNonQuery();

                    MessageBox.Show("All non-admin accounts and related data have been removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void otherSettings_clearDatabaseBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the entire database? This action cannot be undone.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand clearDatabaseCommand = new MySqlCommand(
                    "DELETE FROM accounts; ALTER TABLE accounts AUTO_INCREMENT = 1; " +
                    "DELETE FROM products; ALTER TABLE products AUTO_INCREMENT = 1;" +
                    "DELETE FROM shopping_cart; ALTER TABLE shopping_cart AUTO_INCREMENT = 1;" +
                    "DELETE FROM orders; ALTER TABLE orders AUTO_INCREMENT = 1;" +
                    "DELETE FROM receipts; ALTER TABLE receipts AUTO_INCREMENT = 1;"
                    , connection);
                try
                {
                    clearDatabaseCommand.ExecuteNonQuery();
                    MessageBox.Show("Database cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // End of Other Settings Panel
    }
}
