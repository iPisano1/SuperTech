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

namespace Computer_Shop_System
{
    public partial class CustomerForm: Form
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
                    MessageBox.Show("Failed to update cart counter: " + ex.Message);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to add to cart: " + ex.Message);
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
                MySqlCommand displayCommand = new MySqlCommand("SELECT `Product ID`, `Image`, `Name`, `Type`, `Price` FROM products", connection);
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
                    MessageBox.Show("Failed to load products: " + ex.Message);
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

                        cart_DataGrid.Columns.Add("ProductQuantity", "Quantity");
                        cart_DataGrid.Columns["ProductQuantity"].FillWeight = 60;
                        cart_DataGrid.Columns["ProductQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        cart_DataGrid.Columns["ProductQuantity"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        cart_DataGrid.Columns.Add("TotalPrice", "Total Price");
                        cart_DataGrid.Columns["TotalPrice"].FillWeight = 60;
                        //cart_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        cart_DataGrid.Columns["TotalPrice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        cart_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
                        cart_DataGrid.Columns["TotalPrice"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");
                    }

                    foreach (DataRow row in dataTable.Rows)
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
                MySqlCommand displayCommand = new MySqlCommand("SELECT s.`Cart ID`, p.`Product ID`, s.`Quantity`, s.`Total Price` FROM shopping_cart s JOIN products p ON p.`Product ID` = s.`Product ID` WHERE s.`User ID` = @userID", connection);
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

                        checkout_DataGrid.Columns.Add("ProductID", "Product");

                        checkout_DataGrid.Columns.Add("Quantity", "Quantity");
                        checkout_DataGrid.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                        checkout_DataGrid.Columns.Add("TotalPrice", "Price");
                        checkout_DataGrid.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
                        checkout_DataGrid.Columns["TotalPrice"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        int cartId = Convert.ToInt32(row["Cart ID"]);
                        int productID = Convert.ToInt32(row["Product ID"]);
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        decimal totalPrice = Convert.ToDecimal(row["Total Price"]);

                        grandTotal += totalPrice;

                        checkout_DataGrid.Rows.Add(cartId, productID, quantity, totalPrice);
                        checkout_DataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    checkout_TotalAmountDisplay.Text = grandTotal.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));

                    checkout_DataGrid.ClearSelection();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
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

        // Products Panel
        int productId;
        decimal products_UnitPrice = 0m;


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

                products_QuantityDisplay.Text = "1";
            }
        }


        private void QuantityIncreaseBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(products_NameDisplay.Text))
            {
                MessageBox.Show("Please select a product first.");
                return;
            }

            if (int.TryParse(products_QuantityDisplay.Text, out int quantity))
            {
                quantity++;
                products_QuantityDisplay.Text = quantity.ToString();

                decimal totalPrice = products_UnitPrice * quantity;
                products_PriceDisplay.Text = totalPrice.ToString("C2");
            }
            else
            {
                MessageBox.Show("Invalid quantity.");
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

                    decimal totalPrice = products_UnitPrice * quantity;
                    products_PriceDisplay.Text = totalPrice.ToString("C2");
                }
                else
                {
                    MessageBox.Show("Quantity cannot be less than 1.");
                }
            }
            else
            {
                products_QuantityDisplay.Text = "1";
                products_PriceDisplay.Text = products_UnitPrice.ToString("C2");
            }
        }


        private void products_AddToCartBtn_Click(object sender, EventArgs e)
        {
            if (productId == 0 || string.IsNullOrEmpty(products_NameDisplay.Text) || string.IsNullOrEmpty(products_PriceDisplay.Text))
            {
                MessageBox.Show("Please select a product and specify a valid quantity.");
                return;
            }

            if (!int.TryParse(products_QuantityDisplay.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity greater than 0.");
                return;
            }

            if (quantity > 100)
            {
                MessageBox.Show("You cannot add more than 100 items to the cart at once.");
                return;
            }

            if (Session.UserId == 0)
            {
                MessageBox.Show("You must be logged in to add items to the cart.");
                return;
            }

            if (products_DataGrid.SelectedRows.Count == 0 || products_DataGrid.SelectedRows[0].Cells[3].Value == null)
            {
                MessageBox.Show("Please select a valid product from the list.");
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
            products_QuantityDisplay.Text = "1";
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

                int quantity = Convert.ToInt32(cart_DataGrid.Rows[e.RowIndex].Cells[4].Value);
                decimal totalPrice = Convert.ToDecimal(cart_DataGrid.Rows[e.RowIndex].Cells[5].Value);

                cart_UnitPrice = totalPrice / quantity;

                cart_NameDisplay.Text = selectedRow.Cells[3].Value?.ToString();
                cart_QuantityDisplay.Text = quantity.ToString();
                cart_TotalPriceDisplay.Text = totalPrice.ToString("C2", CultureInfo.GetCultureInfo("en-PH"));
            }
        }

        private void cart_QuantityIncreaseBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cart_NameDisplay.Text))
            {
                MessageBox.Show("Please select a product first.");
                return;
            }

            if (int.TryParse(cart_QuantityDisplay.Text, out int quantity))
            {
                quantity++;
                cart_QuantityDisplay.Text = quantity.ToString();

                decimal totalPrice = cart_UnitPrice * quantity;
                cart_TotalPriceDisplay.Text = totalPrice.ToString("C2");
            }
            else
            {
                MessageBox.Show("Invalid quantity.");
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

                    decimal totalPrice = cart_UnitPrice * quantity;
                    cart_TotalPriceDisplay.Text = totalPrice.ToString("C2");
                }
                else
                {
                    MessageBox.Show("Quantity cannot be less than 1.");
                }
            }
            else
            {
                cart_QuantityDisplay.Text = "1";
                cart_TotalPriceDisplay.Text = cart_UnitPrice.ToString("C2");
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
                MessageBox.Show("Please enter a product name to search.");
                return;
            }
            if (cart_DataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items to your cart before searching.");
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
                MessageBox.Show("Please select a valid product");
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand updateCommand = new MySqlCommand("UPDATE shopping_cart SET `Quantity` = @quantity, `Total Price` = @totalPrice WHERE `Cart ID` = @cartId AND `User ID` = @userId AND `Product ID` = @productID", connection);
                updateCommand.Parameters.AddWithValue("@quantity", Convert.ToInt32(cart_QuantityDisplay.Text));
                int quantity = Convert.ToInt32(cart_QuantityDisplay.Text);
                decimal totalPrice;
                if (!decimal.TryParse(cart_TotalPriceDisplay.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out totalPrice))
                {
                    MessageBox.Show("Invalid total price format.");
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
                        MessageBox.Show("Failed to update cart. Please try again.");
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
                MessageBox.Show("Please select a product to remove from the cart.");
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
                MessageBox.Show("Your cart is empty. Please add items to your cart before checking out.");
                return;
            }
            ShowOnlyPanel(checkoutPanel);
            DisplayBillingInformation();
        }

        private void cart_RefreshBtn_Click(object sender, EventArgs e)
        {
            ClearCartSelection();
            DisplayCart();
        }

        // Check Out Panel

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
                MessageBox.Show("Your cart is empty. Please add items to your cart before checking out.");
                return;
            }
            if (!(checkout_CODBox.Checked || checkout_CardBox.Checked))
            {
                MessageBox.Show("Select a payment method.");
                return;
            }
            if (CheckIfCheckOutFieldisEmpty())
            {
                if (checkout_CODBox.Checked)
                {
                    MessageBox.Show("Please fill in all required fields for Cash on Delivery.");
                }
                else if (checkout_CardBox.Checked)
                {
                    MessageBox.Show("Please fill in all required fields for Card Payment.");
                }
                return;
            }

            if (!decimal.TryParse(checkout_TotalAmountDisplay.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-PH"), out decimal totalAmount))
            {
                MessageBox.Show("Invalid total amount format.");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                connection.Open();
                // Insert User's Order to Order Table
                MySqlCommand insertCommand = new MySqlCommand("INSERT INTO orders(`User ID`, `Total Amount`) VALUES(@userID, @totalAmount)", connection);
                insertCommand.Parameters.AddWithValue("@userID", Session.UserId);
                insertCommand.Parameters.AddWithValue("@totalAmount", totalAmount);

                try
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Failed to place order. Please try again.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to place order: " + ex.Message);
                    return;
                }

                // Delete User's Shopping Cart
                MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM shopping_cart WHERE `User ID` = @userID", connection);
                deleteCommand.Parameters.AddWithValue("@userID", Session.UserId);
                try
                {
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Checkout successful! Thank you for your purchase.");
                        ClearCheckoutField();
                        productsBtn.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Checkout failed. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to checkout: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void checkout_CardBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkout_CardBox.Checked)
            {   
                checkout_CardNumberLabel.Visible = true;
                checkout_CardNumberText.Visible = true;
                checkout_CVCLabel.Visible = true;
                checkout_CVCText.Visible = true;
            }
            else
            {
                checkout_CardNumberLabel.Visible = false;
                checkout_CardNumberText.Visible = false;
                checkout_CVCLabel.Visible = false;
                checkout_CVCText.Visible = false;
            }
        }
    }
}
