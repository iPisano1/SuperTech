using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Computer_Shop_System
{
    public partial class StaffForm: Form
    {
        public StaffForm()
        {
            InitializeComponent();
        }

        private void StaffForm_Load(object sender, EventArgs e)
        {
            dashboardBtn.PerformClick();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public static extern void SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void staffForm_Dashboard_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Session.UserId = 0;
            Session.Username = null;
            Session.Role = null;
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void ShowOnlyPanel(Panel panel) { 
            dashboardPanel.Visible = false;
            viewStocksPanel.Visible = false;

            panel.Visible = true;
        }

        public void ShowButtonPanel(Button button) {
            dashboardBtn.BackColor = Color.FromArgb(137, 214, 251);
            viewStocksBtn.BackColor = Color.FromArgb(137, 214, 251);
            addProductBtn.BackColor = Color.FromArgb(137, 214, 251);
            manageOrdersBtn.BackColor = Color.FromArgb(137, 214, 251);

            button.BackColor = Color.Silver;
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            ShowButtonPanel(dashboardBtn);
            ShowOnlyPanel(dashboardPanel);
            UpdateDashboardCounter();
        }

        private void viewStocksBtn_Click(object sender, EventArgs e)
        {
            ShowButtonPanel(viewStocksBtn);
            ShowOnlyPanel(viewStocksPanel);
            stocks_SortBox.SelectedIndex = 0;
            stocks_DataGrid.ClearSelection();
            DisplayStocks();
        }

        private void addProductBtn_Click(object sender, EventArgs e)
        {
            ShowButtonPanel(addProductBtn);
        }

        private void manageOrdersBtn_Click(object sender, EventArgs e)
        {
            ShowButtonPanel(manageOrdersBtn);
        }

        public void UpdateDashboardCounter()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand customerCounter = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE `Permission` = 'Customer'", connection);
                    totalCustomerCounter.Text = customerCounter.ExecuteScalar().ToString();

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

                    MySqlCommand pendingCounter = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE `Status` = 'Pending'", connection);
                    pendingOrdersCounter.Text = pendingCounter.ExecuteScalar().ToString();

                    MySqlCommand approvedCounter = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE `Status` = 'Approved'", connection);
                    approvedOrdersCounter.Text = approvedCounter.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Has Occured.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Display Grids
        public void DisplayStocks()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand displayCommand = new MySqlCommand("SELECT `Product ID`, `Image`, `Name`, `Type`, `Price`, `Stocks` FROM products", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(displayCommand);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                try
                {
                    stocks_DataGrid.Rows.Clear();
                    if (stocks_DataGrid.Columns.Count == 0)
                    {
                        stocks_DataGrid.Columns.Add("ProductID", "Product ID");

                        DataGridViewImageColumn imageCol = new DataGridViewImageColumn
                        {
                            HeaderText = "Product",
                            Name = "Image",
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        stocks_DataGrid.Columns.Add(imageCol);

                        stocks_DataGrid.Columns.Add("ProductName", "Name");

                        stocks_DataGrid.Columns.Add("ProductPrice", "Price");
                        stocks_DataGrid.Columns["ProductPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        stocks_DataGrid.Columns["ProductPrice"].DefaultCellStyle.Format = "C2";
                        stocks_DataGrid.Columns["ProductPrice"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");

                        stocks_DataGrid.Columns.Add("ProductType", "Type");
                        stocks_DataGrid.Columns["ProductID"].Visible = false;
                        stocks_DataGrid.Columns["ProductType"].Visible = false;

                        stocks_DataGrid.Columns.Add("Stocks", "Stocks");
                        stocks_DataGrid.Columns["Stocks"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        stocks_DataGrid.Columns["Stocks"].FillWeight = 50;
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        byte[] imageData = (byte[])row["Image"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image productImage = Image.FromStream(ms);
                            Image resized = new Bitmap(productImage, new Size(100, 100));

                            int productID = Convert.ToInt32(row["Product ID"]);
                            string name = row["Name"].ToString();
                            string type = row["Type"].ToString();
                            int price = Convert.ToInt32(row["Price"]);
                            int stocks = Convert.ToInt32(row["Stocks"]);

                            stocks_DataGrid.Rows.Add(productID, resized, name, price, type, stocks);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load products: " + ex.Message);
                }
            }
        }

        // View Stocks Panel
        int productId;
        decimal products_UnitPrice;

        private void stocks_DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                object imageCellValue = stocks_DataGrid.Rows[e.RowIndex].Cells[1].Value;
                if (imageCellValue != null && imageCellValue is Image img)
                {
                    stocks_PictureBox.BackgroundImage = img;
                    stocks_PictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                }

                productId = Convert.ToInt32(stocks_DataGrid.Rows[e.RowIndex].Cells[0].Value);
                stocks_NameDisplay.Text = stocks_DataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (decimal.TryParse(stocks_DataGrid.Rows[e.RowIndex].Cells[3].Value.ToString(), out decimal price))
                {
                    products_UnitPrice = price;
                    stocks_PriceDisplay.Text = products_UnitPrice.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                }
                else
                {
                    stocks_PriceDisplay.Text = stocks_DataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                }
            }
        }

        private void stocks_SortBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = stocks_SortBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedType)) return;

            if (stocks_SortBox.SelectedIndex == 0)
            {
                DisplayStocks();
                stocks_DataGrid.ClearSelection();
                return;
            }

            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand sortCommand = new MySqlCommand("SELECT `Product ID`, `Image`, `Name`, `Type`, `Price` FROM products WHERE `Type` = @type", connection);
                sortCommand.Parameters.AddWithValue("@type", selectedType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sortCommand);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                try
                {
                    stocks_DataGrid.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["Product ID"]);
                        string name = row["Name"].ToString();
                        string type = row["Type"].ToString();
                        int price = Convert.ToInt32(row["Price"]);
                        byte[] imageData = (byte[])row["Image"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image productImage = Image.FromStream(ms);
                            Image resized = new Bitmap(productImage, new Size(100, 100));
                            stocks_DataGrid.Rows.Add(id, resized, name, price, type);
                        }
                    }

                    stocks_DataGrid.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sort failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
