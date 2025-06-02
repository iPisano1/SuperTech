using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Computer_Shop_System
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            productsBtn.PerformClick();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public static extern void SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void customerForm_Dashboard_MouseDown(object sender, MouseEventArgs e)
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
            Session.Role = null;
            Session.Password = null;
            Session.Email = null;
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        public void ShowOnlyPanel(Panel panel) {
            productsPanel.Visible = false;
            cartPanel.Visible = false;
            checkoutPanel.Visible = false;
            orderHistoryPanel.Visible = false;
            profilePanel.Visible = false;

            panel.Visible = true;
        }

        public void ShowButtonPanel(Button button) {
            productsBtn.BackColor = Color.FromArgb(137, 214, 251);
            cartBtn.BackColor = Color.FromArgb(137, 214, 251);
            orderHistoryBtn.BackColor = Color.FromArgb(137, 214, 251);
            profileBtn.BackColor = Color.FromArgb(137, 214, 251);

            button.BackColor = Color.Silver;
        }

        private void productsBtn_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(productsPanel);
            ShowButtonPanel(productsBtn);
            products_SortBox.SelectedIndex = 0;
            DisplayProducts();
            ClearProductSelection();
            UpdateCartCounter();
        }

        private void cartBtn_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(cartPanel);
            ShowButtonPanel(cartBtn);
            DisplayCart();
            ClearCartSelection();
            UpdateCartCounter();
        }

        private void orderHistoryBtn_Click(object sender, EventArgs e)
        {
            ShowButtonPanel(orderHistoryBtn);
            ShowOnlyPanel(orderHistoryPanel);
            DisplayOrderHistory();
            UpdateCartCounter();
            receiptPanel.Visible = false;
            orderHistory_DataGrid.Visible = true;
            orderHistory_ViewReceiptBtn.Visible = false;
            orderHistory_ViewReceiptBtn.Text = "View Receipt";
        }

        private void profileBtn_Click(object sender, EventArgs e)
        {
            ShowButtonPanel(profileBtn);
            ShowOnlyPanel(profilePanel);
            DisplayProfile();
        }

        public void UpdateCartCounter()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand countCommand = new MySqlCommand("SELECT COUNT(*) FROM shopping_cart WHERE `User ID` = @userId", connection);
                countCommand.Parameters.AddWithValue("@userId", Session.UserId);
                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(countCommand.ExecuteScalar());
                    cartCounterLabel.Text = count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update cart counter: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally {
                    connection.Close();
                }
            }
        }



        public void AddToCart(int userId, int productId, int quantity, decimal totalPrice)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand insertCommand = new MySqlCommand("INSERT INTO shopping_cart(`User ID`, `Product ID`, `Quantity`, `Total Price`) VALUES(@userId, @productId, @quantity, @totalPrice)", connection);
                insertCommand.Parameters.AddWithValue("@userId", userId);
                insertCommand.Parameters.AddWithValue("@productId", productId);
                insertCommand.Parameters.AddWithValue("@quantity", quantity);
                insertCommand.Parameters.AddWithValue("@totalPrice", totalPrice);

                try
                {
                    connection.Open();
                    MySqlCommand searchCommand = new MySqlCommand("SELECT COUNT(*) FROM shopping_cart WHERE `User ID` = @userID AND `Product ID` = @productID", connection);
                    searchCommand.Parameters.AddWithValue("@userID", userId);
                    searchCommand.Parameters.AddWithValue("@productID", productId);
                    int searchCount = Convert.ToInt32(searchCommand.ExecuteScalar());
                    if (searchCount > 0)
                    {
                        MySqlCommand mySqlCommand = new MySqlCommand("UPDATE shopping_cart SET `Quantity` = `Quantity` + @quantity, `Total Price` = `Total Price` + @totalPrice WHERE `User ID` = @userID AND `Product ID` = @productID", connection);
                        mySqlCommand.Parameters.AddWithValue("@userID", userId);
                        mySqlCommand.Parameters.AddWithValue("@productID", productId);
                        mySqlCommand.Parameters.AddWithValue("@quantity", quantity);
                        mySqlCommand.Parameters.AddWithValue("@totalPrice", totalPrice);
                        mySqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Item Updated to Cart!");
                        UpdateCartCounter();
                        return;
                    }
                    insertCommand.ExecuteNonQuery();
                    MessageBox.Show("Item added to cart!");
                    UpdateCartCounter();

                    // Update Stocks
                    //UpdateStocks();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to add to cart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally {
                    connection.Close();
                }
            }
        }

        // Display Grids

        public void DisplayProducts()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand displayCommand = new MySqlCommand("SELECT `Product ID`, `Image`, `Name`, `Type`, `Price` FROM products WHERE `Stocks` >= 1", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(displayCommand);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                try
                {
                    products_DataGrid.Rows.Clear();
                    if (products_DataGrid.Columns.Count == 0)
                    {
                        products_DataGrid.Columns.Add("ProductID", "Product ID");

                        DataGridViewImageColumn imageCol = new DataGridViewImageColumn
                        {
                            HeaderText = "Product",
                            Name = "Image",
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        products_DataGrid.Columns.Add(imageCol);

                        products_DataGrid.Columns.Add("ProductName", "Name");

                        products_DataGrid.Columns.Add("ProductPrice", "Price");
                        products_DataGrid.Columns["ProductPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        products_DataGrid.Columns["ProductPrice"].DefaultCellStyle.Format = "C2";
                        products_DataGrid.Columns["ProductPrice"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");

                        products_DataGrid.Columns.Add("ProductType", "Type");
                        products_DataGrid.Columns["ProductID"].Visible = false;
                        products_DataGrid.Columns["ProductType"].Visible = false;
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        byte[] imageData = (byte[])row["Image"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image productImage = Image.FromStream(ms);
                            Image resized = new Bitmap(productImage, new Size(100, 100));

                            int id = Convert.ToInt32(row["Product ID"]);
                            string name = row["Name"].ToString();
                            string type = row["Type"].ToString();
                            int price = Convert.ToInt32(row["Price"]);

                            products_DataGrid.Rows.Add(id, resized, name, price, type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void DisplayCart()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand displayCommand = new MySqlCommand("SELECT s.`Cart ID`, p.`Product ID`, p.`Image`, p.`Name`, s.`Quantity`, s.`Total Price` FROM shopping_cart s JOIN products p ON p.`Product ID` = s.`Product ID` WHERE s.`User ID` = @userId", connection);
                displayCommand.Parameters.AddWithValue("@userId", Session.UserId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(displayCommand);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                try
                {
                    cart_DataGrid.Rows.Clear();

                    if (cart_DataGrid.Columns.Count == 0)
                    {
                        cart_DataGrid.Columns.Add("ProductID", "Product ID");
                        cart_DataGrid.Columns["ProductID"].Visible = false;

                        cart_DataGrid.Columns.Add("CartID", "Cart ID");
                        cart_DataGrid.Columns["CartID"].Visible = false;

                        var imageCol = new DataGridViewImageColumn
                        {
                            Name = "Image",
                            HeaderText = "Product",
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        cart_DataGrid.Columns.Add(imageCol);
                        cart_DataGrid.Columns["Image"].FillWeight = 70;
                        cart_DataGrid.Columns["Image"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        cart_DataGrid.Columns.Add("ProductName", "Name");
                        cart_DataGrid.Columns["ProductName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        cart_DataGrid.Columns.Add("ProductQuantity", "Quantity");
                        cart_DataGrid.Columns["ProductQuantity"].FillWeight = 60;
                        cart_DataGrid.Columns["ProductQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        cart_DataGrid.Columns["ProductQuantity"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        cart_DataGrid.Columns.Add("TotalPrice", "Total Price");
                        cart_DataGrid.Columns["TotalPrice"].FillWeight = 60;
                        cart_DataGrid.Columns["TotalPrice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        cart_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
                        cart_DataGrid.Columns["TotalPrice"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        int id = Convert.ToInt32(row["Product ID"]);
                        int cartId = Convert.ToInt32(row["Cart ID"]);
                        string name = row["Name"].ToString();
                        string quantity = "x" + row["Quantity"].ToString();
                        decimal totalPrice = Convert.ToDecimal(row["Total Price"]);

                        byte[] imageData = (byte[])row["Image"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image productImage = Image.FromStream(ms);
                            Image resizedImage = new Bitmap(productImage, new Size(80, 80));

                            cart_DataGrid.Rows.Add(id, cartId, resizedImage, name, quantity, totalPrice);
                        }
                    }
                    cart_DataGrid.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to display cart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void DisplayBillingInformation()
        {
            decimal grandTotal = 0m;
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand displayCommand = new MySqlCommand("SELECT s.`Cart ID`, p.`Product ID`, p.`Name`, s.`Quantity`, s.`Total Price` FROM shopping_cart s JOIN products p ON p.`Product ID` = s.`Product ID` WHERE s.`User ID` = @userID", connection);
                displayCommand.Parameters.AddWithValue("@userID", Session.UserId);

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(displayCommand);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                try
                {
                    checkout_DataGrid.Rows.Clear();
                    if (checkout_DataGrid.Columns.Count == 0) {

                        checkout_DataGrid.Columns.Add("CartID", "Cart ID");
                        checkout_DataGrid.Columns["CartID"].Visible = false;

                        checkout_DataGrid.Columns.Add("ProductID", "Product ID");
                        checkout_DataGrid.Columns["ProductID"].Visible = false;

                        checkout_DataGrid.Columns.Add("Name", "Product Name");
                        //checkout_DataGrid.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        checkout_DataGrid.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        checkout_DataGrid.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        checkout_DataGrid.Columns.Add("Quantity", "Quantity");
                        checkout_DataGrid.Columns["Quantity"].FillWeight = 60;
                        //checkout_DataGrid.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        checkout_DataGrid.Columns.Add("TotalPrice", "Price");
                        checkout_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
                        checkout_DataGrid.Columns["TotalPrice"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");
                        checkout_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        int cartId = Convert.ToInt32(row["Cart ID"]);
                        int productId = Convert.ToInt32(row["Product ID"]);
                        string productName = row["Name"].ToString();
                        string quantity = "x" + Convert.ToString(row["Quantity"]);
                        decimal totalPrice = Convert.ToDecimal(row["Total Price"]);

                        grandTotal += totalPrice;

                        checkout_DataGrid.Rows.Add(cartId, productId, productName, quantity, totalPrice);
                        checkout_DataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    checkout_TotalAmountDisplay.Text = grandTotal.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));

                    checkout_DataGrid.ClearSelection();
                }
                catch (Exception ex) {
                    MessageBox.Show("Failed to display billing information." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void DisplayOrderHistory()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand displayCommand = new MySqlCommand("SELECT `Order ID`, `User ID`, `Total Amount`, `Date Ordered` FROM orders WHERE `User ID` = @userID AND `Status` = 'Approved'", connection);
                displayCommand.Parameters.AddWithValue("@userID", Session.UserId);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(displayCommand);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                try
                {
                    orderHistory_DataGrid.Rows.Clear();
                    if (orderHistory_DataGrid.Columns.Count == 0)
                    {
                        orderHistory_DataGrid.Columns.Add("OrderID", "Order ID");
                        orderHistory_DataGrid.Columns["OrderID"].Visible = false;

                        orderHistory_DataGrid.Columns.Add("UserID", "User ID");
                        orderHistory_DataGrid.Columns["UserID"].Visible = false;

                        orderHistory_DataGrid.Columns.Add("TotalAmount", "Total Amount");
                        orderHistory_DataGrid.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                        orderHistory_DataGrid.Columns["TotalAmount"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");

                        orderHistory_DataGrid.Columns.Add("DateOrdered", "Date Ordered");
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        int orderId = Convert.ToInt32(row["Order ID"]);
                        int userId = Convert.ToInt32(row["User ID"]);
                        decimal totalAmount = Convert.ToDecimal(row["Total Amount"]);
                        DateTime dateOrdered = Convert.ToDateTime(row["Date Ordered"]);
                        orderHistory_DataGrid.Rows.Add(orderId, userId, totalAmount, dateOrdered);
                    }
                    orderHistory_DataGrid.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to display order history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void DisplayReceipt() {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand displayCommand = new MySqlCommand("SELECT r.`Receipt ID`, r.`Order ID`, r.`User ID`, r.`Product ID`, p.`Name`, r.`Quantity`, r.`Total Price`, r.`Payment Method`, r.`Date Ordered` FROM receipts r LEFT JOIN products p ON p.`Product ID` = r.`Product ID` WHERE r.`Order ID` = @orderId AND r.`User ID` = @userID", connection);
                displayCommand.Parameters.AddWithValue("@orderId", SelectedOrderID);
                displayCommand.Parameters.AddWithValue("@userID", Session.UserId);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(displayCommand);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                try
                {
                    receipt_DataGrid.Rows.Clear();
                    if (receipt_DataGrid.Columns.Count == 0)
                    {
                        receipt_DataGrid.Columns.Add("ReceiptID", "Receipt ID");
                        receipt_DataGrid.Columns["ReceiptID"].Visible = false;

                        receipt_DataGrid.Columns.Add("OrderID", "Order ID");
                        receipt_DataGrid.Columns["OrderID"].Visible = false;

                        receipt_DataGrid.Columns.Add("UserID", "User ID");
                        receipt_DataGrid.Columns["UserID"].Visible = false;

                        receipt_DataGrid.Columns.Add("ProductID", "Product ID");
                        receipt_DataGrid.Columns["ProductID"].Visible = false;

                        receipt_DataGrid.Columns.Add("ProductName", "Product Name");
                        receipt_DataGrid.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        receipt_DataGrid.Columns["ProductName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        receipt_DataGrid.Columns["ProductName"].Frozen = true;

                        receipt_DataGrid.Columns.Add("Quantity", "Quantity");
                        receipt_DataGrid.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        receipt_DataGrid.Columns["Quantity"].FillWeight = 60;

                        receipt_DataGrid.Columns.Add("TotalPrice", "Total Price");
                        receipt_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
                        receipt_DataGrid.Columns["TotalPrice"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");
                        receipt_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        receipt_DataGrid.Columns.Add("PaymentMethod", "Payment Method");
                        receipt_DataGrid.Columns["PaymentMethod"].Visible = false;

                        receipt_DataGrid.Columns.Add("DateOrdered", "Date Ordered");
                        receipt_DataGrid.Columns["DateOrdered"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt";
                        receipt_DataGrid.Columns["DateOrdered"].Visible = false;
                    }

                    decimal grandtotal = 0m;
                    int totalItems = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        int receiptId = Convert.ToInt32(row["Receipt ID"]);
                        int orderId = Convert.ToInt32(row["Order ID"]);
                        int userId = Convert.ToInt32(row["User ID"]);
                        int productId = Convert.ToInt32(row["Product ID"]);
                        string productName = row["Name"] == DBNull.Value ? "[Deleted Product]" : row["Name"].ToString();
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        string quantityDisplay = "x" + quantity.ToString();
                        decimal totalPrice = Convert.ToDecimal(row["Total Price"]);
                        string paymentMethod = row["Payment Method"].ToString();
                        DateTime dateOrdered = Convert.ToDateTime(row["Date Ordered"]);

                        totalItems += quantity;
                        grandtotal += totalPrice;

                        receipt_DataGrid.Rows.Add(receiptId, orderId, userId, productId, productName, quantityDisplay, totalPrice, paymentMethod, dateOrdered);
                    }
                    receipt_DataGrid.ClearSelection();

                    receipt_TotalItems.Text = totalItems.ToString();

                    receipt_TotalPrice.Text = grandtotal.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                    AdjustLabelRight(receipt_TotalPrice);

                    decimal vat = grandtotal * 0.12m;
                    receipt_Vat.Text = vat.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                    AdjustLabelRight(receipt_Vat);

                    receipt_TotalAmountDue.Text = (grandtotal + vat).ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                    AdjustLabelRight(receipt_TotalAmountDue);

                    if (dt.Rows.Count > 0)
                    {
                        receipt_PaymentMethod.Text = dt.Rows[0]["Payment Method"].ToString();
                        AdjustLabelRight(receipt_PaymentMethod);

                        DateTime firstDate = Convert.ToDateTime(dt.Rows[0]["Date Ordered"]);
                        receipt_Date.Text = firstDate.ToString("MM/dd/yyyy");
                        receipt_Time.Text = firstDate.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                        AdjustLabelRight(receipt_Time);
                    }
                    else
                    {
                        receipt_PaymentMethod.Text = "";
                        receipt_Date.Text = "";
                        receipt_Time.Text = "";
                    }

                    MySqlCommand searchUserCommand = new MySqlCommand("SELECT * FROM accounts WHERE `User ID` = @userID", connection);
                    searchUserCommand.Parameters.AddWithValue("@userID", Session.UserId);
                    MySqlDataReader reader = searchUserCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        string firstName = reader["First Name"].ToString().Trim();
                        string lastName = reader["Last Name"].ToString().Trim();

                        receipt_Name.Text = $"{firstName} {lastName}";
                        AdjustLabelRight(receipt_Name);

                        receipt_Number.Text = reader["Phone Number"].ToString();
                        AdjustLabelRight(receipt_Number);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to display receipts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AdjustLabelRight(Label label, int rightPadding = 10)
        {
            label.Padding = new Padding(0, 0, rightPadding, 0);
            label.Left = label.Parent.ClientSize.Width - label.PreferredWidth - rightPadding;
        }

        public int CheckStocks() {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand checkCommand = new MySqlCommand("SELECT `Stocks` FROM products WHERE `Product ID` = @productId", connection);
                checkCommand.Parameters.AddWithValue("@productId", productId);
                try
                {
                    int stocks = Convert.ToInt32(checkCommand.ExecuteScalar());
                    return stocks;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to check stocks: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
        }

        // Products Panel
        int productId;
        decimal products_UnitPrice = 0m;

        private void LoadSelectedProduct(int productId)
        {
            this.productId = productId;

            products_QuantityDisplay.Text = "1";
            products_PriceDisplay.Text = (products_UnitPrice).ToString("C2", CultureInfo.GetCultureInfo("en-PH"));

            int stock = CheckStocks();
            products_QuantityIncreaseBtn.Enabled = stock > 1;
            products_QuantityDecreaseBtn.Enabled = false;
        }

        private void products_DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                object imageCellValue = products_DataGrid.Rows[e.RowIndex].Cells[1].Value;
                if (imageCellValue != null && imageCellValue is Image img)
                {
                    products_PictureBox.BackgroundImage = img;
                    products_PictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                }

                productId = Convert.ToInt32(products_DataGrid.Rows[e.RowIndex].Cells[0].Value);
                products_NameDisplay.Text = products_DataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (decimal.TryParse(products_DataGrid.Rows[e.RowIndex].Cells[3].Value.ToString(), out decimal price))
                {
                    products_UnitPrice = price;
                    products_PriceDisplay.Text = products_UnitPrice.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                }
                else
                {
                    products_PriceDisplay.Text = products_DataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                }

                LoadSelectedProduct(productId);
            }
        }

        private void QuantityIncreaseBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(products_NameDisplay.Text))
            {
                MessageBox.Show("Please select a product first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(products_QuantityDisplay.Text, out int quantity))
            {
                int stock = CheckStocks();
                if (stock == -1) return;

                if (quantity < stock)
                {
                    quantity++;
                    products_QuantityDisplay.Text = quantity.ToString();
                    products_PriceDisplay.Text = (products_UnitPrice * quantity).ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                    products_QuantityDecreaseBtn.Enabled = quantity > 1;
                    products_QuantityIncreaseBtn.Enabled = quantity < stock;
                }
            }
        }


        private void QuantityDecreaseBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(products_QuantityDisplay.Text, out int quantity))
            {
                if (quantity > 1)
                {
                    quantity--;
                    products_QuantityDisplay.Text = quantity.ToString();
                    products_PriceDisplay.Text = (products_UnitPrice * quantity).ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                    products_QuantityDecreaseBtn.Enabled = quantity > 1;
                    products_QuantityIncreaseBtn.Enabled = true;
                }
            }
        }


        private void products_AddToCartBtn_Click(object sender, EventArgs e)
        {
            if (productId == 0 || string.IsNullOrEmpty(products_NameDisplay.Text) || string.IsNullOrEmpty(products_PriceDisplay.Text))
            {
                MessageBox.Show("Please select a product and specify a valid quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(products_QuantityDisplay.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity greater than 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (quantity > 100)
            {
                MessageBox.Show("You cannot add more than 100 items to the cart at once.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Session.UserId == 0)
            {
                MessageBox.Show("You must be logged in to add items to the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (products_DataGrid.SelectedRows.Count == 0 || products_DataGrid.SelectedRows[0].Cells[3].Value == null)
            {
                MessageBox.Show("Please select a valid product from the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal totalPrice = products_UnitPrice * quantity;
            AddToCart(Session.UserId, productId, quantity, totalPrice);

            ClearProductSelection();
        }

        private void ClearProductSelection()
        {
            products_SearchText.Clear();
            productId = 0;
            products_PictureBox.BackgroundImage = null;
            products_NameDisplay.Clear();
            products_PriceDisplay.Clear();
            products_UnitPrice = 0m;
            products_QuantityDisplay.Text = "0";
            products_DataGrid.ClearSelection();
        }


        private void products_SearchBtn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand searchCommand = new MySqlCommand("SELECT `Product ID`, `Image`, `Name`, `Type`,`Price` FROM products WHERE `Name` LIKE @searchText", connection);
                searchCommand.Parameters.AddWithValue("@searchText", "%" + products_SearchText.Text + "%");
                MySqlDataAdapter adapter = new MySqlDataAdapter(searchCommand);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                try
                {
                    products_DataGrid.Rows.Clear();
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
                            products_DataGrid.Rows.Add(id, resized, name, price, type);
                        }
                    }
                    products_DataGrid.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void products_SortBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = products_SortBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedType)) return;

            if (products_SortBox.SelectedIndex == 0) {
                DisplayProducts();
                products_DataGrid.ClearSelection();
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
                    products_DataGrid.Rows.Clear();
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
                            products_DataGrid.Rows.Add(id, resized, name, price, type);
                        }
                    }

                    products_DataGrid.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sort failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Carts Panel
        decimal cart_UnitPrice = 0m;

        private void LoadSelectedCart(int productId, int quantity, decimal unitPrice)
        {
            this.productId = productId;
            cart_UnitPrice = unitPrice;

            cart_QuantityDisplay.Text = quantity.ToString();
            cart_TotalPriceDisplay.Text = (unitPrice * quantity).ToString("C2", CultureInfo.GetCultureInfo("en-PH"));

            int stock = CheckStocks();
            cart_QuantityIncreaseBtn.Enabled = quantity < stock;
            cart_QuantityDecreaseBtn.Enabled = quantity > 1;
        }


        private void cart_DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = cart_DataGrid.Rows[e.RowIndex];

                object imageCellValue = selectedRow.Cells[2].Value;
                if (imageCellValue is Image img)
                {
                    cart_PictureBox.BackgroundImage = img;
                    cart_PictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                }

                int quantity = Convert.ToInt32(Convert.ToString(selectedRow.Cells[4].Value).TrimStart('x'));
                decimal totalPrice = Convert.ToDecimal(selectedRow.Cells[5].Value);
                decimal unitPrice = totalPrice / quantity;

                cart_NameDisplay.Text = selectedRow.Cells[3].Value?.ToString();

                LoadSelectedCart(Convert.ToInt32(selectedRow.Cells[0].Value), quantity, unitPrice);
            }
        }


        private void cart_QuantityIncreaseBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cart_NameDisplay.Text))
            {
                MessageBox.Show("Please select a product first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(cart_QuantityDisplay.Text, out int quantity))
            {
                int stock = CheckStocks();
                if (stock == -1) return;

                if (quantity < stock)
                {
                    quantity++;
                    cart_QuantityDisplay.Text = quantity.ToString();
                    cart_TotalPriceDisplay.Text = (cart_UnitPrice * quantity).ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                    cart_QuantityDecreaseBtn.Enabled = quantity > 1;
                    cart_QuantityIncreaseBtn.Enabled = quantity < stock;
                }
            }
        }

        private void cart_QuantityDecreaseBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(cart_QuantityDisplay.Text, out int quantity))
            {
                if (quantity > 1)
                {
                    quantity--;
                    cart_QuantityDisplay.Text = quantity.ToString();
                    cart_TotalPriceDisplay.Text = (cart_UnitPrice * quantity).ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
                    cart_QuantityDecreaseBtn.Enabled = quantity > 1;
                    cart_QuantityIncreaseBtn.Enabled = true;
                }
            }
        }

        public void ClearCartSelection() {
            cart_SearchText.Clear();
            cart_UnitPrice = 0m;
            cart_PictureBox.BackgroundImage = null;
            cart_NameDisplay.Clear();
            cart_TotalPriceDisplay.Clear();
            cart_QuantityDisplay.Text = "1";
            cart_DataGrid.ClearSelection();
        }

        private void cart_SearchBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cart_SearchText.Text))
            {
                MessageBox.Show("Please enter a product name to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cart_DataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items to your cart before searching.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand searchCommand = new MySqlCommand("SELECT s.`Cart ID`, p.`Product ID`, p.`Image`, p.`Name`, s.`Quantity`, s.`Total Price` FROM shopping_cart s JOIN products p ON p.`Product ID` = s.`Product ID` WHERE s.`User ID` = @userId AND p.`Name` LIKE @searchText", connection);
                searchCommand.Parameters.AddWithValue("@userId", Session.UserId);
                searchCommand.Parameters.AddWithValue("@searchText", "%" + cart_SearchText.Text + "%");
                MySqlDataAdapter adapter = new MySqlDataAdapter(searchCommand);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                try
                {
                    cart_DataGrid.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["Product ID"]);
                        int cartId = Convert.ToInt32(row["Cart ID"]);
                        string name = row["Name"].ToString();
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        decimal totalPrice = Convert.ToDecimal(row["Total Price"]);
                        byte[] imageData = (byte[])row["Image"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image productImage = Image.FromStream(ms);
                            Image resizedImage = new Bitmap(productImage, new Size(80, 80));
                            cart_DataGrid.Rows.Add(id, cartId, resizedImage, name, quantity, totalPrice);
                        }
                    }
                    cart_DataGrid.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cart_UpdateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cart_NameDisplay.Text)) {
                MessageBox.Show("Please select a valid product", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand updateCommand = new MySqlCommand("UPDATE shopping_cart SET `Quantity` = @quantity, `Total Price` = @totalPrice WHERE `Cart ID` = @cartId AND `User ID` = @userId AND `Product ID` = @productID", connection);
                updateCommand.Parameters.AddWithValue("@quantity", Convert.ToInt32(cart_QuantityDisplay.Text));
                int quantity = Convert.ToInt32(cart_QuantityDisplay.Text);
                if (!decimal.TryParse(cart_TotalPriceDisplay.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-PH"), out decimal totalPrice))
                {
                    MessageBox.Show("Invalid total price format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                updateCommand.Parameters.AddWithValue("@totalPrice", totalPrice);
                updateCommand.Parameters.AddWithValue("@cartId", Convert.ToInt32(cart_DataGrid.SelectedRows[0].Cells[1].Value));
                updateCommand.Parameters.AddWithValue("@userId", Session.UserId);
                updateCommand.Parameters.AddWithValue("@productID", Convert.ToInt32(cart_DataGrid.SelectedRows[0].Cells[0].Value));
                try
                {
                    connection.Open();
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        DisplayCart();
                        UpdateCartCounter();
                        ClearCartSelection();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update cart. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update cart: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void cart_RemoveBtn_Click(object sender, EventArgs e)
        {
            if (cart_DataGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to remove from the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM shopping_cart WHERE `User ID` = @userID AND `Cart ID` = @cartID AND `Product ID` = @productID", connection);
                deleteCommand.Parameters.AddWithValue("@userID", Session.UserId);
                deleteCommand.Parameters.AddWithValue("@cartID", Convert.ToInt32(cart_DataGrid.SelectedRows[0].Cells[1].Value));
                deleteCommand.Parameters.AddWithValue("productID", Convert.ToInt32(cart_DataGrid.SelectedRows[0].Cells[0].Value));
                try
                {
                    // Delete Product
                    deleteCommand.ExecuteNonQuery();
                    DisplayCart();
                    UpdateCartCounter();
                    ClearCartSelection();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
                finally {
                    connection.Close();
                }
            }
        }

        private void cartCounterBtn_Click(object sender, EventArgs e)
        {
            cartBtn.PerformClick();
        }

        private void cart_CheckOutBtn_Click(object sender, EventArgs e)
        {
            if (cart_DataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items to your cart before checking out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();

                foreach (DataGridViewRow row in cart_DataGrid.Rows)
                {
                    if (row.IsNewRow) continue;

                    string productName = row.Cells["ProductName"].Value?.ToString();
                    if (string.IsNullOrEmpty(productName))
                    {
                        MessageBox.Show("A product in your cart has an invalid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int quantityInCart = Convert.ToInt32(Convert.ToString(row.Cells["ProductQuantity"].Value).TrimStart('x'));

                    using (MySqlCommand cmd = new MySqlCommand("SELECT `Stocks` FROM products WHERE `Name` = @name", connection))
                    {
                        cmd.Parameters.AddWithValue("@name", productName);
                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show($"Product '{productName}' no longer exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        int currentStock = Convert.ToInt32(result);
                        if (quantityInCart > currentStock)
                        {
                            MessageBox.Show($"Not enough stock for '{productName}'. Available: {currentStock}, In Cart: {quantityInCart}.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }

            ShowOnlyPanel(checkoutPanel);
            ClearCheckoutField();
            DisplayBillingInformation();
            CheckIfUserDetailsExist();
        }

        private void cart_RefreshBtn_Click(object sender, EventArgs e)
        {
            ClearCartSelection();
            DisplayCart();
        }

        // Check Out Panel

        public void CheckIfUserDetailsExist()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand checkUserCommand = new MySqlCommand("SELECT * FROM accounts WHERE `User ID` = @userID", connection);
                checkUserCommand.Parameters.AddWithValue("@userID", Session.UserId);
                MySqlDataReader reader = checkUserCommand.ExecuteReader();
                if (reader.Read())
                {
                    checkout_FirstNameText.Text = reader["First Name"].ToString();
                    checkout_LastNameText.Text = reader["Last Name"].ToString();
                    checkout_EmailText.Text = reader["Email"].ToString();
                    checkout_PhoneNumberText.Text = reader["Phone Number"].ToString();
                    checkout_AddressText.Text = reader["Address"].ToString();
                }
                else
                {
                    MessageBox.Show("User details not found. Please fill in your details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void checkout_BackBtn_Click(object sender, EventArgs e)
        {
            cartBtn.PerformClick();
        }

        public void ClearCheckoutField() {
            checkout_TotalAmountDisplay.Clear();
            checkout_FirstNameText.Clear();
            checkout_LastNameText.Clear();
            checkout_EmailText.Clear();
            checkout_PhoneNumberText.Clear();
            checkout_AddressText.Clear();
            checkout_CVCText.Clear();
            checkout_CardNumberText.Clear();
            checkout_CardBox.Checked = false;
            checkout_CODBox.Checked = false;
            checkout_CardNumberLabel.Visible = false;
            checkout_CardNumberText.Visible = false;
            checkout_CVCLabel.Visible = false;
            checkout_CVCText.Visible = false;
        }

        public bool CheckIfCheckOutFieldisEmpty() {
            if (checkout_CODBox.Checked)
            {
                if (string.IsNullOrEmpty(checkout_TotalAmountDisplay.Text) ||
                    string.IsNullOrEmpty(checkout_FirstNameText.Text) ||
                    string.IsNullOrEmpty(checkout_LastNameText.Text) ||
                    string.IsNullOrEmpty(checkout_EmailText.Text) ||
                    string.IsNullOrEmpty(checkout_PhoneNumberText.Text) ||
                    string.IsNullOrEmpty(checkout_AddressText.Text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (checkout_CardBox.Checked)
            {
                if (string.IsNullOrEmpty(checkout_TotalAmountDisplay.Text) ||
                    string.IsNullOrEmpty(checkout_FirstNameText.Text) ||
                    string.IsNullOrEmpty(checkout_LastNameText.Text) ||
                    string.IsNullOrEmpty(checkout_EmailText.Text) ||
                    string.IsNullOrEmpty(checkout_PhoneNumberText.Text) ||
                    string.IsNullOrEmpty(checkout_AddressText.Text) ||
                    string.IsNullOrEmpty(checkout_CardNumberText.Text) ||
                    string.IsNullOrEmpty(checkout_CVCText.Text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        private void checkout_PayBtn_Click(object sender, EventArgs e)
        {
            if (checkout_DataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items to your cart before checking out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(checkout_CODBox.Checked || checkout_CardBox.Checked))
            {
                MessageBox.Show("Select a payment method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CheckIfCheckOutFieldisEmpty())
            {
                string method = checkout_CODBox.Checked ? "Cash on Delivery" : "Card Payment";
                MessageBox.Show($"Please fill in all required fields for {method}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(checkout_TotalAmountDisplay.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-PH"), out decimal totalAmount))
            {
                MessageBox.Show("Invalid total amount format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to proceed with checkout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();

                // Update Product Stocks
                try
                {
                    foreach (DataGridViewRow row in checkout_DataGrid.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                            int quantity = Convert.ToInt32(Convert.ToString(row.Cells["Quantity"].Value).TrimStart('x'));
                            MySqlCommand updateStock = new MySqlCommand("UPDATE products SET `Stocks` = `Stocks` - @quantity WHERE `Product ID` = @productId", connection);
                            updateStock.Parameters.AddWithValue("@quantity", quantity);
                            updateStock.Parameters.AddWithValue("@productId", productId);
                            updateStock.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update product stocks: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert new order
                MySqlCommand insertOrder = new MySqlCommand("INSERT INTO orders(`User ID`, `Email`, `Total Amount`) VALUES(@userID, @email, @totalAmount)", connection);
                insertOrder.Parameters.AddWithValue("@userID", Session.UserId);
                insertOrder.Parameters.AddWithValue("@email", checkout_EmailText.Text.Trim());
                insertOrder.Parameters.AddWithValue("@totalAmount", totalAmount);

                long orderId;
                try
                {
                    insertOrder.ExecuteNonQuery();
                    orderId = insertOrder.LastInsertedId;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to place order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert User Details
                MySqlCommand InsertUserCommand = new MySqlCommand("UPDATE accounts SET `First Name` = @firstName, `Last Name` = @lastName, `Address` = @address, `Email` = @email, `Phone Number` = @phoneNumber WHERE `User ID` = @userID", connection);
                InsertUserCommand.Parameters.AddWithValue("@userID", Session.UserId);
                InsertUserCommand.Parameters.AddWithValue("@firstName", checkout_FirstNameText.Text.Trim());
                InsertUserCommand.Parameters.AddWithValue("@lastName", checkout_LastNameText.Text.Trim());
                InsertUserCommand.Parameters.AddWithValue("@address", checkout_AddressText.Text.Trim());
                InsertUserCommand.Parameters.AddWithValue("@email", checkout_EmailText.Text.Trim());
                InsertUserCommand.Parameters.AddWithValue("@phoneNumber", checkout_PhoneNumberText.Text.Trim());
                try
                {
                    InsertUserCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to insert user details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert products into receipts
                string paymentMethod = checkout_CODBox.Checked ? "Cash" : "Card";
                try
                {
                    foreach (DataGridViewRow row in checkout_DataGrid.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                            int quantity = Convert.ToInt32(Convert.ToString(row.Cells["Quantity"].Value).TrimStart('x'));
                            decimal totalPrice = Convert.ToDecimal(row.Cells["TotalPrice"].Value);

                            using (MySqlCommand insertReceipt = new MySqlCommand("INSERT INTO receipts(`Order ID`, `User ID`, `Product ID`, `Quantity`, `Total Price`, `Payment Method`) VALUES(@orderId, @userID, @productId, @quantity, @totalPrice, @paymentMethod)", connection))
                            {
                                insertReceipt.Parameters.AddWithValue("@orderId", orderId);
                                insertReceipt.Parameters.AddWithValue("@userID", Session.UserId);
                                insertReceipt.Parameters.AddWithValue("@productId", productId);
                                insertReceipt.Parameters.AddWithValue("@quantity", quantity);
                                insertReceipt.Parameters.AddWithValue("@totalPrice", totalPrice);
                                insertReceipt.Parameters.AddWithValue("@paymentMethod", paymentMethod);

                                insertReceipt.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to process receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Clear shopping cart
                try
                {
                    MySqlCommand deleteCart = new MySqlCommand("DELETE FROM shopping_cart WHERE `User ID` = @userID", connection);
                    deleteCart.Parameters.AddWithValue("@userID", Session.UserId);

                    int deleted = deleteCart.ExecuteNonQuery();
                    if (deleted > 0)
                    {
                        MessageBox.Show("Checkout successful! Thank you for your purchase.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearCheckoutField();
                        productsBtn.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Cart was already empty or not removed properly.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to clear cart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool suppressEvents = false;
        private void checkout_CardBox_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEvents) return;

            if (checkout_CardBox.Checked)
            {
                suppressEvents = true;
                checkout_CODBox.Checked = false;
                suppressEvents = false;

                checkout_CardNumberLabel.Visible = true;
                checkout_CardNumberText.Visible = true;
                checkout_CVCLabel.Visible = true;
                checkout_CVCText.Visible = true;
            }
        }

        private void checkout_CODBox_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEvents) return;

            if (checkout_CODBox.Checked)
            {
                suppressEvents = true;
                checkout_CardBox.Checked = false;
                suppressEvents = false;

                checkout_CardNumberLabel.Visible = false;
                checkout_CardNumberText.Visible = false;
                checkout_CVCLabel.Visible = false;
                checkout_CVCText.Visible = false;
            }
        }

        // Order History Panel
        int SelectedOrderID = 0;

        private void orderHistory_ViewReceiptBtn_Click(object sender, EventArgs e)
        {
            if (orderHistory_DataGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to view the receipt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (receiptPanel.Visible == false)
            {
                orderHistory_ViewReceiptBtn.Text = "Hide Receipt";
                receiptPanel.Visible = true;
                orderHistory_DataGrid.Visible = false;
                DisplayReceipt();
            }
            else {
                orderHistory_ViewReceiptBtn.Text = "View Receipt";
                receiptPanel.Visible = false;
                orderHistory_DataGrid.Visible = true;
                orderHistory_ViewReceiptBtn.Visible = false;
                orderHistory_DataGrid.ClearSelection();
            }

        }

        private void orderHistory_DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                orderHistory_ViewReceiptBtn.Visible = true;
                SelectedOrderID = Convert.ToInt32(orderHistory_DataGrid.Rows[e.RowIndex].Cells[0].Value);
            }
            else
            {
                SelectedOrderID = 0;
                orderHistory_ViewReceiptBtn.Visible = false;
            }
        }

        // Profile Panel

        public void DisplayProfile()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand displayCommand = new MySqlCommand("SELECT * FROM accounts WHERE `User ID` = @userID", connection))
                    {
                        displayCommand.Parameters.AddWithValue("@userID", Session.UserId);

                        using (MySqlDataReader reader = displayCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                /*profile_PictureBox.Image = null;*/ // Clear previous image

                                if (reader["Profile Photo"] != DBNull.Value)
                                {
                                    byte[] imageData = (byte[])reader["Profile Photo"];
                                    using (MemoryStream ms = new MemoryStream(imageData))
                                    {
                                        // Dispose the previous image if needed
                                        Image profileImage = Image.FromStream(ms);
                                        profile_PictureBox.Image = profileImage;
                                        profile_PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                    }
                                }

                                profile_UsernameText.Text = reader["Username"].ToString();
                                profile_PasswordText.Text = reader["Password"].ToString();
                                profile_FirstNameText.Text = reader["First Name"].ToString();
                                profile_LastNameText.Text = reader["Last Name"].ToString();
                                profile_EmailText.Text = reader["Email"].ToString();
                                profile_PhoneNumberText.Text = reader["Phone Number"].ToString();
                                profile_AddressText.Text = reader["Address"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("User details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    profile_OrdersLabel.Text = GetOrderCount().ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to display profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public int GetOrderCount()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                MySqlCommand countCommand = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE `User ID` = @userID AND `Status` = 'Approved'", connection);
                countCommand.Parameters.AddWithValue("@userID", Session.UserId);
                try
                {
                    return Convert.ToInt32(countCommand.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to get order count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
        }

        private void ToggleProfileFields(bool editMode)
        {
            profile_SelectPhotoBtn.Visible = editMode;
            profile_UsernameText.ReadOnly = !editMode;
            profile_PasswordText.ReadOnly = !editMode;
            profile_PasswordText.PasswordChar = editMode ? '\0' : '*';
            profile_FirstNameText.ReadOnly = !editMode;
            profile_LastNameText.ReadOnly = !editMode;
            profile_EmailText.ReadOnly = !editMode;
            profile_PhoneNumberText.ReadOnly = !editMode;
            profile_AddressText.ReadOnly = !editMode;
        }

        private bool CheckIfAccountExist()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE `Username` = @username AND `User ID` != @userID", connection))
                {
                    cmd.Parameters.AddWithValue("@username", profile_UsernameText.Text.Trim());
                    cmd.Parameters.AddWithValue("@userID", Session.UserId);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void profile_EditBtn_Click(object sender, EventArgs e)
        {
            bool isEditMode = profile_EditBtn.Text == "Edit";

            if (isEditMode)
            {
                ToggleProfileFields(editMode: true);
                profile_EditBtn.Text = "Save";
            }
            else
            {
                if (CheckIfAccountExist()) 
                {
                    if (Session.Username != profile_UsernameText.Text) 
                    {
                        MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
                {
                    connection.Open();
                    using (MySqlCommand updateCommand = new MySqlCommand("UPDATE accounts SET `Profile Photo` = @profilePhoto, `Username` = @username, `Password` = @password, `First Name` = @firstName, `Last Name` = @lastName, `Email` = @email, `Phone Number` = @phoneNumber, `Address` = @address WHERE `User ID` = @userID", connection))
                    {
                        if (profile_PictureBox.Image != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                profile_PictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                updateCommand.Parameters.AddWithValue("@profilePhoto", ms.ToArray());
                            }
                        }
                        else
                        {
                            updateCommand.Parameters.AddWithValue("@profilePhoto", DBNull.Value);
                        }

                        updateCommand.Parameters.AddWithValue("@username", profile_UsernameText.Text.Trim());
                        updateCommand.Parameters.AddWithValue("@password", profile_PasswordText.Text.Trim());
                        updateCommand.Parameters.AddWithValue("@firstName", profile_FirstNameText.Text.Trim());
                        updateCommand.Parameters.AddWithValue("@lastName", profile_LastNameText.Text.Trim());
                        updateCommand.Parameters.AddWithValue("@email", profile_EmailText.Text.Trim());
                        updateCommand.Parameters.AddWithValue("@phoneNumber", profile_PhoneNumberText.Text.Trim());
                        updateCommand.Parameters.AddWithValue("@address", profile_AddressText.Text.Trim());
                        updateCommand.Parameters.AddWithValue("@userID", Session.UserId);

                        try
                        {
                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No changes were made to the profile.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to update profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                ToggleProfileFields(editMode: false);
                profile_EditBtn.Text = "Edit";
                DisplayProfile();
            }
        }

        private void profile_SelectPhotoBtn_Click(object sender, EventArgs e)
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
                        profile_PictureBox.Image = new Bitmap(imgTemp);
                        profile_PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
