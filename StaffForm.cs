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
            manageOrdersPanel.Visible = false;

            panel.Visible = true;
        }

        public void ShowButtonPanel(Button button) {
            dashboardBtn.BackColor = Color.FromArgb(137, 214, 251);
            viewStocksBtn.BackColor = Color.FromArgb(137, 214, 251);
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
            ClearStocksField();
        }

        private void manageOrdersBtn_Click(object sender, EventArgs e)
        {
            ShowButtonPanel(manageOrdersBtn);
            ShowOnlyPanel(manageOrdersPanel);
            DisplayOrders();
            manageOrders_DataGrid.ClearSelection();
            manageOrders_StatusBox.SelectedIndex = -1;
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
                    MessageBox.Show("An Error Has Occured." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void DisplayOrders() {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand displayCommand = new MySqlCommand("SELECT `Order ID`, `User ID`, `Email`, `Total Amount`, `Date Ordered`, `Status` FROM orders", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(displayCommand);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                try
                {
                    manageOrders_DataGrid.Rows.Clear();
                    if (manageOrders_DataGrid.Columns.Count == 0)
                    {
                        manageOrders_DataGrid.Columns.Add("OrderID", "Order ID");
                        manageOrders_DataGrid.Columns["OrderID"].Visible = false;
                        //manageOrders_DataGrid.Columns["OrderID"].FillWeight = 60;

                        manageOrders_DataGrid.Columns.Add("UserID", "User ID");
                        manageOrders_DataGrid.Columns["UserID"].Visible = false;
                        //manageOrders_DataGrid.Columns["UserID"].FillWeight = 40;

                        manageOrders_DataGrid.Columns.Add("Email", "Email");
                        manageOrders_DataGrid.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                        manageOrders_DataGrid.Columns.Add("TotalAmount", "Total Amount");
                        manageOrders_DataGrid.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        manageOrders_DataGrid.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                        manageOrders_DataGrid.Columns["TotalAmount"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");

                        manageOrders_DataGrid.Columns.Add("DateOrdered", "Date Ordered");

                        manageOrders_DataGrid.Columns.Add("Status", "Status");
                        manageOrders_DataGrid.Columns["Status"].FillWeight = 50;
                        manageOrders_DataGrid.Columns["Status"].DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        int orderId = Convert.ToInt32(row["Order ID"]);
                        int userId = Convert.ToInt32(row["User ID"]);
                        string email = row["Email"].ToString();
                        decimal totalAmount = Convert.ToDecimal(row["Total Amount"]);

                        DateTime dateOrdered = Convert.ToDateTime(row["Date Ordered"]);
                        string dateOnly = dateOrdered.ToString("MMMM dd, yyyy");

                        string status = row["Status"].ToString();
                        manageOrders_DataGrid.Rows.Add(orderId, userId, email, totalAmount, dateOnly, status);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load orders: " + ex.Message);
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
                stocks_TypeDisplay.Text = stocks_DataGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                stocks_StocksDisplay.Text = stocks_DataGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
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

        private void stocks_SelectImageBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Title = "Select Profile Photo";
            opf.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var imgTemp = new Bitmap(opf.FileName))
                    {
                        stocks_PictureBox.BackgroundImage = new Bitmap(imgTemp);
                    }

                    stocks_PictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void stocks_StocksIncreaseBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(stocks_StocksDisplay.Text, out int stocks))
            {
                stocks++;
                stocks_StocksDisplay.Text = stocks.ToString();
            }
        }

        private void stocks_StocksDecreaseBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stocks_NameDisplay.Text)) { 
            
            }
            if (int.TryParse(stocks_StocksDisplay.Text, out int stocks))
            {
                stocks--;
                if (stocks < 0)
                {
                    MessageBox.Show("Stocks cannot be negative.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                stocks_StocksDisplay.Text = stocks.ToString();
            }
        }

        public void ClearStocksField() {
            productId = 0;
            products_UnitPrice = 0m;
            stocks_PictureBox.BackgroundImage = null;
            stocks_SearchText.Clear();
            stocks_NameDisplay.Clear();
            stocks_PriceDisplay.Clear();
            stocks_TypeDisplay.SelectedIndex = -1;
            stocks_StocksDisplay.Text = "0";
            stocks_DataGrid.ClearSelection();
        }

        public bool CheckIfProductExist()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand searchCommand = new MySqlCommand("SELECT COUNT(*) FROM products WHERE `Name` = @name AND `Price` = @price", connection);
                searchCommand.Parameters.AddWithValue("@name", stocks_NameDisplay.Text);
                if (decimal.TryParse(stocks_PriceDisplay.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-PH"), out decimal price))
                {
                    searchCommand.Parameters.AddWithValue("@price", price);
                }
                int count = Convert.ToInt32(searchCommand.ExecuteScalar());
                if (count > 0)
                {
                    return true;
                }
                connection.Close();
                return false;
            }
        }

        private void stocks_UpdateBtn_Click(object sender, EventArgs e)
        {   
            if (!decimal.TryParse(stocks_PriceDisplay.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-PH"), out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price in the correct format (e.g., ₱1,000.00).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (productId == 0)
            {
                MessageBox.Show("Please select a product to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                try
                {
                    string query = "UPDATE products SET `Image` = @image, `Name` = @name, `Type` = @type, `Price` = @price, `Stocks` = @stocks WHERE `Product ID` = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", stocks_NameDisplay.Text);
                    command.Parameters.AddWithValue("@type", stocks_TypeDisplay.Text);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@stocks", int.Parse(stocks_StocksDisplay.Text));
                    command.Parameters.AddWithValue("@id", productId);
                    if (stocks_PictureBox.BackgroundImage != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stocks_PictureBox.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageData = ms.ToArray();
                            command.Parameters.AddWithValue("@image", imageData);
                        }
                    }
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        //MessageBox.Show("Product updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayStocks();
                        ClearStocksField();
                    }
                    else
                    {
                        MessageBox.Show("No product found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void stocks_AddBtn_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(stocks_PriceDisplay.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-PH"), out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price in the correct format (e.g., ₱1,000.00).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(stocks_NameDisplay.Text) || string.IsNullOrEmpty(stocks_TypeDisplay.Text) || int.Parse(stocks_StocksDisplay.Text) < 0)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckIfProductExist())
            {
                MessageBox.Show("Product already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                try
                {
                    string query = "INSERT INTO products (`Image`, `Name`, `Type`, `Price`, `Stocks`) VALUES (@image, @name, @type, @price, @stocks)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", stocks_NameDisplay.Text);
                    command.Parameters.AddWithValue("@type", stocks_TypeDisplay.Text);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@stocks", int.Parse(stocks_StocksDisplay.Text));

                    if (stocks_PictureBox.BackgroundImage != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stocks_PictureBox.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageData = ms.ToArray();
                            command.Parameters.AddWithValue("@image", imageData);
                        }
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@image", DBNull.Value);
                    }
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayStocks();
                        ClearStocksField();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while adding the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void stocks_RemoveBtn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM products WHERE `Product ID` = @id", connection);
                deleteCommand.Parameters.AddWithValue("@id", productId);
                try
                {
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayStocks();
                        ClearStocksField();
                    }
                    else
                    {
                        MessageBox.Show("No product found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while removing the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void stocks_SearchBtn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                string searchText = stocks_SearchText.Text.Trim();
                if (string.IsNullOrEmpty(searchText))
                {
                    MessageBox.Show("Please enter a product name to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    MySqlCommand searchCommand = new MySqlCommand("SELECT `Product ID`, `Image`, `Name`, `Type`, `Price`, `Stocks` FROM products WHERE `Name` LIKE @search", connection);
                    searchCommand.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(searchCommand);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    stocks_DataGrid.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["Product ID"]);
                        string name = row["Name"].ToString();
                        string type = row["Type"].ToString();
                        int price = Convert.ToInt32(row["Price"]);
                        int stocks = Convert.ToInt32(row["Stocks"]);
                        byte[] imageData = (byte[])row["Image"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image productImage = Image.FromStream(ms);
                            Image resized = new Bitmap(productImage, new Size(100, 100));
                            stocks_DataGrid.Rows.Add(id, resized, name, price, type, stocks);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Manage Order Panel

        private void manageOrders_DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            { 
                manageOrders_StatusBox.SelectedItem = manageOrders_DataGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void manageOrders_RefreshBtn_Click(object sender, EventArgs e)
        {
            DisplayOrders();
            manageOrders_DataGrid.ClearSelection();
            manageOrders_SearchText.Clear();
            manageOrders_StatusBox.SelectedIndex = -1;
        }

        private void manageOrders_ChangeBtn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                if (manageOrders_DataGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an order to change its status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int orderId = Convert.ToInt32(manageOrders_DataGrid.SelectedRows[0].Cells[0].Value);
                string newStatus = manageOrders_StatusBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(newStatus))
                {
                    MessageBox.Show("Please select a new status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    MySqlCommand updateCommand = new MySqlCommand("UPDATE orders SET `Status` = @status WHERE `Order ID` = @id", connection);
                    updateCommand.Parameters.AddWithValue("@status", newStatus);
                    updateCommand.Parameters.AddWithValue("@id", orderId);
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Order status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayOrders();
                        manageOrders_StatusBox.SelectedIndex = -1;
                        manageOrders_DataGrid.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("No order found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the order status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void manageOrders_SearchBtn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                string searchText = manageOrders_SearchText.Text.Trim();
                if (string.IsNullOrEmpty(searchText))
                {
                    MessageBox.Show("Please enter an email to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    MySqlCommand searchCommand = new MySqlCommand("SELECT `Order ID`, `User ID`, `Email`, `Total Amount`, `Date Ordered`, `Status` FROM orders WHERE `Email` LIKE @search", connection);
                    searchCommand.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(searchCommand);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    manageOrders_DataGrid.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        int orderId = Convert.ToInt32(row["Order ID"]);
                        int userId = Convert.ToInt32(row["User ID"]);
                        string email = row["Email"].ToString();
                        decimal totalAmount = Convert.ToDecimal(row["Total Amount"]);
                        DateTime dateOrdered = Convert.ToDateTime(row["Date Ordered"]);
                        string dateOnly = dateOrdered.ToString("MMMM dd, yyyy");
                        string status = row["Status"].ToString();
                        manageOrders_DataGrid.Rows.Add(orderId, userId, email, totalAmount, dateOnly, status);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
